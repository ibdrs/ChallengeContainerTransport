using ContainerChallenge.Domain;
using ContainerChallenge.Validation;

namespace ContainerChallenge.Algorithm;

public class PlacementAlgorithm
{
    private readonly ConstraintsValidator _validator;

    public PlacementAlgorithm(ConstraintsValidator validator)
    {
        _validator = validator;
    }

    public ShipGrid GenerateLayout(PlacementRequest request)
    {
        var ship = new ShipGrid(request.Length, request.Width);

        // 1) Create containers in priority order
        var containers = new List<Container>();
        containers.AddRange(Make(ContainerType.ValuableCoolable, request.ValuableCoolableCount, request.ValuableCoolableWeightTons));
        containers.AddRange(Make(ContainerType.Coolable, request.CoolableCount, request.CoolableWeightTons));
        containers.AddRange(Make(ContainerType.Normal, request.NormalCount, request.NormalWeightTons));
        containers.AddRange(Make(ContainerType.Valuable, request.ValuableCount, request.ValuableWeightTons)); // last => ends up on top

        // 2) Place each container in the first valid spot
        foreach (var container in containers)
        {
            if (!TryPlace(ship, container))
                throw new InvalidOperationException($"No valid place for {container.Type} ({container.WeightTons}t).");
        }

        // 3) Final validation (balance + min load + valuables on top)
        var final = _validator.ValidateFinal(ship);
        if (!final.IsValid)
            throw new InvalidOperationException($"No valid solution found: {final.Error}");

        return ship;
    }

    private static List<Container> Make(ContainerType type, int count, int weight)
    {
        var list = new List<Container>(count);
        for (int i = 0; i < count; i++)
            list.Add(new Container(type, weight));
        return list;
    }

    private static int[] GetBalancedColumns(int width)
    {
        return width switch
        {
            1 => new[] { 0 },
            2 => new[] { 0, 1 },
            3 => new[] { 1, 0, 2 },
            4 => new[] { 1, 2, 0, 3 },
            5 => new[] { 2, 1, 3, 0, 4 },
            _ => Enumerable.Range(0, width).ToArray() // fallback
        };
    }

    private bool TryPlace(ShipGrid ship, Container container)
    {
        // Build a list of all positions we want to TRY, in a ship-like order
        var positions = new List<Position>();

        // If coolable: only front row
        if (container.IsCoolable)
        {
            foreach (int c in GetBalancedColumns(ship.Width))
                positions.Add(new Position(0, c));
        }
        else
        {
            // For others: go row by row, but columns in balanced order
            for (int r = 0; r < ship.Length; r++)
                foreach (int c in GetBalancedColumns(ship.Width))
                    positions.Add(new Position(r, c));
        }

        // Now: try lowest stacks first (so it fills evenly)
        foreach (var pos in positions.OrderBy(p => ship.GetStack(p).Height))
        {
            var check = _validator.ValidatePlacement(ship, pos, container);
            if (!check.IsValid) continue;

            ship.GetStack(pos).Place(container);
            return true;
        }

        return false;
    }

}
