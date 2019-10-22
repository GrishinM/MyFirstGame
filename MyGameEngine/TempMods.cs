namespace MyGameEngine
{
    public enum TempMods
    {
        [ModsAttr("Защита увеличена на 10", true)] SuperDefence,
        [ModsAttr("Защита понижена на 10", false)] LowerDefence,
        [ModsAttr("Невосприимчивость к лобому урону", true)] DamageImmunity,
        [ModsAttr("Урон увеличен на 40", true)] UpperDamage,
        [ModsAttr("Двойной урон", true)] DoubleDamage,
        [ModsAttr("Атака увеличена на 7", true)] Desolator,
        [ModsAttr("Оглушение", true)] Stunned,
        [ModsAttr("Бессилие (не может атаковать)", false)] Disarmed,
        [ModsAttr("Заглушение (не может использовать способности)", false)] Silenced,
        [ModsAttr("Положительные моды отключены", false)] Breaked,
        [ModsAttr("Бессмертие (здоровье не опускается ниже 1)", true)] Immortal,
        [ModsAttr("Защита увеличена на 5", true)] Defence,
        [ModsAttr("Инициатива увеличена на 30%", true)] Hasted
    }
}