using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformRowMovement : MonoBehaviour
{
    public Vector3 ResetPosition;
    public Transform[] PlayerTargetPosition = new Transform[3];

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

    public UnityEvent OnRowActive = new UnityEvent();

    private Vector3 previousPosition;

    private void Start()
    {
        if (transform.position.z >= GameManager.Instance.RowActiveZPos)
        {
            ResetState(State.Ready, State.Ready);
            if (transform.position.z == GameManager.Instance.RowActiveZPos)
            {
                Invoke("ChangeToActive", GameManager.Instance.GameStartTimeOffset);
            }
        }
        else
        {
            ResetState(State.Active, State.Active);
            OnRowActive.Invoke();
            if (transform.position.z == GameManager.Instance.RowDisableZPos)
            {
                Invoke("ChangeToReady", GameManager.Instance.GameStartTimeOffset);
            }
        }
    }
    private void OnEnable()
    {
        ResetState(State.Ready, State.Ready);
        previousPosition = transform.position;
        GameManager.Instance.OnRowMove.RemoveListener(ChangeToMove);
        GameManager.Instance.OnRowMove.AddListener(ChangeToMove);
    }

    private void ResetState(State curent, State previous)
    {
        currentState = curent;
        previousState = previous;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Ready:
                ReadyUpdate();
                break;
            case State.Move:
                MoveUpdate();
                break;
            case State.Active:
                ActiveUpdate();
                break;
            case State.Sink:
                SinkUpdate();
                break;
        }
    }


    private void ReadyUpdate()
    {
    }

    private void MoveUpdate()
    {
        if(transform.position.z > previousPosition.z - GameManager.Instance.PlatformOffset)
        {
            transform.Translate(0f, 0f, GameManager.Instance.PlatformSpeed * Time.deltaTime * -1f);
        }
        else
        {
            currentState = previousState;
            if(previousState == State.Ready)
            {
                if(transform.position.z <= GameManager.Instance.RowActiveZPos)
                {
                    ChangeToActive();
                }
            }
            else if(previousState == State.Active)
            {
                if(transform.position.z <= GameManager.Instance.RowDisableZPos)
                {
                    ChangeToReady();
                }
            }
        }
    }

    private void ActiveUpdate()
    {

    }

    private void SinkUpdate()
    {
        if (transform.position.y > previousPosition.y - GameManager.Instance.PlatformOffset)
        {
            transform.Translate(0f, GameManager.Instance.PlatformSpeed * Time.deltaTime * -1f, 0f);
        }
        else
        {
            currentState = previousState;
            if(transform.position.z <= GameManager.Instance.RowDisableZPos)
            {
                ResetRow();
            }
        }
    }

    private void ChangeToActive()
    {
        OnRowActive.Invoke();
        previousPosition = transform.position;
        previousState = State.Active;
        currentState = State.Sink;
    }

    private void ChangeToReady()
    {
        previousPosition = transform.position;
        previousState = State.Ready;
        currentState = State.Sink;
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