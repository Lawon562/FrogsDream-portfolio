using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetButterflyPos : MonoBehaviour
{
    public GameObject butterfly;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(
            butterfly.transform.position.x,
            0,
            butterfly.transform.position.z); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
