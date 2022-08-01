using UnityEngine;
using Assets;

public class PlatformMovement : MonoBehaviour
{
    public PlatformShape MyShape = PlatformShape.CIRCLE;
    public PlatformRowMovement Row;

    // 유한 상태 머신
    private enum State
    {
        Ready,
        Idel,
        Sink
    }
    private State currentState;

    // 이동 관련
    private enum MoveDirection
    {
        Down = -1,
        Up = 1
    }
    private float currentYOffset = 0f;

    private void OnEnable()
    {
        GameManager.Instance.OnShapeChange.RemoveListener(Sink);
        GameManager.Instance.OnShapeChange.AddListener(Sink);

        Row.OnRowActive.RemoveListener(Active);
        Row.OnRowActive.AddListener(Active);

        currentYOffset = 0f;
        currentState = State.Ready;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        switch(currentState)
        {
            case State.Ready:
                break;
            case State.Idel:
                IdelFixedUpdate();
                break;
            case State.Sink:
                SinkFixedUpdate();
                break;
        }
    }

    private void IdelFixedUpdate()
    {
        if (currentYOffset < 0f)
        {
            MoveCube((float)MoveDirection.Up);
        }
        else
        {
            currentYOffset = 0f;
        }
    }

    private void SinkFixedUpdate()
    {
        if(currentYOffset > -GameManager.PlatformRowMoveOffset)
        {
            MoveCube((float)MoveDirection.Down);
        }
        else
        {
            currentState = State.Idel;
        }
    }

    private void MoveCube(float sign)
    {
        float sinkDeep = GameManager.Instance.PlatformSpeed * Time.deltaTime * sign;

        currentYOffset += sinkDeep;
        
        transform.Translate(0f, sinkDeep, 0f);
    }

    private void Sink(PlatformShape selectedShape)
    {
        if (selectedShape != MyShape && currentState != State.Ready)
        {
            currentState = State.Sink;
        }
    }
    
    private void Active()
    {
        currentState = State.Idel;
    }

    private void OnDisable()
    {
        Row.OnRowActive.RemoveListener(Active);
    }
}
