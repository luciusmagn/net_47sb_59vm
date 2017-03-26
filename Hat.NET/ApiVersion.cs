/*
This class is part of modification of TerrariaServerApi
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET
{
    public class ApiVersion
    {
        /// <summary>
        /// major version
        /// </summary>
        public static int Major = 0;
        /// <summary>
        /// minor version
        /// </summary>
        public static int Minor = 9;

    }
    /// <summary>
    /// This is the ApiVersion Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ApiVersionAttribute : Attribute
    {
        public Version ApiVersion;
        public ApiVersionAttribute(Version version) { this.ApiVersion = version; }
        public ApiVersionAttribute(int major, int minor) : this(new Version(major, minor)) { }
    }
}
