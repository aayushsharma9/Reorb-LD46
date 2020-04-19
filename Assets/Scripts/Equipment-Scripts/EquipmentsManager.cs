using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentsManager : MonoBehaviour
{
    public GameObject[] equipments;
    public GameObject selectedEquipment, selectedSlot;
    public static EquipmentsManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SelectEquipment(int index)
    {
        selectedEquipment = equipments[index];
    }

    public void ApplyEquipment()
    {
        if (GameManager.instance.coins >= selectedEquipment.GetComponent<Equipment>().cost)
        {
            selectedSlot.GetComponent<BaseSlotBehaviour>().ApplyEquipment(selectedEquipment);
            GameManager.instance.coins -= selectedEquipment.GetComponent<Equipment>().cost;
        }
    }

    public void UpgradeEquipment(int index)
    {
        equipments[index].GetComponent<Equipment>().LevelUp();
    }
}