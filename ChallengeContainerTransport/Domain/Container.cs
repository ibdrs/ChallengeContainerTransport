using ChallengeContainerTransport.Domain.Exceptions;

namespace ContainerChallenge.Domain;

public class Container
{
    public Container(int id, ContainerType type, int weightTons)
    {
        if (weightTons < 4 || weightTons > 30)
            throw new InvalidContainerWeightException(weightTons);

        Id = id;
        Type = type;
        WeightTons = weightTons;
    }

    public int Id { get; }
    public ContainerType Type { get; }
    public int WeightTons { get; }

    public bool IsValuable {
        get { 
            if(Type == ContainerType.Valuable || Type == ContainerType.ValuableCoolable)
                return true;
            return false;
        }
    }
    public bool IsCoolable {
        get
        {
            if (Type == ContainerType.Coolable || Type == ContainerType.ValuableCoolable)
                return true;
            return false;
        }
    }
}
