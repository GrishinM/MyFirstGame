﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Tower : Unit
    {
        public Tower() : base(Units.TOWER, "Башня", Types.Building, 1800, 0, 12, (100, 120), 10,
            new HashSet<Abilities> {})
        {
        }
    }
}