using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttachPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player");
            other.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player");
            other.gameObject.transform.parent = null;
        }
    }
}
