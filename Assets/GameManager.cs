using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Tile tilePrefab;
    public TurretSript turret1Prefab;
    public TurretSript turret2Prefab;
    public TurretSript turret3Prefab;

    private static int mapXStart = -10;
    private static int mapXEnd = 10;
    private static int mapYStart = -10;
    private static int mapYEnd = 10;

    private Vector3? mousePosition = null;

    Tile[,] tileArray = new Tile[mapXEnd-mapXStart, mapYEnd - mapYStart];
    TurretSript[,] turretArray = new TurretSript[mapXEnd - mapXStart, mapYEnd - mapYStart];

    private TurretSript activeTurret = null;
    public GameObject shopDialog;
    public GameObject cityShopItem;
    public GameObject generalTuretShopItem;
    int balance = 15;
    public TMPro.TextMeshProUGUI balanceText;
    int round = 1;
    public TMPro.TextMeshProUGUI demageButtonText;
    public TMPro.TextMeshProUGUI attackSpeedButtonText;
    public TMPro.TextMeshProUGUI healthButtonText;
    public TMPro.TextMeshProUGUI sellButtonText;

    private Tile activeTile = null;
    public GameObject turretBuyDialog;
    public TMPro.TextMeshProUGUI turret1BuyText;
    public TMPro.TextMeshProUGUI turret2BuyText;
    public TMPro.TextMeshProUGUI turret3BuyText;


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
                tile.gameManager = this;
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
        if (activeTurret != null)
        {
            demageButtonText.text = $"Buy for {activeTurret.demageUpgradeCost}$";
            attackSpeedButtonText.text = $"Buy for {activeTurret.attackSpeedUpgradeCost}$";
            healthButtonText.text = $"Buy for {activeTurret.healthUpgradeCost}$";
            sellButtonText.text = $"Sell for {activeTurret.sellPrice}$";
        }
        turret1BuyText.text = $"Basic\nturret\nfor\n{5}$";
        turret2BuyText.text = $"Advanced\nturret\nfor\n{5}$";
        turret3BuyText.text = $"Best\nturret\nfor\n{5}$";
    }

    public IEnumerator ShowShopDialog(TurretSript turret)
    {
        HideDialog();
        yield return new WaitForEndOfFrame();
        activeTurret = turret;
        bool isGeneralTurret = turret.IsGeneralTurret();
        cityShopItem.SetActive(!isGeneralTurret);
        generalTuretShopItem.SetActive(isGeneralTurret);
        shopDialog.SetActive(true);
        yield return null;
    }

    public void HideDialog()
    {
        activeTile = null;
        activeTurret = null;
        shopDialog.SetActive(false);
        turretBuyDialog.SetActive(false);
    }

    public void BuyDemage()
    {
        if(activeTurret.demageUpgradeCost <= balance)
        {
            balance -= activeTurret.demageUpgradeCost;
            activeTurret.UpgradeDemage();
        }
    }

    public void BuyAttackSpeed()
    {
        if (activeTurret.demageUpgradeCost <= balance)
        {
            balance -= activeTurret.attackSpeedUpgradeCost;
            activeTurret.UpgradeAttackSpeed();
        }
    }

    public void BuyHealth()
    {
        if (activeTurret.demageUpgradeCost <= balance)
        {
            balance -= activeTurret.healthUpgradeCost;
            activeTurret.UpgradeHealth();
        }
    }

    public void SellTurret()
    {
        balance += activeTurret.sellPrice;
        for (int x = mapXStart; x < mapXEnd; x++)
        {
            for (int y = mapYStart; y < mapYEnd; y++)
            {
                if (turretArray[x - mapXStart, y - mapYStart] == activeTurret)
                {
                    turretArray[x - mapXStart, y - mapYStart] = null;
                }
            }
        }
    }

    public IEnumerator BuyTile(Tile tile)
    {
        for (int x = mapXStart; x < mapXEnd; x++)
        {
            for (int y = mapYStart; y < mapYEnd; y++)
            {
                if (tileArray[x - mapXStart, y - mapYStart] == tile)
                {
                    if(turretArray[x - mapXStart, y - mapYStart] != null)
                    {
                        StartCoroutine(ShowShopDialog(turretArray[x - mapXStart, y - mapYStart]));
                        yield break;
                    }
                }
            }
        }
        if (!tile.isPlacable) yield break;
        HideDialog();
        yield return new WaitForEndOfFrame();
        activeTile = tile;
        turretBuyDialog.SetActive(true);
        yield return null;
    }

    public void BuyTurret1()
    {
        for (int x = mapXStart; x < mapXEnd; x++)
        {
            for (int y = mapYStart; y < mapYEnd; y++)
            {
                if (tileArray[x - mapXStart, y - mapYStart] == activeTile)
                {
                    TurretSript turret = Instantiate(turret1Prefab, new Vector2(x, y), Quaternion.identity);
                    turret.gameManager = this;
                    turret.name = $"Turret1: {x} {y}";
                    turretArray[x - mapXStart, y - mapYStart] = turret;
                }
            }
        }
        HideDialog();
    }

    public void BuyTurret2()
    {
        for (int x = mapXStart; x < mapXEnd; x++)
        {
            for (int y = mapYStart; y < mapYEnd; y++)
            {
                if (tileArray[x - mapXStart, y - mapYStart] == activeTile)
                {
                    TurretSript turret = Instantiate(turret2Prefab, new Vector2(x, y), Quaternion.identity);
                    turret.gameManager = this;
                    turret.name = $"Turret2: {x} {y}";
                    turretArray[x - mapXStart, y - mapYStart] = turret;
                }
            }
        }
        HideDialog();
    }

    public void BuyTurret3()
    {
        for (int x = mapXStart; x < mapXEnd; x++)
        {
            for (int y = mapYStart; y < mapYEnd; y++)
            {
                if (tileArray[x - mapXStart, y - mapYStart] == activeTile)
                {
                    TurretSript turret = Instantiate(turret3Prefab, new Vector2(x, y), Quaternion.identity);
                    turret.gameManager = this;
                    turret.name = $"Turret3: {x} {y}";
                    turretArray[x - mapXStart, y - mapYStart] = turret;
                }
            }
        }
        HideDialog();
    }
}
