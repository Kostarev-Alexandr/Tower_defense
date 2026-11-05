using TowerDefense.Model;

namespace TowerDefense.Tests;

// Проверяет движение по прямым участкам, поворотам и остановку на базе.
public class EnemyTests
{
    [Test]
    public void NewEnemy_StartsAtEntrance()
    {
        var map = new GameMap();
        var enemy = new Enemy(map);

        Assert.That(enemy.Column, Is.EqualTo(map.Spawn.Column));
        Assert.That(enemy.Row, Is.EqualTo(map.Spawn.Row));
        Assert.That(enemy.HasReachedBase, Is.False);
    }

    [TestCase(0, 0, 3)]
    [TestCase(0.25, 0.5, 3)]
    [TestCase(1.5, 3, 3)]
    [TestCase(1.75, 3, 2.5)]
    [TestCase(3, 4, 1)]
    [TestCase(5.25, 7, 2.5)]
    public void Move_FollowsRouteIncludingTurns(double seconds, double column, double row)
    {
        var enemy = new Enemy(new GameMap());

        enemy.Move(seconds);

        Assert.That(enemy.Column, Is.EqualTo(column).Within(0.000001));
        Assert.That(enemy.Row, Is.EqualTo(row).Within(0.000001));
        Assert.That(enemy.HasReachedBase, Is.False);
    }

    [Test]
    public void Move_InSeveralStepsMatchesOneStep()
    {
        var map = new GameMap();
        var first = new Enemy(map);
        var second = new Enemy(map);

        first.Move(6);
        for (int i = 0; i < 300; i++)
            second.Move(0.02);

        Assert.That(second.Column, Is.EqualTo(first.Column).Within(0.000001));
        Assert.That(second.Row, Is.EqualTo(first.Row).Within(0.000001));
    }

    [TestCase(8.5)]
    [TestCase(100)]
    public void Move_ReachesBaseWithoutGoingPastIt(double seconds)
    {
        var map = new GameMap();
        var enemy = new Enemy(map);

        enemy.Move(seconds);

        Assert.That(enemy.HasReachedBase, Is.True);
        Assert.That(enemy.Column, Is.EqualTo(map.Base.Column));
        Assert.That(enemy.Row, Is.EqualTo(map.Base.Row));
    }

    [Test]
    public void Move_AfterArrivalStaysAtBase()
    {
        var map = new GameMap();
        var enemy = new Enemy(map);

        enemy.Move(8.5);
        enemy.Move(10);

        Assert.That(enemy.Column, Is.EqualTo(map.Base.Column));
        Assert.That(enemy.Row, Is.EqualTo(map.Base.Row));
        Assert.That(enemy.HasReachedBase, Is.True);
    }

    [TestCase(-1)]
    [TestCase(double.NaN)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity)]
    public void Move_InvalidTimeThrowsWithoutMoving(double seconds)
    {
        var enemy = new Enemy(new GameMap());

        Assert.Throws<ArgumentOutOfRangeException>(() => enemy.Move(seconds));
        Assert.That(enemy.Column, Is.EqualTo(0));
        Assert.That(enemy.Row, Is.EqualTo(3));
    }
}
