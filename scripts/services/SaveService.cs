// scripts/services/SaveService.cs - VERSIÓN CON DEBUG
using Godot;

public static class SaveService
{
	public static uint LoadHighScore()
	{
		try
		{
			var score = FileHelper.LoadHighScore();
			GD.Print($"🔵 [SaveService] High score loaded: {score}");
			return score;
		}
		catch (System.Exception e)
		{
			GD.PrintErr($"🔴 [SaveService] Error loading high score: {e.Message}");
			return 0;
		}
	}
	
	public static bool SaveHighScore(uint highScore)
	{
		try
		{
			GD.Print($"🟡 [SaveService] Saving high score: {highScore}");
			FileHelper.SaveHighScore(highScore);
			GD.Print($"🟢 [SaveService] High score saved successfully!");
			return true;
		}
		catch (System.Exception e)
		{
			GD.PrintErr($"🔴 [SaveService] Error saving high score: {e.Message}");
			return false;
		}
	}
	
	public static void ResetSaveData()
	{
		try
		{
			GD.Print($"🟠 [SaveService] Resetting save data...");
			FileHelper.SaveHighScore(0);
			GD.Print($"🟢 [SaveService] Save data reset successfully!");
		}
		catch (System.Exception e)
		{
			GD.PrintErr($"🔴 [SaveService] Error resetting save data: {e.Message}");
		}
	}
}
