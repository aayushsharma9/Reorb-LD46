using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Toggle[] equipmentButtons;
    [SerializeField] private Button upgradePanelPositiveButton, upgradePanelNegativeButton, shopPanelDoneButton, equipmentPanelUpgradeButton, equipmentPanelDiscardButton, slotButton, gameOverRetry, gameOverExit;
    [SerializeField] private GameObject upgradesPanel, ShopPanel, equipmentPanel, GameOverPanel;
    [SerializeField] private TextMeshProUGUI levelText, coinsText, equipmentUpgradeValueText, equipmentDiscardValueText, equipmentPanelName, equipmentPanelLevel, gameOverScore, gameOverLevel;
    public GameObject selectedEquipment;
    public static GUIManager instance;

    void Awake()
    {
        instance = this;

        GameOverPanel.SetActive(false);
        ShopPanel.SetActive(false);
        equipmentPanel.SetActive(false);
        upgradesPanel.SetActive(false);

        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            int closureIndex = i;
            equipmentButtons[i].onValueChanged.AddListener(delegate { EquipmentButtonOnClickListener(closureIndex); });
        }

        upgradePanelPositiveButton.onClick.AddListener(delegate { upgradePanelPositiveButtonOnClickListener(); AudioManager.instance.Play("ButtonPress"); });
        upgradePanelNegativeButton.onClick.AddListener(delegate { upgradePanelNegativeButtonOnClickListener(); AudioManager.instance.Play("ButtonPress"); });
        shopPanelDoneButton.onClick.AddListener(delegate { shopPanelDoneButtonOnClickListener(); AudioManager.instance.Play("ButtonPress"); });
        equipmentPanelUpgradeButton.onClick.AddListener(delegate { equipmentPanelUpgradeButtonOnClickListener(); AudioManager.instance.Play("ButtonPress"); });
        equipmentPanelDiscardButton.onClick.AddListener(delegate { equipmentPanelDiscardButtonOnClickListener(); AudioManager.instance.Play("ButtonPress"); });
        slotButton.onClick.AddListener(delegate { slotButtonOnClickListener(); AudioManager.instance.Play("ButtonPress"); });
        gameOverRetry.onClick.AddListener(delegate { SceneManager.LoadScene(1); AudioManager.instance.Play("ButtonPress"); });
        gameOverExit.onClick.AddListener(delegate { SceneManager.LoadScene(0); AudioManager.instance.Play("ButtonPress"); });
        // OpenUpgradesPanel();
    }

    void Update()
    {
        levelText.text = "Level " + (GameManager.instance.levelCount + 1).ToString();
        coinsText.text = "" + GameManager.instance.coins;

        if (!equipmentButtons[0].GetComponent<ToggleGroup>().AnyTogglesOn())
        {
            upgradePanelPositiveButton.interactable = false;
        }
        else
        {
            upgradePanelPositiveButton.interactable = true;
        }
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void EquipmentButtonOnClickListener(int index)
    {
        if (equipmentButtons[index].isOn)
            EquipmentsManager.instance.SelectEquipment(index);
    }

    public void OpenUpgradesPanel(GameObject gameObject)
    {
        EquipmentsManager.instance.selectedSlot = gameObject;
        upgradesPanel.SetActive(true);
        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            equipmentButtons[i].isOn = false;
            equipmentButtons[i].transform.Find("Text-Cost").GetComponent<TextMeshProUGUI>().text = "" + EquipmentsManager.instance.equipments[i].GetComponent<Equipment>().cost;
            if (EquipmentsManager.instance.equipments[i].GetComponent<Equipment>().cost > GameManager.instance.coins)
            {
                equipmentButtons[i].interactable = false;
                // equipmentPanel[i].GetComponent<Animator>().Play("Normal");
            }
            else
            {
                equipmentButtons[i].interactable = true;
            }
        }
    }

    public void CloseUpgradesPanel()
    {
        upgradesPanel.SetActive(false);
    }

    void upgradePanelPositiveButtonOnClickListener()
    {
        EquipmentsManager.instance.ApplyEquipment();
        CloseUpgradesPanel();
    }

    void upgradePanelNegativeButtonOnClickListener()
    {
        CloseUpgradesPanel();
    }

    public void OpenEquipmentPanel(GameObject gameObj)
    {
        equipmentPanel.SetActive(true);
        Equipment equipment = gameObj.GetComponent<Equipment>();
        if (equipment.upgradeCost > GameManager.instance.coins || equipment.maxLevel == equipment.level)
        {
            equipmentPanelUpgradeButton.interactable = false;
        }
        else
        {
            equipmentPanelUpgradeButton.interactable = true;
        }

        if (equipment.maxLevel == equipment.level) equipmentUpgradeValueText.text = "Max Level";
        else equipmentUpgradeValueText.text = "-" + equipment.upgradeCost;
        equipmentDiscardValueText.text = "+" + equipment.resellValue;
        equipmentPanelName.text = equipment.equipmentName;
        equipmentPanelLevel.text = "Level " + equipment.level;
        selectedEquipment = gameObj;
    }

    void equipmentPanelUpgradeButtonOnClickListener()
    {
        selectedEquipment.GetComponent<Equipment>().LevelUp();
        equipmentPanel.SetActive(false);
    }

    void equipmentPanelDiscardButtonOnClickListener()
    {
        selectedEquipment.GetComponent<Equipment>().Discard();
        equipmentPanel.SetActive(false);
    }

    void slotButtonOnClickListener()
    {
        BaseSlotManager.instance.IncreaseCapacity();
        if (GameManager.instance.coins > 500) slotButton.interactable = true;
        else slotButton.interactable = false;
    }

    void shopPanelDoneButtonOnClickListener()
    {
        ToggleShopMode();
    }

    public void ToggleShopMode()
    {
        GameManager.instance.isShopMode = !GameManager.instance.isShopMode;
        ShopPanel.SetActive(GameManager.instance.isShopMode);
        if (!GameManager.instance.isShopMode)
        {
            upgradesPanel.SetActive(false);
            equipmentPanel.SetActive(false);
            HazardSpawner.instance.SpawnHazards();
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Base-Slot"))
            {
                obj.GetComponent<BaseSlotBehaviour>().ChangeState("base-slot-inactive");
            }
        }
        else
        {
            if (GameManager.instance.coins > 500) slotButton.interactable = true;
            else slotButton.interactable = false;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Base-Slot"))
            {
                obj.GetComponent<BaseSlotBehaviour>().ChangeState("base-slot-active");
            }
        }
    }

    public void GameOver()
    {
        gameOverScore.text = "Final Score: " + GameManager.instance.maxCoins;
        gameOverLevel.text = "Max Level: " + (GameManager.instance.levelCount + 1);
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
    }
}