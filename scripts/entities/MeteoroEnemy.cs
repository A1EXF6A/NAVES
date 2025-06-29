using Godot;

public partial class MeteoroEnemy : Enemy
{
	public override void Initialize()
	{
		base.Initialize();
		
		// Configuración específica del meteoro
		Speed = 60.0f;
		Value = 200;
		Damage = 30;
		
		if (_healthComponent != null)
		{
			_healthComponent.SetMaxHealth(50);
		}
		
		if (_movementComponent != null)
		{
			_movementComponent.SetMovementParameters(Speed, Vector2.Down);
		}
	}
	
	protected override void ConfigureMovementStrategy()
	{
		// Los meteoros usan movimiento serpenteante
		_movementComponent?.SetMovementStrategy(new WobbleMovementStrategy());
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_movementComponent?.Move(delta);
	}
	
	protected override void OnDied()
	{
		// Efecto de sonido específico para meteoro
		AudioService.Instance?.PlayExplosion();
		
		base.OnDied();
	}
}
