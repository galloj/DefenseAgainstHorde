using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    internal GameManager gameManager;
    internal bool isPlacable = true;
    public GameObject highlight;
    Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = highlight.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer highlightRenderer = highlight.GetComponent<SpriteRenderer>();
        if (!isPlacable)
        {
            Color color = originalColor;
            color.g = 0;
            color.b = 0;
            highlightRenderer.color = color;
        } else
        {
            highlightRenderer.color = originalColor;
        }
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        StartCoroutine(gameManager.BuyTile(this));
    }
}
