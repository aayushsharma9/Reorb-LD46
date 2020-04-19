using System.Collections;
using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] hazardObjects;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int maxSpawnCount;
    private int currentSpawnCount;
    private Coroutine spawnHazardsCoroutine;
    private Vector3 spawnPosition;
    private float timer;

    public static HazardSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnHazards()
    {
        spawnHazardsCoroutine = StartCoroutine(SpawnHazardsCoroutine());
    }

    IEnumerator SpawnHazardsCoroutine()
    {
        int spawnLimit = maxSpawnCount + GameManager.instance.levelCount * 5;
        int level = GameManager.instance.levelCount / 2;
        if (level > hazardObjects.Length) level = 5;
        float spawnCooldown = Mathf.Max(spawnInterval - (GameManager.instance.levelCount * 0.5f), 0.5f);
        while (currentSpawnCount < spawnLimit)
        {
            timer += Time.deltaTime;
            if (timer >= spawnCooldown)
            {
                Vector3 spawnPos = GenerateRandomPosition();
                GameObject obj = Instantiate(hazardObjects[Random.Range(0, level)], spawnPos, Quaternion.identity);
                timer = 0;
                currentSpawnCount += 1;
            }
            yield return new WaitForEndOfFrame();
        }

        while (true)
        {
            CheckLastOrb();
            yield return new WaitForEndOfFrame();
        }
    }

    private void CheckLastOrb()
    {
        if (GameObject.FindGameObjectsWithTag("Hazard").Length <= 0)
        {
            GameManager.instance.IncreaseLevel();
            currentSpawnCount = 0;
            StopCoroutine(spawnHazardsCoroutine);
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        float x = 0, y = 0;
        switch (Random.Range(0, 4)) //Selecting one of the 4 edges
        {
            case 0:
                //leftEdge selected
                x = CameraBounds.instance.leftEdge;
                y = Random.Range(CameraBounds.instance.bottomEdge, CameraBounds.instance.topEdge);
                break;
            case 1:
                //rightEdge selected
                x = CameraBounds.instance.rightEdge;
                y = Random.Range(CameraBounds.instance.bottomEdge, CameraBounds.instance.topEdge);
                break;
            case 2:
                //topEdge selected
                x = Random.Range(CameraBounds.instance.leftEdge, CameraBounds.instance.rightEdge);
                y = CameraBounds.instance.topEdge;
                break;
            case 3:
                //bottomEdge selected
                x = Random.Range(CameraBounds.instance.leftEdge, CameraBounds.instance.rightEdge);
                y = CameraBounds.instance.bottomEdge;
                break;
            default:
                break;
        }

        Vector3 position = new Vector3(x, y, 0);
        return position;
    }
}