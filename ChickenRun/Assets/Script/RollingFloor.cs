using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingFloor : MonoBehaviour
{
    public Vector3 ResetPosition = new Vector3(0f, 0.2f, 20f);
    public float Speed = 0.5f;
    public float ReturnZPosition = -20f;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z > ReturnZPosition)
        {
            transform.Translate(0f, 0f, Speed * Time.deltaTime * -1f);
        }
        else
        {
            transform.position = ResetPosition;
        }
    }
}
