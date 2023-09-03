using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFoot : MonoBehaviour
{
    public GameObject particle;
    public GameObject pt;
    public Slider leftTimeSlider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "Enemy") //¥Í¿∫ ∞¥√º∞° ¿˚¿Ã∏È ¡◊¿Ã±‚
        {
            pt.SendMessage("addTime");
            Destroy(other.gameObject);
            Destroy(Instantiate(particle, this.transform), 3f);
        }
        if (other.transform.gameObject.CompareTag("Tornado"))
        {
            GameObject player = GameObject.Find("Player");
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerJump>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;


            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<Rigidbody>().useGravity = true;

        }
    }

}
