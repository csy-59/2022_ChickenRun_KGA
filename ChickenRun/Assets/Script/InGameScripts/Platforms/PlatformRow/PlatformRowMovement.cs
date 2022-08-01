using UnityEngine;
using UnityEngine.Events;

public class PlatformRowMovement : MonoBehaviour
{
    // 유한 상태 머신
    public enum State
    {
        Ready,
        Move,
        Active,
        Stop,
        Sink
    }
    private State currentState;
    private State previousState;
    public State PreviousState { get { return previousState; } }

    // 이동 관련
    public Vector3 ResetPosition;
    private Vector3 previousPosition;

    // 해당 행 활성화
    public UnityEvent OnRowActive = new UnityEvent();

    // CubeRowManager를 위한 플레이어 타겟 위치
    public Transform[] PlayerTargetPosition = new Transform[3];

    private void Start()
    {
        if (transform.position.z >= GameManager.RowActiveZPos)
        {
            ChangeState(State.Ready, State.Ready);

            if (transform.position.z == GameManager.RowActiveZPos)
            {
                Invoke("ChangeToActive", GameManager.StartTimeOffset);
            }
        }
        else
        {
            ChangeState(State.Active, State.Active);
            
            OnRowActive.Invoke();
            
            if (transform.position.z == GameManager.RowDisableZPos)
            {
                Invoke("ChangeToReady", GameManager.StartTimeOffset);
            }
        }
    }
    private void OnEnable()
    {
        ChangeState(State.Ready, State.Ready);
        
        previousPosition = transform.position;

        GameManager.Instance.OnRowMove.RemoveListener(ChangeToMove);
        GameManager.Instance.OnRowMove.AddListener(ChangeToMove);
    }

    private void ChangeState(State curent, State previous)
    {
        currentState = curent;
        previousState = previous;
    }

    private void FixedUpdate()
    {
        switch(currentState)
        {
            case State.Ready:
            case State.Active:
                break;

            case State.Move:
                MoveFixedUpdate();
                break;

            case State.Sink:
                SinkFixedUpdate();
                break;
        }
    }

    private void MoveFixedUpdate()
    {
        if(transform.position.z > previousPosition.z - GameManager.PlatformRowMoveOffset)
        {
            transform.Translate(0f, 0f, GameManager.Instance.PlatformSpeed * Time.deltaTime * -1f);
        }
        else
        {
            currentState = previousState;
            if(previousState == State.Ready)
            {
                if(transform.position.z <= GameManager.RowActiveZPos)
                {
                    ChangeToActive();
                }
            }
            else if(previousState == State.Active)
            {
                if(transform.position.z <= GameManager.RowDisableZPos)
                {
                    ChangeToReady();
                }
            }
        }
    }

    private void SinkFixedUpdate()
    {
        if (transform.position.y > previousPosition.y - GameManager.PlatformRowMoveOffset)
        {
            transform.Translate(0f, GameManager.Instance.PlatformSpeed * Time.deltaTime * -1f, 0f);
        }
        else
        {
            currentState = previousState;
            if(transform.position.z <= GameManager.RowDisableZPos)
            {
                ResetRow();
            }
        }
    }

    private void ChangeToActive()
    {
        OnRowActive.Invoke();

        previousPosition = transform.position;

        ChangeState(State.Sink, State.Active);
    }

    private void ChangeToReady()
    {
        previousPosition = transform.position;

        ChangeState(State.Sink, State.Ready);
    }

    private void ChangeToMove()
    {
        currentState = State.Move;

        previousPosition = transform.position;
    }

    private void ResetRow()
    {
        gameObject.SetActive(false);
        
        gameObject.transform.position = ResetPosition;
        
        gameObject.SetActive(true);
    }
}
