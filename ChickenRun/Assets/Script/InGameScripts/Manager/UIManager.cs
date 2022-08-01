using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets;

public class UIManager : MonoBehaviour
{
    public Sprite[] ShapeSprites;

    public GameObject InGameUI;
    public GameObject ReadyTextObject;
    public TextMeshProUGUI InGameScoreText;
    public Image ShapeImage;
    
    public GameObject GameOverUI;
    public TextMeshProUGUI GameOverText;
    public RectTransform ChickenImageTransform;
    public TextMeshProUGUI GameOverUIScoreText;
    public TextMeshProUGUI GameOverUIBestScoreText;

    public float ColorChangeSpeed = 0.05f;
    public float ScaleChangeSpeed = 0.1f;
    public float MinScale = 0.5f;
    public float MaxScale = 1.6f;

    public TextMeshProUGUI FlowerCountText;

    private PlayerEncounter encounterScripts;

    private void OnEnable()
    {
        // Game Start 연결
        GameManager.Instance.OnGameStart.RemoveListener(StartGame);
        GameManager.Instance.OnGameStart.AddListener(StartGame);

        // Game Over 연결
        GameManager.Instance.OnGameOver.RemoveListener(ActiveGameOverUI);
        GameManager.Instance.OnGameOver.AddListener(ActiveGameOverUI);

        // Shape 스프라이트 선택(변경) 연결
        GameManager.Instance.OnSelectShape.RemoveListener(ShowShape);
        GameManager.Instance.OnSelectShape.AddListener(ShowShape);

        // Shape 스프라이트 적용 연결
        GameManager.Instance.OnShapeChange.RemoveListener(WarningEnd);
        GameManager.Instance.OnShapeChange.AddListener(WarningEnd);

        // Shape 스프라이트 경고 연결
        GameManager.Instance.OnShapeChangeWarning.RemoveListener(WarningStart);
        GameManager.Instance.OnShapeChangeWarning.AddListener(WarningStart);

        // 스코어 text 변경 연결
        GameManager.Instance.OnScoreChange.RemoveListener(ChangeTime);
        GameManager.Instance.OnScoreChange.AddListener(ChangeTime);


        GameOverText.color = new Color(1f, 0f, 0f);

        PlayerPrefsKey.GetIntByKey(PlayerPrefsKey.BestScoreKey);
        InGameUI.SetActive(true);
        GameOverUI.SetActive(false);
        ShowShape();
    }

    private void Start()
    {

        // 꽃 획득 연결
        encounterScripts = FindObjectOfType<PlayerEncounter>();
        encounterScripts.OnGainFlower.RemoveListener(ChangeFlowerCount);
        encounterScripts.OnGainFlower.AddListener(ChangeFlowerCount);
    }

    private void ActiveGameOverUI()
    {
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
        StartCoroutine(GameOverColorEffect());
        StartCoroutine(ChickenPoundingEffect());
        GameOverUIScoreText.text = $"Final Score : {GameManager.Instance.Score / 10}.{GameManager.Instance.Score % 10}s";
        int bestScore = PlayerPrefsKey.GetIntByKey(PlayerPrefsKey.BestScoreKey);
        if (bestScore < GameManager.Instance.Score)
        {
            bestScore = GameManager.Instance.Score;
            PlayerPrefs.SetInt("BestScore", GameManager.Instance.Score);
        }
        GameOverUIBestScoreText.text = $"Best Score : {bestScore / 10}.{bestScore % 10}s";
    }

    private void ShowShape()
    {
        switch(GameManager.Instance.CurrentSafeShape)
        {
            case PlatformShape.CIRCLE:
                ShapeImage.sprite = ShapeSprites[0];
                break;
            case PlatformShape.TRIANGLE:
                ShapeImage.sprite = ShapeSprites[1];
                break;
            case PlatformShape.SQUARE:
                ShapeImage.sprite = ShapeSprites[2];
                break;
        }
    }

    private void ChangeTime()
    {
        InGameScoreText.text = $"{GameManager.Instance.Score / 10}.{GameManager.Instance.Score % 10}s";
    }

    private void ChangeFlowerCount()
    {
        FlowerCountText.text = $"X {encounterScripts.FlowerCount}";
    }

    private IEnumerator ShapeWarningEffect()
    {
        while(true)
        {
            ShapeImage.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            ShapeImage.color = Color.red;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void WarningStart()
    {
        StartCoroutine(ShapeWarningEffect());
    }

    private void WarningEnd(PlatformShape shape)
    {
        StopAllCoroutines();
        ShapeImage.color = Color.white;
    }

    private IEnumerator GameOverColorEffect()
    {
        const int R = 0;
        const int G = 1;
        const int B = 2;
        const int COLOR_MAX = 3;

        int curColor = G;

        float elapsedTime = 0;
        // ChangeSpeed

        float startValue = 1;
        float endValue = 0;
        float curValue;

        while (true)
        {
            if (elapsedTime >= ColorChangeSpeed)
            {
                Color textColor = GameOverText.color;
                elapsedTime = 0f;
                switch (curColor)
                {
                    case R:
                        textColor.r = endValue;
                        break;
                    case G:
                        textColor.g = endValue;
                        break;
                    case B:
                        textColor.b = endValue;
                        break;
                }
                GameOverText.color = textColor;

                switch (curColor)
                {
                    case R:
                        if (textColor.r == 1f)
                        {
                            startValue = 1f;
                            endValue = 0f;
                        }
                        else
                        {
                            startValue = 0f;
                            endValue = 1f;
                        }
                        break;
                    case G:
                        if (textColor.g == 1f)
                        {
                            startValue = 1f;
                            endValue = 0f;
                        }
                        else
                        {
                            startValue = 0f;
                            endValue = 1f;
                        }
                        break;
                    case B:
                        if (textColor.b == 1f)
                        {
                            startValue = 1f;
                            endValue = 0f;
                        }
                        else
                        {
                            startValue = 0f;
                            endValue = 1f;
                        }
                        break;
                }

                curColor = (curColor + 1) % COLOR_MAX;
            }
            else
            {
                elapsedTime += Time.deltaTime;
                curValue = Mathf.Lerp(startValue, endValue, elapsedTime / ColorChangeSpeed);
                Color textColor = GameOverText.color;
                switch (curColor)
                {
                    case R:
                        textColor.r = curValue;
                        break;
                    case G:
                        textColor.g = curValue;
                        break;
                    case B:
                        textColor.b = curValue;
                        break;
                }
                GameOverText.color = textColor;
            }

            yield return null;
        }
    }

    private IEnumerator ChickenPoundingEffect()
    {
        float startScale = MinScale;
        float endScale = MaxScale;
        float currentScale;

        float elapsedTime = 0f;

        // 초기화
        float originalWidth = ChickenImageTransform.sizeDelta.x;
        float originalHeight = ChickenImageTransform.sizeDelta.y;
        
        while(true)
        {
            if(elapsedTime >= ScaleChangeSpeed)
            {
                elapsedTime = 0f;
                ChickenImageTransform.sizeDelta = new Vector2(originalWidth * endScale, originalHeight * endScale);

                float temp = startScale;
                startScale = endScale;
                endScale = temp;
            }
            else
            {
                elapsedTime += Time.deltaTime;
                currentScale = Mathf.Lerp(startScale, endScale, elapsedTime / ScaleChangeSpeed);
                ChickenImageTransform.sizeDelta = new Vector2(originalWidth * currentScale, originalHeight * currentScale);
            }
            yield return null;
        }
    }

    private IEnumerator GameStartTextChange()
    {
        Debug.Log("GameStart");
        TextMeshProUGUI readyText = ReadyTextObject.GetComponent<TextMeshProUGUI>();

        readyText.text = "Ready...";
        ReadyTextObject.SetActive(true);
        yield return new WaitForSeconds(GameManager.StartTimeOffset / 2);

        readyText.text = "Start!";
        yield return new WaitForSeconds(GameManager.StartTimeOffset / 2);
        ReadyTextObject.SetActive(false);
    }
    private void StartGame()
    {
        StartCoroutine(GameStartTextChange());
    }
}
