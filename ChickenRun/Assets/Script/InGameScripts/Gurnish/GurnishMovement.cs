using UnityEngine;
using Assets;

public class GurnishMovement : MonoBehaviour
{
    public Transform ModelTransform;

    private bool isOnGround = false;
    private Vector3 originalPosition;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        Invoke("DisableSelf", 10f);
    }

    private void FixedUpdate()
    {
        if(isOnGround)
        {
            Move();
            Rotate();

            if (gameObject.transform.position.z < -7.5f)
                DisableSelf();
        }
    }

    private void Move()
    {
        Vector3 offset = GameManager.Instance.GurnishMoveSpeed * Time.fixedDeltaTime * transform.forward;
        rigidBody.MovePosition(rigidBody.position + offset);
    }

    private void Rotate()
    {
        ModelTransform.Rotate(GameManager.Instance.GurnishRotateSpeed * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isOnGround && other.tag == "Platform")
        {
            isOnGround = true;
        }

        else if(other.tag == "Lava")
        {
            DisableSelf();
        }
    }

    private void DisableSelf()
    {
        gameObject.transform.position = originalPosition;
        isOnGround = false;
        gameObject.SetActive(false);
    }
}
