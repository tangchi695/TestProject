using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent (typeof(PersonCharacter))]
public class ThirdPersonController : MonoBehaviour
{
    private PersonCharacter character;
    private Animator m_Anim;
    private Vector3 m_move;
    private Vector3 m_CamForward;
    private Transform cam;
    private bool m_jump;
    private bool m_IsAttacking;
    private void Start()
    {       
        cam = Camera.main.transform;
        character = GetComponent<PersonCharacter>();
        m_Anim = GetComponent<Animator>();
    }
    private void Update()
    {
        //每一帧都获取攻击状态，只有这样才能实现动画播放前几秒能转向，后面就不能在移动了
        if(!m_jump)
        {
            m_jump = Input.GetButtonDown("Jump");
        }
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")&&
            !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Air")&&
            !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("ContactGround"))
        {
            m_IsAttacking = true;
        }
        else
        {
            m_IsAttacking = false;
        }

    }
    private void FixedUpdate()
    {
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
            if (cam != null)
            {
                m_CamForward = Vector3.Scale(cam.forward, new Vector3(1f, 0f, 1f)).normalized;
                m_move = v * m_CamForward + h * cam.right;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                m_move *= 0.5f; 
            }
        if (m_IsAttacking)
        {
            if ( m_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f)
            {
                m_move = Vector3.zero;
            }      
        }
        character.Move(m_move, m_jump);
        m_jump = false;
    
    }
}
