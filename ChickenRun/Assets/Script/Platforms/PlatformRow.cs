using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformRow : MonoBehaviour
{
    public Transform[] CubePosition = new Transform[3];
    public GameObject[] CubePrefabs = new GameObject[3];
    public Vector3 ResetPosition;

    public float ActiveZPoint = 1.5f;
    public float DisableZPoint = -6f;

    private const int cubePositionCount = 3;

    private enum State
    {
        Ready,
        Move,
        Active,
        Stop,
        Sink
    }
    private State currentState;
    private State previousState;

    public UnityEvent OnRowActive = new UnityEvent();

    private Vector3 previousPosition;

    private void Start()
    {
        if (transform.position.z >= ActiveZPoint)
        {
            ResetState(State.Ready, State.Ready);
            if (transform.position.z == ActiveZPoint)
            {
                ChangeToActive();
            }
        }
        else
        {
            ResetState(State.Active, State.Active);
            if (transform.position.z == DisableZPoint)
            {
                ChangeToReady();
            }
        }
    }
    private void OnEnable()
    {
        SetRow();
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

    private void SetRow()
    {
        bool[] isFilled = new bool[3];
        foreach (GameObject cube in CubePrefabs)
        {
            int randomPosition;
            do
            {
                randomPosition = Random.Range(0, cubePositionCount);
            } while (isFilled[randomPosition]);

            isFilled[randomPosition] = true;
            cube.transform.position = CubePosition[randomPosition].position;
        }
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
        if(transform.position.z > previousPosition.z - GameManager.Instance.RowPositionOffset)
        {
            transform.Translate(0f, 0f, GameManager.Instance.RowMoveSpeed * Time.deltaTime * -1f);
        }
        else
        {
            currentState = previousState;
            if(previousState == State.Ready)
            {
                if(transform.position.z <= ActiveZPoint)
                {
                    ChangeToActive();
                }
            }
            else if(previousState == State.Active)
            {
                if(transform.position.z <= DisableZPoint)
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
        if (transform.position.y > previousPosition.y - GameManager.Instance.RowPositionOffset)
        {
            transform.Translate(0f, GameManager.Instance.RowMoveSpeed * Time.deltaTime * -1f, 0f);
        }
        else
        {
            currentState = previousState;
            if(transform.position.z <= DisableZPoint)
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

    private void OnDisable()
    {
        GameManager.Instance.OnRowMove.RemoveListener(ChangeToMove);
    }
}
