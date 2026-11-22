using TowerDefense.Model;

namespace TowerDefense.Tests;

public class CombatTests
{
    [Test]
    public void Update_TowerFiresWhenEnemyEntersCircularRange()
    {
        var game = new GameState();
        game.BuildTower(2, 1);
        game.StartNextWave();

        game.Update(0.2);
        Assert.That(game.Enemies[0].Health, Is.EqualTo(60));
        Assert.That(game.Shots, Is.Empty);

        game.Update(0.05);
        Assert.That(game.Enemies[0].Health, Is.EqualTo(40));
        Assert.That(game.Shots.Count, Is.EqualTo(1));
        Assert.That(game.Shots[0].TargetColumn, Is.EqualTo(0.5).Within(0.000001));
    }

    [Test]
    public void Update_WaitsBetweenShotsAndRemovesExpiredTrace()
    {
        var game = new GameState();
        game.BuildTower(1, 2);
        game.StartNextWave();

        game.Update(0.05);
        game.Update(0.64);
        Assert.That(game.Enemies[0].Health, Is.EqualTo(40));
        Assert.That(game.Shots, Is.Empty);

        game.Update(0.01);
        Assert.That(game.Enemies[0].Health, Is.EqualTo(20));
        Assert.That(game.Shots.Count, Is.EqualTo(1));
    }

    [Test]
    public void Update_LethalShotRewardsOnceAndLeavesBaseIntact()
    {
        var game = new GameState();
        game.BuildTower(1, 2);
        game.StartNextWave();

        game.Update(0.05);
        Enemy firstEnemy = game.Enemies[0];
        game.Update(0.65);
        Assert.That(game.Money, Is.EqualTo(100));

        game.Update(0.65);
        Assert.That(firstEnemy.Health, Is.Zero);
        Assert.That(game.Enemies, Does.Not.Contain(firstEnemy));
        Assert.That(game.Shots.Count, Is.EqualTo(1));
        double lastColumn = firstEnemy.Column;
        Assert.That(game.Money, Is.EqualTo(125));

        game.BuildTower(4, 2);
        game.Update(10);
        int finalMoney = game.Money;
        game.Update(10);
        Assert.That(firstEnemy.Column, Is.EqualTo(lastColumn));
        Assert.That(firstEnemy.HasReachedBase, Is.False);
        Assert.That(game.Money, Is.EqualTo(finalMoney));
        Assert.That(game.BaseHealth, Is.EqualTo(100));
        Assert.That(game.Shots, Is.Empty);
    }

    [Test]
    public void Update_LateTickDoesNotSkipEnemyPassingTower()
    {
        var game = new GameState();
        game.BuildTower(1, 2);
        game.BuildTower(4, 2);
        game.StartNextWave();

        game.Update(8.5);

        Assert.That(game.EnemiesDefeated, Is.EqualTo(3));
        Assert.That(game.BaseHealth, Is.EqualTo(100));
        Assert.That(game.Money, Is.EqualTo(125));
    }

    [Test]
    public void Update_WoundedEnemyReachingBaseDamagesItOnceWithoutReward()
    {
        var game = new GameState();
        game.BuildTower(0, 1);
        game.StartNextWave();
        game.Update(0.05);
        Enemy firstEnemy = game.Enemies[0];

        game.Update(30);
        Assert.That(firstEnemy.HasReachedBase, Is.True);
        Assert.That(firstEnemy.Health, Is.EqualTo(20));
        Assert.That(game.Phase, Is.EqualTo(GamePhase.Preparing));
        Assert.That(game.BaseHealth, Is.EqualTo(25));

        game.Update(10);
        Assert.That(game.BaseHealth, Is.EqualTo(25));
        Assert.That(game.Money, Is.EqualTo(100));
    }

    [Test]
    public void Update_SeveralTowersStopFiringAfterLethalHit()
    {
        var game = new GameState();
        game.BuildTower(1, 2);
        game.StartNextWave();
        game.Update(0.84);
        Enemy firstEnemy = game.Enemies[0];
        Assert.That(firstEnemy.Health, Is.EqualTo(20));
        Assert.That(game.Shots, Is.Empty);

        game.BuildTower(0, 2);
        game.BuildTower(1, 4);
        game.Update(0.05);

        Assert.That(firstEnemy.IsDefeated, Is.True);
        Assert.That(game.Shots.Count, Is.EqualTo(1));
        Assert.That(game.Money, Is.EqualTo(25));
        Assert.That(game.BaseHealth, Is.EqualTo(100));
    }

    [Test]
    public void Update_MultipleTargetsShareOneTowerCooldown()
    {
        var game = new GameState();
        game.BuildTower(1, 2);
        game.StartNextWave();
        game.Update(1.25);
        Enemy first = game.Enemies[0];
        Enemy second = game.Enemies[1];

        Assert.That(first.Health, Is.EqualTo(20));
        Assert.That(second.Health, Is.EqualTo(60));
        game.Update(0.1);
        Assert.That(first.IsDefeated, Is.True);
        Assert.That(second.Health, Is.EqualTo(60));
    }
}
