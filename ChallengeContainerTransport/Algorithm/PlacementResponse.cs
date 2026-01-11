using ContainerChallenge.Domain;

public class PlacementResponse
{
    public required ShipGrid Ship { get; init; }
    public List<string> Errors { get; init; } = new();
    public int UnplacedCount { get; init; }
    public List<UnplacedContainerInfo> ?Unplaced { get; init; }
}

public record UnplacedContainerInfo(
    int Id,
    ContainerType Type,
    int WeightTons,
    List<string> Reasons
);