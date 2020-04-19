using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlotBehaviour : MonoBehaviour
{
    public bool isEquipped;
    private GameObject equipment;
    private Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        ChangeState("base-slot-active");
    }

    public void ChangeState(string state)
    {
        if (isEquipped) return;
        animator.Play(state);
    }
    private void OnMouseEnter()
    {
        if (GUIManager.instance.IsPointerOverUIObject()) return;
        if (!GameManager.instance.isShopMode) return;
        ChangeState("base-slot-highlighted");
    }

    private void OnMouseExit()
    {
        // if (GUIManager.instance.IsPointerOverUIObject()) return;
        if (!GameManager.instance.isShopMode) return;
        ChangeState("base-slot-active");
    }

    void OnMouseOver()
    {
        if (isEquipped) return;
        if (!GameManager.instance.isShopMode) return;
        if (GUIManager.instance.IsPointerOverUIObject()) return;
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.Play("ButtonPress");
            GUIManager.instance.OpenUpgradesPanel(gameObject);
        }
    }

    void Update()
    {

    }

    public void ApplyEquipment(GameObject equipment)
    {
        GameObject.Find("Base").GetComponent<BaseController>().enabled = true;
        this.equipment = Instantiate(equipment, transform.position, transform.rotation, transform);
        this.equipment.name = equipment.name;
        ChangeState("base-slot-inactive");
        isEquipped = true;
    }
}