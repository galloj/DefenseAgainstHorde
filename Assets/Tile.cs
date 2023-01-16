using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isPlacable = true;
    public GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        if(!isPlacable)
        {
            SpriteRenderer highlightRenderer = highlight.GetComponent<SpriteRenderer>();
            Color color = highlightRenderer.color;
            color.g = 0;
            color.b = 0;
            highlightRenderer.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
