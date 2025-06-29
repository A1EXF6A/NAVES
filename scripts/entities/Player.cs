using Godot;

public partial class Player : CharacterBody2D, IDamageable, IInitializable
{
	private Node2D _muzzle;
	private PackedScene _laserScene;
	private Health _healthComponent;
	private Movement _movementComponent;
	private Timer _fireRateTimer;
	
	// NUEVAS VARIABLES PARA PATRÓN COMMAND
	private CommandInvoker _commandInvoker;
	private Vector2 _currentMovementDirection = Vector2.Zero;
	
	// Comandos de movimiento reutilizables
	private MoveUpCommand _moveUpCommand;
	private MoveDownCommand _moveDownCommand;
	private MoveLeftCommand _moveLeftCommand;
	private MoveRightCommand _moveRightCommand;
	private StopCommand _stopCommand;

	[Export(PropertyHint.Range, "100,1000,1,or_greater")]
	public int FireRate { get; set; } = Constants.DefaultFireRate;

	[Signal] public delegate void LaserShotEventHandler(PackedScene laserScene, Vector2 location);
	[Signal] public delegate void KilledEventHandler();

	// Implementación de IDamageable
	public bool IsAlive => _healthComponent?.IsAlive ?? false;
	public int CurrentHealth => _healthComponent?.CurrentHealth ?? 0;
	public int MaxHealth => _healthComponent?.MaxHealth ?? 0;

	public override void _Ready()
	{
		Initialize();
	}

	public virtual void Initialize()
	{
		AddToGroup(Constants.PlayerGroup);
		
		_muzzle = GetNode<Node2D>("Muzzle");
		_laserScene = ResourceLoader.Load<PackedScene>(Constants.LaserScenePath);
		
		_healthComponent = GetNode<Health>("Health");
		_movementComponent = GetNode<Movement>("Movement");
		_fireRateTimer = GetNode<Timer>("FireRateTimer");
		
		// INICIALIZAR COMMAND INVOKER
		InitializeCommandSystem();
		
		if (_healthComponent != null)
		{
			_healthComponent.SetMaxHealth(Constants.DefaultPlayerHealth);
			_healthComponent.Died += OnDied;
		}
		
		if (_movementComponent != null)
		{
			_movementComponent.SetMovementParameters(Constants.PlayerSpeed, Vector2.Zero);
			_movementComponent.SetMovementStrategy(new PlayerMovementStrategy());
		}
		
		if (_fireRateTimer != null)
		{
			_fireRateTimer.WaitTime = FireRate / 1000.0;
			_fireRateTimer.OneShot = true;
		}
	}
	
	// NUEVO MÉTODO: Inicializar sistema de comandos
	private void InitializeCommandSystem()
	{
		_commandInvoker = new CommandInvoker();
		_commandInvoker.Name = "CommandInvoker";
		AddChild(_commandInvoker);
		
		// Crear comandos reutilizables
		_moveUpCommand = new MoveUpCommand(this);
		_moveDownCommand = new MoveDownCommand(this);
		_moveLeftCommand = new MoveLeftCommand(this);
		_moveRightCommand = new MoveRightCommand(this);
		_stopCommand = new StopCommand(this);
	}

	public override void _Process(double delta)
	{
		HandleInput();
		
		// PROCESAR COMANDOS ENCOLADOS
		_commandInvoker?.ProcessQueuedCommands();
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleMovement();
	}

	// MÉTODO ACTUALIZADO: Manejo de input usando patrón Command
	private void HandleInput()
	{
		if (Input.IsActionJustPressed("quit"))
		{
			GetTree().Quit();
		}
		else if (Input.IsActionJustPressed("reset"))
		{
			GetTree().ReloadCurrentScene();
		}
		else if (Input.IsActionPressed("shoot"))
		{
			TryShoot();
		}
		
		// NUEVO: Manejo de movimiento usando comandos
		HandleMovementCommands();
	}
	
	// NUEVO MÉTODO: Manejo de comandos de movimiento
	private void HandleMovementCommands()
	{
		Vector2 inputDirection = Vector2.Zero;
		
		// Detectar input de movimiento
		if (Input.IsActionPressed("move_up"))
			inputDirection.Y -= 1;
		if (Input.IsActionPressed("move_down"))
			inputDirection.Y += 1;
		if (Input.IsActionPressed("move_left"))
			inputDirection.X -= 1;
		if (Input.IsActionPressed("move_right"))
			inputDirection.X += 1;
		
		// Normalizar dirección diagonal
		if (inputDirection.Length() > 1)
			inputDirection = inputDirection.Normalized();
		
		// Ejecutar comando apropiado solo si cambió la dirección
		if (inputDirection != _currentMovementDirection)
		{
			ICommand commandToExecute = null;
			
			if (inputDirection == Vector2.Zero)
			{
				commandToExecute = _stopCommand;
			}
			else if (inputDirection == Vector2.Up)
			{
				commandToExecute = _moveUpCommand;
			}
			else if (inputDirection == Vector2.Down)
			{
				commandToExecute = _moveDownCommand;
			}
			else if (inputDirection == Vector2.Left)
			{
				commandToExecute = _moveLeftCommand;
			}
			else if (inputDirection == Vector2.Right)
			{
				commandToExecute = _moveRightCommand;
			}
			else
			{
				// Para movimientos diagonales o complejos, usar comando genérico
				commandToExecute = new MoveCommand(this, inputDirection) { };
			}
			
			if (commandToExecute != null)
			{
				_commandInvoker.ExecuteCommand(commandToExecute);
			}
		}
	}

	private void HandleMovement()
	{
		if (_movementComponent != null)
		{
			_movementComponent.Direction = _currentMovementDirection;
			_movementComponent.Move(GetPhysicsProcessDeltaTime());
		}
	}

	private void TryShoot()
	{
		if (_fireRateTimer != null && _fireRateTimer.IsStopped())
		{
			EmitSignal(SignalName.LaserShot, _laserScene, _muzzle.GlobalPosition);
			_fireRateTimer.Start();
		}
	}

	public void TakeDamage(int damage)
	{
		_healthComponent?.TakeDamage(damage);
	}

	private void OnDied()
	{
		EmitSignal(SignalName.Killed);
		QueueFree();
	}
	
	// NUEVOS MÉTODOS PÚBLICOS para el patrón Command
	public void SetMovementDirection(Vector2 direction)
	{
		_currentMovementDirection = direction;
	}
	
	public Vector2 GetCurrentDirection()
	{
		return _currentMovementDirection;
	}
	
	// Método para deshacer último movimiento (útil para debug o funcionalidades especiales)
	public void UndoLastMovement()
	{
		_commandInvoker?.UndoLastCommand();
	}
	
	// Método para limpiar comandos (útil al pausar el juego)
	public void ClearMovementCommands()
	{
		_commandInvoker?.ClearQueue();
		_commandInvoker?.ClearHistory();
	}
}
