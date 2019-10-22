﻿using System;

namespace MyGameEngine
{
    public class UnitsStack
    {
        public Army Army;
        public readonly string Name;
        public Unit Unit { get; }
        public int Count { get; }
        public bool CanTurn;
        //public float Initiative { get; set; }

        protected UnitsStack(Unit unit, int count, string name)
        {
            Unit = unit;
            Count = count;
            Name = name;
            Army = null;
            CanTurn = true;
            //Initiative = unit.Initiative;
        }

//        public bool IsAlive()
//        {
//            return Count > 0;
//        }
    }
}