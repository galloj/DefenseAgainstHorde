using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Tile tilePrefab;
    public Turret turret1Prefab;
    public Turret turret2Prefab;
    public Turret turret3Prefab;

    private static int mapXStart = -10;
    private static int mapXEnd = 10;
    private static int mapYStart = -10;
    private static int mapYEnd = 10;

    private Vector3? mousePosition = null;

    Tile[,] tileArray = new Tile[mapXEnd-mapXStart, mapYEnd - mapYStart];
    Turret[,] turretArray = new Turret[mapXEnd - mapXStart, mapYEnd - mapYStart];

    private Turret activeTurret = null;
    public GameObject shopDialog;
    public GameObject cityShopItem;
    public GameObject generalTuretShopItem;
    int balance = 15;
    public TMPro.TextMeshProUGUI balanceText;
    int round = 0;
    public TMPro.TextMeshProUGUI demageText;
    public TMPro.TextMeshProUGUI demageButtonText;
    public TMPro.TextMeshProUGUI attackSpeedText;
    public TMPro.TextMeshProUGUI attackSpeedButtonText;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI healthButtonText;
    public TMPro.TextMeshProUGUI sellButtonText;

    private Tile activeTile = null;
    public GameObject turretBuyDialog;
    public TMPro.TextMeshProUGUI turret1BuyText;
    public TMPro.TextMeshProUGUI turret2BuyText;
    public TMPro.TextMeshProUGUI turret3BuyText;

    public Enemy enemyPrefab;
    internal List<Enemy> enemies = new List<Enemy>();

    internal int turret1Price = 5;
    internal int turret2Price = 10;
    internal int turret3Price = 15;

    public Turret baseTurret;

    public GameObject endGameDialog;
    public TMPro.TextMeshProUGUI gameOverText;
    internal int enemiesKilled = 0;

    public TMPro.TextMeshProUGUI roundText;
    public GameObject nextRoundUI;


    private
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = 3f;
        SetupGame();
        //NextRound();
        nextRoundUI.SetActive(true);
    }

    void SetupGame()
    {
        for (int x = mapXStart; x < mapXEnd; x++)
        {
            for (int y = mapYStart; y < mapYEnd; y++)
            {
                Tile tile = tileArray[x - mapXStart, y - mapYStart];
                if(tile != null)
                {
                    Destroy(tile.gameObject);
                    tileArray[x - mapXStart, y - mapYStart] = null;
                }
                Turret turret = turretArray[x - mapXStart, y - mapYStart];
                if(turret != null)
                {
                    Destroy(turret.gameObject);
                    turretArray[x - mapXStart, y - mapYStart] = null;
                }
            }
        }
        foreach(Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
        enemiesKilled = 0;
        balance = 15;
        round = 0;
        baseTurret.ResetTurret();
        for (int x = mapXStart; x < mapXEnd; x++)
        {
            for (int y = mapYStart; y < mapYEnd; y++)
            {
                Tile tile = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                tile.gameManager = this;
                tile.name = $"Tile {x} {y}";
                // make titles of city not placable
                if (x >= -1 && x <= 1 && y >= -1 && y <= 1)
                {
                    tile.isPlacable = false;
                }
                tileArray[x - mapXStart, y - mapYStart] = tile;
            }
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;
        switch(Random.Range(0,4))
        {
            case 0:
                spawnPosition = new Vector3(mapXStart+0.5f, Random.Range(mapYStart, mapYEnd));
                break;
            case 1:
                spawnPosition = new Vector3(mapXEnd - 0.5f, Random.Range(mapYStart, mapYEnd));
                break;
            case 2:
                spawnPosition = new Vector3(Random.Range(mapXStart, mapXEnd), mapYStart + 0.5f);
                break;
            case 3:
                spawnPosition = new Vector3(Random.Range(mapXStart, mapXEnd), mapYEnd - 0.5f);
                break;
            default:
                Debug.Log("Spawn Enemy err");
                spawnPosition = new Vector3();
                break;
        }
        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.gameManager = this;
        enemies.Add(enemy);
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

        // update dynamic texts
        balanceText.text = $"Balance: {balance}$";
        if (activeTurret != null)
        {
            demageText.text = $"Demage: {activeTurret.demageUpgradeCount + 1}";
            demageButtonText.text = $"Buy for {activeTurret.demageUpgradeCost}$";
            attackSpeedText.text = $"Attack speed: {activeTurret.attackSpeedUpgradeCount + 1}";
            attackSpeedButtonText.text = $"Buy for {activeTurret.attackSpeedUpgradeCost}$";
            healthText.text = $"Health: {activeTurret.healthUpgradeCount + 1}";
            healthButtonText.text = $"Buy for {activeTurret.healthUpgradeCost}$";
            sellButtonText.text = $"Sell for {activeTurret.sellPrice}$";
        }
        turret1BuyText.text = $"Basic\nturret\nfor\n{turret1Price}$";
        turret2BuyText.text = $"Advanced\nturret\nfor\n{turret2Price}$";
        turret3BuyText.text = $"Best\nturret\nfor\n{turret3Price}$";
        roundText.text = $"Current round: {round}";
        gameOverText.text = $"Enemies killed: {enemiesKilled}\nRound reached: {round}";

        // show next round button on round end
        if (enemies.Count == 0 && !IsGameOver()) nextRoundUI.SetActive(true);

        // show end screen if game over
        endGameDialog.SetActive(IsGameOver());
    }

    public IEnumerator ShowShopDialog(Turret turret)
    {
        if (IsGameOver()) yield break;
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
        endGameDialog.SetActive(false);
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
        if (activeTurret.attackSpeedUpgradeCost <= balance)
        {
            balance -= activeTurret.attackSpeedUpgradeCost;
            activeTurret.UpgradeAttackSpeed();
        }
    }

    public void BuyHealth()
    {
        if (IsGameActive()) return;
        if (activeTurret.healthUpgradeCost <= balance)
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
        Destroy(activeTurret.gameObject);
        activeTurret = null;
        HideDialog();
    }

    public IEnumerator BuyTile(Tile tile)
    {
        if (IsGameOver()) yield break;
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
        activeTile = tile;
        yield return new WaitForEndOfFrame();
        turretBuyDialog.SetActive(true);
        yield return null;
    }

    public void BuyTurret1()
    {
        if (balance < turret1Price) return;
        balance -= turret1Price;
        for (int x = mapXStart; x < mapXEnd; x++)
        {
            for (int y = mapYStart; y < mapYEnd; y++)
            {
                if (tileArray[x - mapXStart, y - mapYStart] == activeTile)
                {
                    Turret turret = Instantiate(turret1Prefab, new Vector2(x, y), Quaternion.identity);
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
        if (balance < turret2Price) return;
        balance -= turret2Price;

        for (int x = mapXStart; x < mapXEnd; x++)
        {
            for (int y = mapYStart; y < mapYEnd; y++)
            {
                if (tileArray[x - mapXStart, y - mapYStart] == activeTile)
                {
                    Turret turret = Instantiate(turret2Prefab, new Vector2(x, y), Quaternion.identity);
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
        if (balance < turret2Price) return;
        balance -= turret2Price;

        for (int x = mapXStart; x < mapXEnd; x++)
        {
            for (int y = mapYStart; y < mapYEnd; y++)
            {
                if (tileArray[x - mapXStart, y - mapYStart] == activeTile)
                {
                    Turret turret = Instantiate(turret3Prefab, new Vector2(x, y), Quaternion.identity);
                    turret.gameManager = this;
                    turret.name = $"Turret3: {x} {y}";
                    turretArray[x - mapXStart, y - mapYStart] = turret;
                }
            }
        }
        HideDialog();
    }

    public void KillEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
        enemies.Remove(enemy);
        balance += 3;
        enemiesKilled += 1;
    }

    public void PlayAgain()
    {
        HideDialog();
        SetupGame();
    }

    public void NextRound()
    {
        HideDialog();
        nextRoundUI.SetActive(false);
        round += 1;
        StartCoroutine(Spawner());
    }

    public IEnumerator Spawner()
    {
        for (int i = 0; i < round + 1; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.03f);
        }
    }

    public bool IsGameOver()
    {
        return baseTurret.health <= 0;
    }

    public bool IsGameActive()
    {
        return enemies.Count > 0 && !IsGameOver();
    }
}
