﻿using System.Collections.Generic;

namespace MyGameEngine
{
    public class Keeper_of_the_light : Unit
    {
        public Keeper_of_the_light() : base(Units.KEEPER_OF_THE_LIGHT, "Эзалор", Types.Hero, 520, 0, 1, (43, 50), 0,
            new HashSet<Abilities> {Abilities.Light, Abilities.FireDamage})
        {
        }
    }
}