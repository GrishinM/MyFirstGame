namespace MyGameEngine
{
    public enum Abilities
    {
        [AbilitiesAttr("Силы тьмы (наносит на 20% больше урона по тьме)", false)] Dark,
        [AbilitiesAttr("Силы света (наносит на 20% больше урона по свету)", false)] Light,
        [AbilitiesAttr("Сопротивление огню 50%", false)] ImmuneToFire,
        [AbilitiesAttr("Стрелок (с шансом 40% наносит 100 дополнительного урона)", false)] Headshot,
        [AbilitiesAttr("Огненный урон", false)] FireDamage,
        [AbilitiesAttr("Отключает положительные эффекты цели на 1 ход", true)] Break,
        [AbilitiesAttr("Снимает отрицательные эффекты с цели", true)] Heavenly_Grace,
        [AbilitiesAttr("Уменьшает защиту цели на 10 на 1 ход", true)] Curse,
        [AbilitiesAttr("Увеличивает защиту цели на 10 на 1 ход", true)] Shield,
        [AbilitiesAttr("Запрещает цели атаковать на 1 ход", true)] Disarm,
        [AbilitiesAttr("Запрещает цели использовать способности на 1 ход", true)] Silence,
        [AbilitiesAttr("Оглушает цель на 1 ход", true)] Stun,
        [AbilitiesAttr("Увеличивает инициативу цели на 30%", true)] Haste
    }
}