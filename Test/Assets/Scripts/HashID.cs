using UnityEngine;
using System.Collections;

public class HashID : MonoBehaviour
{
    public int speedFloat;
    public int onGroundBool;
    public int jumpFloat;
    public int speedControlFloat;
    public int attackBool;

    void Awake()
    {
        speedFloat = Animator.StringToHash("Speed");
        onGroundBool = Animator.StringToHash("OnGround");
        jumpFloat = Animator.StringToHash("Jump");
        speedControlFloat = Animator.StringToHash("SpeedControl");
        attackBool = Animator.StringToHash("Attack");
    }
	
}
