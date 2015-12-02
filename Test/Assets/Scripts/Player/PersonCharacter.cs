using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (Rigidbody ))]
[RequireComponent (typeof (CapsuleCollider))]
public class PersonCharacter : MonoBehaviour
{
    [SerializeField]
    float turnIndensity = 5f;
    [SerializeField]
    float m_JumpPower = 12f;
    [Range(1f, 4f)]
    [SerializeField]
    float m_GravityMultiplier = 2f;
    [SerializeField]
    float m_MoveSpeedMultiplier = 1f;
    [SerializeField]
    float M_AnimSpeedMultiplier = 1f;
    [SerializeField]
    float m_GroundCheckDistance = 0.1f;

    Rigidbody m_Rigidbody;
    Animator m_Anim;
    HashID hash;
    bool m_isGround;
    float m_TurnAmount;
    float m_ForwardAmount;
    float m_OriginGroundCheckDistance;
    Vector3 m_GroundNormal;
    Vector3 m_move;


    void Start()
    {
        m_Anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashID>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        m_OriginGroundCheckDistance = m_GroundCheckDistance;

    }
    public void Move(Vector3 move, bool jump)
    {
        if (move.magnitude > 1f)
                move.Normalize();

        m_move = move;
        //将move向量转换为本地坐标系,只有本地坐标系才能很好的处理旋转,和移动
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        
        m_TurnAmount = Mathf.Atan2(move.x, move.z)*Mathf.Rad2Deg;
        
        m_ForwardAmount = move.z;
        ApplyTurnRotation();
        if (m_isGround)
        {
            HandleGroundJump(jump);
        }
        else
        {
            HandleAirbornMovement();
        }
        UpdateAnimator(move);
    }

    private void CheckGroundStatus()
    {
        Debug.DrawLine(transform.position + Vector3.up * 0.3f, transform.position + Vector3.up * 0.3f + Vector3.down * m_GroundCheckDistance,Color.red);
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + Vector3.up *0.3f, Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            if (hitInfo.transform.tag != Tags.player)
            {
                m_GroundNormal = hitInfo.normal;
                m_isGround = true;
            }
           
        }
        else
        {
            m_isGround = false;
            m_GroundNormal = Vector3.up;
        }
    }
    private void ApplyTurnRotation()
    {
        transform.Rotate(0f, m_TurnAmount * turnIndensity * Time.deltaTime, 0f);
    }
    private void HandleGroundJump(bool jump)
    {

        if (jump && m_Anim.GetCurrentAnimatorStateInfo(0).IsName ("Locomotion"))
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            m_isGround = false;
            m_GroundCheckDistance = 0.1f;
        }
    }
    private void HandleAirbornMovement()
    {
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);
        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OriginGroundCheckDistance : 0.01f;
    }
    private void UpdateAnimator(Vector3 move)
    {
        m_Anim.SetFloat(hash.speedFloat, m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Anim.SetBool(hash.onGroundBool, m_isGround);
        if (!m_isGround)
        {
            m_Anim.SetFloat(hash.jumpFloat, m_Rigidbody.velocity.y);
        }
        
            if (m_isGround && move.magnitude > 0f)
            {
                m_Anim.speed = M_AnimSpeedMultiplier;
            }
            else
            {
                m_Anim.speed = 1f;
            }
    }
     void OnAnimatorMove()
    {
        if (m_isGround && Time.deltaTime > 0)
        {
             m_move = Vector3.ProjectOnPlane(m_move, m_GroundNormal);
            Vector3 v = m_move * m_MoveSpeedMultiplier;
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }
}
