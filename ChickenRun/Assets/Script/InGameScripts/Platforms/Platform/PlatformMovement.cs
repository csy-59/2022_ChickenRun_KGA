using UnityEngine;
using Assets;

public class PlatformMovement : MonoBehaviour
{
    public PlatformShape shape = PlatformShape.CIRCLE;
    public PlatformRowMovement row;

    private enum State
    {
        Ready,
        Idel,
        Sink
    }
    private State currentState;

    private float currentYOffset = 0f;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        GameManager.Instance.OnShapeChange.RemoveListener(Sink);
        GameManager.Instance.OnShapeChange.AddListener(Sink);

        row.OnRowActive.RemoveListener(Active);
        row.OnRowActive.AddListener(Active);

        currentYOffset = 0f;
        currentState = State.Ready;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Ready:
                break;
            case State.Idel:
                IdelUpdate();
                break;
            case State.Sink:
                SinkUpdate();
                break;
        }
    }

    private void IdelUpdate()
    {
        if (currentYOffset < 0f)
        {
            MoveCube(1f);
        }
        else
        {
            currentYOffset = 0f;
        }
    }

    private void SinkUpdate()
    {
        if(currentYOffset > -GameManager.PlatformRowMoveOffset)
        {
            MoveCube(-1f);
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
        if (selectedShape != shape && currentState != State.Ready)
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
        row.OnRowActive.RemoveListener(Active);
    }
}
