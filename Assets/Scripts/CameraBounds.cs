using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public float leftEdge, rightEdge, topEdge, bottomEdge;
    private GameObject[] worldBounds;
    public static CameraBounds instance;

    void Awake()
    {
        CameraBounds.instance = this;

        // worldBounds = new GameObject[4];

        // worldBounds[0] = GameObject.Find("wall_left");
        // worldBounds[1] = GameObject.Find("wall_top");
        // worldBounds[2] = GameObject.Find("wall_right");
        // worldBounds[3] = GameObject.Find("wall_bottom");

        ResetBounds();
    }

    void Update()
    {

    }

    public void ResetBounds()
    {
        Camera cam = GetComponent<Camera>();

        // worldBounds[0].transform.position = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, -cam.transform.position.z)); //left
        // worldBounds[1].transform.position = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, -cam.transform.position.z)); //top
        // worldBounds[2].transform.position = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, -cam.transform.position.z)); //right
        // worldBounds[3].transform.position = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, -cam.transform.position.z)); //bottom

        leftEdge = cam.ViewportToWorldPoint(new Vector3(0f, 0f, -cam.transform.position.z)).x;
        rightEdge = cam.ViewportToWorldPoint(new Vector3(1f, 0f, -cam.transform.position.z)).x;
        topEdge = cam.ViewportToWorldPoint(new Vector3(0f, 1f, -cam.transform.position.z)).y;
        bottomEdge = cam.ViewportToWorldPoint(new Vector3(0f, 0f, -cam.transform.position.z)).y;
    }

    public void ZoomOut()
    {
        gameObject.GetComponent<Camera>().orthographicSize += 5;
        ResetBounds();
    }
}