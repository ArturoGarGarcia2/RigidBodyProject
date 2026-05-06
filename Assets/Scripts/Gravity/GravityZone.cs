using UnityEngine;
    using System.Collections.Generic;

public class GravityZone : TerminalActivable
{
    private List<GravitableObject> affectedObjects = new List<GravitableObject>();
    public Direction dir;
    public Renderer[] gravityFieldRenderers;
    public Transform parent;
    public bool mantainGravity;
    public bool mantainPlayerGravity;

    void Start()
    {
        UpdateMaterials();
    }

    public override void ToggleActive()
    {
        isActive = !isActive;

        if (!isActive)
            DisableZoneEffects();
        else
            ApplyGravityToObjectsInside();

        UpdateMaterials();
    }

    public override void SetActive(bool active)
    {
        isActive = active;

        if (!isActive)
            DisableZoneEffects();
        else
            ApplyGravityToObjectsInside();

        UpdateMaterials();
    }

    private void ApplyGravityToObjectsInside()
    {
        Collider[] colliders = Physics.OverlapBox(
            transform.position,
            transform.localScale / 2f,
            transform.rotation
        );

        foreach (var col in colliders)
        {
            var gravObj = col.GetComponent<GravitableObject>();
            if (gravObj != null)
            {
                gravObj.ChangeGravity(GetVectorFromEnum(dir));
                gravObj.useLocalGravity = true;

                if (!affectedObjects.Contains(gravObj))
                    affectedObjects.Add(gravObj);

                if (parent != null)
                    gravObj.transform.SetParent(parent);
            }
        }
    }

    private void DisableZoneEffects()
{
    foreach (var gravObj in affectedObjects)
    {
        if (gravObj == null) continue;

        if (!mantainGravity)
        {
            gravObj.ResetToWorldGravity();
            gravObj.useLocalGravity = false;
        }

        if (parent != null)
            gravObj.transform.SetParent(null);
    }

    affectedObjects.Clear();
}

    public void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        var gravObj = other.GetComponent<GravitableObject>();
        if (gravObj != null)
        {
            gravObj.ChangeGravity(GetVectorFromEnum(dir));

            if (!affectedObjects.Contains(gravObj))
                affectedObjects.Add(gravObj);

            if(parent != null)
                other.transform.SetParent(parent);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (!isActive) return;

        var gravObj = other.GetComponent<GravitableObject>();
        if (gravObj != null)
            gravObj.useLocalGravity = true;
    }

    public void OnTriggerExit(Collider other)
    {
        var gravObj = other.GetComponent<GravitableObject>();
        if (gravObj != null)
        {
            affectedObjects.Remove(gravObj);
            if (!mantainGravity)
            {
                gravObj.ResetToWorldGravity();
                gravObj.useLocalGravity = false;
            }

            if(parent != null)
                other.transform.SetParent(null);
        }
    }

    public void UpdateMaterials()
    {
        if (!isActive)
        {
            foreach (Renderer renderer in gravityFieldRenderers)
            {
                renderer.material.SetColor("_BaseColor", Color.gray);
                renderer.material.SetColor("_EmissionColor", Color.gray);
            }
        }
        else
        {
            Color c = GetColorFromEnum(dir);

            foreach (Renderer renderer in gravityFieldRenderers)
            {
                renderer.material.SetColor("_BaseColor", c);
                renderer.material.SetColor("_EmissionColor", c * 2f);
                renderer.material.EnableKeyword("_EMISSION");
            }
        }
    }

    private Vector3 GetVectorFromEnum(Direction d) { 
        switch (d)
        {
            case Direction.UP:      return Vector3.up;
            case Direction.DOWN:    return Vector3.down;
            case Direction.LEFT:    return Vector3.left;
            case Direction.RIGHT:   return Vector3.right;
            case Direction.FORWARD: return Vector3.forward;
            case Direction.BACK:    return Vector3.back;
            default:                return Vector3.down; 
        }
    }

    private Color GetColorFromEnum(Direction d) { 
        switch (d)
        {
            case Direction.UP:      return Color.yellow;
            case Direction.DOWN:    return Color.white;
            case Direction.LEFT:    return Color.green;
            case Direction.RIGHT:   return Color.blue;
            case Direction.FORWARD: return Color.red;
            case Direction.BACK:    return new Color(1f, .5f, 0f);
            default:                return Color.white; 
        }
    }

    void OnValidate()
    {
        if(mantainGravity) mantainPlayerGravity = false;
        if(mantainPlayerGravity) mantainGravity = false;
    }
}