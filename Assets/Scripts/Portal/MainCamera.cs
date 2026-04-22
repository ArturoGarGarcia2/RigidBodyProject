using UnityEngine;

public class MainCamera : MonoBehaviour {

    Portal[] portals;

    void Awake () {
        portals = FindObjectsOfType<Portal> ();
    }

    void LateUpdate()
    {
        if (portals == null || portals.Length == 0)
            portals = FindObjectsOfType<Portal>();

        for (int i = 0; i < portals.Length; i++)
            portals[i].PrePortalRender();

        for (int i = 0; i < portals.Length; i++)
            portals[i].Render();

        for (int i = 0; i < portals.Length; i++)
            portals[i].PostPortalRender();
    }

}