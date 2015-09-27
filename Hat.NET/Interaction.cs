using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET.Interaction
{
    public delegate void Hook(HttpProcessor processor, HandledEventArgs e);
    public class Interaction
    {
        /// <summary>
        /// String should be the extension of the file you wanna hook
        /// </summary>
        public static List<ValuePair<string, Hook>> 
    }

    public class ControlArgs
    {
    }
}
