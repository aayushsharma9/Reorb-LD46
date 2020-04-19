using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentCannon : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float speed;
    [SerializeField] private float chargeTime;
    private float baseChargeTime;
    private Slider cannonChargeSlider;
    private float timer;

    void Start()
    {
        cannonChargeSlider = GetComponentInChildren<Slider>();
        baseChargeTime = chargeTime;
    }

    void Update()
    {
        if (GameManager.instance.isShopMode) return;
        timer += Time.deltaTime;
        cannonChargeSlider.value = timer / chargeTime;
        if (timer >= chargeTime)
        {
            GameObject bulletObject = Instantiate(bullet, transform.Find("Equipment-Cannon-Exit-Point").gameObject.transform.position, gameObject.transform.rotation);
            bulletObject.GetComponent<BulletBehaviour>().level = gameObject.GetComponent<Equipment>().level;
            bulletObject.GetComponent<Rigidbody2D>().velocity = bulletObject.transform.TransformDirection(Vector2.down) * speed;
            timer = 0;
            chargeTime = baseChargeTime - (gameObject.GetComponent<Equipment>().level * 0.5f);
        }
    }

    void CheckAngles()
    {
        //TODO: Lock on to nearest hazard
    }
}