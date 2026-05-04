using TMPro;
using UnityEngine;

public class GravityFloorDetector : MonoBehaviour
{
    private PlayerController player;
    [Header("false: Forbid / true: Allow")]
    public bool mode;
    public GravityDirection[] workingDirections;
    [SerializeField] private GameObject pane;
    [SerializeField] protected TMP_Text display;
    private string forbidden = "Gravity direction\n\n\n\n\nNot allowed";
    private string allowed = "Gravity direction\n\n\n\n\n\n\nAllowed";

    void Start()
    {
        player = PlayerController.Instance;
    }

    void Update()
    {
        Vector3 currentGravity = player.useLocalGravity 
            ? player.localGravityDir 
            : GravityManager.worldGravityDir;

        bool matches = false;

        foreach (GravityDirection direction in workingDirections)
            if (SameDirection(currentGravity, direction.GetVector()))
            {
                matches = true;
                break;
            }

        bool shouldBlock = mode ? !matches : matches;

        pane.SetActive(shouldBlock);
        display.text = shouldBlock ? forbidden : allowed;
        display.color = shouldBlock ? Color.red : Color.green;
    }

    bool SameDirection(Vector3 a, Vector3 b)
    {
        return Vector3.Dot(a.normalized, b.normalized) > 0.99f;
    }
}
