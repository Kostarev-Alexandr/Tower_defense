using TowerDefense.Model;

namespace TowerDefense.Views;

public class GameField : Control
{
    private GameMap? map;

    public GameField()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    public void DisplayMap(GameMap gameMap)
    {
        map = gameMap;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (map == null)
            return;

        int cellSize = Math.Min(ClientSize.Width / map.Width, ClientSize.Height / map.Height);
        if (cellSize < 1)
            return;

        int left = (ClientSize.Width - map.Width * cellSize) / 2;
        int top = (ClientSize.Height - map.Height * cellSize) / 2;

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
    }
}
