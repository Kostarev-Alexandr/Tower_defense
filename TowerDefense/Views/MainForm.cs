using TowerDefense.Model;

namespace TowerDefense.Views;

// Окно отображает состояние игры и сообщает контроллеру о срабатываниях таймера.
public partial class MainForm : Form
{
    private GameState? game;

    public event EventHandler? UpdateRequested;
    public event EventHandler<CellClickedEventArgs>? CellClicked;

    public MainForm()
    {
        InitializeComponent();
        gameField.CellClicked += OnCellClicked;
    }

    public void DisplayGame(GameState gameState)
    {
        game = gameState;
        gameField.DisplayGame(gameState);
        RefreshGame();
    }

    public void StartUpdates()
    {
        updateTimer.Start();
    }

    public void StopUpdates()
    {
        updateTimer.Stop();
    }

    public void SelectTower(Tower? tower)
    {
        gameField.SelectTower(tower);
    }

    public void RefreshGame()
    {
        if (game == null)
            return;

        moneyLabel.Text = $"База: {game.BaseHealth}/{GameState.MaxBaseHealth}     Монеты: {game.Money}     Цена башни: {GameState.TowerCost}";
        if (game.Enemy.IsDefeated)
            hintLabel.Text = $"Враг уничтожен. Награда — {Enemy.Reward} монет.\nДля повтора запустите игру заново.";
        else if (game.Enemy.HasReachedBase)
            hintLabel.Text = $"Враг прорвался. База потеряла {Enemy.BaseDamage} здоровья.\nДля повтора запустите игру заново.";
        else
            hintLabel.Text = "Башни автоматически стреляют, когда враг входит в радиус.";
        gameField.Invalidate();
    }

    public void ShowBuildResult(BuildResult result)
    {
        // Модель возвращает причину, а представление выбирает текст сообщения.
        switch (result)
        {
            case BuildResult.Success:
                buildStatusLabel.Text = "Башня построена.";
                break;
            case BuildResult.OutsideMap:
                buildStatusLabel.Text = "Строить можно только внутри поля.";
                break;
            case BuildResult.InvalidTerrain:
                buildStatusLabel.Text = "На дороге, входе врагов и базе строить нельзя.";
                break;
            case BuildResult.Occupied:
                buildStatusLabel.Text = $"Башня выбрана. Урон {Tower.Damage}, радиус {Tower.AttackRange:0.#} кл.";
                break;
            case BuildResult.NotEnoughMoney:
                buildStatusLabel.Text = "Недостаточно денег для строительства башни.";
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
}
