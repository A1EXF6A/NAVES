using Godot;

public static class Constants
{
	// Game Settings
	public const int TotalEnemyTypes = 4; // Actualizado de 3 a 4
	public const int ScrollSpeed = 90;
	public const string SaveGameFile = "user://savegame.data";
	
	// Groups
	public const string EnemyGroup = "enemy";
	public const string PlayerGroup = "player";
	
	// Player Settings
	public const float PlayerSpeed = 300.0f;
	public const int DefaultFireRate = 250;
	public const int DefaultPlayerHealth = 40;
	
	// Enemy Settings
	public const float DefaultEnemySpeed = 100.0f;
	public const uint DefaultEnemyValue = 100;
	public const int DefaultEnemyHealth = 20;
	public const uint DefaultEnemyDamage = 20;
	
	// Laser Settings
	public const float DefaultLaserSpeed = 600.0f;
	public const int DefaultLaserDamage = 10;
	public const string LaserScenePath = "res://scenes/laser.tscn";
	
	// Scene Paths - NUEVAS RUTAS
	public const string EnemyScenePath = "res://scenes/enemy.tscn";
	public const string RamEnemyScenePath = "res://scenes/ram_enemy.tscn";
	public const string DiverEnemyScenePath = "res://scenes/diver_enemy.tscn";
	public const string MeteoroEnemyScenePath = "res://scenes/meteoro_enemy.tscn";
	
	// Timeouts
	public const double GameLoadTimeout = 1.5;
	public const double PlayerDeathTimeout = 1.5;
}
