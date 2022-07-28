using UnityEngine;

public class TittleCamaraMovement : MonoBehaviour
{
    public float RotateSpeed = 10f;

    private void Update()
    {
        transform.Rotate(0f, RotateSpeed * Time.deltaTime, 0f);
    }
}
