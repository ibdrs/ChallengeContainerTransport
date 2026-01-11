using System.Collections.ObjectModel;

namespace ContainerChallenge.Domain;

public class Stack
{
    private readonly List<Container> _containers = new(); // bottom -> top

    public ReadOnlyCollection<Container> Containers
    {
        get
        {
            return _containers.AsReadOnly();
        }
    }

    public int Height
    {
        get
        {
            return _containers.Count;
        }
    }

    public int TotalWeightTons()
    {
        if (_containers == null || _containers.Count == 0)
        {
            return 0;
        }
        
        var totalWeight = 0;

        foreach (var container in _containers)
        {
            totalWeight += container.WeightTons;
        }

        return totalWeight;
    }

    public Container? Top()
    {
        if (_containers.Count == 0)
        {
            return null;
        }

        return _containers[_containers.Count - 1];
    }


    public void Place(Container container)
    {
        _containers.Add(container);
    }
}
