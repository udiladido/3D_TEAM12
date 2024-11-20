public interface IStatHandler
{
    void AddStatModifier(params StatModifier[] modifiers);
    void RemoveStatModifier(params StatModifier[] modifier);
    void UpdateStatModifier();
}