namespace TowerDefense.Model;

// Хранит игровые объекты и деньги, проверяет правила строительства.
public class GameState
{
    public const int StartingMoney = 150;
    public const int TowerCost = 50;

    private readonly List<Tower> towers = new List<Tower>();

    public GameMap Map { get; }
    public Enemy Enemy { get; }
    public IReadOnlyList<Tower> Towers { get; }
    public int Money { get; private set; } = StartingMoney;

    public GameState()
    {
        Map = new GameMap();
        Enemy = new Enemy(Map);
        Towers = towers.AsReadOnly();
    }

    public BuildResult BuildTower(int column, int row)
    {
        if (!Map.IsInside(column, row))
            return BuildResult.OutsideMap;

        if (!Map.IsBuildableCell(column, row))
            return BuildResult.InvalidTerrain;

        foreach (Tower tower in towers)
        {
            if (tower.Position.Column == column && tower.Position.Row == row)
                return BuildResult.Occupied;
        }

        if (Money < TowerCost)
            return BuildResult.NotEnoughMoney;

        // Состояние меняется только после успешного прохождения всех проверок.
        towers.Add(new Tower(new GridPosition(column, row)));
        Money -= TowerCost;
        return BuildResult.Success;
    }

    public void Update(double seconds)
    {
        Enemy.Move(seconds);
    }
}
