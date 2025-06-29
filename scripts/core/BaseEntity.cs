using Godot;

public abstract partial class BaseEntity : Area2D, IInitializable
{
	protected Health _healthComponent;
	public Movement _movementComponent;
	
	[Signal] public delegate void EntityDestroyedEventHandler(BaseEntity entity);
	
	public virtual void Initialize()
	{
		_healthComponent = GetNode<Health>("Health");
		_movementComponent = GetNode<Movement>("Movement");
		
		if (_healthComponent != null)
		{
			_healthComponent.Died += OnDied;
		}
		
		// Configurar estrategia de movimiento por defecto
		ConfigureMovementStrategy();
	}
	
	protected virtual void ConfigureMovementStrategy()
	{
		// Las clases derivadas pueden sobrescribir esto para usar diferentes estrategias
		_movementComponent?.SetMovementStrategy(new LinearMovementStrategy());
	}
	
	public override void _Ready()
	{
		Initialize();
	}
	
	public virtual void TakeDamage(int damage)
	{
		_healthComponent?.TakeDamage(damage);
	}
	
	protected virtual void OnDied()
	{
		EmitSignal(SignalName.EntityDestroyed, this);
		QueueFree();
	}
	
	public virtual void OnVisibleOnScreenExited()
	{
		QueueFree();
	}
}
