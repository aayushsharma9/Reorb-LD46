using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlotManager : MonoBehaviour
{
    [SerializeField] private GameObject segment;
    [SerializeField] private int ringSize;
    private Vector3 nextPosition;
    private float t, segmentLength;
    public static BaseSlotManager instance;

    void Awake()
    {
        instance = this;
        segmentLength = segment.transform.Find("Base-Slot-Sprite").localScale.x;
        GenerateRingOfSize(ringSize);
    }

    void Update() { }

    public void IncreaseCapacity()
    {
        ringSize++;
        GenerateRingOfSize(ringSize);
        CameraBounds.instance.ZoomOut();
        GameManager.instance.coins -= 500;
    }

    private void GenerateRingOfSize(int n)
    {
        float angle = 180 - (360 / n);
        nextPosition = new Vector3(-segmentLength / 2, Mathf.Tan((angle / 2) * Mathf.Deg2Rad) * -segmentLength / 2, 0);

        for (int i = 0; i < n; i++)
        {
            GameObject currentSegment;
            if (transform.Find("Base-Slot-" + i) == null)
            {
                currentSegment = Instantiate(segment);
                currentSegment.transform.parent = gameObject.transform;
                currentSegment.name = "Base-Slot-" + i;
            }
            else
            {
                currentSegment = transform.Find("Base-Slot-" + i).gameObject;
            }
            currentSegment.transform.position = nextPosition;
            currentSegment.transform.rotation = Quaternion.Euler(0, 0, i * (360 / n));
            nextPosition = currentSegment.transform.Find("Base-Slot-End").position;
        }
    }
}