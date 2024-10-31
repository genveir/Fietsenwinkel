namespace Fietsenwinkel.Shared.Results;

public interface ICombinable<TCombinableType> where TCombinableType : ICombinable<TCombinableType>
{
    /// <summary>
    /// The neutral element, when combined with any other element, should result in the other element
    /// </summary>
    /// <returns>The neutral element</returns>
    abstract static TCombinableType GetNeutral();

    /// <summary>
    /// Combine two entities into one. The new can, but does not have to be, one of the original entities
    /// </summary>
    /// <param name="other">The entity that this entity will combine with</param>
    /// <returns>The combined entitiy</returns>
    TCombinableType Combine(TCombinableType other);
}