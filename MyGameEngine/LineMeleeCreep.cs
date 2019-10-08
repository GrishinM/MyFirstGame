﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class LineMeleeeCreep : Unit
    {
        public LineMeleeeCreep() : base(Units.LINEMELEECREEP, "Крип-мечник", Types.Creep, 550, 0, 2, (19, 23), 2,
            new HashSet<Abilities>{})
        {
        }
    }
}