﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Line_meleee_creep : Unit
    {
        public Line_meleee_creep() : base(Units.LINE_MELEE_CREEP, "Крип-мечник", Types.Creep, 550, 0, 2, (19, 23), 9,
            new HashSet<Abilities>{})
        {
        }
    }
}