using TowerDefense.Model;

namespace TowerDefense.Tests;

public class GameStateTests
{
    [Test]
    public void NewGame_HasStartingMoneyAndNoTowers()
    {
        var game = new GameState();

        Assert.That(game.Money, Is.EqualTo(150));
        Assert.That(game.Towers, Is.Empty);
    }

    [Test]
    public void Update_MovesEnemyAndKeepsTowerStill()
    {
        var game = new GameState();
        game.BuildTower(4, 3);

        game.Update(0.25);

        Assert.That(game.Enemy.Column, Is.EqualTo(0.5));
        Assert.That(game.Enemy.Row, Is.EqualTo(3));
        Assert.That(game.Towers[0].Position.Column, Is.EqualTo(4));
        Assert.That(game.Towers[0].Position.Row, Is.EqualTo(3));
        Assert.That(game.Money, Is.EqualTo(100));
    }

    [Test]
    public void Update_ReportsArrivalAtBase()
    {
        var game = new GameState();

        game.Update(8.5);

        Assert.That(game.Enemy.HasReachedBase, Is.True);
    }
}
