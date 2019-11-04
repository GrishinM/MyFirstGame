using System;

namespace MyGameEngine
{
    public class Attr : Attribute
    {
        public string Description { get; }

        protected Attr(string description)
        {
            Description = description;
        }
    }
}