using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private GameObject[] sprites;
    public string equipmentName;
    public int level = 1, maxLevel;
    public int cost, upgradeCost, resellValue;
    float newSize = 1.5f;

    void Awake()
    {
        upgradeCost = cost + (level * 100);
        resellValue = cost / 2;
        SelectSprite();
    }

    public void LevelUp()
    {
        if (GameManager.instance.coins >= upgradeCost)
        {
            level++;
            GameManager.instance.coins -= upgradeCost;
            upgradeCost = cost + (level * 100);
            resellValue = upgradeCost / 2;
            SelectSprite();
        }
    }

    private void SelectSprite()
    {
        for (int i = 0; i < maxLevel; i++)
        {
            if (i == level - 1)
            {
                sprites[i].SetActive(true);
            }
            else
            {
                sprites[i].SetActive(false);
            }
        }
    }

    public void Discard()
    {
        GameManager.instance.IncreaseCoins(resellValue);
        transform.parent.gameObject.GetComponent<BaseSlotBehaviour>().isEquipped = false;
        Destroy(gameObject);
    }

    void OnMouseOver()
    {
        if (GUIManager.instance.IsPointerOverUIObject()) return;
        if (!GameManager.instance.isShopMode) return;
        if (GUIManager.instance.IsPointerOverUIObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.Play("ButtonPress");
            GUIManager.instance.OpenEquipmentPanel(gameObject);
        }
    }

    private void OnMouseEnter()
    {
        if (GUIManager.instance.IsPointerOverUIObject()) return;
        if (!GameManager.instance.isShopMode) return;
        SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sprites)
            s.transform.localScale = new Vector3(newSize, newSize, newSize);
    }

    private void OnMouseExit()
    {
        // if (GUIManager.instance.IsPointerOverUIObject()) return;
        if (!GameManager.instance.isShopMode) return;
        SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sprites)
            s.transform.localScale = new Vector3(1, 1, 1);
    }
}