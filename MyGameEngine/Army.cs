﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGameEngine
{
    public class Army
    {
        private int Count => Stacks.Count;
        public string Name { get; }
        private readonly Dictionary<string, BattleUnitsStack> Stacks;

        public Dictionary<string, BattleUnitsStack> SelfStacks =>
            new Dictionary<string, BattleUnitsStack>(Stacks);

        public Army(string name)
        {
            Stacks = new Dictionary<string, BattleUnitsStack>();
            Name = name;
        }

        public Army(string name, Dictionary<string, BattleUnitsStack> stacks): this(name)
        {
            Stacks = stacks;
        }

        public bool Add(BattleUnitsStack stack, string name)
        {
            if (Count > 5)
                return false;
            Stacks.Add(name, stack);
            return true;
        }

        public bool Remove(string name)
        {
            if (!Stacks.TryGetValue(name, out var st))
                return false;
            Stacks.Remove(name);
            st.Army = null;
            return true;
        }

        public bool IsAlive()
        {
            foreach (var stack in Stacks.Values)
                if (stack.IsAlive())
                    return true;
            return false;
        }

        public bool CanTurn()
        {
            foreach (var stack in Stacks)
                if (stack.Value.CanTurn)
                    return true;
            return false;
        }
    }
}