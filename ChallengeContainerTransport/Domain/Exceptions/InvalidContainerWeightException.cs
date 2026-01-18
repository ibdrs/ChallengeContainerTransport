namespace ChallengeContainerTransport.Domain.Exceptions;

public class InvalidContainerWeightException : Exception
{
    public int WeightTons { get; }
    public InvalidContainerWeightException(int weightTons)
        : base($"Container weight must be between 4 and 30 tons. Given: {weightTons}.")
    {
        WeightTons = weightTons;
    }
}