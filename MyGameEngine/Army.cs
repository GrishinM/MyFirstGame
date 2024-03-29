﻿using System.Collections.Generic;
using System.Linq;


namespace MyGameEngine
{
    public class Army
    {
        public int Count => stacks.Count;
        public string Name { get; }
        private readonly Dictionary<string, BattleUnitsStack> stacks;

        public IEnumerable<BattleUnitsStack> Stacks => new List<BattleUnitsStack>(stacks.Values);

        public Army(string name)
        {
            stacks = new Dictionary<string, BattleUnitsStack>();
            Name = name;
        }

        public Army(string name, Dictionary<string, BattleUnitsStack> stacks) : this(name)
        {
            this.stacks = stacks;
        }

        public void Add(BattleUnitsStack stack)
        {
            if (Count == 6)
            {
                throw new MyException("Превышено максимальное количество стеков");
            }

            if (stacks.ContainsKey(stack.Name))
            {
                throw new MyException("В этой армии уже есть стек с таким названием");
            }

            stacks.Add(stack.Name, stack);
            stack.Army = this;
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
            return stacks.Values.Any(stack => stack.IsAlive());
        }

        public bool CanTurn()
        {
            return stacks.Values.Any(stack => stack.CanTurn);
        }
    }
}