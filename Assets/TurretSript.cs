using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSript : MonoBehaviour
{
    public GameManager gameManager;

    private Color defaultColor;

    public int turretType = 0;

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
                break;
            case 1:
                cooldown = 1.5f;
                demage = 0.25f;
                break;
            case 2:
                cooldown = 2f;
                demage = 0.15f;
                break;
            case 3:
                cooldown = 3.5f;
                demage = 0.1f;
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
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    private void OnMouseDown()
    {
        gameManager.ShowShopDialog(this);
    }
}
