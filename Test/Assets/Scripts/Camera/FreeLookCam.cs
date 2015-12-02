using UnityEngine;
using System.Collections;
using System;

public class FreeLookCam : PivotBaseCameraRig
{
    [SerializeField]
    public float m_MoveSpeed = 1f;
    [Range(1f, 10f)]
    [SerializeField]
    public float m_TurnSpeed = 1.5f;
    [SerializeField]
    public float m_TurnSmooth = 0.1f;
    [SerializeField]
    public float m_TitleMan = 75f;
    [SerializeField]
    public float m_TitleMin = 45f;
    [SerializeField]
    public bool m_LockCursor = false;
    [SerializeField]
    public bool m_VerticalAutoReturn = false;


    private float m_LookAngle;
    private float m_TitleAngle;
    private Vector3 m_PivotEular;
    private Quaternion m_PivotTargetRot;
    private Quaternion m_TransformTargetRot;

    protected override void Awake()
    {
        base.Awake();
        Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !m_LockCursor;

        m_PivotEular = m_Pivot.rotation.eulerAngles;
        m_PivotTargetRot = m_Pivot.transform.localRotation;
        m_TransformTargetRot = transform.localRotation;

    }
    void Update()
    {
        HandleRotateMovment();
        if (!m_LockCursor && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !m_LockCursor;
        }

    }
    protected override void FollowTarget(float detalTime)
    {
        if (m_Target == null)
            return;
        transform.position = Vector3.Lerp(transform.position, m_Target.position, m_MoveSpeed * Time.deltaTime);
    }

    private void HandleRotateMovment()
    {
        if (Time.timeScale < float.Epsilon)
            return;
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        m_LookAngle += x * m_TurnSpeed;
        m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

        if (m_VerticalAutoReturn)
        {
            y = y > 0 ? Mathf.Lerp(0f, -m_TitleMin, y) : Mathf.Lerp(0, m_TitleMan, -y);
        }
        else
        {
            m_TitleAngle -= y * m_TurnSpeed;
            m_TitleAngle = Mathf.Clamp(m_TitleAngle, -m_TitleMin, m_TitleMan);
        }

        m_PivotTargetRot = Quaternion.Euler(m_TitleAngle, m_PivotEular.y, m_PivotEular.z);

        if (m_TurnSmooth > 0)
        {
            m_Pivot.transform.localRotation = Quaternion.Slerp(m_Pivot.transform.localRotation, m_PivotTargetRot, m_TurnSmooth);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmooth * Time.deltaTime);
        }
        else
        {
            m_Pivot.transform.localRotation = m_PivotTargetRot;
            transform.localRotation = m_TransformTargetRot;
        }
    }
}
