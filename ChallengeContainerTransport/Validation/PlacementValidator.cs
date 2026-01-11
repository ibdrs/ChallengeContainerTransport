using ChallengeContainerTransport.Domain.Rules.Interfaces;
using ContainerChallenge.Domain;

public class PlacementValidator
{
    private readonly IReadOnlyList<IPlacementRule> _rules;

    public PlacementValidator(IEnumerable<IPlacementRule> rules)
    {
        _rules = rules.ToList();
    }

    public bool CanPlace(ShipGrid ship, Position pos, Container container, out List<string> errors)
    {
        errors = new List<string>();

        foreach (var rule in _rules)
        {
            if (!rule.CanPlace(ship, pos, container, out var error))
                errors.Add(error);
        }

        return errors.Count == 0;
    }
}
