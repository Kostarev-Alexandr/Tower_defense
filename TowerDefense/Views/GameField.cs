using TowerDefense.Model;

namespace TowerDefense.Views;

// Рисует игровой мир и переводит щелчок мыши из пикселей в координаты клетки.
public class GameField : Control
{
    private GameState? game;

    public event EventHandler<CellClickedEventArgs>? CellClicked;

    public GameField()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    public void DisplayGame(GameState gameState)
    {
        game = gameState;
        Invalidate();
    }

    // Отрисовка и обработка мыши используют одинаковые границы карты.
    private Rectangle GetMapBounds()
    {
        if (game == null)
            return Rectangle.Empty;

        int cellSize = Math.Min(ClientSize.Width / game.Map.Width, ClientSize.Height / game.Map.Height);
        int width = game.Map.Width * cellSize;
        int height = game.Map.Height * cellSize;
        return new Rectangle((ClientSize.Width - width) / 2, (ClientSize.Height - height) / 2, width, height);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);

        if (game == null || e.Button != MouseButtons.Left)
            return;

        Rectangle bounds = GetMapBounds();
        if (!bounds.Contains(e.Location))
            return;

        int cellSize = bounds.Width / game.Map.Width;
        int column = (e.X - bounds.Left) / cellSize;
        int row = (e.Y - bounds.Top) / cellSize;
        CellClicked?.Invoke(this, new CellClickedEventArgs(column, row));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (game == null)
            return;

        GameMap map = game.Map;
        Rectangle bounds = GetMapBounds();
        int cellSize = bounds.Width / map.Width;
        if (cellSize < 1)
            return;

        int left = bounds.Left;
        int top = bounds.Top;

        using var grassBrush = new SolidBrush(Color.FromArgb(126, 166, 102));
        using var roadBrush = new SolidBrush(Color.FromArgb(222, 202, 153));
        using var spawnBrush = new SolidBrush(Color.FromArgb(85, 158, 186));
        using var baseBrush = new SolidBrush(Color.FromArgb(185, 91, 82));
        using var gridPen = new Pen(Color.FromArgb(74, 91, 65));
        using var markerFont = new Font("Segoe UI", cellSize / 3F, FontStyle.Bold, GraphicsUnit.Pixel);
        using var textFormat = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        for (int column = 0; column < map.Width; column++)
        {
            for (int row = 0; row < map.Height; row++)
            {
                CellType cellType = map.GetCellType(column, row);
                Brush brush = grassBrush;

                switch (cellType)
                {
                    case CellType.Road: brush = roadBrush; break;
                    case CellType.Spawn: brush = spawnBrush; break;
                    case CellType.Base: brush = baseBrush; break;
                }

                var rectangle = new Rectangle(left + column * cellSize, top + row * cellSize, cellSize, cellSize);
                e.Graphics.FillRectangle(brush, rectangle);
                e.Graphics.DrawRectangle(gridPen, rectangle.X, rectangle.Y, cellSize - 1, cellSize - 1);

                if (cellType == CellType.Spawn)
                    e.Graphics.DrawString("В", markerFont, Brushes.White, rectangle, textFormat);
                else if (cellType == CellType.Base)
                    e.Graphics.DrawString("Б", markerFont, Brushes.White, rectangle, textFormat);
            }
        }

        foreach (Tower tower in game.Towers)
            DrawTower(e.Graphics, tower, left, top, cellSize);
        DrawEnemy(e.Graphics, game.Enemy, left, top, cellSize);
    }

    private void DrawTower(Graphics graphics, Tower tower, int left, int top, int cellSize)
    {
        float x = left + (tower.Position.Column + 0.5F) * cellSize;
        float y = top + (tower.Position.Row + 0.5F) * cellSize;
        float size = cellSize * 0.65F;
        using var towerBrush = new SolidBrush(Color.FromArgb(44, 78, 135));
        using var barrelPen = new Pen(Color.FromArgb(28, 46, 76), cellSize * 0.12F);

        graphics.FillRectangle(towerBrush, x - size / 2, y - size / 2, size, size);
        graphics.DrawLine(barrelPen, x, y, x - cellSize * 0.45F, y);
    }

    private void DrawEnemy(Graphics graphics, Enemy enemy, int left, int top, int cellSize)
    {
        float x = left + ((float)enemy.Column + 0.5F) * cellSize;
        float y = top + ((float)enemy.Row + 0.5F) * cellSize;
        float size = cellSize * 0.55F;
        using var enemyBrush = new SolidBrush(Color.FromArgb(205, 49, 50));
        using var outlinePen = new Pen(Color.FromArgb(100, 30, 30), 2);

        graphics.FillEllipse(enemyBrush, x - size / 2, y - size / 2, size, size);
        graphics.DrawEllipse(outlinePen, x - size / 2, y - size / 2, size, size);
    }
}
