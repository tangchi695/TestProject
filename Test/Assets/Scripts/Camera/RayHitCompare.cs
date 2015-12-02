using UnityEngine;
using System.Collections;

public class RayHitCompare:IComparer {

    public int Compare(object a, object b)
    {
        return ((RaycastHit)a).distance.CompareTo(((RaycastHit)b).distance);
    }
}
