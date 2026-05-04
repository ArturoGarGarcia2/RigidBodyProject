using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class FinishLevelCommand : OneShotCommand
{
    protected override void OnExecute()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        Match match = Regex.Match(currentScene, @"Level(\d+)");
        
        if (!match.Success)
        {
            Debug.LogError("Formato incorrecto: Level#");
            return;
        }

        int nextLevel = int.Parse(match.Groups[1].Value) + 1;
        GravityManager.ChangeWorldGravity(Vector3.down);
        SceneManager.LoadScene("Level" + nextLevel);
    }

    protected override string GetDisplayText() =>
        "<color=#0F0>Finish</color>";
        
    protected override string GetTrivialDisplay1Text() => "finish_stage";
    protected override string GetTrivialDisplay2Text() => "";
}