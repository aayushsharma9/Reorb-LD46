using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    private Vector3 targetPosition;

    void Start()
    {

    }

    void Update()
    {
        if (GUIManager.instance.IsPointerOverUIObject()) return;
        if (GameManager.instance.isShopMode) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        float x = targetPosition.x - transform.position.x;
        float y = targetPosition.y - transform.position.y;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}