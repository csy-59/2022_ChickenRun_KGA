using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformRow : MonoBehaviour
{
    public Transform[] CubePosition = new Transform[3];
    public GameObject[] CubePrefabs = new GameObject[3];

    public float RowPositionOffset = 1.5f;
    public float ActiveZPoint = 1.5f;
    public float EnableZPoint = -6f;

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
    private State nextState;

    public UnityEvent OnRowActive = new UnityEvent();

    private Vector3 previousPosition;

    private void OnEnable()
    {
        SetRow();
        currentState = transform.position.z >= 1.5f ? State.Ready : State.Active;
        previousPosition = transform.position;
        StartCoroutine(ChangeToMove());
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
        Debug.Log("Ready");
    }

    private void MoveUpdate()
    {
        Debug.Log("Move");
        if(transform.position.z > previousPosition.z - RowPositionOffset)
        {
            transform.Translate(0f, 0f, GameManager.RowMoveSpeed * Time.deltaTime * -1f);
        }
        else
        {
            if(transform.position.z > ActiveZPoint && previousState == State.Ready)
            {
                currentState = State.Ready;
            }
            else
            {
                ChangeToActive();
            }

        }
    }
    
    private void ActiveUpdate()
    {

    }
    
    private void SinkUpdate()
    {
        Debug.Log("Sink");
        if (transform.position.y > previousPosition.y - RowPositionOffset)
        {
            transform.Translate(0f, GameManager.RowMoveSpeed * Time.deltaTime * -1f, 0f);
        }
        else
        {
            if (transform.position.z < EnableZPoint)
            {
                gameObject.SetActive(false);
            }
            else
            {
                currentState = State.Active;
            }
        }
    }

    private void ChangeToActive()
    {
        OnRowActive.Invoke();
        previousPosition = transform.position;
        currentState = State.Sink;
    }

    private IEnumerator ChangeToMove()
    {
        while(currentState != State.Stop)
        {
            Debug.Log("ChangeToMove");
            previousState = currentState;
            currentState = State.Move;
            previousPosition = transform.position;
            yield return new WaitForSeconds(RowPositionOffset * 2);
        }
    }
}
