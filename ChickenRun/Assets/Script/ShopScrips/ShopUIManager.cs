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
    private int flowerCount = 0;

    private void OnEnable()
    {
        manager.OnShowModelChange.RemoveListener(EditBuyPanel);
        manager.OnShowModelChange.AddListener(EditBuyPanel);

        ShopSet();

        flowerCount = PlayerPrefsKey.GetIntByKey(PlayerPrefsKey.FlowerCount);
        SetFlowerCount();
    }

    private void SetFlowerCount()
    {
        FlowerCountText.text = flowerCount.ToString();
    }

    private void ShopSet()
    {
        PlayerPrefsKey.SetModelInfo();
    }

    public void EditBuyPanel(PlayerModelType type)
    {
        PlayerModel model = PlayerPrefsKey.GetModel(type);

        ModelNameText.text = model.Name;

        if(model.IsBought)
        {
            LetsRunButton.SetActive(model.IsSelected);
            SelectButton.SetActive(!model.IsSelected);

            BuyButton.SetActive(false);
        }
        else
        {
            BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = model.Price.ToString();
            BuyButton.GetComponent<Button>().interactable = (flowerCount < model.Price);

            LetsRunButton.SetActive(false);
            SelectButton.SetActive(false);

            BuyButton.SetActive(true);
        }
    }

    public void OnClickBuyButton()
    {
        PlayerModel model = PlayerPrefsKey.GetModel(manager.CurrentModelType);
        
        flowerCount -= model.Price;
        SetFlowerCount();
        PlayerPrefs.SetInt(PlayerPrefsKey.FlowerCount, flowerCount);

        model.IsBought = true;

        BuyButton.SetActive(model.IsBought);
        SelectButton.SetActive(model.IsSelected);
    }

    public void OnClickSelecButton()
    {
        SelectButton.SetActive(false);
        LetsRunButton.SetActive(true);

        PlayerModel model = PlayerPrefsKey.GetModel(manager.CurrentModelType);
        model.IsSelected = true;
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
