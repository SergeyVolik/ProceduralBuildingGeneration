using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    Animator door;
   
    const string DoorOpen = "DoorOpen";
    const string Anim = "Anim";
    [SerializeField]
    private int anim;
    private void OnTriggerEnter(Collider other)
    {
        if (!door.GetBool(DoorOpen))
        {
            door.SetBool(DoorOpen, true);
            door.SetInteger(Anim, anim);
            StartCoroutine(WaitSec());
            door.GetComponent<AudioSource>().Play();
        }
    }

    IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(1f);
        door.GetComponent<Collider>().enabled = false;
    }
}
