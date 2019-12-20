using System.Collections.Generic;
using System.Linq;
using PriorityQueue;

namespace MyGameEngine
{
    public class Scale
    {
        private readonly PriorityQueue<BattleUnitsStack> stacks;

        public Scale(IEnumerable<BattleUnitsStack> stacks)
        {
            this.stacks = new PriorityQueue<BattleUnitsStack>();
            foreach (var stack in stacks.Where(stack => stack.CanTurn && stack.IsAlive() && stack.Army != null))
            {
                this.stacks.Add(stack.Initiative, stack);
            }
        }

        public BattleUnitsStack GetStack()
        {
            return stacks.Get();
        }

        public void ch(BattleUnitsStack stack)
        {
            if (stack.Initiative < 0)
            {
                throw new MyException("Нельзя больше 1 раза за ход");
            }

            stack.Initiative = -stack.Unit.Initiative;
            stacks.Add(stack.Initiative, stack);
        }

        public IEnumerable<BattleUnitsStack> Stacks => stacks.Values();
    }
}