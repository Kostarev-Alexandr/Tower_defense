using TowerDefense.Model;

namespace TowerDefense.Tests;

// Проверяет переходы между подготовкой, волнами и результатом партии.
public class WaveTests
{
    [Test]
    public void NewGame_WaitsForManualStart()
    {
        var game = new GameState();

        game.Update(100);

        Assert.That(game.Phase, Is.EqualTo(GamePhase.Preparing));
        Assert.That(game.WaveNumber, Is.Zero);
        Assert.That(game.EnemiesRemaining, Is.Zero);
        Assert.That(game.Enemies, Is.Empty);
        Assert.That(game.BaseHealth, Is.EqualTo(100));
    }

    [Test]
    public void StartNextWave_CannotStartAnotherWhileRunning()
    {
        var game = new GameState();

        Assert.That(game.StartNextWave(), Is.True);
        Assert.That(game.StartNextWave(), Is.False);
        Assert.That(game.WaveNumber, Is.EqualTo(1));
        Assert.That(game.EnemiesRemaining, Is.EqualTo(3));
    }

    [Test]
    public void Update_SpawnsEnemiesGraduallyAndKeepsPendingCount()
    {
        var game = new GameState();
        game.StartNextWave();

        game.Update(0.45);
        Assert.That(game.Enemies.Count, Is.EqualTo(1));
        Assert.That(game.EnemiesRemaining, Is.EqualTo(3));
        game.Update(0.45);
        Assert.That(game.Enemies.Count, Is.EqualTo(2));
        Assert.That(game.Enemies[0].Column, Is.GreaterThan(game.Enemies[1].Column));
        game.Update(0.9);
        Assert.That(game.Enemies.Count, Is.EqualTo(3));
    }

    [Test]
    public void LastEnemy_MustFinishBeforePreparationResumes()
    {
        var game = new GameState();
        game.StartNextWave();

        game.Update(9.5);
        Assert.That(game.Phase, Is.EqualTo(GamePhase.WaveRunning));
        Assert.That(game.EnemiesRemaining, Is.EqualTo(1));
        game.Update(2);
        Assert.That(game.Phase, Is.EqualTo(GamePhase.Preparing));
        Assert.That(game.EnemiesRemaining, Is.Zero);
        Assert.That(game.BaseHealth, Is.EqualTo(25));
        game.Update(100);
        Assert.That(game.WaveNumber, Is.EqualTo(1));
        Assert.That(game.BaseHealth, Is.EqualTo(25));
        Assert.That(game.BuildTower(4, 3), Is.EqualTo(BuildResult.Success));
    }

    [Test]
    public void SecondWave_HasMoreEnemiesAndMoreHealth()
    {
        var game = new GameState();
        game.StartNextWave();
        game.Update(30);

        Assert.That(game.StartNextWave(), Is.True);
        Assert.That(game.EnemiesRemaining, Is.EqualTo(4));
        game.Update(0.05);
        Assert.That(game.Enemies[0].MaxHealth, Is.EqualTo(100));
        Assert.That(game.Enemies[0].Health, Is.EqualTo(100));
    }

    [Test]
    public void BaseDestroyed_EndsGameAndRejectsMoreWavesAndBuilding()
    {
        var game = new GameState();
        game.StartNextWave();
        game.Update(30);
        game.StartNextWave();

        game.Update(100);

        Assert.That(game.Phase, Is.EqualTo(GamePhase.Defeat));
        Assert.That(game.BaseHealth, Is.Zero);
        Assert.That(game.EnemiesEscaped, Is.EqualTo(4));
        Assert.That(game.EnemiesRemaining, Is.Zero);
        Assert.That(game.StartNextWave(), Is.False);
        Assert.That(game.BuildTower(4, 3), Is.EqualTo(BuildResult.GameOver));
        game.Update(100);
        Assert.That(game.Money, Is.EqualTo(150));
        Assert.That(game.BaseHealth, Is.Zero);
        Assert.That(game.EnemiesEscaped, Is.EqualTo(4));
    }

    [Test]
    public void FiveDefendedWaves_EndInVictory()
    {
        var game = new GameState();
        game.BuildTower(1, 2);
        game.BuildTower(4, 2);
        game.BuildTower(7, 6);
        GridPosition[] extraPositions =
        {
            new GridPosition(4, 3),
            new GridPosition(6, 4),
            new GridPosition(8, 4),
            new GridPosition(2, 2),
            new GridPosition(6, 2),
            new GridPosition(8, 2)
        };
        int nextPosition = 0;
        int[] expectedHealth = { 60, 100, 160, 220, 300 };

        for (int wave = 1; wave <= GameState.TotalWaves; wave++)
        {
            Assert.That(game.StartNextWave(), Is.True);
            game.Update(0.05);
            Assert.That(game.Enemies[0].MaxHealth, Is.EqualTo(expectedHealth[wave - 1]));
            game.Update(30);
            Assert.That(game.BaseHealth, Is.GreaterThan(0), $"Волна {wave}");
            Assert.That(game.WaveNumber, Is.EqualTo(wave));

            if (wave < GameState.TotalWaves)
            {
                Assert.That(game.Phase, Is.EqualTo(GamePhase.Preparing));
                while (game.Money >= GameState.TowerCost && nextPosition < extraPositions.Length)
                {
                    GridPosition position = extraPositions[nextPosition++];
                    Assert.That(game.BuildTower(position.Column, position.Row), Is.EqualTo(BuildResult.Success));
                }
            }
        }

        Assert.That(game.Phase, Is.EqualTo(GamePhase.Victory));
        Assert.That(game.EnemiesRemaining, Is.Zero);
        Assert.That(game.EnemiesDefeated + game.EnemiesEscaped, Is.EqualTo(25));
        Assert.That(game.StartNextWave(), Is.False);
        Assert.That(game.BuildTower(10, 0), Is.EqualTo(BuildResult.GameOver));
    }

    [Test]
    public void StartingDefence_RequiresReinforcementsForLaterWaves()
    {
        var game = new GameState();
        game.BuildTower(1, 2);
        game.BuildTower(4, 2);
        game.BuildTower(7, 6);

        for (int wave = 0; wave < GameState.TotalWaves && !game.IsGameOver; wave++)
        {
            game.StartNextWave();
            game.Update(30);
        }

        Assert.That(game.Phase, Is.EqualTo(GamePhase.Defeat));
        Assert.That(game.WaveNumber, Is.GreaterThanOrEqualTo(3));
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Restart_ClearsActiveAndFinishedGame(bool finish)
    {
        var game = new GameState();
        game.StartNextWave();
        if (finish)
        {
            game.Update(30);
            game.StartNextWave();
            game.Update(30);
        }
        else
        {
            game.BuildTower(1, 2);
            game.Update(0.05);
            Assert.That(game.Shots, Is.Not.Empty);
        }

        game.Restart();

        Assert.That(game.Phase, Is.EqualTo(GamePhase.Preparing));
        Assert.That(game.WaveNumber, Is.Zero);
        Assert.That(game.EnemiesRemaining, Is.Zero);
        Assert.That(game.Enemies, Is.Empty);
        Assert.That(game.Towers, Is.Empty);
        Assert.That(game.Shots, Is.Empty);
        Assert.That(game.Money, Is.EqualTo(150));
        Assert.That(game.BaseHealth, Is.EqualTo(100));
        Assert.That(game.EnemiesDefeated, Is.Zero);
        Assert.That(game.EnemiesEscaped, Is.Zero);
        game.Update(1);
        Assert.That(game.Enemies, Is.Empty);
        Assert.That(game.StartNextWave(), Is.True);
        game.Update(0.05);
        Assert.That(game.Enemies[0].MaxHealth, Is.EqualTo(60));
    }
}
