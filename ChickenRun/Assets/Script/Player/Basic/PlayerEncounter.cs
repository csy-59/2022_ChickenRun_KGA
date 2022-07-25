using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEncounter : MonoBehaviour
{
    public Material model;

    public float StannedTime = 1f;
    public bool isStanned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Flower")
        {
            other.gameObject.SetActive(false);
            GameManager.Instance.GetFlower();
        }

        else if (other.tag == "Gurnish")
        {
            gameObject.layer = 8;
            isStanned = true;
            StartCoroutine(ColorChange());
            Invoke("UnStanned", StannedTime);
        }
    }

    private void UnStanned()
    {
        isStanned = false;
        model.color = new Color(1f, 1f, 1f, 1f);
    }

    private IEnumerator ColorChange()
    {
        while(isStanned)
        {
            model.color = new Color(1f, 0f, 0f, 0.5f);
            yield return new WaitForSeconds(0.3f);
            model.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
