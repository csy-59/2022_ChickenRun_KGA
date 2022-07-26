using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    // 게임 진행 관련
    public float GameStartTimeOffset = 3f;

    // Platform 움직임 관련
    public float RowMoveSpeed = 0.5f;
    public readonly float RowPositionOffset = 1.5f;

    public float CubeSpeed = 0.5f;
    public float CubeSinkOffset = 1.2f;

    public float RowActiveZPos = 1.5f;
    public float RowDisableZPos = -4.5f;
    public UnityEvent OnRowMove = new UnityEvent();

    // Platform 선택 관련
    public enum PlatformShape
    {
        CIRCLE,
        TRIANGLE,
        SQUARE
    }
    public const int PlatformShapeCount = 3;

    public UnityEvent<PlatformShape> OnSelectShape = new UnityEvent<PlatformShape>();
    public UnityEvent<PlatformShape> OnShapeChange = new UnityEvent<PlatformShape>();
    private static PlatformShape safeShape;
    public static PlatformShape Shape
    {
        get { return safeShape; }
    }

    // 장애물, 씨앗 소환 관련
    public float GurnishMoveSpeed = 1f;
    public float GurnishRotateSpeed = 1f;

    public float MinGurnishCooltime = 10f;
    public float MaxGurnishCooltime = 15f;

    public float FlowerGenerateRate = 10f;

    // Platform Row
    public PlatformRowSettings[] PlatformRows;

    public bool IsGameOver = false;

    // 플래이어 정보 관련
    private int flowerCount = 0;
    public int FlowerCount { get; private set; }
    public float MinMoveableOffset = 0.3f;

    // UI
    public UnityEvent<bool> OnActiveUI = new UnityEvent<bool>();

    // Start is called before the first frame update
    void Awake()
    {
        Invoke("StartGame", GameStartTimeOffset);
        OnActiveUI.Invoke(IsGameOver);
        PickShape();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                PickShape();
                StopAllCoroutines();
                ResetGame();
            }
        }
        
    }

    private IEnumerator RowMove()
    {
        while(!IsGameOver)
        {
            yield return new WaitForSeconds(RowPositionOffset / RowMoveSpeed + 0.1f);
            OnRowMove.Invoke();
            yield return new WaitForSeconds(RowPositionOffset / RowMoveSpeed + 0.1f);
        }
    }

    private IEnumerator CubeSelect()
    {
        while(!IsGameOver)
        {
            OnShapeChange.Invoke(Shape);
            yield return new WaitForSeconds(CubeSinkOffset / CubeSpeed * 2 + 0.1f);
            PickShape();
            yield return new WaitForSeconds(CubeSinkOffset / CubeSpeed * 2 + 0.1f);
        }
    }

    void PickShape()
    {
        safeShape = (PlatformShape)Random.Range(0, PlatformShapeCount);
        Debug.Log(safeShape);
        OnSelectShape.Invoke(safeShape);
    }

    private void StartGame()
    {
        StartCoroutine(RowMove());
        StartCoroutine(CubeSelect());
    }

    public void PlayerDead()
    {
        IsGameOver = true;
        Invoke("TimeScale0", 0.5f);
        OnActiveUI.Invoke(IsGameOver);
        //StopAllCoroutines();
    }
    public void TimeScale0()
    {
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        IsGameOver = false;
        SceneManager.LoadScene(0);
        Invoke("StartGame", GameStartTimeOffset);
        OnActiveUI.Invoke(IsGameOver);
        Time.timeScale = 1f;
    }

    public void GetFlower()
    {
        ++FlowerCount;
    }
}
