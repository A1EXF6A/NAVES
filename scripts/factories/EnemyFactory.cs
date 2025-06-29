using Godot;
using Godot.Collections;

public static class EnemyFactory
{
	public static Enemy CreateEnemy(PackedScene enemyScene, Vector2 position)
	{
		if (enemyScene?.Instantiate() is Enemy enemy)
		{
			enemy.GlobalPosition = position;
			return enemy;
		}
		
		GD.PrintErr("Failed to create enemy from scene");
		return null;
	}
	
	public static Enemy CreateRandomEnemy(Array<PackedScene> enemyScenes, Vector2 position, RandomNumberGenerator rng)
	{
		if (enemyScenes == null || enemyScenes.Count == 0)
		{
			GD.PrintErr("No enemy scenes available");
			return null;
		}
		
		var randomIndex = rng.RandiRange(0, enemyScenes.Count - 1);
		var enemyScene = enemyScenes[randomIndex];
		
		return CreateEnemy(enemyScene, position);
	}
}
