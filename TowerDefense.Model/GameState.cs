namespace TowerDefense.Model;

// Хранит состояние карты, строительства и боя.
public class GameState
{
    public const int StartingMoney = 150;
    public const int TowerCost = 50;
    public const int MaxBaseHealth = 100;
    public const int TotalWaves = 5;
    public const double SpawnInterval = 0.9;

    private readonly List<Tower> towers = new List<Tower>();
    private readonly List<Shot> shots = new List<Shot>();
    private readonly List<Enemy> enemies = new List<Enemy>();
    private static readonly int[] waveHealth = { 60, 100, 160, 220, 300 };
    private int enemiesToSpawn;
    private double spawnTimer;

    public GameMap Map { get; }
    public IReadOnlyList<Enemy> Enemies { get; }
    public IReadOnlyList<Tower> Towers { get; }
    public IReadOnlyList<Shot> Shots { get; }
    public int Money { get; private set; } = StartingMoney;
    public int BaseHealth { get; private set; } = MaxBaseHealth;
    public GamePhase Phase { get; private set; } = GamePhase.Preparing;
    public int WaveNumber { get; private set; }
    public int WaveEnemyHealth => waveHealth[Math.Max(0, WaveNumber - 1)];
    public int EnemiesRemaining => enemies.Count + enemiesToSpawn;
    public int EnemiesDefeated { get; private set; }
    public int EnemiesEscaped { get; private set; }
    public bool IsGameOver => Phase == GamePhase.Victory || Phase == GamePhase.Defeat;

    public GameState()
    {
        Map = new GameMap();
        Enemies = enemies.AsReadOnly();
        Towers = towers.AsReadOnly();
        Shots = shots.AsReadOnly();
    }

    public BuildResult BuildTower(int column, int row)
    {
        if (IsGameOver)
            return BuildResult.GameOver;

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
        while (seconds > 0 && Phase == GamePhase.WaveRunning)
        {
            double step = Math.Min(seconds, 0.05);
            seconds -= step;
            AdvanceShots(step);
            UpdateWave(step);
        }

        AdvanceShots(seconds);
    }

    public bool StartNextWave()
    {
        if (Phase != GamePhase.Preparing)
            return false;

        WaveNumber++;
        enemiesToSpawn = WaveNumber + 2;
        spawnTimer = 0;
        Phase = GamePhase.WaveRunning;
        foreach (Tower tower in towers)
            tower.ResetCooldown();
        return true;
    }

    public void Restart()
    {
        towers.Clear();
        enemies.Clear();
        shots.Clear();
        enemiesToSpawn = 0;
        spawnTimer = 0;
        WaveNumber = 0;
        EnemiesDefeated = 0;
        EnemiesEscaped = 0;
        Money = StartingMoney;
        BaseHealth = MaxBaseHealth;
        Phase = GamePhase.Preparing;
    }

    private void UpdateWave(double seconds)
    {
        spawnTimer -= seconds;
        if (enemiesToSpawn > 0 && spawnTimer <= 1e-9)
        {
            enemies.Add(new Enemy(Map, WaveEnemyHealth));
            enemiesToSpawn--;
            spawnTimer += SpawnInterval;
        }

        // Прорвавшиеся враги сразу удаляются, чтобы урон не повторялся.
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemies[i];
            enemy.Move(seconds);
            if (enemy.HasReachedBase)
            {
                BaseHealth = Math.Max(0, BaseHealth - Enemy.BaseDamage);
                EnemiesEscaped++;
                enemies.RemoveAt(i);
                if (BaseHealth == 0)
                {
                    Phase = GamePhase.Defeat;
                    enemiesToSpawn = 0;
                    enemies.Clear();
                    shots.Clear();
                    return;
                }
            }
        }

        foreach (Tower tower in towers)
        {
            Enemy? target = tower.Attack(enemies, seconds);
            if (target == null)
                continue;

            shots.Add(new Shot(tower.Position, target));
            if (target.IsDefeated)
            {
                Money += Enemy.Reward;
                EnemiesDefeated++;
                enemies.Remove(target);
            }
        }

        if (EnemiesRemaining == 0)
            Phase = WaveNumber == TotalWaves ? GamePhase.Victory : GamePhase.Preparing;
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
