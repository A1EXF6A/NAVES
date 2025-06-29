using Godot;

public class MoveUpCommand : MoveCommand
{
	public MoveUpCommand(Player player) : base(player, Vector2.Up)
	{
	}
	
	public override void Execute()
	{
		base.Execute();
		// Lógica adicional específica para movimiento hacia arriba si es necesaria
	}
}
