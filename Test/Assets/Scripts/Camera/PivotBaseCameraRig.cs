using UnityEngine;
using System.Collections;

public abstract class PivotBaseCameraRig :AbstractTargetFollower
{
    protected Transform m_Pivot;
    protected Transform cam;

    protected virtual void Awake()
    {
        cam = GetComponentInChildren<Camera>().transform;
        m_Pivot = cam.parent;
    }
}
