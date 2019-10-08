﻿using System;

namespace MyGameEngine
{
    public class UnitsStack
    {
        public string Army;
        public readonly string Name;
        public Unit unit { get; }
        public int Count { get; }
        public bool CanTurn;
        public float Initiative { get; set; }

        protected UnitsStack(Unit unit, int count, string name)
        {
            this.unit = unit;
            Count = count;
            Name = name;
            Army = null;
            CanTurn = true;
            Initiative = unit.Initiative;
        }

//        public bool IsAlive()
//        {
//            return Count > 0;
//        }
    }
}