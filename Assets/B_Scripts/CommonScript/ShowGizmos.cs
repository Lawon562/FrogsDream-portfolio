using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGizmos : MonoBehaviour
{    
    [SerializeField]
    private float radius;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

}
