using ChallengeContainerTransport.Domain.Rules.Interfaces;
using ContainerChallenge.Domain;

public class NoContainerAboveValuableRule : IPlacementRule
{
    public bool CanPlace(ShipGrid ship, Position pos, Container container, out string error)
    {
        error = "";
        var stack = ship.GetStack(pos);

        var top = stack.Top();
        if (top == null) return true;

        if (top.Type == ContainerType.Valuable || top.Type == ContainerType.ValuableCoolable)
        {
            error = "Cannot place a container on top of a valuable container.";
            return false;
        }

        return true;
    }
}
