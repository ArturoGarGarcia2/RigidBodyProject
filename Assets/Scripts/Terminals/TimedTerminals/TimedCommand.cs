using UnityEngine;
using System.Collections;

public abstract class TimedCommand : TerminalCommand
{
    [Header ("Time Management")]
    [SerializeField] private float duration = 5f;
    private bool isRunning;


    public override void Execute()
    {
        if (isRunning) return;
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        isRunning = true;

        OnStart();

        float timeLeft = duration;

        while (timeLeft > 0)
        {
            timeDisplay.text = Mathf.Ceil(timeLeft).ToString();

            RefreshDisplay();

            timeLeft -= Time.deltaTime;
            yield return null;
        }

        timeDisplay.text = "";

        OnEnd();
        isRunning = false;

        RefreshDisplay();
    }

    protected abstract void OnStart();
    protected abstract void OnEnd();

    protected override string GetDisplayText() =>
        isRunning
        ? "<size=50%><color=#F80>Running...</color></size>"
        : "<size=75%><color=#FF0>Ready</color></size>";

    protected override string GetTrivialDisplay1Text() => "time_of_effect: "+duration+"s";
}