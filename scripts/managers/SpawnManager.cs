using Godot;
using Godot.Collections;

public partial class SpawnManager : Node, IUpdatable, IInitializable
{
	[Export] public Array<PackedScene> EnemyScenes { get; set; } = new();
	[Export] public float BaseMinSpawnTime { get; set; } = 1.0f; // Tiempo base mínimo
	[Export] public float SpawnReduction { get; set; } = 0.001f; //dificultad
	
	// PROPIEDADES PARA CONTROLAR METEOROS
	[Export] public float MeteoroSpawnChance { get; set; } = 0.30f;
	[Export] public float MinTimeBetweenMeteoros { get; set; } = 4.0f;
	
	private Timer _spawnTimer;
	private Node2D _enemyContainer;
	private RandomNumberGenerator _rng = new();
	private float _lastMeteoroSpawnTime = 0.0f;
	private float _gameTime = 0.0f;
	
	// NUEVAS VARIABLES PARA SISTEMA DE NIVELES
	private LevelManager _levelManager;	
	private float _currentMinSpawnTime;
	
	[Signal] public delegate void EnemySpawnedEventHandler(Enemy enemy);
	
	public override void _Ready()
	{
		Initialize();
	}
	
	public void Initialize()
	{
		_spawnTimer = GetNode<Timer>("SpawnTimer");
		_enemyContainer = GetParent().GetNode<Node2D>("EnemyContainer");
		
		// Obtener referencia al LevelManager
		_levelManager = GetParent().GetNodeOrNull<LevelManager>("LevelManager");
		
		// Inicializar tiempo de spawn actual
		_currentMinSpawnTime = BaseMinSpawnTime;
		
		if (_spawnTimer != null)
		{
			_spawnTimer.Timeout += OnSpawnTimerTimeout;
		}
		
		// Conectar a cambios de dificultad
		if (_levelManager != null)
		{
			_levelManager.DifficultyUpdated += OnDifficultyUpdated;
		}
	}
	
	public override void _Process(double delta)
	{
		UpdateLogic(delta);
		_gameTime += (float)delta;
	}
	
	public void UpdateLogic(double delta)
	{
		// Aplicar reducción de spawn time con modificador de nivel
		if (_spawnTimer != null && _spawnTimer.WaitTime > _currentMinSpawnTime)
		{
			_spawnTimer.WaitTime -= delta * SpawnReduction;
		}
	}
	
	private void OnDifficultyUpdated(float speedMultiplier, float spawnRateMultiplier)
	{
		// Ajustar el tiempo mínimo de spawn basado en el multiplicador de nivel
		_currentMinSpawnTime = _levelManager?.GetAdjustedSpawnTime(BaseMinSpawnTime) ?? BaseMinSpawnTime;
		
		GD.Print($"🔄 SpawnManager: Nuevo tiempo mínimo de spawn: {_currentMinSpawnTime:F2}s (Multiplicador: {spawnRateMultiplier:F1}x)");
		
		// Si el timer actual es mayor que el nuevo mínimo, ajustarlo inmediatamente
		if (_spawnTimer != null && _spawnTimer.WaitTime > _currentMinSpawnTime)
		{
			_spawnTimer.WaitTime = _currentMinSpawnTime;
		}
	}
	
	private void OnSpawnTimerTimeout()
	{
		SpawnRandomEnemy();
	}
	
	private void SpawnRandomEnemy()
	{
		if (EnemyScenes == null || EnemyScenes.Count == 0 || _enemyContainer == null) 
			return;
		
		var spawnX = _rng.RandfRange(0, 540);
		var spawnPosition = new Vector2(spawnX, -10);
		
		// Determinar si spawear un meteoro o enemigo normal
		PackedScene selectedScene = SelectEnemyScene();
		
		if (selectedScene != null)
		{
			var enemy = EnemyFactory.CreateEnemy(selectedScene, spawnPosition);
			
			if (enemy != null)
			{
				// APLICAR MULTIPLICADOR DE VELOCIDAD POR NIVEL
				ApplyLevelDifficultyToEnemy(enemy);
				
				_enemyContainer.AddChild(enemy);
				EmitSignal(SignalName.EnemySpawned, enemy);
				
				// Si es un meteoro, actualizar el tiempo del último spawn
				if (enemy is MeteoroEnemy)
				{
					_lastMeteoroSpawnTime = _gameTime;
					GD.Print($"🌑 Meteoro spawneado (Level {_levelManager?.CurrentLevel ?? 1}). Velocidad: {enemy.Speed:F0}");
				}
			}
		}
	}
	
	private void ApplyLevelDifficultyToEnemy(Enemy enemy)
	{
		if (_levelManager != null && enemy != null)
		{
			// Aplicar multiplicador de velocidad del nivel
			float adjustedSpeed = _levelManager.GetAdjustedSpeed(enemy.Speed);
			enemy.Speed = adjustedSpeed;
			
			// Actualizar el componente de movimiento del enemigo
			if (enemy._movementComponent != null)
			{
				enemy._movementComponent.SetMovementParameters(adjustedSpeed, enemy._movementComponent.Direction);
			}
		}
	}
	
	private PackedScene SelectEnemyScene()
	{
		if (EnemyScenes.Count == 0) return null;
		
		// Verificar si hay meteoros en la lista
		PackedScene meteoroScene = null;
		Array<PackedScene> nonMeteoroScenes = new();
		
		foreach (PackedScene scene in EnemyScenes)
		{
			// Identificar meteoro por el path de la escena
			if (scene.ResourcePath.Contains("meteoro") || scene.ResourcePath.Contains("Meteoro"))
			{
				meteoroScene = scene;
			}
			else
			{
				nonMeteoroScenes.Add(scene);
			}
		}
		
		// Decidir si spawear meteoro
		bool canSpawnMeteoro = meteoroScene != null && 
							  (_gameTime - _lastMeteoroSpawnTime) >= MinTimeBetweenMeteoros;
		
		bool shouldSpawnMeteoro = canSpawnMeteoro && _rng.Randf() < MeteoroSpawnChance;
		
		if (shouldSpawnMeteoro)
		{
			return meteoroScene;
		}
		else
		{
			// Spawear enemigo normal
			if (nonMeteoroScenes.Count > 0)
			{
				var randomIndex = _rng.RandiRange(0, nonMeteoroScenes.Count - 1);
				return nonMeteoroScenes[randomIndex];
			}
			else
			{
				// Si no hay enemigos normales, usar cualquiera
				var randomIndex = _rng.RandiRange(0, EnemyScenes.Count - 1);
				return EnemyScenes[randomIndex];
			}
		}
	}
	
	// Método para reiniciar dificultad cuando se reinicia el juego
	public void ResetDifficulty()
	{
		_currentMinSpawnTime = BaseMinSpawnTime;
		_gameTime = 0.0f;
		_lastMeteoroSpawnTime = 0.0f;
		
		if (_spawnTimer != null)
		{
			_spawnTimer.WaitTime = 1.0f; // Resetear a tiempo inicial
		}
	}
}
