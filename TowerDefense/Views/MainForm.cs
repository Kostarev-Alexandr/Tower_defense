using TowerDefense.Model;

namespace TowerDefense.Views;

// Окно отображает состояние игры и сообщает контроллеру о срабатываниях таймера.
public partial class MainForm : Form
{
    private GameState? game;

    public event EventHandler? UpdateRequested;

    public MainForm()
    {
        InitializeComponent();
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

        hintLabel.Text = game.Enemy.HasReachedBase
            ? "Враг достиг базы и остановился.\nЧтобы повторить движение, закройте окно и запустите проект снова."
            : "Красный круг — враг, синяя башня неподвижна.\nВраг движется от входа к базе со скоростью 2 клетки в секунду.";
        gameField.Invalidate();
    }

    private void OnUpdateTimerTick(object? sender, EventArgs e)
    {
        UpdateRequested?.Invoke(this, EventArgs.Empty);
    }
}
