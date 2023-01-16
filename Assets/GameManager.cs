using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Tile tilePrefab;

    private static int mapXStart = -10;
    private static int mapXEnd = 10;
    private static int mapYStart = -10;
    private static int mapYEnd = 10;

    private Vector3? mousePosition = null;

    Tile[,] tileArray = new Tile[mapXEnd-mapXStart, mapYEnd - mapYStart];

    public GameObject shopDialog;
    int balance = 15;
    public TMPro.TextMeshProUGUI balanceText;
    int round = 1;

    private 
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = 3f;
        for(int x= mapXStart; x< mapXEnd; x++)
        {
            for(int y= mapYStart; y< mapYEnd; y++)
            {
                Tile tile = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                tile.name = $"Tile {x} {y}";
                // make titles of city not placable
                if(x>=-1 && x<=1 && y>=-1 && y<=1)
                {
                    tile.isPlacable = false;
                }
                tileArray[x-mapXStart, y-mapYStart] = tile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // move camera by mouse
        if(Input.GetMouseButton(1))
        {
            if(mousePosition != null)
            {
                Vector3 delta = Input.mousePosition - (Vector3)mousePosition;
                Camera.main.transform.position -= delta / Camera.main.pixelHeight * Camera.main.orthographicSize * 3;
            }
        }
        mousePosition = Input.mousePosition;

        // clip of camera position
        float cameraXStart = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;
        float cameraXEnd = Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect;
        float cameraYStart = Camera.main.transform.position.y - Camera.main.orthographicSize;
        float cameraYEnd = Camera.main.transform.position.y + Camera.main.orthographicSize;

        // make the range intentionally bit bigger so it is easy to see we are on the end of map
        float cameraXMin = mapXStart - 1.1f;
        float cameraXMax = mapXEnd + 0.1f;
        float cameraYMin = mapYStart - 1.1f;
        float cameraYMax = mapYEnd + 0.1f;

        Vector3 camPos = Camera.main.transform.position;
        if (cameraXStart < cameraXMin)
        {
            camPos.x += cameraXMin - cameraXStart;
        }
        if (cameraXEnd > cameraXMax)
        {
            camPos.x += cameraXMax - cameraXEnd;
        }
        if (cameraYStart < cameraYMin)
        {
            camPos.y += cameraYMin - cameraYStart;
        }
        if (cameraYEnd > cameraYMax)
        {
            camPos.y += cameraYMax - cameraYEnd;
        }
        Camera.main.transform.position = camPos;

        balanceText.text = $"Balance: {balance}$";
    }

    public void ShowShopDialog(TurretSript turret)
    {
        shopDialog.SetActive(true);
    }

    public void HideDialog()
    {
        shopDialog.SetActive(false);
    }
}
