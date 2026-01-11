using ChallengeContainerTransport.Domain.Rules.Interfaces;
using ContainerChallenge.Domain;

namespace ChallengeContainerTransport.Domain.Rules
{
    public class ValuableMustBeEdgeRowRule : IPlacementRule
    {
        public bool CanPlace(ShipGrid ship, Position pos, Container container, out string error)
        {
            error = "";

            if (container.Type != ContainerType.Valuable &&
                container.Type != ContainerType.ValuableCoolable)
                return true;

            var lastRow = ship.Length - 1;

            if (pos.Row != 0 && pos.Row != lastRow) // index 0: rij 1 - index lastRow: laatste rij
            {
                error = "Valuable containers must be placed either in the front OR back row (edge row)";
                return false;
            }
            return true;
        }
    }
}