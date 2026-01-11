using ChallengeContainerTransport.Domain.Rules.Interfaces;
using ContainerChallenge.Domain;

namespace ChallengeContainerTransport.Domain.Rules
{
    public class CoolableMustBeFrontRowRule : IPlacementRule
    {
        public bool CanPlace(ShipGrid ship, Position pos, Container container, out string error)
        {
            error = "";

            if (container.Type != ContainerType.Coolable &&
            container.Type != ContainerType.ValuableCoolable)
                return true;

            if (pos.Row != 0) // index 0: rij 1
            {
                error = "Coolable containers must be placed in the front row (row 1).";
                return false;
            }
            return true;
        }
    }
}