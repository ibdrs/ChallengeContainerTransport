namespace ChallengeContainerTransport.Domain.Exceptions;

public class InvalidPlacementRequestException : Exception
{
    public InvalidPlacementRequestException(string message) : base(message) { }
}
