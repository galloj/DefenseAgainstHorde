using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSript : MonoBehaviour
{
    public GameManager gameManager;

    private Color defaultColor;

    public int turretType = 0;

    public int sellPrice = 0;
    public int demageUpgradeCost;
    public int attackSpeedUpgradeCost;
    public int healthUpgradeCost = 10;

    float cooldown;
    float demage;
    int cooldownReduction = 0;
    int demageIncrease = 0;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = GetComponent<SpriteRenderer>().color;
        switch(turretType)
        {
            case 0:
                cooldown = 1f;
                demage = 0.2f;
                demageUpgradeCost = 5;
                attackSpeedUpgradeCost = 5;
                break;
            case 1:
                sellPrice = 3;
                cooldown = 1.5f;
                demage = 0.25f;
                demageUpgradeCost = 5;
                attackSpeedUpgradeCost = 5;
                break;
            case 2:
                sellPrice = 10;
                cooldown = 2f;
                demage = 0.15f;
                demageUpgradeCost = 5;
                attackSpeedUpgradeCost = 5;
                break;
            case 3:
                sellPrice = 20;
                cooldown = 3.5f;
                demage = 0.1f;
                demageUpgradeCost = 5;
                attackSpeedUpgradeCost = 5;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(turretType)
        {
            // TODO
        }
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        StartCoroutine(gameManager.ShowShopDialog(this));
    }

    public void UpgradeDemage()
    {

    }

    public void UpgradeAttackSpeed()
    {

    }

    public void UpgradeHealth()
    {

    }

    public bool IsGeneralTurret()
    {
        return turretType != 0;
    }
}
