using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hat.NET
{
    public class ValuePair<Left, Right>
    {
        Left leftValue;
        Right rightValue;

        public Left LeftValue
        {
            get { return leftValue; }
            set { leftValue = value; }
        }
        public Right RightValue
        {
            get { return rightValue; }
            set { rightValue = value; }
        }
        public ValuePair(Left l, Right r)
        {
            SetLeft(l);
            SetRight(r);
        }

        public void SetRight(Right r)
        {
            if (r != null)
            {
                rightValue = r;
            }
            else
            {
                throw new System.NullReferenceException();
            }
        }

        public void SetLeft(Left l)
        {
            if (l != null)
            {
                leftValue = l;
            }
            else
            {
                throw new System.NullReferenceException();
            }
        }
    }
}
