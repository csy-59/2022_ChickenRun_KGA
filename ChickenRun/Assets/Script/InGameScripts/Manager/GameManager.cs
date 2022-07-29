using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Assets;

public class GameManager : SingletonBehaviour<GameManager>
{
    // 게임 진행 관련
    public const float StartTimeOffset = 1.5f;

    // Platform 움직임 관련
    public float PlatformSpeed = 4f;
    public float SelectDelay = 0.6f;

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
    public float GurnishMoveSpeed = 6f;
    public float GurnishRotateSpeed = 100f;
    public const float hitupGurnishMoveSpeedUpValue = 0.05f;

    public float MinGurnishCooltime = 5f;
    public float MaxGurnishCooltime = 7f;
    private const float hitupGurnishCooltimeDownValue = 0.5f;

    public float FlowerGenerateRate = 10f;

    // Platform Row
    public PlatformRowSettings[] PlatformRows;

    public bool IsGameOver = false;

    // 플래이어 정보 관련
    public GameObject[] PlayerPrefabs;
    public float MinMoveableOffset = 0.3f;

    // UI Events
    public UnityEvent OnGameStart = new UnityEvent();
    public UnityEvent OnScoreChange = new UnityEvent();
    public UnityEvent OnSelectShape = new UnityEvent();
    public UnityEvent OnShapeChangeWarning = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();

    public int Score = 0;
    private int hitUpCount = 1;

    // Start is called before the first frame update
    void Awake()
    {
        int selectedPlayer = PlayerPrefsKey.GetIntByKey(PlayerPrefsKey.SelectedPlayer);
        Instantiate(PlayerPrefabs[selectedPlayer]);

        PickShape();
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
            OnScoreChange.Invoke();
        }
    }

    private IEnumerator GameStart()
    {
        OnGameStart.Invoke();
        yield return new WaitForSeconds(StartTimeOffset);
        StartAllCoroutines();
    }

    void PickShape()
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
        StopAllCoroutines();
        AllStop();
    }
    public void TimeScale0()
    {
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        Score = 0;
        hitUpCount = 1;

        PlatformSpeed = 4f;
        GurnishMoveSpeed = 6f;
        GurnishRotateSpeed = 100f;
        MinGurnishCooltime = 6f;
        MaxGurnishCooltime = 10f;
        
        IsGameOver = false;
        StartCoroutine(GameStart());
        SceneManager.LoadScene(1);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
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
