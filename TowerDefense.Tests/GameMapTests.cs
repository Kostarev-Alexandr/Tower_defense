using TowerDefense.Model;

namespace TowerDefense.Tests;

public class GameMapTests
{
    [Test]
    public void NewMap_HasExpectedSize()
    {
        var map = new GameMap();

        Assert.That(map.Width, Is.EqualTo(12));
        Assert.That(map.Height, Is.EqualTo(8));
    }

    [TestCase(0, 0, true)]
    [TestCase(11, 7, true)]
    [TestCase(-1, 0, false)]
    [TestCase(0, -1, false)]
    [TestCase(12, 0, false)]
    [TestCase(0, 8, false)]
    public void IsInside_ChecksMapBoundaries(int column, int row, bool expected)
    {
        var map = new GameMap();

        Assert.That(map.IsInside(column, row), Is.EqualTo(expected));
    }

    [Test]
    public void Route_StartsAtEntranceAndEndsAtBase()
    {
        var map = new GameMap();

        Assert.That(map.Route.Count, Is.GreaterThan(1));
        Assert.That(map.Spawn.Column, Is.EqualTo(0));
        Assert.That(map.Base.Column, Is.EqualTo(map.Width - 1));
        Assert.That(map.GetCellType(map.Spawn.Column, map.Spawn.Row), Is.EqualTo(CellType.Spawn));
        Assert.That(map.GetCellType(map.Base.Column, map.Base.Row), Is.EqualTo(CellType.Base));
    }

    [Test]
    public void Route_StaysInsideMapAndMovesOnlyToNeighbourCells()
    {
        var map = new GameMap();
        var visited = new HashSet<(int, int)>();

        for (int i = 0; i < map.Route.Count; i++)
        {
            GridPosition current = map.Route[i];
            Assert.That(map.IsInside(current.Column, current.Row), Is.True);
            Assert.That(visited.Add((current.Column, current.Row)), Is.True, "Маршрут не должен повторять клетки.");

            if (i > 0)
            {
                GridPosition previous = map.Route[i - 1];
                int distance = Math.Abs(current.Column - previous.Column) + Math.Abs(current.Row - previous.Row);
                Assert.That(distance, Is.EqualTo(1), "Враг должен переходить в соседнюю клетку без диагоналей.");
            }
        }
    }

    [Test]
    public void RouteCells_AreNotBuildable()
    {
        var map = new GameMap();

        for (int i = 0; i < map.Route.Count; i++)
        {
            GridPosition position = map.Route[i];
            CellType expected = CellType.Road;
            if (i == 0) expected = CellType.Spawn;
            else if (i == map.Route.Count - 1) expected = CellType.Base;

            Assert.That(map.GetCellType(position.Column, position.Row), Is.EqualTo(expected));
            Assert.That(map.IsBuildableCell(position.Column, position.Row), Is.False);
        }
    }

    [Test]
    public void CellsOutsideRoute_AreGrassAndBuildable()
    {
        var map = new GameMap();
        var routeCells = new HashSet<(int, int)>();
        foreach (GridPosition position in map.Route)
            routeCells.Add((position.Column, position.Row));

        for (int column = 0; column < map.Width; column++)
        {
            for (int row = 0; row < map.Height; row++)
            {
                if (!routeCells.Contains((column, row)))
                {
                    Assert.That(map.GetCellType(column, row), Is.EqualTo(CellType.Grass));
                    Assert.That(map.IsBuildableCell(column, row), Is.True);
                }
            }
        }
    }

    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(12, 0)]
    [TestCase(0, 8)]
    public void OutsideMap_IsNotBuildable(int column, int row)
    {
        var map = new GameMap();

        Assert.That(map.IsBuildableCell(column, row), Is.False);
    }

    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(12, 0)]
    [TestCase(0, 8)]
    public void GetCellType_OutsideMapThrowsClearException(int column, int row)
    {
        var map = new GameMap();

        Assert.Throws<ArgumentOutOfRangeException>(() => map.GetCellType(column, row));
    }
}
