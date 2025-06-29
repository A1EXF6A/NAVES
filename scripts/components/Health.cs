using Godot;

public partial class Health : Node, IDamageable
{
	[Export] public int MaxHealth { get; set; } = 100;
	[Export] public int CurrentHealth { get; private set; }
	
	[Signal] public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);
	[Signal] public delegate void DiedEventHandler();
	
	public bool IsAlive => CurrentHealth > 0;
	
	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
	}
	
	public void TakeDamage(int damage)
	{
		CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
		EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
		
		if (CurrentHealth <= 0)
		{
			EmitSignal(SignalName.Died);
		}
	}
	
	public void Heal(int amount)
	{
		CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
		EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
	}
	
	public void SetMaxHealth(int maxHealth)
	{
		MaxHealth = maxHealth;
		CurrentHealth = maxHealth;
	}
}
