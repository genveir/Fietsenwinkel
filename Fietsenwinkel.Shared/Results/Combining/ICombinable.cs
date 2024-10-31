namespace Fietsenwinkel.Shared.Results;

public interface ICombinable<TCombinableType> where TCombinableType : ICombinable<TCombinableType>
{
    abstract static TCombinableType GetEmpty();

    TCombinableType Combine(TCombinableType combinable);
}
