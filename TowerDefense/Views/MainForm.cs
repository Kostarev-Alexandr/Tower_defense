using TowerDefense.Model;

namespace TowerDefense.Views;

// Окно отображает состояние игры и сообщает контроллеру о срабатываниях таймера.
public partial class MainForm : Form
{
    private GameState? game;

    public event EventHandler? UpdateRequested;
    public event EventHandler<CellClickedEventArgs>? CellClicked;
    public event EventHandler? NextWaveRequested;
    public event EventHandler? RestartRequested;

    public MainForm()
    {
        InitializeComponent();
        gameField.CellClicked += OnCellClicked;
    }

    public void DisplayGame(GameState gameState)
    {
        game = gameState;
        gameField.DisplayGame(gameState);
        buildStatusLabel.Text = "";
        RefreshGame();
    }

    public void StartUpdates()
    {
        updateTimer.Start();
    }

    public void SelectTower(Tower? tower)
    {
        gameField.SelectTower(tower);
    }

    public void RefreshGame()
    {
        if (game == null)
            return;

        baseValueLabel.Text = $"{game.BaseHealth} / {GameState.MaxBaseHealth}";
        healthFill.Width = healthTrack.Width * game.BaseHealth / GameState.MaxBaseHealth;
        healthFill.BackColor = game.BaseHealth <= 25
            ? Color.FromArgb(177, 74, 62)
            : Color.FromArgb(47, 117, 94);
        moneyValueLabel.Text = game.Money.ToString();
        waveLabel.Text = $"{game.WaveNumber} / {GameState.TotalWaves}";
        remainingLabel.Text = game.EnemiesRemaining.ToString();
        enemyHealthLabel.Text = $"{game.WaveEnemyHealth} здоровья";
        priceValueLabel.Text = $"{GameState.TowerCost} монет";
        priceValueLabel.ForeColor = game.Money >= GameState.TowerCost
            ? Color.FromArgb(166, 117, 39)
            : Color.FromArgb(125, 134, 140);
        towerStatsLabel.Text = $"Урон {Tower.Damage} · Радиус {Tower.AttackRange:0.#}";
        summaryLabel.Text = $"Башни {game.Towers.Count}      Уничтожено {game.EnemiesDefeated}      Прорвалось {game.EnemiesEscaped}";
        nextWaveButton.Enabled = game.Phase == GamePhase.Preparing;
        nextWaveButton.BackColor = nextWaveButton.Enabled
            ? Color.FromArgb(47, 117, 94)
            : Color.FromArgb(232, 238, 240);
        nextWaveButton.Text = game.WaveNumber == 0 ? "Начать волну" : "Следующая волна";
        phaseLabel.Text = "Подготовка";
        if (game.Phase == GamePhase.WaveRunning)
        {
            phaseLabel.Text = "Атака";
            nextWaveButton.Text = "Идёт волна";
        }
        else if (game.WaveNumber > 0 && !game.IsGameOver)
        {
            phaseLabel.Text = "Волна завершена";
        }

        resultPanel.Visible = game.IsGameOver;
        if (game.IsGameOver)
        {
            bool victory = game.Phase == GamePhase.Victory;
            phaseLabel.Text = victory ? "База защищена" : "База потеряна";
            resultTitleLabel.Text = victory ? "Победа!" : "Поражение";
            resultTitleLabel.ForeColor = victory ? Color.FromArgb(36, 111, 78) : Color.FromArgb(161, 64, 55);
            resultDetailsLabel.Text = $"Уничтожено {game.EnemiesDefeated}    ·    Прорвалось {game.EnemiesEscaped}\nЗдоровье базы {game.BaseHealth} / {GameState.MaxBaseHealth}";
            nextWaveButton.Text = "Партия завершена";
            resultPanel.Location = new Point(
                gameField.Left + (gameField.Width - resultPanel.Width) / 2,
                gameField.Top + (gameField.Height - resultPanel.Height) / 2);
            resultPanel.BringToFront();
        }

        gameField.Invalidate();
    }

    public void ShowBuildResult(BuildResult result)
    {
        // Модель возвращает причину, а представление выбирает текст сообщения.
        buildStatusLabel.ForeColor = result == BuildResult.Success || result == BuildResult.Occupied
            ? Color.FromArgb(47, 117, 94)
            : Color.FromArgb(177, 74, 62);
        switch (result)
        {
            case BuildResult.Success:
                buildStatusLabel.Text = $"Башня построена · −{GameState.TowerCost} монет";
                break;
            case BuildResult.OutsideMap:
                buildStatusLabel.Text = "Клетка за пределами поля";
                break;
            case BuildResult.InvalidTerrain:
                buildStatusLabel.Text = "Клетка недоступна для строительства";
                break;
            case BuildResult.Occupied:
                buildStatusLabel.Text = "Башня выбрана";
                break;
            case BuildResult.NotEnoughMoney:
                buildStatusLabel.Text = "Недостаточно монет";
                break;
            case BuildResult.GameOver:
                buildStatusLabel.Text = "Партия завершена";
                break;
        }
    }

    private void OnCellClicked(object? sender, CellClickedEventArgs e)
    {
        CellClicked?.Invoke(this, e);
    }

    private void OnUpdateTimerTick(object? sender, EventArgs e)
    {
        UpdateRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnNextWaveClick(object? sender, EventArgs e)
    {
        NextWaveRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnRestartClick(object? sender, EventArgs e)
    {
        RestartRequested?.Invoke(this, EventArgs.Empty);
    }
}
