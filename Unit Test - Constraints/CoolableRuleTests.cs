using ChallengeContainerTransport.Domain.Rules;
using ContainerChallenge.Domain;

namespace Unit_Test___Constraints;

public class CoolableRuleTests
{
    [Fact]
    public void Allows_Coolable_InFirstRow()
    {
        var ship = new ShipGrid(length: 8, width: 5);
        var pos = new Position(Row: 0, Col: 2); // first row/front

        var rule = new CoolableMustBeFrontRowRule();
        var ok = rule.CanPlace(ship, pos, new Container(1, ContainerType.Coolable, 30), out var error);

        Assert.True(ok);
        Assert.Equal("", error);
    }

    [Fact]
    public void Rejects_Coolable_NotInFirstRow()
    {
        var ship = new ShipGrid(length: 8, width: 5);
        var pos = new Position(Row: 3, Col: 2);

        var rule = new CoolableMustBeFrontRowRule();
        var ok = rule.CanPlace(ship, pos, new Container(1, ContainerType.Coolable, 30), out var error);

        Assert.False(ok);
        Assert.Contains("front row", error, StringComparison.OrdinalIgnoreCase);
    }

    // valuable coolables are counted in this rule aswell
    [Fact]
    public void Rejects_ValuableCoolable_NotInFirstRow()
    {
        var ship = new ShipGrid(length: 8, width: 5);
        var pos = new Position(Row: 3, Col: 2);

        var rule = new CoolableMustBeFrontRowRule();
        var ok = rule.CanPlace(ship, pos, new Container(1, ContainerType.ValuableCoolable, 30), out var error);

        Assert.False(ok);
        Assert.Contains("front row", error, StringComparison.OrdinalIgnoreCase);
    }
}
