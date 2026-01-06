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


    public bool CanPlace(Container container, int maxWeightAbovePerContainerTons = 120)
    {
        // nothing may be stacked on top of valuable cargo
        if (Top()?.IsValuable == true)
            return false;

        // Create a "virtual stack" after placement
        var future = new List<Container>(_containers) { container };

        // For every container i: sum of weights above it must be <= 120
        for (int i = 0; i < future.Count; i++)
        {
            int weightAbove = 0;
            for (int j = i + 1; j < future.Count; j++)
                weightAbove += future[j].WeightTons;

            if (weightAbove > maxWeightAbovePerContainerTons)
                return false;
        }

        return true;
    }


    public void Place(Container container)
    {
        _containers.Add(container);
    }
}
