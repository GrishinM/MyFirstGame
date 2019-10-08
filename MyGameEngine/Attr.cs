namespace MyGameEngine
{
    public class Attr : System.Attribute
    {
        public string Description { get; }

        protected Attr(string description)
        {
            Description = description;
        }
    }
}