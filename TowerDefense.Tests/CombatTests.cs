using TowerDefense.Model;

namespace TowerDefense.Tests;

public class CombatTests
{
    [Test]
    public void Update_TowerFiresWhenEnemyEntersCircularRange()
    {
        var game = new GameState();
        game.BuildTower(2, 1);

        game.Update(0.2);
        Assert.That(game.Enemy.Health, Is.EqualTo(60));
        Assert.That(game.Shots, Is.Empty);

        game.Update(0.05);
        Assert.That(game.Enemy.Health, Is.EqualTo(40));
        Assert.That(game.Shots.Count, Is.EqualTo(1));
        Assert.That(game.Shots[0].TargetColumn, Is.EqualTo(0.5).Within(0.000001));
    }

    [Test]
    public void Update_WaitsBetweenShotsAndRemovesExpiredTrace()
    {
        var game = new GameState();
        game.BuildTower(1, 2);

        game.Update(0.05);
        game.Update(0.64);
        Assert.That(game.Enemy.Health, Is.EqualTo(40));
        Assert.That(game.Shots, Is.Empty);

        game.Update(0.01);
        Assert.That(game.Enemy.Health, Is.EqualTo(20));
        Assert.That(game.Shots.Count, Is.EqualTo(1));
    }

    [Test]
    public void Update_LethalShotRewardsOnceAndLeavesBaseIntact()
    {
        var game = new GameState();
        game.BuildTower(1, 2);

        game.Update(0.05);
        game.Update(0.65);
        Assert.That(game.Money, Is.EqualTo(100));

        game.Update(0.65);
        Assert.That(game.Enemy.Health, Is.Zero);
        Assert.That(game.IsEncounterFinished, Is.True);
        Assert.That(game.Shots.Count, Is.EqualTo(1));
        double lastColumn = game.Enemy.Column;

        game.Update(10);
        Assert.That(game.Enemy.Column, Is.EqualTo(lastColumn));
        Assert.That(game.Enemy.HasReachedBase, Is.False);
        Assert.That(game.Money, Is.EqualTo(125));
        Assert.That(game.BaseHealth, Is.EqualTo(100));
        Assert.That(game.Shots, Is.Empty);
    }

    [Test]
    public void Update_LateTickDoesNotSkipEnemyPassingTower()
    {
        var game = new GameState();
        game.BuildTower(1, 2);

        game.Update(8.5);

        Assert.That(game.Enemy.IsDefeated, Is.True);
        Assert.That(game.BaseHealth, Is.EqualTo(100));
        Assert.That(game.Money, Is.EqualTo(125));
    }

    [Test]
    public void Update_WoundedEnemyReachingBaseDamagesItOnceWithoutReward()
    {
        var game = new GameState();
        game.BuildTower(0, 1);

        game.Update(8.5);
        Assert.That(game.Enemy.HasReachedBase, Is.True);
        Assert.That(game.Enemy.Health, Is.EqualTo(20));
        Assert.That(game.IsEncounterFinished, Is.True);
        Assert.That(game.BaseHealth, Is.EqualTo(75));

        game.Update(10);
        Assert.That(game.BaseHealth, Is.EqualTo(75));
        Assert.That(game.Money, Is.EqualTo(100));
    }

    [Test]
    public void Update_SeveralTowersStopFiringAfterLethalHit()
    {
        var game = new GameState();
        game.BuildTower(1, 2);
        game.Update(0.84);
        Assert.That(game.Enemy.Health, Is.EqualTo(20));
        Assert.That(game.Shots, Is.Empty);

        game.BuildTower(0, 2);
        game.BuildTower(1, 4);
        game.Update(0.05);

        Assert.That(game.Enemy.IsDefeated, Is.True);
        Assert.That(game.Shots.Count, Is.EqualTo(1));
        Assert.That(game.Money, Is.EqualTo(25));
        Assert.That(game.BaseHealth, Is.EqualTo(100));
    }
}
