﻿using System.Collections.Generic;
using System.Linq;
using PriorityQueue;

namespace MyGameEngine
{
    public class Scale
    {
        private PriorityQueue<BattleUnitsStack> Stacks { get; }

        public Scale(IEnumerable<BattleUnitsStack> stacks)
        {
            Stacks = new PriorityQueue<BattleUnitsStack>();
            foreach (var stack in stacks.Where(stack => stack.CanTurn && stack.IsAlive() && stack.Army != null))
            {
                Stacks.Add(stack.Initiative, stack);
            }
        }

        public BattleUnitsStack GetStack()
        {
            return Stacks.Get();
        }

        public void ch(BattleUnitsStack stack)
        {
            if (stack.Initiative < 0)
            {
                throw new MyException("Нельзя больше 1 раза за ход");
            }

            stack.Initiative = -stack.Unit.Initiative;
            Stacks.Add(stack.Initiative, stack);
        }

        public IEnumerable<BattleUnitsStack> Values()
        {
            return Stacks.Values();
        }
    }
}