namespace TowerDefense.Model;

public class GameState
{
    public GameMap Map { get; }
    public Enemy Enemy { get; }
    public Tower Tower { get; }

    public GameState()
    {
        Map = new GameMap();
        Enemy = new Enemy(Map);
        Tower = new Tower(new GridPosition(4, 3));
    }

    public void Update(double seconds)
    {
        Enemy.Move(seconds);
    }
}
