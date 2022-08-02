using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Assets;

public class GameManager : SingletonBehaviour<GameManager>
{
    // 게임 진행 관련
    public const float StartTimeOffset = 1.5f;
    public bool IsGameOver { get; private set; }
    private int score = 0;
    public int Score 
    { 
        get { return score; } 
        private set
        {
            score = value;
            OnScoreChange.Invoke();
        }
    }
    private int hitUpCount = 1;

    // Platform 움직임 관련
    public float PlatformSpeed { get; private set; }
    public float SelectDelay { get; private set; }

    private const float hitUpDelayDownValue = 0.05f;
    private const float hitupPlatformSpeedUpValue = 0.07f;

    public const float PlatformRowMoveOffset = 1.5f;
    public const float RowActiveZPos = 1.5f;
    public const float RowDisableZPos = -4.5f;

    public UnityEvent OnRowMove = new UnityEvent();

    // Platform 선택 관련
    public UnityEvent<PlatformShape> OnShapeChange = new UnityEvent<PlatformShape>();
    private PlatformShape currentSafeShape;
    public PlatformShape CurrentSafeShape
    {
        get { return currentSafeShape; }
        private set
        {
            currentSafeShape = value;
            OnSelectShape.Invoke();
        }
    }

    // 장애물, 씨앗 소환 관련
    public float GurnishMoveSpeed { get; private set; }
    public float GurnishRotateSpeed { get; private set; }
    private const float hitupGurnishMoveSpeedUpValue = 0.05f;

    public float MinGurnishCooltime { get; private set; }
    public float MaxGurnishCooltime { get; private set; }
    private const float hitupGurnishCooltimeDownValue = 0.5f;

    // 플래이어 정보 관련
    public GameObject[] PlayerPrefabs;

    // UI Events
    public UnityEvent OnGameStart = new UnityEvent();
    public UnityEvent OnScoreChange = new UnityEvent();
    public UnityEvent OnSelectShape = new UnityEvent();
    public UnityEvent OnShapeChangeWarning = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();

    void Awake()
    {
        // 기본 정보 초기화
        IsGameOver = false;

        PlatformSpeed = 5f;
        SelectDelay = 0.6f;

        GurnishMoveSpeed = 6f;
        GurnishRotateSpeed = 100f;
        MinGurnishCooltime = 5f;
        MaxGurnishCooltime = 7f;

        int selectedPlayer = PlayerPrefsKey.GetIntByKey(PlayerPrefsKey.SelectedPlayerKey);
        Instantiate(PlayerPrefabs[selectedPlayer]);

        PickShape();
        StartCoroutine(GameStart());
    }

    private void Start()
    {
        StartCoroutine(GameStart());
    }

    private void Update()
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
            if(Score > 150 * hitUpCount)
            {
                if (SelectDelay > 0.4f)
                    SelectDelay -= hitUpDelayDownValue;
                PlatformSpeed += hitupPlatformSpeedUpValue;

                GurnishMoveSpeed += hitupGurnishMoveSpeedUpValue;
                MinGurnishCooltime -= hitupGurnishCooltimeDownValue;
                MaxGurnishCooltime -= hitupGurnishCooltimeDownValue;

                ++hitUpCount;
            }
        }
    }

    private IEnumerator CubeSelect()
    {
        while (!IsGameOver)
        {
            OnShapeChangeWarning.Invoke();
            yield return new WaitForSeconds(PlatformRowMoveOffset / PlatformSpeed * 1 + SelectDelay);

            OnShapeChange.Invoke(CurrentSafeShape);
            yield return new WaitForSeconds(PlatformRowMoveOffset / PlatformSpeed * 2 + SelectDelay);

            OnRowMove.Invoke();
            PickShape();
            yield return new WaitForSeconds(PlatformRowMoveOffset / PlatformSpeed * 2 + SelectDelay);
        }
    }

    private IEnumerator ChangeScore()
    {
        while (!IsGameOver)
        {
            yield return new WaitForSeconds(0.1f);
            ++Score;
        }
    }

    private IEnumerator GameStart()
    {
        OnGameStart.Invoke();
        yield return new WaitForSeconds(StartTimeOffset);

        StartAllCoroutines();
    }

    private void PickShape()
    {
        CurrentSafeShape = (PlatformShape)Random.Range(0, (int) PlatformShape.PlatformCount);
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
        
        AllStop();
        StopAllCoroutines();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene((int)SceneType.InGame);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene((int)SceneType.Main);
    }

    private void AllStop()
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
