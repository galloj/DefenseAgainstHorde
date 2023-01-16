using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSript : MonoBehaviour
{
    Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = defaultColor;
    }
}
