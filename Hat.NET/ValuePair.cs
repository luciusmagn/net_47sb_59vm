using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET
{
    public class ValuePair<Left, Right>
    {
        public Left LeftValue;
        public Right RightValue;

        public ValuePair(Left l, Right r)
        {
            SetLeft(l);
            SetRight(r);
        }

        public void SetRight(Right r)
        {
            if (r != null)
                RightValue = r;
            else
                throw new System.NullReferenceException();
        }

        public void SetLeft(Left l)
        {
            if (l != null)
                LeftValue = l;
            else
                throw new System.NullReferenceException();
        }
    }
}
