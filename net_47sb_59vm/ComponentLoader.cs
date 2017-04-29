/*
This class is modification of ServerApi.cs from TerrariaServerApi
Copyright (C) 2011-2015 Nyx Studios (fka. The TShock Team)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Linq;

namespace net_47sb_59vm
{
    public static class ComponentLoader
    {
        public static readonly string ExtensionsPath = Program.cfg.componentspath;
        private static readonly Dictionary<string, Assembly> loadedAssemblies = new Dictionary<string, Assembly>();
        private static readonly List<ComponentContainer> extensions = new List<ComponentContainer>();
        public static string ExtensionsDirectoryPath { get; private set; }
        public static ReadOnlyCollection<ComponentContainer> Extensions { get { return new ReadOnlyCollection<ComponentContainer>(extensions); } }

        internal static void Initialize()
        {
            ExtensionsDirectoryPath = Path.Combine(Environment.CurrentDirectory, ExtensionsPath);
            if (!Directory.Exists(ExtensionsDirectoryPath))
            {
                string lcDirectoryPath =
                    Path.Combine(Path.GetDirectoryName(ExtensionsDirectoryPath), ExtensionsPath.ToLower());
                if (Directory.Exists(lcDirectoryPath))
                {
                    Directory.Move(lcDirectoryPath, ExtensionsDirectoryPath);
                    Logger.Log("Case sensitive filesystem detected, extensions directory has been renamed.");
                }
                else
                    Directory.CreateDirectory(ExtensionsDirectoryPath);
            }
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            LoadExtensions();
        }

        internal static void DeInitialize() { UnloadExtensions(); }

        internal static void LoadExtensions()
        {
            List<FileInfo> fileInfos = new DirectoryInfo(ExtensionsDirectoryPath).GetFiles("*.dll").ToList();
            fileInfos.AddRange(new DirectoryInfo(ExtensionsDirectoryPath).GetFiles("*.extension"));
            foreach (FileInfo fileInfo in fileInfos)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
                try
                {
                    Assembly assembly;
                    if (!loadedAssemblies.TryGetValue(fileNameWithoutExtension, out assembly))
                    {
                        try { assembly = Assembly.Load(File.ReadAllBytes(fileInfo.FullName)); }
                        catch (BadImageFormatException) { continue; }
                        loadedAssemblies.Add(fileNameWithoutExtension, assembly);
                    }
                    foreach (Type type in assembly.GetExportedTypes())
                    {
                        if (!type.IsSubclassOf(typeof(HatComponent)) || !type.IsPublic || type.IsAbstract)
                            continue;
                        object[] customAttributes = type.GetCustomAttributes(typeof(ApiVersionAttribute), false);
                        if (customAttributes.Length == 0)
                            continue;
                        var apiVersionAttribute = (ApiVersionAttribute)customAttributes[0];
                        Version apiVersion = apiVersionAttribute.ApiVersion;
                        if (apiVersion.Major != ApiVersion.Major || apiVersion.Minor != ApiVersion.Minor)
                        {
                            Logger.Log(
                                string.Format("Extension \"{0}\" is designed for a different Halcyon API version ({1}) and was ignored.",
                                type.FullName, apiVersion.ToString(2)));
                            continue;
                        }
                        HatComponent extensionInstance;
                        extensionInstance = (HatComponent)Activator.CreateInstance(type);
                        try { extensionInstance = (HatComponent)Activator.CreateInstance(type); }
                        catch (Exception ex) { Logger.Log(String.Format("Could not create an instance of extension class \"{0}\""), type.FullName + "\n" + ex); }
                        extensions.Add(new ComponentContainer(extensionInstance));
                    }
                }
                catch (Exception ex) { Logger.Log(string.Format("Failed to load assembly \"{0}\".", fileInfo.Name) + ex); }
            }
            IOrderedEnumerable<ComponentContainer> orderedExtensionSelector =
                from x in Extensions
                orderby x.Component.Order, x.Component.Name
                select x;
            try
            {
                int count = 0;
                foreach (ComponentContainer current in orderedExtensionSelector)
                    count++;
                foreach (ComponentContainer current in orderedExtensionSelector)
                {
                    try
                    {
                        current.Initialize();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogNoTrace(ex.Message);
                        Logger.LogNoTrace(ex.Source);
                        Logger.LogNoTrace(ex.HelpLink);
                        Logger.LogNoTrace(ex.StackTrace);
                        // Broken extensions better stop the entire server init.
                        break;
                    }
                    Logger.Log(string.Format(
                        "Extension {0} v{1} (by {2}) initiated.", current.Component.Name, current.Component.Version, current.Component.Author));
                }
            }
            catch { }
        }

        internal static void UnloadExtensions()
        {
            foreach (ComponentContainer ComponentContainer in extensions)
            {
                try
                {
                    ComponentContainer.DeInitialize();
                    Logger.Log(string.Format(
                        "Extension \"{0}\" was deinitialized", ComponentContainer.Component.Name));
                }
                catch (Exception ex)
                {
                    Logger.Log(string.Format(
                        "Extension \"{0}\" has thrown an exception while being deinitialized:\n{1}", ComponentContainer.Component.Name, ex));
                }
            }

            foreach (ComponentContainer ComponentContainer in extensions)
            {
                try { ComponentContainer.Dispose(); }
                catch (Exception ex)
                {
                    Logger.Log(string.Format(
                        "Extension \"{0}\" has thrown an exception while being disposed:\n{1}", ComponentContainer.Component.Name, ex));
                }
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string fileName = args.Name.Split(',')[0];
            string path = Path.Combine(ExtensionsDirectoryPath, fileName + ".dll");
            try
            {
                if (File.Exists(path))
                {
                    Assembly assembly;
                    if (!loadedAssemblies.TryGetValue(fileName, out assembly))
                    {
                        assembly = Assembly.Load(File.ReadAllBytes(path));
                        loadedAssemblies.Add(fileName, assembly);
                    }
                    return assembly;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    string.Format("Error on resolving assembly \"{0}.dll\":\n{1}", fileName, ex));
            }
            return null;
        }
    }
}

