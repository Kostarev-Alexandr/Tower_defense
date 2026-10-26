using TowerDefense.Model;
using TowerDefense.Views;

namespace TowerDefense.Controllers;

public class GameController
{
    private readonly GameMap map;

    public MainForm View { get; }

    public GameController()
    {
        map = new GameMap();
        View = new MainForm();
        View.DisplayMap(map);
    }
}
