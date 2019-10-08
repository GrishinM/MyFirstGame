using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyGameEngine
{
    public abstract class Interface
    {
        protected static readonly SortedDictionary<string, Army> armys = new SortedDictionary<string, Army>();
        protected static readonly SortedDictionary<string, BattleUnitsStack> stacks = new SortedDictionary<string, BattleUnitsStack>();

        protected static readonly Unit[] units =
        {
            new Abaddon(), new Axe(), new Doom(), new Drow_Ranger(), new Huskar(), new Juggernaut(), new Keeper_of_the_light(), new Lich(), new LineMeleeeCreep(),
            new Omniknight(), new Sniper(), new Sven(), new Tower()
        };

        protected int round;
        protected int delta;
        protected int players;
        protected bool GAME;
        protected bool game;
        protected bool menu;
        protected bool success;
        protected int step;
        protected int player;

        public abstract void Start();
        protected abstract void MainMenu();
        protected abstract void PrintRound(string round);
        protected abstract void PrintUnits();
        protected abstract void PrintUnit(Unit unit);
        protected abstract void CreateStack();
        protected abstract void PrintStacks();
        protected abstract void PrintStack(BattleUnitsStack stack);
        protected abstract void PrintArmys();
        protected abstract void PrintArmy(Army army);
        protected abstract void AddStackToArmy();
        protected abstract void DeleteStackFromArmy();
        protected abstract void PrintAbilities();
        protected abstract void PrintTempMods();
        protected abstract void ChangeInitiative();
        protected abstract void StartRound();
        protected abstract bool Attack(BattleUnitsStack attacker);
        protected abstract bool Cast(BattleUnitsStack caster);
        protected abstract void Pass(BattleUnitsStack stack);
        protected abstract void IsWinner();

        protected static string GetDescription(object enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()?
                .GetCustomAttribute<Attr>()?
                .Description;
        }

        public static bool GetIsPositive(object enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<ModsAttr>()
                .IsPositive;
        }

        public static bool GetIsActive(object enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<AbilitiesAttr>()
                .IsActive;
        }

        protected static int GetAliveArmy(out string a)
        {
            var c = 0;
            a = null;
            foreach (var army in armys)
                if (army.Value.IsAlive())
                {
                    a = army.Key;
                    c++;
                }

            return c;
        }

        protected static bool IsTurn()
        {
            foreach (var stack in stacks.Values)
                if (stack.CanTurn && stack.IsAlive())
                    return true;
            return false;
        }

        protected static void NextRound()
        {
            foreach (var i in stacks.Values)
                i.NextRound();
        }
    }
}