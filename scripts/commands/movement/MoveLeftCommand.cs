using Godot;

public class MoveLeftCommand : MoveCommand
{
	public MoveLeftCommand(Player player) : base(player, Vector2.Left)
	{
	}
	
	public override void Execute()
	{
		base.Execute();
		// Lógica adicional específica para movimiento hacia la izquierda si es necesaria
	}
}
