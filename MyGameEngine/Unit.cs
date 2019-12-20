using System.Collections.Generic;

namespace MyGameEngine
{
    public class Unit
    {
        private readonly HashSet<Abilities> abilities;

        public IEnumerable<Abilities> Abilities => new HashSet<Abilities>(abilities);

        public string Id { get; }
        public string Name { get; }
        public Types Type { get; }
        public int Hitpoints { get; }
        public int Attack { get; }
        public int Defence { get; }
        public Dmg Damage { get; }
        public float Initiative { get; }

        public Unit(string id, string name, Types type, int hitpoints, int attack, int defence, (int, int) damage, float initiative, HashSet<Abilities> abilities)
        {
            Id = id;
            Name = name;
            Type = type;
            Hitpoints = hitpoints;
            Attack = attack;
            Defence = defence;
            Damage = new Dmg(damage);
            Initiative = initiative;
            this.abilities = abilities;
        }

        public bool HasAbility(Abilities x)
        {
            return abilities.Contains(x);
        }
    }
}