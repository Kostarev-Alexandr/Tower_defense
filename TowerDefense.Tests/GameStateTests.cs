using TowerDefense.Model;

namespace TowerDefense.Tests;

public class GameStateTests
{
    [Test]
    public void NewGame_PlacesTowerOnGrassOutsideRoute()
    {
        var game = new GameState();
        GridPosition position = game.Tower.Position;

        Assert.That(game.Map.IsBuildableCell(position.Column, position.Row), Is.True);
        foreach (GridPosition routePosition in game.Map.Route)
        {
            bool sameCell = position.Column == routePosition.Column && position.Row == routePosition.Row;
            Assert.That(sameCell, Is.False);
        }
    }

    [Test]
    public void Update_MovesEnemyAndKeepsTowerStill()
    {
        var game = new GameState();

        game.Update(0.25);

        Assert.That(game.Enemy.Column, Is.EqualTo(0.5));
        Assert.That(game.Enemy.Row, Is.EqualTo(3));
        Assert.That(game.Tower.Position.Column, Is.EqualTo(4));
        Assert.That(game.Tower.Position.Row, Is.EqualTo(3));
    }

    [Test]
    public void Update_ReportsArrivalAtBase()
    {
        var game = new GameState();

        game.Update(8.5);

        Assert.That(game.Enemy.HasReachedBase, Is.True);
    }
}
