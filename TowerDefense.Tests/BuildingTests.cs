using TowerDefense.Model;

namespace TowerDefense.Tests;

// Проверяет размещение башен и списание денег при успешных и неудачных попытках.
public class BuildingTests
{
    [Test]
    public void BuildTower_OnGrassCreatesTowerAndPaysItsCost()
    {
        var game = new GameState();

        BuildResult result = game.BuildTower(4, 3);

        Assert.That(result, Is.EqualTo(BuildResult.Success));
        Assert.That(game.Towers.Count, Is.EqualTo(1));
        Assert.That(game.Towers[0].Position.Column, Is.EqualTo(4));
        Assert.That(game.Towers[0].Position.Row, Is.EqualTo(3));
        Assert.That(game.Money, Is.EqualTo(100));
        Assert.That(game.Enemy.Column, Is.EqualTo(0));
        Assert.That(game.Enemy.Row, Is.EqualTo(3));
    }

    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(12, 0)]
    [TestCase(0, 8)]
    public void BuildTower_OutsideMapDoesNotChangeGame(int column, int row)
    {
        var game = new GameState();

        BuildResult result = game.BuildTower(column, row);

        Assert.That(result, Is.EqualTo(BuildResult.OutsideMap));
        Assert.That(game.Money, Is.EqualTo(150));
        Assert.That(game.Towers, Is.Empty);
    }

    [Test]
    public void BuildTower_OnAnyRouteCellDoesNotSpendMoney()
    {
        var game = new GameState();

        foreach (GridPosition position in game.Map.Route)
        {
            BuildResult result = game.BuildTower(position.Column, position.Row);

            Assert.That(result, Is.EqualTo(BuildResult.InvalidTerrain));
            Assert.That(game.Money, Is.EqualTo(150));
            Assert.That(game.Towers, Is.Empty);
        }
    }

    [Test]
    public void BuildTower_InOccupiedCellKeepsExistingTowerAndMoney()
    {
        var game = new GameState();
        game.BuildTower(4, 3);
        Tower firstTower = game.Towers[0];

        BuildResult result = game.BuildTower(4, 3);

        Assert.That(result, Is.EqualTo(BuildResult.Occupied));
        Assert.That(game.Towers.Count, Is.EqualTo(1));
        Assert.That(game.Towers[0], Is.SameAs(firstTower));
        Assert.That(game.Money, Is.EqualTo(100));
    }

    [Test]
    public void BuildTower_CanSpendExactBalanceButCannotBuildForFree()
    {
        var game = new GameState();

        Assert.That(game.BuildTower(0, 0), Is.EqualTo(BuildResult.Success));
        Assert.That(game.BuildTower(1, 0), Is.EqualTo(BuildResult.Success));
        Assert.That(game.BuildTower(2, 0), Is.EqualTo(BuildResult.Success));
        Assert.That(game.Money, Is.EqualTo(0));
        Assert.That(game.Towers.Count, Is.EqualTo(3));

        BuildResult result = game.BuildTower(3, 0);

        Assert.That(result, Is.EqualTo(BuildResult.NotEnoughMoney));
        Assert.That(game.Money, Is.EqualTo(0));
        Assert.That(game.Towers.Count, Is.EqualTo(3));
    }

    [Test]
    public void BuildTower_AfterArrivalStillWorks()
    {
        var game = new GameState();
        game.Update(8.5);

        BuildResult result = game.BuildTower(4, 3);

        Assert.That(result, Is.EqualTo(BuildResult.Success));
        Assert.That(game.Money, Is.EqualTo(100));
        Assert.That(game.Enemy.HasReachedBase, Is.True);
    }

    [Test]
    public void BuildTower_InSeparateGamesDoesNotShareMoneyOrTowers()
    {
        var first = new GameState();
        var second = new GameState();

        first.BuildTower(4, 3);

        Assert.That(second.Money, Is.EqualTo(150));
        Assert.That(second.Towers, Is.Empty);
    }
}
