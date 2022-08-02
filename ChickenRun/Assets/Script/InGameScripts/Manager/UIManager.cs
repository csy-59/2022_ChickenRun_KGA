using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets;

public class UIManager : MonoBehaviour
{
    // 인게임 패널
    public GameObject InGameUI;
    public GameObject ReadyTextObject;
    public TextMeshProUGUI InGameScoreText;
    public Image ShapeImage;
    public Sprite[] ShapeSprites;

    private float warningEffectOffset = 0.1f;
    
    // 게임 오버 패널
    public GameObject GameOverUI;
    public TextMeshProUGUI GameOverText;
    public RectTransform ChickenImageTransform;
    public TextMeshProUGUI GameOverUIScoreText;
    public TextMeshProUGUI GameOverUIBestScoreText;

    // 게임 오버 효과 관련
    private float colorChangeSpeed = 0.05f;
    private float scaleChangeSpeed = 0.25f;
    private float minScale = 0.5f;
    private float maxScale = 1.4f;
    private static readonly Color red = Color.red;
    private static readonly Color white = Color.white;

    // 꽃 개수 관련
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

        GameOverText.color = red;

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

        ChangeFlowerCount();
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
        int second = GameManager.Instance.Score / 10;
        int milliSecond = GameManager.Instance.Score % 10;

        InGameScoreText.text = $"{second}.{milliSecond}s";
    }

    private void ChangeFlowerCount()
    {
        FlowerCountText.text = $"X {encounterScripts.FlowerCount}";
    }

    private IEnumerator ShapeWarningEffect()
    {
        while(true)
        {
            ShapeImage.color = white;
            yield return new WaitForSeconds(warningEffectOffset);

            ShapeImage.color = red;
            yield return new WaitForSeconds(warningEffectOffset);
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

    private enum ColorType
    {
        R,
        G,
        B,
        COLOR_MAX
    }

    private IEnumerator GameOverColorEffect()
    {
        ColorType currentColor = ColorType.G;

        float elapsedTime = 0;

        float startValue = 1;
        float endValue = 0;
        float curValue;

        while (true)
        {
            if (elapsedTime >= colorChangeSpeed)
            {
                elapsedTime = 0f;

                SetGameOverTextColor(currentColor, endValue);

                float temp = endValue;
                endValue = startValue;
                startValue = temp;

                currentColor = (ColorType) ((int)(currentColor + 1) % (int) ColorType.COLOR_MAX);
            }
            else
            {
                elapsedTime += Time.deltaTime;

                curValue = Mathf.Lerp(startValue, endValue, elapsedTime / colorChangeSpeed);

                SetGameOverTextColor(currentColor, curValue);
            }

            yield return null;
        }
    }

    private void SetGameOverTextColor(ColorType currentColor, float value)
    {
        Color textColor = GameOverText.color;
        switch (currentColor)
        {
            case ColorType.R:
                textColor.r = value;
                break;
            case ColorType.G:
                textColor.g = value;
                break;
            case ColorType.B:
                textColor.b = value;
                break;
        }
        GameOverText.color = textColor;
    }

    private IEnumerator ChickenPoundingEffect()
    {
        float startScale = minScale;
        float endScale = maxScale;
        float currentScale;

        float elapsedTime = 0f;

        // 초기화
        float originalWidth = ChickenImageTransform.sizeDelta.x;
        float originalHeight = ChickenImageTransform.sizeDelta.y;
        
        while(true)
        {
            if(elapsedTime >= scaleChangeSpeed)
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

                currentScale = Mathf.Lerp(startScale, endScale, elapsedTime / scaleChangeSpeed);
                
                ChickenImageTransform.sizeDelta = new Vector2(originalWidth * currentScale, originalHeight * currentScale);
            }
            yield return null;
        }
    }

    private IEnumerator GameStartTextChange()
    {
        TextMeshProUGUI readyText = ReadyTextObject.GetComponent<TextMeshProUGUI>();

        ReadyTextObject.SetActive(true);
        readyText.text = "Ready...";
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
