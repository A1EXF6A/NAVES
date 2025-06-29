using Godot;

public interface IDamageable
{
	void TakeDamage(int damage);
	bool IsAlive { get; }
	int CurrentHealth { get; }
	int MaxHealth { get; }
}
