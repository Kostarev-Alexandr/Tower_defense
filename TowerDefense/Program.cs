namespace TowerDefense;

static class Program
{
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        var controller = new Controllers.GameController();
        Application.Run(controller.View);
    }    
}
