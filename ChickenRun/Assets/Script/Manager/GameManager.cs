using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    // ���� ���� ����
    public float GameStartTimeOffset = 1.5f;

    // Platform ������ ����
    public float PlatformSpeed = 5.5f;
    public readonly float PlatformOffset = 1.5f;

    public float RowActiveZPos = 1.5f;
    public float RowDisableZPos = -4.5f;
    public UnityEvent OnRowMove = new UnityEvent();

    public float SelectDelay = 1f;

    // Platform ���� ����
    public enum PlatformShape
    {
        CIRCLE,
        TRIANGLE,
        SQUARE
    }
    public const int PlatformShapeCount = 3;

    public UnityEvent<PlatformShape> OnShapeChange = new UnityEvent<PlatformShape>();
    private PlatformShape safeShape;
    public PlatformShape Shape
    {
        get { return safeShape; }
        private set
        {
            safeShape = value;
            OnSelectShape.Invoke();
        }
    }

    // ��ֹ�, ���� ��ȯ ����
    public float GurnishMoveSpeed = 6f;
    public float GurnishRotateSpeed = 100f;

    public float MinGurnishCooltime = 6f;
    public float MaxGurnishCooltime = 10f;

    public float FlowerGenerateRate = 10f;

    // Platform Row
    public PlatformRowSettings[] PlatformRows;

    public bool IsGameOver = false;

    // �÷��̾� ���� ����
    private int flowerCount = 0;
    public int FlowerCount { get; }
    public float MinMoveableOffset = 0.3f;

    // UI
    public UnityEvent OnSelectShape = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();
    public UnityEvent OnScoreChange = new UnityEvent();

    public int score = 0;
    private int hitUpCount = 1;

    // Start is called before the first frame update
    void Awake()
    {
        PickShape();
        Invoke("StartGame", GameStartTimeOffset);
        IsGameOver = false;
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
        else
        {
            if(score > 150 * hitUpCount)
            {
                if (SelectDelay > 0.4f)
                    SelectDelay -= 0.02f;
                PlatformSpeed += 0.07f;
                ++hitUpCount;
            }
        }
    }

    private IEnumerator CubeSelect()
    {
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

    private IEnumerator ChangeScore()
    {
        while (!GameManager.Instance.IsGameOver)
        {
            yield return new WaitForSeconds(0.1f);
            ++score;
            OnScoreChange.Invoke();
        }
    }

    void PickShape()
    {
        Shape = (PlatformShape)Random.Range(0, PlatformShapeCount);
    }

    private void StartGame()
    {
        StartCoroutine(CubeSelect());
        StartCoroutine(ChangeScore());
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
