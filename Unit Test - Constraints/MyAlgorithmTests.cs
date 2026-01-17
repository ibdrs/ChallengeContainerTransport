using ChallengeContainerTransport.Domain.Rules;
using ChallengeContainerTransport.Domain.Rules.Interfaces;
using ContainerChallenge.Algorithm;
using ContainerChallenge.Domain;
using System.Reflection;
using Xunit;

namespace Unit_Test___Constraints;
public class MyAlgorithmTests
{
    [Fact]
    public void Run_Places_CoolableOnly_InFrontRow()
    {
        // Arrange
        // valid ship layout
        var request = new PlacementRequest
        {
            Length = 6,
            Width = 4,

            NormalCount = 64,
            ValuableCount = 4,
            CoolableCount = 10,
            ValuableCoolableCount = 2,

            NormalWeightTons = 30,
            ValuableWeightTons = 30,
            CoolableWeightTons = 30,
            ValuableCoolableWeightTons = 30
        };

        var placementRules = new IPlacementRule[]
        {
            new CoolableMustBeFrontRowRule()
        };

        var shipRules = new IShipRule[]
        {
            new BalanceRule(),
            new MinimumLoadRule()
        };

        var algorithm = new MyAlgorithm(
            new PlacementValidator(placementRules),
            new ShipValidator(shipRules)
        );

        // Act
        var response = algorithm.Run(request);
        var ship = response.Ship;

        // Assert
        Assert.NotNull(ship);
        Assert.Equal(0, response.UnplacedCount);

        int coolablesInFrontRow = 0;
        int coolablesNotInFrontRow = 0;

        for (int row = 0; row < ship.Length; row++)
            for (int col = 0; col < ship.Width; col++)
            {
                var stack = ship.GetStack(new Position(row, col));
                foreach (var c in stack.Containers)
                {
                    if (c.Type == ContainerType.Coolable || c.Type == ContainerType.ValuableCoolable )
                    {
                        if (row == 0) coolablesInFrontRow++;
                        else coolablesNotInFrontRow++;
                    }
                }
            }

        Assert.Equal(request.CoolableCount + request.ValuableCoolableCount, coolablesInFrontRow);
        Assert.Equal(0, coolablesNotInFrontRow);
    }

    [Fact]
    public void Run_WhenNoSpace_AddsToUnplaced_WithReasons()
    {
        // Arrange: 1x1 schip, maar 2 containers -> 1 moet unplaced
        var request = new PlacementRequest
        {
            Length = 1,
            Width = 1,
            NormalCount = 2,
            NormalWeightTons = 30
        };

        // rule: We maken een regel aan om na 1 container op de stapel plaatsing te blokkeren
        // dit test: de TryPlace loop, we willen straks kijken of er UnplacedContainerInfo uit komt

        var placementRules = new IPlacementRule[]
        {
            new MaxOnePerStackRule()
        };

        var shipRules = Array.Empty<IShipRule>();

        var algorithm = new MyAlgorithm(
            new PlacementValidator(placementRules),
            new ShipValidator(shipRules)
        );

        // Act
        var response = algorithm.Run(request);

        // Assert
        Assert.Equal(1, response.UnplacedCount);
        Assert.NotNull(response.Unplaced);
        Assert.Single(response.Unplaced);

        var unplaced = response.Unplaced[0];
        Assert.True(unplaced.Reasons.Count >= 1);
        Assert.Contains("Stack already has a container.", unplaced.Reasons);
    }

    [Fact]
    public void Run_NormalContainers_AreDistributed_ByLowestStackFirst()
    {
        // Arrange: 1x3 grid, 3 normale containers
        var request = new PlacementRequest
        {
            Length = 1,
            Width = 3,
            NormalCount = 3,
            NormalWeightTons = 30
        };

        // rule: max 1 container per stack => algorithm moet spreiden
        var placementRules = new IPlacementRule[]
        {
            new MaxOnePerStackRule()
        };

        var shipRules = Array.Empty<IShipRule>();

        var algorithm = new MyAlgorithm(
            new PlacementValidator(placementRules),
            new ShipValidator(shipRules)
        );

        // Act
        var response = algorithm.Run(request);
        var ship = response.Ship;

        // Assert
        Assert.Equal(0, response.UnplacedCount);

        int nonEmptyStacks = 0;
        for (int col = 0; col < ship.Width; col++)
        {
            var stack = ship.GetStack(new Position(0, col));
            if (stack.Height > 0) nonEmptyStacks++;
        }

        // 3 containers, 3 stapels => zou alle 3 vullen
        Assert.Equal(3, nonEmptyStacks);
    }

    private class MaxOnePerStackRule : IPlacementRule
    {
        public bool CanPlace(ShipGrid ship, Position pos, Container container, out string error)
        {
            error = "";
            if (ship.GetStack(pos).Height >= 1)
            {
                error = "Stack already has a container.";
                return false;
            }
            return true;
        }
    }
}
