using UnityEngine;
using System.Collections;

public class AttackDesition : MonoBehaviour  
{
    //主角的攻击角度
    public  float minAngle = -45f;
    public  float maxAngle = 45f;

    //存放敌人的数组
    GameObject[] enemys;
    void Start()
    {
        enemys = GameObject.FindGameObjectsWithTag(Tags.enemy);
    }
    void Update()
    {
        //用于测试
         Quaternion r3 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + minAngle, transform.rotation.eulerAngles.z);
        Quaternion r4 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + maxAngle, transform.rotation.eulerAngles.z);
        Vector3 f5 = transform.position + (r3 * Vector3.forward) * 15;
        Vector3 f6 = transform.position + (r4 * Vector3.forward) * 15;
        Debug.DrawLine(transform.position, f5,Color.red);
        Debug.DrawLine(transform.position, f6,Color.red);
        Debug.DrawLine(f5, f6,Color.red);
    }
    public void EnemyDesition(float JudgeDistance)
    {
        float count = 0;
        for (int i = 0; i < enemys.Length; i++)
        {
            
            Quaternion r1 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + minAngle, transform.rotation.eulerAngles.z);
            Quaternion r2 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + maxAngle, transform.rotation.eulerAngles.z);

            Vector3 f2 = transform.position + (r1 * Vector3.forward )* JudgeDistance;
            Vector3 f3 = transform.position + (r2 * Vector3.forward )* JudgeDistance;

            if (IsIntriangle(enemys[i].transform.position, transform.position, f2, f3))
            {
                count++;               
            }
            
        }
        Debug.Log(count + "人被击中");
        Debug.Log((enemys.Length - count) + "人没有被击中");
    }
    private    bool IsIntriangle(Vector3 point, Vector3 f1, Vector3 f2,Vector3 f3)
    {
        float x = point.x;
        float y = point.z;

        float x1 = f1.x;
        float y1 = f1.z;

        float x2 = f2.x;
        float y2 = f2.z;

        float x3 = f3.x;
        float y3 = f3.z;

        float t = TriangleArea(x1, y1, x2, y2, x3, y3);
        float d = TriangleArea(x1,y1, x2, y2, x, y) + TriangleArea(x1, y1, x, y, x3, y3) + TriangleArea(x, y, x2, y2, x3, y3);

        if (Mathf.Abs(t - d) < 0.01)
        {
            return true;
        }
        else
        {
            return false; 
        }
    }
    private float  TriangleArea(float x1, float y1, float x2, float y2, float x3, float y3)
    {
        return Mathf.Abs( (x1 * y2 + x2 * y3 + x3 * y1 - x2*y1 - x3 * y2 - x1 * y3) / 2);
    }

}
