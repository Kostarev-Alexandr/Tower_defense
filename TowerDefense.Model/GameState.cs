namespace TowerDefense.Model;

// Хранит состояние карты, строительства и боя.
public class GameState
{
    public const int StartingMoney = 150;
    public const int TowerCost = 50;
    public const int MaxBaseHealth = 100;

    private readonly List<Tower> towers = new List<Tower>();
    private readonly List<Shot> shots = new List<Shot>();

    public GameMap Map { get; }
    public Enemy Enemy { get; }
    public IReadOnlyList<Tower> Towers { get; }
    public IReadOnlyList<Shot> Shots { get; }
    public int Money { get; private set; } = StartingMoney;
    public int BaseHealth { get; private set; } = MaxBaseHealth;
    public bool IsEncounterFinished => Enemy.IsDefeated || Enemy.HasReachedBase;

    public GameState()
    {
        Map = new GameMap();
        Enemy = new Enemy(Map);
        Towers = towers.AsReadOnly();
        Shots = shots.AsReadOnly();
    }

    public BuildResult BuildTower(int column, int row)
    {
        if (!Map.IsInside(column, row))
            return BuildResult.OutsideMap;

        if (!Map.IsBuildableCell(column, row))
            return BuildResult.InvalidTerrain;

        if (GetTowerAt(column, row) != null)
            return BuildResult.Occupied;

        if (Money < TowerCost)
            return BuildResult.NotEnoughMoney;

        // Состояние меняется только после успешного прохождения всех проверок.
        towers.Add(new Tower(new GridPosition(column, row)));
        Money -= TowerCost;
        return BuildResult.Success;
    }

    public Tower? GetTowerAt(int column, int row)
    {
        foreach (Tower tower in towers)
        {
            if (tower.Position.Column == column && tower.Position.Row == row)
                return tower;
        }

        return null;
    }

    public void Update(double seconds)
    {
        if (!double.IsFinite(seconds) || seconds < 0)
            throw new ArgumentOutOfRangeException(nameof(seconds));

        // Короткие шаги позволяют башням заметить врага даже после задержки таймера.
        while (seconds > 0 && !IsEncounterFinished)
        {
            double step = Math.Min(seconds, 0.05);
            seconds -= step;
            AdvanceShots(step);
            Enemy.Move(step);

            if (Enemy.HasReachedBase)
            {
                BaseHealth = Math.Max(0, BaseHealth - Enemy.BaseDamage);
                break;
            }

            foreach (Tower tower in towers)
            {
                if (!tower.TryAttack(Enemy, step))
                    continue;

                shots.Add(new Shot(tower.Position, Enemy));
                if (Enemy.IsDefeated)
                {
                    Money += Enemy.Reward;
                    break;
                }
            }
        }

        AdvanceShots(seconds);
    }

    private void AdvanceShots(double seconds)
    {
        for (int i = shots.Count - 1; i >= 0; i--)
        {
            shots[i].TimeRemaining -= seconds;
            if (shots[i].TimeRemaining <= 0)
                shots.RemoveAt(i);
        }
    }
}
