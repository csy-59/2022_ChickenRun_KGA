using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    // 게임 진행 관련
    public float GameStartTimeOffset = 1.5f;

    // Platform 움직임 관련
    public float PlatformSpeed = 5.5f;
    public readonly float PlatformOffset = 1.5f;

    public float RowActiveZPos = 1.5f;
    public float RowDisableZPos = -4.5f;
    public UnityEvent OnRowMove = new UnityEvent();

    private Coroutine platformActive;
    public float SelectDelay = 1f;

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
    }

    // 장애물, 씨앗 소환 관련
    public float GurnishMoveSpeed = 6f;
    public float GurnishRotateSpeed = 100f;

    public float MinGurnishCooltime = 6f;
    public float MaxGurnishCooltime = 10f;

    public float FlowerGenerateRate = 10f;

    // Platform Row
    public PlatformRowSettings[] PlatformRows;

    public bool IsGameOver = false;

    // 플래이어 정보 관련
    private int flowerCount = 0;
    public int FlowerCount { get; }
    public float MinMoveableOffset = 0.3f;

    // UI
    public UnityEvent<PlatformShape> OnSelectShape = new UnityEvent<PlatformShape>();
    public UnityEvent OnGameOver = new UnityEvent();

    // Start is called before the first frame update
    void Awake()
    {
        Invoke("StartGame", GameStartTimeOffset);
        IsGameOver = false;
        PickShape();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetGame();
            }
        }
    }

    private IEnumerator CubeSelect()
    {
        PickShape();
        while (!IsGameOver)
        {
            yield return new WaitForSeconds(PlatformOffset / PlatformSpeed * 1 + SelectDelay);

            OnShapeChange.Invoke(Shape);
            yield return new WaitForSeconds(PlatformOffset / PlatformSpeed * 2 + SelectDelay);

            OnRowMove.Invoke();
            PickShape();
            yield return new WaitForSeconds(PlatformOffset / PlatformSpeed * 2 + SelectDelay);
        }
    }

    void PickShape()
    {
        safeShape = (PlatformShape)Random.Range(0, PlatformShapeCount);
        Debug.Log($"GM: {safeShape}");
        OnSelectShape.Invoke(safeShape);
    }

    private void StartGame()
    {
        if(platformActive != null)
            StopCoroutine(platformActive);
        platformActive = StartCoroutine(CubeSelect());
    }

    public void PlayerDead()
    {
        IsGameOver = true;
        OnGameOver.Invoke();
        StopAllCoroutines();
        Invoke("TimeScale0", 0.5f);
    }
    public void TimeScale0()
    {
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Reset");
        IsGameOver = false;
        Time.timeScale = 1f;
        Invoke("StartGame", GameStartTimeOffset);
    }

    public void GetFlower()
    {
        ++flowerCount;
    }
}
