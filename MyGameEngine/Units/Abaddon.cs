using System.Collections.Generic;

namespace MyGameEngine
{
    public class Abaddon : Unit
    {
        public Abaddon() : base(Units.ABADDON, "Абаддон", Types.Hero, 660, 0, 3, (55, 65), 11,
            new HashSet<Abilities> {Abilities.Dark})
        {
        }
    }
}