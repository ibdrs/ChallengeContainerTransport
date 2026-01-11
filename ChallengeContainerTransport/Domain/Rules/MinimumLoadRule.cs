using ChallengeContainerTransport.Domain.Rules.Interfaces;
using ContainerChallenge.Domain;

namespace ChallengeContainerTransport.Domain.Rules
{
    public class MinimumLoadRule : IShipRule
    {
        private const int MaxStackHeight = 8;
        public bool IsValid(ShipGrid ship, out string error)
        {
            error = "";
            int total = ship.TotalWeightTons();

            int maxCapacity = ship.Length * ship.Width * MaxStackHeight * 30;
            if (total < 0.5 * maxCapacity) return false; 
            error = $"Minimum load violation: total={total}, required>={0.5 * maxCapacity:0.##} (max={maxCapacity}).";

            return true;
        }
    }
}