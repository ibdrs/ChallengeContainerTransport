namespace ContainerChallenge.Algorithm;

public class PlacementRequest
{
    public int Length { get; init; }
    public int Width { get; init; }

    // counts
    public int NormalCount { get; init; }
    public int ValuableCount { get; init; }
    public int CoolableCount { get; init; }
    public int ValuableCoolableCount { get; init; }

    // weights (tons)
    public int NormalWeightTons { get; init; } = 30;
    public int ValuableWeightTons { get; init; } = 30;
    public int CoolableWeightTons { get; init; } = 30;
    public int ValuableCoolableWeightTons { get; init; } = 30;
}
