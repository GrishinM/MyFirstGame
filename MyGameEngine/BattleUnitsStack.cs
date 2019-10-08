﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGameEngine
{
    public class BattleUnitsStack : UnitsStack
    {
        private readonly Random rand = new Random();
        public int CurrentHealth;
        public int CurrentCount;
        private bool CanCounterAttack;
        private SortedDictionary<TempMods, int> Mods { get; }

        public SortedDictionary<TempMods, int> SelfMods => new SortedDictionary<TempMods, int>(Mods);

        private int Attack
        {
            get
            {
                var x = unit.Attack;
                if (Mods.ContainsKey(TempMods.Desolator))
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
                var x = unit.Defence;
                if (Mods.ContainsKey(TempMods.DamageImmunity))
                {
                    return Int32.MaxValue;
                }

                if (Mods.ContainsKey(TempMods.SuperDefence))
                {
                    x += 50;
                }

                if (Mods.ContainsKey(TempMods.LowerDefence))
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
                var x = new Dmg(unit.Damage);
                if (Mods.ContainsKey(TempMods.DoubleDamage))
                {
                    x.Mul(2);
                }

                if (Mods.ContainsKey(TempMods.UpperDamage))
                {
                    x.Add(40);
                }

                return x;
            }
        }


        public BattleUnitsStack(Unit unit, int count, string name, SortedDictionary<TempMods, int> mods) : base(unit, count, name)
        {
            CurrentHealth = unit.Hitpoints;
            CurrentCount = count;
            Mods = mods;
            CanCounterAttack = true;
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

            if (!isCounter && Mods.ContainsKey(TempMods.Stunned))
                throw new MyException("Стек оглушен");

            if (!isCounter && Mods.ContainsKey(TempMods.Disarmed))
                throw new MyException("Стек не может атаковать");

            if (!enemy.IsAlive())
                throw new MyException("Защищающийся стек уже мертв");

            var lst = new List<(TempMods, int)>();
            if (enemy.Mods.ContainsKey(TempMods.Breaked))
            {
                foreach (var mod in enemy.SelfMods)
                {
                    if (!Interface.GetIsPositive(mod.Key))
                        continue;
                    lst.Add((mod.Key, mod.Value));
                    enemy.Mods.Remove(mod.Key);
                }
            }

            var enemyDefence = enemy.Defence;
            var selfAttack = Attack;
            var selfDamge = new Dmg(Damage);

            if (unit.HasAbility(Abilities.Dark) && enemy.unit.HasAbility(Abilities.Light) ||
                unit.HasAbility(Abilities.Light) && enemy.unit.HasAbility(Abilities.Dark))
                selfDamge.Mul(1.2);

            if (unit.HasAbility(Abilities.Headshot) && enemy.unit.Type != Types.Building)
            {
                if (rand.Next(0, 100) < 40)
                    selfDamge.Add(100);
            }

            if (unit.HasAbility(Abilities.FireDamage) && enemy.unit.HasAbility(Abilities.ImmuneToFire))
                selfDamge.Mul(0.5);

            if (unit.HasAbility(Abilities.FireDamage) && enemy.unit.Type == Types.Building)
                selfDamge.Mul(0);

            var dm = rand.Next(selfDamge.Item1(), selfDamge.Item2());
            var dmg = Convert.ToInt32((selfAttack > enemyDefence)
                ? CurrentCount * dm * (1 + 0.05 * (selfAttack - enemyDefence))
                : CurrentCount * dm / (1 + 0.05 * (enemyDefence - selfAttack)));

            var a = (enemy.CurrentCount - 1) * enemy.unit.Hitpoints + enemy.CurrentHealth;
            a -= dmg;
            if (a > 0)
            {
                if (a % enemy.unit.Hitpoints == 0)
                {
                    enemy.CurrentCount = a / enemy.unit.Hitpoints;
                    enemy.CurrentHealth = enemy.unit.Hitpoints;
                }
                else
                {
                    enemy.CurrentCount = a / enemy.unit.Hitpoints + 1;
                    enemy.CurrentHealth = a % enemy.unit.Hitpoints;
                }
            }
            else
            {
                enemy.CurrentCount = enemy.Mods.ContainsKey(TempMods.Immortal) ? 1 : 0;
                enemy.CurrentHealth = enemy.Mods.ContainsKey(TempMods.Immortal) ? 1 : 0;
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
                enemy.Mods.Add(m, c);
            if (!isCounter)
                CanTurn = false;

            return message;
        }

        public void Cast(Abilities ability, BattleUnitsStack target)
        {
            if (!CanTurn)
                throw new MyException("Стек уже ходил в этом раунде");

            if (Mods.ContainsKey(TempMods.Stunned))
                throw new MyException("Стек оглушен");

            if (Mods.ContainsKey(TempMods.Silenced))
                throw new MyException("На стеке молчание");

            if (!unit.HasAbility(ability))
                throw new MyException("У этого юнита нет такой способности");

            if (!Interface.GetIsActive(ability))
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
                    foreach (var mod in target.SelfMods.Keys)
                        if (Interface.GetIsPositive(mod))
                            target.Mods.Remove(mod);
                    break;
                default:
                    throw new MyException("Способность не применена");
            }

            CanTurn = false;
        }

        private void AddMod(TempMods mod, int count = -1)
        {
            if (Mods.ContainsKey(mod))
            {
                if (Mods[mod] != -1 && (count > Mods[mod] || count == -1))
                    Mods[mod] = count;
            }
            else
                Mods.Add(mod, count);
        }

        public void NextRound()
        {
            var ModsCopyKeys = Mods.Keys.ToList();
            foreach (var mod in ModsCopyKeys)
            {
                if (Mods[mod] > 1)
                    Mods[mod]--;
                else if (Mods[mod] == 1)
                    Mods.Remove(mod);
            }

            CanTurn = true;
            CanCounterAttack = true;
        }
    }
}