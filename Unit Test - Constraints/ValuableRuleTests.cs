using ChallengeContainerTransport.Domain.Rules;
using ContainerChallenge.Domain;

namespace Unit_Test___Constraints;

public class ValuableRuleTests
{
    [Fact]
    public void Rejects_PlacingOnTopOfValuable()
    {
        var ship = new ShipGrid(1, 1);
        var pos = new Position(0, 0);

        var stack = ship.GetStack(pos);
        stack.Place(new Container(1, ContainerType.Valuable, 30));

        var rule = new NoContainerAboveValuableRule();
        var ok = rule.CanPlace(ship, pos, new Container(1, ContainerType.Normal, 30), out var error);

        Assert.False(ok);
        Assert.Contains("valuable", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Allows_PlacingValuable_OnEmptyStack()
    {
        var ship = new ShipGrid(1, 1);
        var pos = new Position(0, 0);

        var rule = new NoContainerAboveValuableRule();
        var ok = rule.CanPlace(ship, pos, new Container(1, ContainerType.Valuable, 30), out var error);

        Assert.True(ok);
        Assert.Equal("", error);
    }

    [Fact]
    public void Valuable_MayBe_Placed_OnEdgeRow()
    {
        // Arrange
        var ship = new ShipGrid(length: 5, width: 3);
        var pos = new Position(Row: 4, Col: 0); // edge row (back)
        var rule = new ValuableMustBeEdgeRowRule();

        // Act
        var ok = rule.CanPlace(
            ship,
            pos,
            new Container(1, ContainerType.Valuable, 30),
            out var error
        );

        // Assert
        Assert.True(ok);
        Assert.Equal("", error);
    }

    [Fact]
    public void Valuable_MustNot_BePlaced_InMiddleRow()
    {
        // Arrange
        var ship = new ShipGrid(length: 5, width: 3);
        var pos = new Position(Row: 2, Col: 1); // middle row
        var rule = new ValuableMustBeEdgeRowRule();

        // Act
        var ok = rule.CanPlace(
            ship,
            pos,
            new Container(1, ContainerType.Valuable, 30),
            out var error
        );

        // Assert
        Assert.False(ok);
        Assert.Contains("edge", error, StringComparison.OrdinalIgnoreCase);
    }

}
