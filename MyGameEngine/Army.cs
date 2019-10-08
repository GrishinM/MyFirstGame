﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGameEngine
{
    public class Army
    {
        private int Count;
        private readonly Dictionary<string, BattleUnitsStack> Stacks;

        public Dictionary<string, BattleUnitsStack> SelfStacks =>
            new Dictionary<string, BattleUnitsStack>(Stacks);

        public Army()
        {
            Stacks = new Dictionary<string, BattleUnitsStack>();
            Count = 0;
        }

        public bool Add(BattleUnitsStack stack, string name)
        {
            if (Count > 5)
                return false;
            Count++;
            Stacks.Add(name, stack);
            Sort();
            return true;
        }

        public bool Remove(string name)
        {
            if (!Stacks.TryGetValue(name, out var st))
                return false;
            Count--;
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

        public void Sort()
        {
            var b = Stacks.ToList();
            b.Sort(delegate(KeyValuePair<string, BattleUnitsStack> pair, KeyValuePair<string, BattleUnitsStack> valuePair)
            {
                return (pair.Value.Initiative == valuePair.Value.Initiative ? 0 : (pair.Value.Initiative < valuePair.Value.Initiative ? 1 : -1));
            });
            Stacks.Clear();
            foreach (var i in b)
                Stacks.Add(i.Key, i.Value);
        }

        public BattleUnitsStack GetStack()
        {
            foreach (var i in Stacks.Values)
                if (i.CanTurn && i.IsAlive())
                    return i;
            return null;
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