using ContainerChallenge.Domain;

namespace ContainerChallenge.Validation;

public class ConstraintsValidator
{
    public ConstraintsValidator(int maxWeightAboveTons = 120, int maxStackHeight = 8)
    {
        MaxWeightAboveTons = maxWeightAboveTons;
        MaxStackHeight = maxStackHeight;
    }

    public int MaxWeightAboveTons { get; }
    public int MaxStackHeight { get; }

    public ValidationResult ValidatePlacement(ShipGrid ship, Position pos, Container container)
    {
        // Coolable must be front row (row 0)
        if (container.IsCoolable && pos.Row != 0)
            return ValidationResult.Fail("Coolable containers must be placed in the front row (row 0).");

        var stack = ship.GetStack(pos);

        // Optional height cap (if you decide to enforce a max height)
        if (stack.Height >= MaxStackHeight)
            return ValidationResult.Fail($"Stack exceeds max height {MaxStackHeight}.");

        // Stack rules (valuable-on-top + 120 weight above)
        if (!stack.CanPlace(container, MaxWeightAboveTons))
            return ValidationResult.Fail("Cannot place container due to stack constraints (valuable/top or weight-above limit).");

        // Valuable accessibility (simple model: must be front/back row)
        if (container.IsValuable)
        {
            bool accessibleRow = pos.Row == 0 || pos.Row == ship.Length - 1;
            if (!accessibleRow)
                return ValidationResult.Fail("Valuable containers must be accessible from front or back (row 0 or last row).");
        }

        return ValidationResult.Ok();
    }

    public ValidationResult ValidateFinal(ShipGrid ship)
    {
        // Balance: left/right halves differ <= 20% of total weight
        int total = ship.TotalWeightTons();
        if (total == 0) return ValidationResult.Fail("Ship has no load.");

        var (left, right) = ship.GetHalfWeightsTons();
        int diff = Math.Abs(left - right);
        if (diff > 0.2 * total)
            return ValidationResult.Fail($"Balance violation: left={left} right={right} diff={diff} (>{0.2 * total:0.##}).");

        // Minimum load >= 50% of max capacity
        // Max capacity depends on chosen MaxStackHeight constant.
        //int maxCapacity = ship.Length * ship.Width * MaxStackHeight * 30;
        //if (total < 0.5 * maxCapacity)
        //    return ValidationResult.Fail($"Minimum load violation: total={total}, required>={0.5 * maxCapacity:0.##} (max={maxCapacity}).");

        // Ensure all valuable containers are on top of their stack
        for (int row = 0; row < ship.Length; row++)
            for (int column = 0; column < ship.Width; column++)
            {
                var stack = ship.Stacks[row, column];
                for (int i = 0; i < stack.Containers.Count - 1; i++)
                {
                    if (stack.Containers[i].IsValuable)
                        return ValidationResult.Fail($"Valuable container not on top at (row={row}, col={column}).");
                }
            }

        return ValidationResult.Ok();
    }
}
