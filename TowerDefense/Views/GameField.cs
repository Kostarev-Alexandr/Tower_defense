using System.Drawing.Drawing2D;
using TowerDefense.Model;

namespace TowerDefense.Views;

// Рисует игровой мир и переводит щелчок мыши из пикселей в координаты клетки.
public class GameField : Control
{
    private GameState? game;
    private Tower? selectedTower;
    private Point? hoveredCell;

    public event EventHandler<CellClickedEventArgs>? CellClicked;

    public GameField()
    {
        BackColor = Color.FromArgb(220, 232, 222);
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    public void DisplayGame(GameState gameState)
    {
        game = gameState;
        Invalidate();
    }

    public void SelectTower(Tower? tower)
    {
        selectedTower = tower;
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

    private Point? GetCellAt(Point location)
    {
        if (game == null)
            return null;

        Rectangle bounds = GetMapBounds();
        if (!bounds.Contains(location))
            return null;

        int cellSize = bounds.Width / game.Map.Width;
        return new Point((location.X - bounds.Left) / cellSize, (location.Y - bounds.Top) / cellSize);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);

        if (e.Button == MouseButtons.Left && GetCellAt(e.Location) is Point cell)
            CellClicked?.Invoke(this, new CellClickedEventArgs(cell.X, cell.Y));
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        SetHoveredCell(GetCellAt(e.Location));
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        SetHoveredCell(null);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (hoveredCell.HasValue)
            SetHoveredCell(GetCellAt(PointToClient(Cursor.Position)));
    }

    private void SetHoveredCell(Point? cell)
    {
        if (hoveredCell == cell)
            return;

        hoveredCell = cell;
        Invalidate();
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

        using var grassLight = new SolidBrush(Color.FromArgb(161, 195, 145));
        using var grassDark = new SolidBrush(Color.FromArgb(150, 186, 135));
        using var roadBrush = new SolidBrush(Color.FromArgb(224, 205, 167));
        using var spawnBrush = new SolidBrush(Color.FromArgb(129, 71, 64));
        using var baseBrush = new SolidBrush(Color.FromArgb(47, 99, 111));
        using var gridPen = new Pen(Color.FromArgb(45, 46, 77, 59));
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
                Brush brush = (column + row) % 2 == 0 ? grassLight : grassDark;

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

        // Радиус обрезается краями карты и лежит под игровыми объектами.
        GraphicsState mapLayer = e.Graphics.Save();
        e.Graphics.SetClip(bounds, CombineMode.Intersect);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        PointF? previewCenter = null;
        if (hoveredCell is Point cell &&
            map.IsBuildableCell(cell.X, cell.Y) &&
            game.GetTowerAt(cell.X, cell.Y) == null)
        {
            PointF center = GetCenter(cell.X, cell.Y, left, top, cellSize);
            previewCenter = center;
            DrawRange(e.Graphics, center, cellSize, isPreview: true);
        }

        foreach (Tower tower in game.Towers)
        {
            PointF center = GetCenter(tower.Position.Column, tower.Position.Row, left, top, cellSize);
            DrawRange(e.Graphics, center, cellSize);
        }

        foreach (Tower tower in game.Towers)
        {
            PointF center = GetCenter(tower.Position.Column, tower.Position.Row, left, top, cellSize);
            DrawTower(e.Graphics, center, cellSize, tower == selectedTower);
        }

        if (previewCenter is PointF preview)
            DrawTower(e.Graphics, preview, cellSize, selected: false, opacity: 115);

        if (!game.IsEncounterFinished)
            DrawEnemy(e.Graphics, game.Enemy, left, top, cellSize);

        using var shotPen = new Pen(Color.FromArgb(255, 244, 168), Math.Max(2F, cellSize * 0.06F));
        foreach (Shot shot in game.Shots)
        {
            PointF origin = GetCenter(shot.Origin.Column, shot.Origin.Row, left, top, cellSize);
            PointF target = GetCenter(shot.TargetColumn, shot.TargetRow, left, top, cellSize);
            e.Graphics.DrawLine(shotPen, origin, target);
        }

        e.Graphics.Restore(mapLayer);
        using var borderPen = new Pen(Color.FromArgb(61, 89, 77), 2F);
        e.Graphics.DrawRectangle(borderPen, bounds.Left, bounds.Top, bounds.Width - 1, bounds.Height - 1);
    }

    private static PointF GetCenter(double column, double row, int left, int top, int cellSize)
    {
        return new PointF(left + ((float)column + 0.5F) * cellSize, top + ((float)row + 0.5F) * cellSize);
    }

    private static void DrawRange(Graphics graphics, PointF center, int cellSize, bool isPreview = false)
    {
        float radius = (float)Tower.AttackRange * cellSize;
        using var fill = new SolidBrush(Color.FromArgb(isPreview ? 7 : 14, 36, 115, 155));
        using var outline = new Pen(Color.FromArgb(isPreview ? 85 : 175, 41, 100, 134), 1.5F)
        {
            DashStyle = DashStyle.Dash
        };
        graphics.FillEllipse(fill, center.X - radius, center.Y - radius, radius * 2, radius * 2);
        graphics.DrawEllipse(outline, center.X - radius, center.Y - radius, radius * 2, radius * 2);
    }

    private static void DrawTower(Graphics graphics, PointF center, int cellSize, bool selected, int opacity = 255)
    {
        float size = cellSize * 0.72F;
        var pedestal = new RectangleF(center.X - size / 2, center.Y - size / 2, size, size);
        using var baseBrush = new SolidBrush(Color.FromArgb(opacity, 40, 65, 78));
        using var turretBrush = new SolidBrush(Color.FromArgb(opacity, 100, 184, 207));
        using var barrelPen = new Pen(Color.FromArgb(opacity, 211, 239, 242), cellSize * 0.11F)
        {
            EndCap = LineCap.Round
        };

        graphics.FillEllipse(baseBrush, pedestal);
        if (selected)
            graphics.DrawEllipse(Pens.White, pedestal);
        graphics.DrawLine(barrelPen, center.X, center.Y, center.X, center.Y - cellSize * 0.38F);
        float turretSize = size * 0.59F;
        graphics.FillEllipse(turretBrush, center.X - turretSize / 2, center.Y - turretSize / 2, turretSize, turretSize);
    }

    private static void DrawEnemy(Graphics graphics, Enemy enemy, int left, int top, int cellSize)
    {
        PointF center = GetCenter(enemy.Column, enemy.Row, left, top, cellSize);
        float size = cellSize * 0.52F;
        using var enemyBrush = new SolidBrush(Color.FromArgb(199, 74, 59));
        using var outline = new Pen(Color.FromArgb(117, 47, 44), 2F);
        using var barBackground = new SolidBrush(Color.FromArgb(51, 51, 47));
        using var healthBrush = new SolidBrush(Color.FromArgb(125, 221, 119));

        graphics.FillEllipse(enemyBrush, center.X - size / 2, center.Y - size / 2, size, size);
        graphics.DrawEllipse(outline, center.X - size / 2, center.Y - size / 2, size, size);

        float barWidth = cellSize * 0.72F;
        float barHeight = Math.Max(3F, cellSize * 0.09F);
        float barX = center.X - barWidth / 2;
        float barY = center.Y - size / 2 - barHeight - 3F;
        float healthFraction = enemy.Health / (float)Enemy.MaxHealth;
        graphics.FillRectangle(barBackground, barX, barY, barWidth, barHeight);
        graphics.FillRectangle(healthBrush, barX, barY, barWidth * healthFraction, barHeight);
    }
}
