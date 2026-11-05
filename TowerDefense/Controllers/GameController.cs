using System.Diagnostics;
using TowerDefense.Model;
using TowerDefense.Views;

namespace TowerDefense.Controllers;

// Передаёт события формы модели и запрашивает обновление представления.
public class GameController
{
    private readonly GameState game;
    private readonly Stopwatch clock = new Stopwatch();

    public MainForm View { get; }

    public GameController()
    {
        game = new GameState();
        View = new MainForm();
        View.DisplayGame(game);
        View.Shown += OnViewShown;
        View.UpdateRequested += OnUpdateRequested;
    }

    private void OnViewShown(object? sender, EventArgs e)
    {
        clock.Start();
        View.StartUpdates();
    }

    private void OnUpdateRequested(object? sender, EventArgs e)
    {
        double seconds = clock.Elapsed.TotalSeconds;
        clock.Restart();
        game.Update(seconds);
        View.RefreshGame();

        if (game.Enemy.HasReachedBase)
        {
            View.StopUpdates();
            clock.Stop();
        }
    }
}
