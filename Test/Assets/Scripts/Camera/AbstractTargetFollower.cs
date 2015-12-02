using UnityEngine;
using System.Collections;

public abstract class AbstractTargetFollower : MonoBehaviour
{
    [SerializeField]
    protected Transform m_Target;
    [SerializeField]
    protected bool m_AutoTargetPlayer = true;
    protected Rigidbody m_TargetRigidbody;

    void Start()
    {
        if (m_AutoTargetPlayer)
        {
            FindAndTargetPlayer();
        }
        if (m_Target == null)
            return;
        m_TargetRigidbody = m_Target.GetComponent<Rigidbody>();
    }
    protected void  FixedUpdate()
    {
        if (m_AutoTargetPlayer && (m_Target == null || m_Target.gameObject.activeSelf))
        {
            FindAndTargetPlayer();
        }
        FollowTarget(Time.deltaTime);
        
    }
    protected  void FindAndTargetPlayer()
    {
        var player = GameObject.FindGameObjectWithTag(Tags.player);
        if (player)
        {
            SetTarget(player.transform);
        }
    }
    protected void SetTarget(Transform transform)
    {
        m_Target = transform;
    }
    protected abstract void FollowTarget(float detalTime);
    
}
