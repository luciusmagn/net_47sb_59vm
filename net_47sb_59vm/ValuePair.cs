namespace net_47sb_59vm
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
