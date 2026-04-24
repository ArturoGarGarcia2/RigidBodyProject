using UnityEngine;

public class MainCamera : MonoBehaviour {

    Portal[] portals;

    void Awake () {
        portals = FindObjectsOfType<Portal> ();
    }

    void LateUpdate()
    {
        if (Portal.freezePortals)
            return;

        portals = FindObjectsOfType<Portal>();

        for (int i = 0; i < portals.Length; i++)
            if (portals[i].linkedPortal != null)
                portals[i].PrePortalRender();

        for (int i = 0; i < portals.Length; i++)
            if (portals[i].linkedPortal != null)
                portals[i].Render();

        for (int i = 0; i < portals.Length; i++)
            if (portals[i].linkedPortal != null)
                portals[i].PostPortalRender();
    }

}