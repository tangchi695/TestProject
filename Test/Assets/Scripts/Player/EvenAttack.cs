using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackInfo
{
    public int id;
    public float time;
    public AttackInfo(int _id,float _time)
    {
        this.id = _id;
        this.time = _time;
    }
}
struct m_AttackState
{
    public const string attack01 = "Attack01";
    public const string attack02 = "Attack02";
    public const string attack03 = "Attack03";
    public const string attack04 = "Attack04";
}

public class EvenAttack : MonoBehaviour
{

    private Dictionary<string, AttackInfo> m_AttackDis = new Dictionary<string, AttackInfo>(3);
    private int hitCount;
    private Animator m_anim;
    private HashID hash;
    private AttackInfo attackInfo;

    private  void Start()
    {
        m_anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashID>();
        m_AttackDis.Add(m_AttackState.attack01, new AttackInfo(1, 0.6f));
        m_AttackDis.Add(m_AttackState.attack02, new AttackInfo(2, 0.5f));
        m_AttackDis.Add(m_AttackState.attack03, new AttackInfo(3, 0.5f)); 
    }
    private  void Update()
    {
        InputManager();
        ApplyAttack();
    }
    private void InputManager()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
            {
                m_anim.SetBool(hash.attackBool, true);
                hitCount = 1;
                
            }
            else
            {
                foreach (KeyValuePair<string ,AttackInfo> item in m_AttackDis)
                {
                    if (m_anim.GetCurrentAnimatorStateInfo(0).IsName(item.Key))
                    {
                        hitCount = item.Value.id + 1;
                    }
                }
            }
        }
    }
    private void ApplyAttack()
    {
        if (!m_anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion") &&
            !m_anim.GetCurrentAnimatorStateInfo(0).IsName("Air") &&
            !m_anim.GetCurrentAnimatorStateInfo(0).IsName("ContactGround"))
        {
            m_anim.SetBool(hash.attackBool, false);
        }
        foreach (KeyValuePair<string, AttackInfo> item in m_AttackDis)
        {
            if (m_anim.GetCurrentAnimatorStateInfo(0).IsName(item.Key) &&
                m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > item.Value.time &&
                hitCount == (item.Value.id + 1))
            {
                m_anim.SetBool(hash.attackBool, true);
            }
        }
        
    }
   


}
