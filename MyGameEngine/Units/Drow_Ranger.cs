﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Drow_Ranger : Unit
    {
        public Drow_Ranger() : base(Units.DROW_RANGER, "Траксекс", Types.Hero, 560, 0, 2, (49, 60), 14,
            new HashSet<Abilities> {Abilities.Silence})
        {
        }
    }
}