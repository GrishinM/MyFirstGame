using System.Collections.Generic;

namespace MyGameEngine
{
    public class Unit
    {
        private readonly HashSet<Abilities> abilitieses;

        public IEnumerable<Abilities> SelfAbilities => new HashSet<Abilities>(abilitieses);

        public Units Id { get; }
        public string Name { get; }
        public Types Type { get; }
        public int Hitpoints { get; }
        public int Attack { get; }
        public int Defence { get; }
        public Dmg Damage { get; }
        public float Initiative { get; }

        public Unit(Units id, string name, Types type, int hitpoints, int attack, int defence, (int, int) damage, float initiative, HashSet<Abilities> abilitieses)
        {
            Id = id;
            Name = name;
            Type = type;
            Hitpoints = hitpoints;
            Attack = attack;
            Defence = defence;
            Damage = new Dmg(damage);
            Initiative = initiative;
            this.abilitieses = abilitieses;
        }

        public bool HasAbility(Abilities x)
        {
            return abilitieses.Contains(x);
        }
    }
}