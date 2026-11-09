namespace TowerDefense.Model;

// Результат попытки строительства без привязки к тексту интерфейса.
public enum BuildResult
{
    Success,
    OutsideMap,
    InvalidTerrain,
    Occupied,
    NotEnoughMoney
}
