using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        // 꽃 획득 연결
        GameManager.Instance.OnGainFlower.RemoveListener(ChangeFlowerCount);
        GameManager.Instance.OnGainFlower.AddListener(ChangeFlowerCount);

        GameOverText.color = new Color(1f, 0f, 0f);

        if (!PlayerPrefs.HasKey("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }
        InGameUI.SetActive(true);
        GameOverUI.SetActive(false);
        ShowShape();
    }

    private void ActiveGameOverUI()
    {
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
        StartCoroutine(GameOverColorEffect());
        StartCoroutine(ChickenPoundingEffect());
        GameOverUIScoreText.text = $"Final Score : {GameManager.Instance.score / 10}.{GameManager.Instance.score % 10}s";
        int bestScore = PlayerPrefs.GetInt("BestScore");
        if (bestScore < GameManager.Instance.score)
        {
            bestScore = GameManager.Instance.score;
            PlayerPrefs.SetInt("BestScore", GameManager.Instance.score);
        }
        GameOverUIBestScoreText.text = $"Best Score : {bestScore / 10}.{bestScore % 10}s";
    }

    private void ShowShape()
    {
        switch(GameManager.Instance.Shape)
        {
            case GameManager.PlatformShape.CIRCLE:
                ShapeImage.sprite = ShapeSprites[0];
                break;
            case GameManager.PlatformShape.TRIANGLE:
                ShapeImage.sprite = ShapeSprites[1];
                break;
            case GameManager.PlatformShape.SQUARE:
                ShapeImage.sprite = ShapeSprites[2];
                break;
        }
    }

    private void ChangeTime()
    {
        InGameScoreText.text = $"{GameManager.Instance.score / 10}.{GameManager.Instance.score % 10}s";
    }

    private void ChangeFlowerCount()
    {
        FlowerCountText.text = $"X {GameManager.Instance.FlowerCount}";
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

    private void WarningEnd(GameManager.PlatformShape shape)
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

            //GameOverGameOverText.color = new Color(
            //    GameOverGameOverText.color.r + colorChangeTable[currentChangePoint] * ChangeSpeed / 255,
            //    GameOverGameOverText.color.g + colorChangeTable[(currentChangePoint + 4) % 6] * ChangeSpeed / 255,
            //    GameOverGameOverText.color.b + colorChangeTable[(currentChangePoint + 5) % 6] * ChangeSpeed / 255
            //);
            //
            //Debug.Log($"{currentChangePoint} {GameOverGameOverText.color}");
            //
            //switch(currentChangePoint)
            //{
            //    case 0:
            //        if (GameOverGameOverText.color.g >= 1)
            //        {
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        }
            //        break;
            //    case 1:
            //        if (GameOverGameOverText.color.r <= 0)
            //        {
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        }
            //        break;
            //    case 2:
            //        if (GameOverGameOverText.color.b >= 1)
            //        {
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        }
            //        break;
            //    case 3:
            //        if (GameOverGameOverText.color.g <= 0)
            //        {
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        }
            //        break;
            //    case 4:
            //        if (GameOverGameOverText.color.g >= 0)
            //        {
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        }
            //        break;
            //    case 5:
            //        if (GameOverGameOverText.color.r >= 1)
            //        {
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        }
            //        break;
            //}

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
        yield return new WaitForSeconds(GameManager.Instance.GameStartTimeOffset / 2);

        readyText.text = "Start!";
        yield return new WaitForSeconds(GameManager.Instance.GameStartTimeOffset / 2);
        ReadyTextObject.SetActive(false);
    }
    private void StartGame()
    {
        StartCoroutine(GameStartTextChange());
    }
}
