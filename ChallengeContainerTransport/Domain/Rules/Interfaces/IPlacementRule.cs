using ContainerChallenge.Domain;

namespace ChallengeContainerTransport.Domain.Rules.Interfaces
{
    public interface IPlacementRule
    {
        bool CanPlace(ShipGrid ship, Position pos, Container container, out string error);
    }
}
