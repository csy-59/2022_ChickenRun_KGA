using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBehaviour<GameManager>
{
    // Platform 움직임 관련
    public float RowMoveSpeed = 1f;
    public readonly float RowPositionOffset = 1.5f;

    public float CubeSpeed = 1f;
    public float CubeSinkOffset = 1.2f;

    public float ShapeSelectTimeOffset = 2.5f;

    public float GameStartTimeOffset = 3f;

    public UnityEvent OnRowMove = new UnityEvent();

    // Platform 선택 관련
    public enum PlatformShape
    {
        CIRCLE,
        TRIANGLE,
        SQUARE
    }
    public const int PlatformShapeCount = 3;

    public UnityEvent<PlatformShape> OnShapeChange = new UnityEvent<PlatformShape>();
    private static PlatformShape safeShape;
    public static PlatformShape Shape
    {
        get { return safeShape; }
        set
        {
            safeShape = value;
            GameManager.Instance.OnShapeChange.Invoke(safeShape);
        }
    }

    // 장애물, 씨앗 소환 관련
    public UnityEvent OnGenarateGurnish = new UnityEvent();

    private bool isGameOver = false;

    // Start is called before the first frame update
    void Awake()
    {
        Invoke("StartGame", GameStartTimeOffset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator RowMove()
    {
        while(!isGameOver)
        {
            yield return new WaitForSeconds(RowPositionOffset / RowMoveSpeed + 0.1f);
            OnRowMove.Invoke();
            yield return new WaitForSeconds(RowPositionOffset / RowMoveSpeed + 0.1f);
        }
    }

    private IEnumerator CubeSelect()
    {
        while(!isGameOver)
        {
            yield return new WaitForSeconds(ShapeSelectTimeOffset);
            PickShape();
            OnShapeChange.Invoke(Shape);
            yield return new WaitForSeconds(CubeSinkOffset / CubeSpeed * 2 + 0.1f);
        }
    }

    void PickShape()
    {
        Shape = (PlatformShape)Random.Range(0, PlatformShapeCount);
    }



    private void StartGame()
    {
        StartCoroutine(RowMove());
        StartCoroutine(CubeSelect());
    }
}
