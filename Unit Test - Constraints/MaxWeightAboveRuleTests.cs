using Xunit;
using ContainerChallenge.Domain;
using ChallengeContainerTransport.Domain.Rules;

namespace Unit_Test___Constraints;

public class MaxWeightAboveRuleTests
{
    [Fact]
    public void Allows_FiveFullContainers_Of30Tons_InOneStack()
    {
        // Arrange
        var pos = new Position(0, 0);
        var rule = new MaxWeightAboveRule();

        var ship = new ShipGrid(length:1, width:1);
        var stack = ship.GetStack(pos);
        for (int i = 0; i < 4; i++)
            stack.Place(new Container(i, ContainerType.Normal, 30));

        // Act
        var ok = rule.CanPlace(ship, pos, new Container(5, ContainerType.Normal, 30), out var error);

        // Assert
        Assert.True(ok);
        Assert.Equal("", error);
    }

    [Fact]
    public void Rejects_SixthFullContainer_Of30Tons_InOneStack()
    {
        // Arrange
        var ship = new ShipGrid(length: 1, width: 1);
        var pos = new Position(0, 0);
        var rule = new MaxWeightAboveRule();

        var stack = ship.GetStack(pos);
        for (int i = 0; i < 5; i++)
            stack.Place(new Container(i, ContainerType.Normal, 30));

        // Act
        var ok = rule.CanPlace(ship, pos, new Container(6, ContainerType.Normal, 30), out var error);

        // Assert
        Assert.False(ok);
        Assert.Contains("max weight-above", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Rejects_WhenAnyContainerWouldHaveMoreThan120Above()
    {
        // Arrange
        // stack has 5x30 (bottom has 120 above). Adding 10t makes bottom above 130 -> false (error).
        var ship = new ShipGrid(1, 1);
        var pos = new Position(0, 0);
        var rule = new MaxWeightAboveRule();

        var stack = ship.GetStack(pos);
        for (int i = 0; i < 5; i++)
            stack.Place(new Container(i, ContainerType.Normal, 30));

        // Act
        var ok = rule.CanPlace(ship, pos, new Container(6, ContainerType.Normal, 10), out var error);

        // Assert
        Assert.False(ok);
        Assert.Contains("120", error);
    }
}
