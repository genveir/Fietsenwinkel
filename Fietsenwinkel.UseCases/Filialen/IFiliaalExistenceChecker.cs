using Fietsenwinkel.Domain.Filialen.Entities;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Filialen;

public interface IFiliaalExistenceChecker
{
    Task<bool> Exists(FiliaalId filiaalId);
}