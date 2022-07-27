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
    public float PlatformSpeed = 4f;
    public readonly float PlatformOffset = 1.5f;

    public readonly float RowActiveZPos = 1.5f;
    public readonly float RowDisableZPos = -4.5f;
    public UnityEvent OnRowMove = new UnityEvent();

    public float SelectDelay = 0.6f;

    // Platform 선택 관련
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
    public GameObject[] PlayerObject;
    public float MinMoveableOffset = 0.3f;
    private int flowerCount = 0;
    public int FlowerCount
    {
        get { return flowerCount; }
        private set
        {
            flowerCount = value;
            OnGainFlower.Invoke();
        }
    }
    public UnityEvent OnGainFlower = new UnityEvent();

    // UI
    public UnityEvent OnSelectShape = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();
    public UnityEvent OnScoreChange = new UnityEvent();
    public UnityEvent OnGameStart = new UnityEvent();
    public UnityEvent OnShapeChangeWarning = new UnityEvent();

    public int score = 0;
    private int hitUpCount = 1;

    // Start is called before the first frame update
    void Awake()
    {
        PickShape();
        if(!PlayerPrefs.HasKey("FlowerCount"))
        {
            PlayerPrefs.SetInt("FlowerCount", 0);
        }
        FlowerCount = PlayerPrefs.GetInt("FlowerCount");

        if (!PlayerPrefs.HasKey("SelectedPlayer"))
        {
            PlayerPrefs.SetInt("SelectedPlayer", 0);
        }
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer");
        PlayerObject[selectedPlayer].SetActive(true);

        StartCoroutine(GameStart());
    }

    private void Start()
    {
        StartCoroutine(GameStart());
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
            OnShapeChangeWarning.Invoke();
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
        while (!IsGameOver)
        {
            yield return new WaitForSeconds(0.1f);
            ++score;
            OnScoreChange.Invoke();
        }
    }

    private IEnumerator GameStart()
    {
        OnGameStart.Invoke();
        yield return new WaitForSeconds(GameStartTimeOffset);
        StartAllCoroutines();
    }

    void PickShape()
    {
        Shape = (PlatformShape)Random.Range(0, PlatformShapeCount);
    }

    private void StartAllCoroutines()
    {
        StopAllCoroutines();
        StartCoroutine(CubeSelect());
        StartCoroutine(ChangeScore());
    }

    public void PlayerDead()
    {
        IsGameOver = true;
        OnGameOver.Invoke();
        StopAllCoroutines();
        PlayerPrefs.SetInt("FlowerCount", FlowerCount);
        AllStop();
    }
    public void TimeScale0()
    {
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        score = 0;
        hitUpCount = 1;
        IsGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void GetFlower()
    {
        ++FlowerCount;
    }

    public void AllStop()
    {
        PlatformSpeed = 0f;
        GurnishMoveSpeed = 0f;
        GurnishRotateSpeed = 0f;
        MinGurnishCooltime = 100000;
        MaxGurnishCooltime = 100001;
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
