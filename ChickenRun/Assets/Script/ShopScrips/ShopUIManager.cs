using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Assets;

public class ShopUIManager : MonoBehaviour
{
    public ShopManager manager;

    // 기본 환경
    public TextMeshProUGUI FlowerCountText;

    // 구매 환경
    public GameObject BuyPanel;
    public TextMeshProUGUI ModelNameText;
    public GameObject BuyButton;
    public GameObject SelectButton;
    public GameObject LetsRunButton;

    // 구매 관련
    public int[] ModelPrice = { 0, 200, 300 };
    private int flowerCount = 0;
    private string[] ModelNames = { "Hannah", "Pips", "Diva" };
    private bool[] hasModelBought = { false, false, false };

    void OnEnable()
    {
        flowerCount = PlayerPrefs.GetInt("FlowerCount");
        SetFlowerCount();

        ShopSet();

        manager.OnShowModelChange.RemoveListener(EditBuyPanel);
        manager.OnShowModelChange.AddListener(EditBuyPanel);
    }

    private void SetFlowerCount()
    {
        FlowerCountText.text = flowerCount.ToString();
    }

    private void ShopSet()
    {
        for (int i = 0; i < (int)PlayerModelType.ModelCount; ++i)
        {
            if (!PlayerPrefs.HasKey(ModelNames[i]))
            {
                if (i != 0)
                    PlayerPrefs.SetInt(ModelNames[i], 0);
                else
                    PlayerPrefs.SetInt(ModelNames[i], 1);
            }

            int hasBought = PlayerPrefs.GetInt(ModelNames[i]);
            if (hasBought == 1)
            {
                hasModelBought[i] = true;
            }
        }
    }

    public void EditBuyPanel(PlayerModelType model)
    {
        ModelNameText.text = model.ToString();

        if(hasModelBought[(int) model])
        {
            bool isSelected = (PlayerPrefs.GetInt("SelectedPlayer") == (int)model);

            LetsRunButton.SetActive(isSelected);
            SelectButton.SetActive(!isSelected);
            BuyButton.SetActive(false);
        }
        else
        {
            BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = ModelPrice[(int)model].ToString();
            if (flowerCount < ModelPrice[(int)model])
            {
                BuyButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                BuyButton.GetComponent<Button>().interactable = true;
            }

            LetsRunButton.SetActive(false);
            SelectButton.SetActive(false);
            BuyButton.SetActive(true);
        }
    }

    public void OnClickBuyButton()
    {
        int modelNumber = (int)manager.CurrentModelType;
        
        flowerCount -= ModelPrice[modelNumber];
        PlayerPrefs.SetInt("FlowerCount", flowerCount);
        SetFlowerCount();

        hasModelBought[modelNumber] = true;
        PlayerPrefs.SetInt(ModelNames[modelNumber], 1);

        BuyButton.SetActive(false);
        SelectButton.SetActive(true);
    }

    public void OnClickSelecButton()
    {
        SelectButton.SetActive(false);
        LetsRunButton.SetActive(true);

        PlayerPrefs.SetInt("SelectedPlayer", (int)manager.CurrentModelType);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickLetsRunButton()
    {
        SceneManager.LoadScene(1);
    }
}
