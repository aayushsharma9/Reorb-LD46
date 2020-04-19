using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int levelCount;
    public int coins, maxCoins;
    public bool isShopMode;

    void Awake()
    {
        if (GameManager.instance == null)
            GameManager.instance = this;
        else
            Destroy(gameObject);

        levelCount = 0;
        isShopMode = true;
        Time.timeScale = 1;
    }

    public void IncreaseLevel()
    {
        levelCount++;
        GameManager.instance.IncreaseCoins(200);
        GUIManager.instance.ToggleShopMode();
    }

    public void IncreaseCoins(int amount)
    {
        coins += amount;
        maxCoins += amount;
        AudioManager.instance.Play("Score");
        // StartCoroutine(IncreaseCoinsCoroutine(amount));
    }

    IEnumerator IncreaseCoinsCoroutine(int amount)
    {
        int targetAmount = coins + amount;
        while (coins < targetAmount)
        {
            coins++;
            yield return new WaitForEndOfFrame();
        }
    }
}