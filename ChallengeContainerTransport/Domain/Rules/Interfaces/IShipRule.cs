using ContainerChallenge.Domain;

namespace ChallengeContainerTransport.Domain.Rules.Interfaces
{
    public interface IShipRule
    {
        bool IsValid(ShipGrid ship, out string error);
    }
}
