using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyGameEngine
{
    public class BattleUnitsStack : UnitsStack
    {
        private readonly Random rand = new Random();
        public int CurrentHealth { get; private set; }
        public int CurrentCount { get; private set; }
        private bool CanCounterAttack;
        private readonly SortedDictionary<TempMods, int> mods;

        public SortedDictionary<TempMods, int> Mods => new SortedDictionary<TempMods, int>(mods);

        private int Attack
        {
            get
            {
                var x = Unit.Attack;
                if (mods.ContainsKey(TempMods.Desolator))
                {
                    x += 7;
                }

                return x;
            }
        }

        private int Defence
        {
            get
            {
                var x = Unit.Defence;
                if (mods.ContainsKey(TempMods.DamageImmunity))
                {
                    return Int32.MaxValue;
                }

                if (mods.ContainsKey(TempMods.Defence))
                {
                    x += 5;
                }

                if (mods.ContainsKey(TempMods.SuperDefence))
                {
                    x += 10;
                }

                if (mods.ContainsKey(TempMods.LowerDefence))
                {
                    x -= 10;
                }

                return x;
            }
        }

        private Dmg Damage
        {
            get
            {
                var x = new Dmg(Unit.Damage);
                if (mods.ContainsKey(TempMods.DoubleDamage))
                {
                    x.Mul(2);
                }

                if (mods.ContainsKey(TempMods.UpperDamage))
                {
                    x.Add(40);
                }

                return x;
            }
        }

        private float initiative;

        public float Initiative
        {
            get
            {
                var x = initiative;
                if (mods.ContainsKey(TempMods.Hasted))
                {
                    x *= (x > 0) ? 1.3f : 0.7f;
                }

                return x;
            }
            set => initiative = value;
        }


        public BattleUnitsStack(Unit unit, int count, string name) : base(unit, count, name)
        {
            mods=new SortedDictionary<TempMods, int>();
            CurrentHealth = unit.Hitpoints;
            CurrentCount = count;
            CanCounterAttack = true;
            initiative = unit.Initiative;
        }

        public BattleUnitsStack(Unit unit, int count, string name, int currentCount, int currentHealth, SortedDictionary<TempMods, int> mods) : this(unit, count, name)
        {
            CurrentHealth = currentHealth;
            CurrentCount = currentCount;
            this.mods = mods;
        }

        public bool IsAlive()
        {
            return CurrentCount > 0;
        }

        public (int, int) Fight(BattleUnitsStack enemy, bool isCounter = false)
        {
            var message = (-1, -1);
            if (!IsAlive() && !isCounter)
                throw new MyException("Атакующий стек мертв");

            if (!isCounter && !CanTurn)
                throw new MyException("Стек уже ходил в этом раунде");

            if (!isCounter && mods.ContainsKey(TempMods.Stunned))
                throw new MyException("Стек оглушен");

            if (!isCounter && mods.ContainsKey(TempMods.Disarmed))
                throw new MyException("Стек не может атаковать");

            if (!enemy.IsAlive())
                throw new MyException("Защищающийся стек уже мертв");

            var lst = new List<(TempMods, int)>();
            if (enemy.mods.ContainsKey(TempMods.Breaked))
            {
                foreach (var mod in enemy.Mods)
                {
                    if (!GetIsPositive(mod.Key))
                        continue;
                    lst.Add((mod.Key, mod.Value));
                    enemy.mods.Remove(mod.Key);
                }
            }

            var enemyDefence = enemy.Defence;
            var selfAttack = Attack;
            var selfDamge = new Dmg(Damage);

            if (Unit.HasAbility(Abilities.Dark) && enemy.Unit.HasAbility(Abilities.Light) ||
                Unit.HasAbility(Abilities.Light) && enemy.Unit.HasAbility(Abilities.Dark))
                selfDamge.Mul(1.2);

            if (Unit.HasAbility(Abilities.Headshot) && enemy.Unit.Type != Types.Building)
            {
                if (rand.Next(0, 100) < 40)
                    selfDamge.Add(100);
            }

            if (Unit.HasAbility(Abilities.FireDamage) && enemy.Unit.HasAbility(Abilities.ImmuneToFire))
                selfDamge.Mul(0.5);

            if (Unit.HasAbility(Abilities.FireDamage) && enemy.Unit.Type == Types.Building)
                selfDamge.Mul(0);

            var dm = rand.Next(selfDamge.Item1(), selfDamge.Item2() + 1);
            var dmg = Convert.ToInt32((selfAttack > enemyDefence)
                ? CurrentCount * dm * (1 + 0.05 * (selfAttack - enemyDefence))
                : CurrentCount * dm / (1 + 0.05 * (enemyDefence - selfAttack)));

            var a = (enemy.CurrentCount - 1) * enemy.Unit.Hitpoints + enemy.CurrentHealth;
            a -= dmg;
            if (a > 0)
            {
                if (a % enemy.Unit.Hitpoints == 0)
                {
                    enemy.CurrentCount = a / enemy.Unit.Hitpoints;
                    enemy.CurrentHealth = enemy.Unit.Hitpoints;
                }
                else
                {
                    enemy.CurrentCount = a / enemy.Unit.Hitpoints + 1;
                    enemy.CurrentHealth = a % enemy.Unit.Hitpoints;
                }
            }
            else
            {
                enemy.CurrentCount = enemy.mods.ContainsKey(TempMods.Immortal) ? 1 : 0;
                enemy.CurrentHealth = enemy.mods.ContainsKey(TempMods.Immortal) ? 1 : 0;
            }

//            message = string.Format("\n{6}:\nСтек {0} нанес стеку {1} {2} единиц урона\nСтек {3}: количество - {4}, здоровье последнего - {5}", Name, enemy.Name, dmg, enemy.Name,
//                enemy.CurrentCount, enemy.CurrentHealth, isCounter ? "Контратака" : "Результат");
            message.Item1 = dmg;
            if (!isCounter && enemy.CanCounterAttack)
            {
                message.Item2 = (enemy.Fight(this, true)).Item1;
                enemy.CanCounterAttack = false;
            }

            foreach (var (m, c) in lst)
                enemy.mods.Add(m, c);
            if (!isCounter)
                CanTurn = false;

            return message;
        }

        public void Cast(Abilities ability, BattleUnitsStack target)
        {
            if (!CanTurn)
                throw new MyException("Стек уже ходил в этом раунде");

            if (mods.ContainsKey(TempMods.Stunned))
                throw new MyException("Стек оглушен");

            if (mods.ContainsKey(TempMods.Silenced))
                throw new MyException("На стеке молчание");

            if (!Unit.HasAbility(ability))
                throw new MyException("У этого юнита нет такой способности");

            if (!GetIsActive(ability))
                throw new MyException("Эта способность пассивная");

            switch (ability)
            {
                case Abilities.Curse:
                    target.AddMod(TempMods.LowerDefence, 1);
                    break;
                case Abilities.Break:
                    target.AddMod(TempMods.Breaked, 1);
                    break;
                case Abilities.Disarm:
                    target.AddMod(TempMods.Disarmed, 1);
                    break;
                case Abilities.Silence:
                    target.AddMod(TempMods.Silenced, 1);
                    break;
                case Abilities.Stun:
                    target.AddMod(TempMods.Stunned, 1);
                    break;
                case Abilities.Shield:
                    target.AddMod(TempMods.SuperDefence, 1);
                    break;
                case Abilities.Heavenly_Grace:
                    foreach (var mod in target.Mods.Keys)
                        if (GetIsPositive(mod))
                            target.mods.Remove(mod);
                    break;
                case Abilities.Haste:
                    target.AddMod(TempMods.Hasted, 1);
                    break;
                default:
                    throw new MyException("Способность не применена");
            }

            CanTurn = false;
        }

        public void Pass()
        {
            CanTurn = false;
        }

        public void Defend()
        {
            AddMod(TempMods.Defence, 1);
            CanTurn = false;
        }

        private void AddMod(TempMods mod, int count = -1)
        {
            if (mods.ContainsKey(mod))
            {
                if (mods[mod] != -1 && (count > mods[mod] || count == -1))
                    mods[mod] = count;
            }
            else
                mods.Add(mod, count);
        }

        public void NextRound()
        {
            var ModsCopyKeys = mods.Keys.ToList();
            foreach (var mod in ModsCopyKeys)
            {
                if (mods[mod] > 1)
                    mods[mod]--;
                else if (mods[mod] == 1)
                    mods.Remove(mod);
            }

            CanTurn = true;
            CanCounterAttack = true;
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
    }
}