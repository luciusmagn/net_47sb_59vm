﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET
{
    /// <summary>
    /// Simplistic config solution
    /// The class you are using as config must extend Config class
    /// All fields you want to save must be public and mustn't start with _ character
    /// All fields must provide default values for serialization
    /// To use execute Config.Save(instanceofyourconfig);
    /// if you want to save, and
    /// instanceofyourconfig.Deserialize(); to retrieve the values from .cfg file
    /// The name of the config file is determined by the class whose values does it hold
    ///     eeg. class Main -> Main.cfg
    /// All configs, provided you are using Save method are stored in configs folder
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Turns public fields into a serialized string
        /// </summary>
        /// <param name="what">
        ///     object whose fields you want to serialize
        ///     it is required that what extends Config class.
        /// </param>
        /// <returns>String containing serialized fields of the class</returns>
        public static string Serialize(object what)
        {
            StringBuilder sb = new StringBuilder();
            if (what.GetType().BaseType == typeof(Config))
            {
                foreach (FieldInfo fld in what.GetType().GetFields())
                {
                    if (fld.IsPublic && !fld.Name.StartsWith("_") && fld.IsStatic == false)
                    {
                        if (fld.GetValue(what).GetType() != typeof(string))
                        {
                            sb.AppendLine(string.Format("{0} = {1}", fld.Name, fld.GetValue(what).ToString()));
                        }
                        else
                        {
                            sb.AppendLine(string.Format("{0} = {1}", fld.Name, "\"" + fld.GetValue(what).ToString() + "\""));
                        }
                    }
                }
            }
            else
            {
                throw new Exception("A serialized object must be an instance of a class extending Config");
            }
            return sb.ToString();
        }
        /// <summary>
        /// Saves config of what
        /// </summary>
        /// <param name="what">instance of a class extending Config</param>
        public static void Save(object what)
        {
            StreamWriter str = File.CreateText(Path.Combine(Environment.CurrentDirectory, "configs", what.GetType().Name + ".cfg"));
            str.WriteLine(Serialize(what));
            str.Flush();
            str.Close();
        }

        public void Deserialize()
        {
            string[] strs = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "configs", this.GetType().Name + ".cfg"));
            Dictionary<string, object> fldlist = new Dictionary<string, object>();
            foreach(string str in strs)
            {
                string name = str.Replace(" = ", "=").Split('=')[0];
                string strvalue = string.Join("=", str.Replace(" = ", "=").Split('=').Skip(1).ToArray());
                object value = new object();
                switch (strvalue)
                {
                    case "True":
                        value = true;
                        break;
                    case "False":
                        value = false;
                        break;
                    default:
                        int num;
                        if (!int.TryParse(strvalue, out num))
                        {
                            if(strvalue.EndsWith("\"") && strvalue.StartsWith("\""))
                            {
                                value = strvalue.Substring(1, strvalue.Length - 2);
                            }
                        }
                        else
                        {
                            value = num;
                        }
                        break;
                }
                fldlist.Add(name, value);
            }
            try
            {
                foreach (KeyValuePair<string, object> pair in fldlist)
                {
                    this.GetType().GetField(pair.Key).SetValue(this, pair.Value);
                }
            }
            catch { }
        } 
    }

    public class Main : Config
    {
        public string codepath = "server";
        public string componentspath = "servercomponents";
        public int defaultport = 8080;
        public bool verbose = false;
        public int maxPOSTmb = 10;
    }
}