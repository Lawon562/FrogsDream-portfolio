using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public GameObject killParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) //차후 EnemyMove스크립트에 병합
    {
        if (other.transform.tag == "Player")
        {
            //Invoke("GoFailScene", 3f);
            Destroy(Instantiate(killParticle, this.transform), 3f);
            Destroy(other.gameObject);
            GameObject.Find("FirstPersonCamera").GetComponent<FirstPersonCameraController>().Player = this.transform;
            GameObject.Find("ThirdPersonCamera").GetComponent<ThirdPersonCameraController>().Player = this.transform;
        }
    }

    
}
