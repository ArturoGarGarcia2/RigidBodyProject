using UnityEngine;

public abstract class TerminalActivable : MonoBehaviour
{
    public bool isActive;
    public bool initialState;

    void Start() => isActive = initialState;

    public virtual void SetActive(bool active){}
    public virtual void ToggleActive(){}
}