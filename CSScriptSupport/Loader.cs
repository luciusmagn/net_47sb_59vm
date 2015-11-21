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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;
using CSScriptLibrary;
using Hat.NET;

namespace CSharpComponent
{
    public static class ComponentLoader
    {
        public const string ComponentPath = "servercomponents";
        private static readonly List<ComponentContainer> components = new List<ComponentContainer>();
        public static string ComponentsDirectoryPath
        {
            get;
            private set;
        }
        public static ReadOnlyCollection<ComponentContainer> Plugins
        {
            get { return new ReadOnlyCollection<ComponentContainer>(components); }
        }
        internal static void Initialize()
        {
            ComponentsDirectoryPath = System.IO.Path.Combine(Environment.CurrentDirectory, ComponentPath);
            if (!Directory.Exists(ComponentsDirectoryPath))
            {
                string lcDirectoryPath =
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(ComponentsDirectoryPath), ComponentPath.ToLower());

                if (Directory.Exists(lcDirectoryPath))
                {
                    Directory.Move(lcDirectoryPath, ComponentsDirectoryPath);
                    Console.WriteLine("Case sensitive filesystem detected, Components directory has been renamed.", TraceLevel.Warning);
                }
                else
                {
                    Directory.CreateDirectory(ComponentsDirectoryPath);
                }
            }
            LoadComponents();
        }

        internal static void DeInitialize()
        {
            UnloadComponents();
        }
        private static AppDomain appdomain;
        public static void AppDomainInitializeCallback(string[] s)
        {
            appdomain.Execute(() =>
            {
                dynamic possiblyComponent = CSScript
                                .Evaluator
                                .ReferenceAssembly(Assembly.GetCallingAssembly())
                                .ReferenceAssembly(Assembly.GetExecutingAssembly())
                                .ReferenceAssembly(Assembly.GetEntryAssembly())
                                .ReferenceAssemblyByName(@"TShockAPI")
                                .ReferenceAssemblyByName("TerrariaServer")
                                .ReferenceDomainAssemblies()
                                .LoadFile(s[0]);
                components.Add(new ComponentContainer(possiblyComponent));
            });
        }
        internal static void LoadComponents()
        {
            string[] ValidExtensions = new string[] { ".cs", ".cscomponent", ".component" };
            List<FileInfo> dif = new DirectoryInfo(ComponentPath).GetFiles().Where(f => ValidExtensions.Contains(f.Extension)).ToList();
            foreach (FileInfo fileInfo in dif)
            {
                try
                {
                    try
                    {
                        Action<string> Add = (name) =>
                        {
                            dynamic possiblyComponent = CSScript
                                .Evaluator
                                .ReferenceAssembly(Assembly.GetCallingAssembly())
                                .ReferenceAssembly(Assembly.GetExecutingAssembly())
                                .ReferenceAssembly(Assembly.GetEntryAssembly())
                                .ReferenceAssemblyByName(@"Hat.NET")
                                .ReferenceDomainAssemblies()
                                .LoadFile(System.IO.Path.Combine(name));
                            components.Add(new ComponentContainer(possiblyComponent));
                        };
                        AppDomain dmn = AppDomain.CurrentDomain.Clone();
                        dmn.Execute<string>(Add, fileInfo.FullName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name) + " couldn't be loaded. You fucked something up");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        Console.WriteLine(ex.Source);
                        continue;
                    }

                }
                catch (Exception ex)
                {
                    // Broken assemblies / plugins better stop the entire server init.
                    Console.WriteLine(string.Format("Failed to load Component \"{0}\".", fileInfo.Name) + ex);
                }
            }
            IOrderedEnumerable<ComponentContainer> orderedPluginSelector =
                from x in Plugins
                orderby x.Component.Order, x.Component.Name
                select x;
            try
            {
                int count = 0;
                foreach (ComponentContainer current in orderedPluginSelector)
                {
                    count++;
                }
                foreach (ComponentContainer current in orderedPluginSelector)
                {
                    try
                    {
                        current.Initialize();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.Source);
                        Console.WriteLine(ex.HelpLink);
                        Console.WriteLine(ex.StackTrace);
                        // Broken Components better stop the entire server init.
                        break;
                    }
                    Console.WriteLine(string.Format(
                        "Component {0} v{1} (by {2}) initiated.", current.Component.Name, current.Component.Version, current.Component.Author),
                        TraceLevel.Info);
                }
            }
            catch
            {
            }
        }

        internal static void UnloadComponents()
        {
            foreach (ComponentContainer ComponentContainer in components)
            {
                try
                {
                    ComponentContainer.DeInitialize();
                    Console.WriteLine(string.Format(
                        "Component \"{0}\" was deinitialized", ComponentContainer.Component.Name),
                        TraceLevel.Error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(
                        "Component \"{0}\" has thrown an exception while being deinitialized:\n{1}", ComponentContainer.Component.Name, ex),
                        TraceLevel.Error);
                }
            }

            foreach (ComponentContainer ComponentContainer in components)
            {
                try
                {
                    ComponentContainer.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(
                        "Component \"{0}\" has thrown an exception while being disposed:\n{1}", ComponentContainer.Component.Name, ex),
                        TraceLevel.Error);
                }
            }
        }
    }
}

