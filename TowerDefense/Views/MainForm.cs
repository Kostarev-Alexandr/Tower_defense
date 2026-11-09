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

    public void RefreshGame()
    {
        if (game == null)
            return;

        moneyLabel.Text = $"Деньги: {game.Money}     Стоимость башни: {GameState.TowerCost}";
        hintLabel.Text = game.Enemy.HasReachedBase
            ? "Враг достиг базы и остановился. Строительство доступно.\nДля повторного прохода закройте окно и запустите проект снова."
            : "Красный круг — враг. Синие башни стоят рядом с дорогой.\nВраг движется от входа к базе со скоростью 2 клетки в секунду.";
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
                buildStatusLabel.Text = "В этой клетке уже стоит башня.";
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
