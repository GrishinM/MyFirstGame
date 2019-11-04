namespace MyGameEngine
{
    public class AbilitiesAttr : Attr
    {
        public bool IsActive { get; }

        public AbilitiesAttr(string description, bool isActive) : base(description)
        {
            IsActive = isActive;
        }
    }
}