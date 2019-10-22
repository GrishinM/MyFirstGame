﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Sven : Unit
    {
        public Sven() : base(Units.SVEN, "Мятежный Рыцарь", Types.Hero, 640, 0, 4, (63, 65), 23,
            new HashSet<Abilities> {Abilities.Stun})
        {
        }
    }
}