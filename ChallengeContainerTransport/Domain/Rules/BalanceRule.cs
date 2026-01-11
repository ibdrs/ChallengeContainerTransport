using ChallengeContainerTransport.Domain.Rules.Interfaces;
using ContainerChallenge.Domain;

namespace ChallengeContainerTransport.Domain.Rules
{
    public class BalanceRule : IShipRule
    {
        public bool IsValid(ShipGrid ship, out string error)
        {
            error = "";
            int total = ship.TotalWeightTons();

            if (total == 0)
            {
                error = "Ship has no load.";
                return false;
            }

            var (left, right) = ship.GetHalfWeightsTons();
            int diff = Math.Abs(left - right);

            if (diff > 0.2 * total)
            {
                error = $"Balance violation: left={left} right={right} diff={diff}.";
                return false;
            }

            return true;
        }
    }
}