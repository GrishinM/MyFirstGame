using System.Collections.Generic;

namespace MyGameEngine
{
    public class Dark_Seer : Unit
    {
        public Dark_Seer() : base(Units.DARK_SEER, "Иш'Кафэль", Types.Hero, 600, 0, 3, (54, 60), 20,
            new HashSet<Abilities> {Abilities.Haste})
        {
        }
    }
}