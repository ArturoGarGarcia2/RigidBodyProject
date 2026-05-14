using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class FinishLevelCommand : OneShotCommand
{
    protected override void OnExecute()
    {
        if(!canBeInteracted) return;

        string currentScene = SceneManager.GetActiveScene().name;

        Match match = Regex.Match(currentScene, @"Level(\d+)");

        if (!match.Success)
        {
            Debug.LogError("Formato incorrecto: Level#");
            return;
        }

        int currentLevel = int.Parse(match.Groups[1].Value);

        GravityManager.ChangeWorldGravity(Vector3.down);

        // Si acabó el último nivel
        if (currentLevel >= 6)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        int nextLevel = currentLevel + 1;
        SceneManager.LoadScene("Level" + nextLevel);
    }

    protected override string GetDisplayText() =>
        "<color=#0F0>Finish</color>";
        
    protected override string GetTrivialDisplay1Text() => "finish_stage";
    protected override string GetTrivialDisplay2Text() => "";
}