namespace MyGameEngine
{
    public class ModsAttr : Attr
    {
        public bool IsPositive { get; }

        public ModsAttr(string description, bool isPositive): base(description)
        {
            IsPositive = isPositive;
        }
    }
}