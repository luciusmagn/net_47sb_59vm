using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using master = Hat.NET;

namespace CSScriptSupport
{
    [master.ApiVersion(0,8)]
    public class CSScriptSupport : master.HatComponent
    {
        public override string Author
        {
            get { return "Lukáš Hozda / magnusi"; }
        }
        public override string Description
        {
            get { return "Provides support for component-like plugins"; }
        }
        public override string Name
        {
            get { return "CSScript Support"; }
        }
        public override Version Version
        {
            get { return new Version(1,0); }
        }
        public override void Initialize()
        {

        }
    }
}
