using TowerDefense.Model;

namespace TowerDefense.Views;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    public void DisplayMap(GameMap map)
    {
        gameField.DisplayMap(map);
    }
}
