using UnityEngine;

public class TittleCamaraMovement : MonoBehaviour
{
    private float rotateSpeed = 45f;

    private void FixedUpdate()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}
