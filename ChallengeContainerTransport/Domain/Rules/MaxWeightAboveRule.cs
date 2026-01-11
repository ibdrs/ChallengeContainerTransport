using ChallengeContainerTransport.Domain.Rules.Interfaces;
using ContainerChallenge.Domain;

namespace ChallengeContainerTransport.Domain.Rules
{
    public class MaxWeightAboveRule : IPlacementRule
    {
        private const int MaxAbove = 120;

        public bool CanPlace(ShipGrid ship, Position pos, Container container, out string error)
        {
            error = "";
            var stack = ship.GetStack(pos);

            // We simuleren “na plaatsing”:
            var weights = stack.Containers.Select(c => c.WeightTons).ToList();
            weights.Add(container.WeightTons);

            // Voor elke container in de stapel: gewicht erboven <= 120
            // bottom->top in jouw Stack
            for (int i = 0; i < weights.Count; i++)
            {
                int above = 0;
                for (int j = i + 1; j < weights.Count; j++)
                    above += weights[j]; // above = som van alle containers boven aantal containers

                if (above > MaxAbove)
                {
                    error = $"Stack[{pos.Row},{pos.Col}] violates max weight-above constraint (120 tons).";
                    return false;
                }
            }

            return true;
        }
    }
}
