using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public TimeState currentTime = TimeState.Present;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void SetTime(TimeState state)
    {
        currentTime = state;
    }

    public bool Is(TimeState state)
    {
        return currentTime == state;
    }
}