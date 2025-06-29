using Godot;

public partial class Enemy : BaseEntity, IDamageable
{
	[Export(PropertyHint.Range, "0,600,1,or_greater")]
	public float Speed { get; set; } = Constants.DefaultEnemySpeed;

	[Export(PropertyHint.Range, "0,600,1,or_greater")]
	public uint Value { get; set; } = Constants.DefaultEnemyValue;

	[Export(PropertyHint.Range, "0,10,1,or_greater")]
	public uint Damage { get; set; } = Constants.DefaultEnemyDamage;

	[Signal] public delegate void KilledEventHandler(Enemy enemy);

	// HACER PÚBLICO EL COMPONENTE DE MOVIMIENTO PARA EL SISTEMA DE NIVELES
	public Movement MovementComponent => _movementComponent;

	public override void Initialize()
	{
		base.Initialize();
		AddToGroup(Constants.EnemyGroup);
		
		if (_healthComponent != null)
		{
			_healthComponent.SetMaxHealth(Constants.DefaultEnemyHealth);
		}
		
		if (_movementComponent != null)
		{
			_movementComponent.SetMovementParameters(Speed, Vector2.Down);
		}
		
		BodyEntered += OnBodyEntered;
	}

	protected override void ConfigureMovementStrategy()
	{
		// Enemigos básicos usan movimiento lineal
		_movementComponent?.SetMovementStrategy(new LinearMovementStrategy());
	}

	public override void _PhysicsProcess(double delta)
	{
		_movementComponent?.Move(delta);
	}

	protected override void OnDied()
	{
		EmitSignal(SignalName.Killed, this);
		base.OnDied();
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup(Constants.PlayerGroup))
		{
			if (body is Player player)
			{
				player.TakeDamage((int)Damage);
			}
			QueueFree();
		}
	}
	
	// Implementación de IDamageable
	public bool IsAlive => _healthComponent?.IsAlive ?? false;
	public int CurrentHealth => _healthComponent?.CurrentHealth ?? 0;
	public int MaxHealth => _healthComponent?.MaxHealth ?? 0;
}
