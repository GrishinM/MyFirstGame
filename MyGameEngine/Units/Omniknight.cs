﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Omniknight : Unit
    {
        public Omniknight() : base(Units.OMNIKNIGHT, "Ревнитель Громобой", Types.Hero, 640, 0, 4, (53, 63), 21,
            new HashSet<Abilities> {Abilities.Light, Abilities.Heavenly_Grace})
        {
        }
    }
}