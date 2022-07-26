using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Sprite[] ShapeSprites;

    public GameObject InGameUI;
    public TextMeshProUGUI InGameScoreText;
    public Image ShapeImage;
    
    public GameObject GameOverUI;
    public TextMeshProUGUI GameOverGameOverText;
    public Image GameOverChickenImage;
    public TextMeshProUGUI GameOverUIScoreText;
    public TextMeshProUGUI GameOverUIBestScoreText;

    public float ChangeSpeed = 1f;

    public TextMeshProUGUI FlowerCountText;

    private int[] colorChangeTable = { 0, -1, 0, 0, 1, 0 };
    private int currentChangePoint = 0; // R: 0  G: 4  B: 5
    //private int[,] colorChangeTable =
    //{
    //    { 0, 1, 0 }, // 처음
    //    { -1, 0, 0 }, //G가 1(255)가 된 후
    //    { 0, 0, 1 }, //R이 0(0)이 된 후
    //    { 0, -1, 0 }, //B가 1(255)가 된 후
    //    { 1, 0, 0 }, //G가 0(0)가 된 후
    //    { 0, 0, -1 } //R이 1(255)이 된 후
    //};

    private void Start()
    {
        GameManager.Instance.OnGameOver.RemoveListener(ActiveUI);
        GameManager.Instance.OnGameOver.AddListener(ActiveUI);

        GameManager.Instance.OnSelectShape.RemoveListener(ShowShape);
        GameManager.Instance.OnSelectShape.AddListener(ShowShape);

        GameManager.Instance.OnScoreChange.RemoveListener(ChangeTime);
        GameManager.Instance.OnScoreChange.AddListener(ChangeTime);

        GameManager.Instance.OnGainFlower.RemoveListener(ChangeFlowerCount);
        GameManager.Instance.OnGainFlower.AddListener(ChangeFlowerCount);

        GameOverGameOverText.color = new Color(1, 0, 0);

        if (!PlayerPrefs.HasKey("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }
        InGameUI.SetActive(true);
        GameOverUI.SetActive(false);
        ShowShape();
        ChangeFlowerCount();
    }

    public void OnDisable()
    {
        GameManager.Instance.OnGameOver.RemoveListener(ActiveUI);
        GameManager.Instance.OnSelectShape.RemoveListener(ShowShape);
        GameManager.Instance.OnScoreChange.RemoveListener(ChangeTime);
    }

    private void ActiveUI()
    {
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
        StartCoroutine(GameOverEffect());
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

    private IEnumerator GameOverEffect()
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
            if (elapsedTime >= ChangeSpeed)
            {
                Color textColor = GameOverGameOverText.color;
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
                GameOverGameOverText.color = textColor;

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
                curValue = Mathf.Lerp(startValue, endValue, elapsedTime / ChangeSpeed);
                Color textColor = GameOverGameOverText.color;
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
                GameOverGameOverText.color = textColor;
            }

            //GameOverGameOverText.color = new Color(
            //    GameOverGameOverText.color.r + colorChangeTable[currentChangePoint],
            //    GameOverGameOverText.color.g + colorChangeTable[(currentChangePoint + 4) % 6],
            //    GameOverGameOverText.color.b + colorChangeTable[(currentChangePoint + 5) % 6]
            //);

            //switch(currentChangePoint)
            //{
            //    case 0:
            //        if (GameOverGameOverText.color.g >= 255)
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        break;
            //    case 1:
            //        if (GameOverGameOverText.color.r <= 0)
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        break;
            //    case 2:
            //        if (GameOverGameOverText.color.b >= 255)
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        break;
            //    case 3:
            //        if (GameOverGameOverText.color.g <= 0)
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        break;
            //    case 4:
            //        if (GameOverGameOverText.color.g >= 0)
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        break;
            //    case 5:
            //        if (GameOverGameOverText.color.r >= 255)
            //            currentChangePoint = (currentChangePoint + 1) % 6;
            //        break;
            //}

            yield return null;
        }
    }
}
