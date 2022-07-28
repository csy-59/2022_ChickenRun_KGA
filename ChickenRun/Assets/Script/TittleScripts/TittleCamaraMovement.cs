using UnityEngine;

public class TittleCamaraMovement : MonoBehaviour
{
    private float RotateSpeed = 45f;

    private void FixedUpdate()
    {
        transform.Rotate(0f, RotateSpeed * Time.deltaTime, 0f);
    }
}
