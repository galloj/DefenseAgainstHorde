using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Tile tilePrefab;

    private int mapXStart = -30;
    private int mapXEnd = 30;
    private int mapYStart = -30;
    private int mapYEnd = 30;

    private 
    // Start is called before the first frame update
    void Start()
    {
        for(int x=-10; x<10; x++)
        {
            for(int y=-10; y<10; y++)
            {
                Tile tile = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                tile.name = $"Tile {x} {y}";
                if(x>=-1 && x<=1 && y>=-1 && y<=1)
                {
                    tile.isPlacable = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
