using UnityEngine;
using System.Collections;
using System;

public class ProtectCam : MonoBehaviour
{
    public float clipMoveTime = 0.05f;
    public float returnTime = 0.4f;

    public float sphereCastRadius = 0.1f;
    public float closestDistance = 0.5f;

    public bool protect { get; private set; }
    public const string dotClipTag = "Player";

    private Transform m_Cam;
    private Transform m_Pivot;

    private float m_OringinalDis;

    private float m_CurrentDis;

    private float m_MoveVelocity;
    private Ray m_Ray;
    private RaycastHit[] m_Hits;
    private RayHitCompare rayHitCompare;

    void Start()
    {
        m_Cam = GetComponentInChildren<Camera>().transform;
        m_Pivot = m_Cam.parent;
        m_OringinalDis = m_Cam.localPosition.magnitude;
        m_CurrentDis = m_OringinalDis;

        rayHitCompare = new RayHitCompare();
    }

    void LateUpdate()
    {
        float m_targetDis = m_OringinalDis;
        m_Ray.origin = m_Pivot.position + m_Pivot.forward * sphereCastRadius;
        m_Ray.direction = -m_Pivot.forward;

        var cols = Physics.OverlapSphere(m_Ray.origin, sphereCastRadius);

        bool initialIntersect = false;
        //bool hitSomething = false;

        for (int i = 0; i < cols.Length; i++)
        {
            if (!cols[i].isTrigger && !(cols[i].attachedRigidbody!=null && cols[i].attachedRigidbody.CompareTag(dotClipTag)))
            {
                initialIntersect = true;
                break;
            }
        }
        if (initialIntersect)
        {
            m_Ray.origin += m_Pivot.forward * sphereCastRadius;
            m_Hits = Physics.RaycastAll(m_Ray, m_OringinalDis - sphereCastRadius);
        }
        else
        {
            m_Hits = Physics.SphereCastAll(m_Ray, sphereCastRadius, m_OringinalDis + sphereCastRadius);
        }

        Array.Sort(m_Hits, rayHitCompare);
        float neaest = Mathf.Infinity;

        for (int i = 0; i < m_Hits.Length; i++)
        {
            if (m_Hits[i].distance < neaest &&
                (!m_Hits[i].collider.isTrigger) && 
                !(m_Hits[i].collider.attachedRigidbody!=null && m_Hits[i].collider.attachedRigidbody.CompareTag(dotClipTag)))
            {
                neaest = m_Hits[i].distance;
                m_targetDis = -m_Pivot.InverseTransformPoint(m_Hits[i].point).z;
                //hitSomething = true;
            }
        }
        //if (hitSomething)
        //{
        //    Debug.DrawRay(m_Ray.origin, -m_Pivot.forward * (m_OringinalDis + sphereCastRadius), Color.red);
        //}
        m_CurrentDis = Mathf.SmoothDamp(m_CurrentDis, m_targetDis, ref m_MoveVelocity, m_CurrentDis > m_targetDis ? clipMoveTime : returnTime);
        m_CurrentDis = Mathf.Clamp(m_CurrentDis, closestDistance, m_OringinalDis);
        m_Cam.localPosition = -Vector3.forward * m_CurrentDis;
    }


}
