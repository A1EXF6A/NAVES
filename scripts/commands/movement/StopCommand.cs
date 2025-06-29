using Godot;

public class StopCommand : MoveCommand
{
	public StopCommand(Player player) : base(player, Vector2.Zero)
	{
	}
	
	public override void Execute()
	{
		base.Execute();
		// Lógica adicional para detener el movimiento si es necesaria
	}
}
