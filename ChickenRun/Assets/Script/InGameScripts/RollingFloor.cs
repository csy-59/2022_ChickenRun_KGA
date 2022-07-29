using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingFloor : MonoBehaviour
{
    private readonly Vector3 resetPosition = new Vector3(0f, 0.2f, 20f);
    private readonly float speed = 0.5f;
    private readonly float lavaReturnZPoint = -20f;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z > lavaReturnZPoint)
        {
            transform.Translate(0f, 0f, speed * Time.deltaTime * -1f);
        }
        else
        {
            transform.position = resetPosition;
        }
    }
}
