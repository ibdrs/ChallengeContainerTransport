namespace ContainerChallenge.Domain;
public class Container
{
    public Container(ContainerType type, int weightTons)
    {
        if (weightTons < 4 || weightTons > 30)
            throw new ArgumentOutOfRangeException(nameof(weightTons), "Container weight must be between 4 and 30 tons.");

        Type = type;
        WeightTons = weightTons;
    }

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
