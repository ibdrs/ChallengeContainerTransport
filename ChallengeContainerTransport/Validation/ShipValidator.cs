using ChallengeContainerTransport.Domain.Rules.Interfaces;
using ContainerChallenge.Domain;

public class ShipValidator
{
    private readonly IReadOnlyList<IShipRule> _rules;

    public ShipValidator(IEnumerable<IShipRule> rules)
    {
        _rules = rules.ToList();
    }

    public bool Validate(ShipGrid ship, out List<string> errors)
    {
        errors = new List<string>();

        foreach (var rule in _rules)
        {
            if (!rule.IsValid(ship, out var error))
                errors.Add(error);
        }

        return errors.Count == 0;
    }
}
