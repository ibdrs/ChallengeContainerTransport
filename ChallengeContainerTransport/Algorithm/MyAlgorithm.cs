using ContainerChallenge.Algorithm;
using ContainerChallenge.Domain;

public class MyAlgorithm
{
    private readonly PlacementValidator _placement;
    private readonly ShipValidator _ship;

    public MyAlgorithm(PlacementValidator placement, ShipValidator ship)
    {
        _placement = placement;
        _ship = ship;
    }

    public PlacementResponse Run(PlacementRequest request)
    {
        var ship = new ShipGrid(request.Length, request.Width);
        var containers = BuildContainers(request);

        var unplaced = new List<UnplacedContainerInfo>();

        foreach (var c in containers)
        {
            if (!TryPlace(ship, c, out var reasons))
            {
                unplaced.Add(new UnplacedContainerInfo(c.Id, c.Type, c.WeightTons, reasons));
            }
        }

        _ship.Validate(ship, out var shipErrors);

        return new PlacementResponse
        {
            Ship = ship,
            Errors = shipErrors,
            UnplacedCount = unplaced.Count,
            Unplaced = unplaced
        };
    }


    private IEnumerable<Position> CandidatePositions(ShipGrid ship, Container c)
    {
        // In deze methode geven we alle mogelijke posities op het schipgrid door
        //
        // Coolable: alleen front row
        if (c.Type == ContainerType.Coolable ||
            c.Type == ContainerType.ValuableCoolable)
        {
            for (int col = 0; col < ship.Width; col++)
                yield return new Position(0, col);
            yield break;
        }

        // Valuable: front or back row
        if (c.Type == ContainerType.Valuable)
        {
            int lastRow = ship.Length - 1;

            // front row
            for (int col = 0; col < ship.Width; col++)
                yield return new Position(0, col);

            //back row
            if (lastRow != 0)
                for (int col = 0; col < ship.Width; col++)
                    yield return new Position(lastRow, col);

            yield break;
        }

        // Normal: overal
        for (int row = 0; row < ship.Length; row++)
            for (int col = 0; col < ship.Width; col++)
                yield return new Position(row, col);
    }

    private IEnumerable<Position> CandidatePositionsByLowestStackFirst(ShipGrid ship)
    {
        var positions = new List<Position>();

        // Haal alle mogelijke posities op het grid op
        for (int row = 0; row < ship.Length; row++)
        {
            for (int col = 0; col < ship.Width; col++)
            {
                positions.Add(new Position(row, col));
            }
        }

        // Sorteer de posities op de hoogte van de stack (kleinst --> grootst / ascending)
        positions.Sort((a, b) => // .Sort gaat door elk element van de positions list
        {
            int heightA = ship.GetStack(a).Height;
            int heightB = ship.GetStack(b).Height;
            return heightA.CompareTo(heightB); // vergelijkt hoogte a en b
            // output: 1 = a > b, 0 = equal, -1 = a < b
            //
            // .Sort sorteert elk element met gebruik van .CompareTo resultaat
        });

        // sorted
        return positions;
    }

    private bool TryPlace(ShipGrid ship, Container c, out List<string> reasons)
    {
        reasons = new List<string>();

        IEnumerable<Position> positions; 
        if(c.Type == ContainerType.Normal)
        {
            positions = CandidatePositionsByLowestStackFirst(ship);
        }
        else
        {
            positions = CandidatePositions(ship, c);
        }

        foreach (var pos in positions)
        {
            if (_placement.CanPlace(ship, pos, c, out var errors))
            {
                ship.GetStack(pos).Place(c);
                return true;
            }

            // bewaar een paar unieke foutredenen (max 6)
            foreach (var e in errors)
            {
                if (reasons.Count >= 6) break;
                if (!reasons.Contains(e)) reasons.Add(e);
            }
        }

        return false;
    }


    private static List<Container> BuildContainers(PlacementRequest r)
    {
        var list = new List<Container>();
        int id = 0;

        // volgorde van containers 

        for (int i = 0; i < r.CoolableCount; i++)
            list.Add(new Container(id++, ContainerType.Coolable, r.CoolableWeightTons));

        for (int i = 0; i < r.NormalCount; i++)
            list.Add(new Container(id++, ContainerType.Normal, r.NormalWeightTons));

        for (int i = 0; i < r.ValuableCoolableCount; i++)
            list.Add(new Container(id++, ContainerType.ValuableCoolable, r.ValuableCoolableWeightTons));

        for (int i = 0; i < r.ValuableCount; i++)
            list.Add(new Container(id++, ContainerType.Valuable, r.ValuableWeightTons));

        return list;
    }

}
