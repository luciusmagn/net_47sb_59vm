/*
This class is modification of TerrariaPlugin.cs from TerrariaServerApi
Copyright(C) 2011-2015 Nyx Studios(fka.The TShock Team)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program.If not, see<http://www.gnu.org/licenses/>.
*/

using System;

namespace net_47sb_59vm
{
    public abstract class HatComponent : IDisposable
    {
        public virtual string Name
        {
            get { return "None"; }
        }
        public virtual Version Version
        {
            get { return new Version(1, 0); }
        }
        public virtual string Author
        {
            get { return "None"; }
        }
        public virtual string Description
        {
            get { return "None"; }
        }
        public int Order { get; set; }

        protected HatComponent() { this.Order = 1; }
        ~HatComponent() { Dispose(false); }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) { }

        public abstract void Initialize();
    }
}
