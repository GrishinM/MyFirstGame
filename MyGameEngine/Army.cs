﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGameEngine
{
    public class Army
    {
        private int Count => stacks.Count;
        public string Name { get; }
        private readonly Dictionary<string, BattleUnitsStack> stacks;

        public Dictionary<string, BattleUnitsStack> Stacks => new Dictionary<string, BattleUnitsStack>(stacks);

        public Army(string name)
        {
            stacks = new Dictionary<string, BattleUnitsStack>();
            Name = name;
        }

        public Army(string name, Dictionary<string, BattleUnitsStack> stacks): this(name)
        {
            this.stacks = stacks;
        }

        public bool Add(BattleUnitsStack stack)
        {
            if (Count > 5)
                return false;
            stacks.Add(stack.Name, stack);
            stack.Army = this;
            return true;
        }

        public bool Remove(string name)
        {
            if (!stacks.TryGetValue(name, out var stack))
                return false;
            stacks.Remove(name);
            stack.Army = null;
            return true;
        }

        public bool IsAlive()
        {
            foreach (var stack in stacks.Values)
                if (stack.IsAlive())
                    return true;
            return false;
        }

        public bool CanTurn()
        {
            foreach (var stack in stacks)
                if (stack.Value.CanTurn)
                    return true;
            return false;
        }
    }
}