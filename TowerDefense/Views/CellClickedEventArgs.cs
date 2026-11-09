namespace TowerDefense.Views;

// Передаёт контроллеру координаты клетки, выбранной на поле.
public class CellClickedEventArgs : EventArgs
{
    public int Column { get; }
    public int Row { get; }

    public CellClickedEventArgs(int column, int row)
    {
        Column = column;
        Row = row;
    }
}
