using Godot;

public partial class Laser : BaseEntity
{
	[Export(PropertyHint.Range, "0,600,1,or_greater")]
	public float Speed { get; set; } = Constants.DefaultLaserSpeed;

	[Export(PropertyHint.Range, "0,10,1,or_greater")]
	public int Damage { get; set; } = Constants.DefaultLaserDamage;

	public override void Initialize()
	{
		base.Initialize();
		
		if (_movementComponent != null)
		{
			_movementComponent.SetMovementParameters(Speed, Vector2.Up);
		}
		
		AreaEntered += OnAreaEntered;
	}

	protected override void ConfigureMovementStrategy()
	{
		// Los láseres usan movimiento lineal simple
		_movementComponent?.SetMovementStrategy(new LinearMovementStrategy());
	}

	public override void _PhysicsProcess(double delta)
	{
		_movementComponent?.Move(delta);
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup(Constants.EnemyGroup) && area is IDamageable damageable)
		{
			damageable.TakeDamage(Damage);
			QueueFree();
		}
	}
}
