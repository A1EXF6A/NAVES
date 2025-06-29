using Godot;

public partial class LevelManager : Node, IInitializable
{
	private uint _currentLevel = 1;
	private uint _scoreForNextLevel = 2000; // Puntos necesarios para el siguiente nivel
	private const uint SCORE_INCREMENT_PER_LEVEL = 2000; // Incremento de puntos por nivel
	
	// Modificadores de dificultad por nivel
	[Export] public float SpeedMultiplierPerLevel { get; set; } = 0.2f; // 20% más rápido por nivel
	[Export] public float SpawnRateMultiplierPerLevel { get; set; } = 0.15f; // 15% más spawn por nivel
	[Export] public float MaxSpeedMultiplier { get; set; } = 3.0f; // Máximo 300% de velocidad
	[Export] public float MaxSpawnRateMultiplier { get; set; } = 2.5f; // Máximo 250% de spawn rate
	
	[Signal] public delegate void LevelChangedEventHandler(uint newLevel);
	[Signal] public delegate void DifficultyUpdatedEventHandler(float speedMultiplier, float spawnRateMultiplier);
	
	public uint CurrentLevel => _currentLevel;
	public uint ScoreForNextLevel => _scoreForNextLevel;
	public float CurrentSpeedMultiplier { get; private set; } = 1.0f;
	public float CurrentSpawnRateMultiplier { get; private set; } = 1.0f;
	
	public override void _Ready()
	{
		Initialize();
	}
	
	public void Initialize()
	{
		CalculateCurrentMultipliers();
	}
	
	public void CheckLevelUp(uint currentScore)
	{
		if (currentScore >= _scoreForNextLevel)
		{
			LevelUp();
		}
	}
	
	private void LevelUp()
	{
		_currentLevel++;
		_scoreForNextLevel = _currentLevel * SCORE_INCREMENT_PER_LEVEL;
		
		CalculateCurrentMultipliers();
		
		GD.Print($"🎉 LEVEL UP! Nivel {_currentLevel}");
		GD.Print($"📈 Velocidad: {CurrentSpeedMultiplier:F1}x, Spawn: {CurrentSpawnRateMultiplier:F1}x");
		GD.Print($"🎯 Siguiente nivel en: {_scoreForNextLevel} puntos");
		
		EmitSignal(SignalName.LevelChanged, _currentLevel);
		EmitSignal(SignalName.DifficultyUpdated, CurrentSpeedMultiplier, CurrentSpawnRateMultiplier);
	}
	
	private void CalculateCurrentMultipliers()
	{
		// Calcular multiplicadores basados en el nivel actual
		float levelBonus = (_currentLevel - 1);
		
		CurrentSpeedMultiplier = Mathf.Min(
			1.0f + (levelBonus * SpeedMultiplierPerLevel),
			MaxSpeedMultiplier
		);
		
		CurrentSpawnRateMultiplier = Mathf.Min(
			1.0f + (levelBonus * SpawnRateMultiplierPerLevel),
			MaxSpawnRateMultiplier
		);
	}
	
	// Método para obtener velocidad ajustada por nivel
	public float GetAdjustedSpeed(float baseSpeed)
	{
		return baseSpeed * CurrentSpeedMultiplier;
	}
	
	// Método para obtener tiempo de spawn ajustado por nivel
	public float GetAdjustedSpawnTime(float baseSpawnTime)
	{
		return baseSpawnTime / CurrentSpawnRateMultiplier;
	}
	
	// Método para reiniciar nivel (cuando se reinicia el juego)
	public void ResetLevel()
	{
		_currentLevel = 1;
		_scoreForNextLevel = SCORE_INCREMENT_PER_LEVEL;
		CalculateCurrentMultipliers();
		
		EmitSignal(SignalName.LevelChanged, _currentLevel);
		EmitSignal(SignalName.DifficultyUpdated, CurrentSpeedMultiplier, CurrentSpawnRateMultiplier);
	}
	
	// Información de nivel para debug
	public string GetLevelInfo()
	{
		return $"Nivel {_currentLevel} | Velocidad: {CurrentSpeedMultiplier:F1}x | Spawn: {CurrentSpawnRateMultiplier:F1}x | Siguiente: {_scoreForNextLevel}";
	}
}
