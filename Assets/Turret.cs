using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Turret : MonoBehaviour
{
    public GameManager gameManager;

    public BasicProjectile basicProjectile;
    public AreaProjectile areaProjectile;
    public EnergyFieldProjectile energyFieldProjectile;

    public HealthBar healthBar;
    internal float health = 1;
    internal float maxHealth = 1;

    private Color defaultColor;

    public int turretType = 0;

    internal int sellPrice = 0;
    internal int demageUpgradeCost;
    internal int attackSpeedUpgradeCost;
    internal int healthUpgradeCost = 10;

    internal int demageUpgradeCount = 0;
    internal int attackSpeedUpgradeCount = 0;
    internal int healthUpgradeCount = 0;

    float cooldown;
    float demage;
    float cooldownTimer = 0;

    public void ResetTurret()
    {
        maxHealth = 1;
        sellPrice = 0;
        demageUpgradeCount = 0;
        attackSpeedUpgradeCount = 0;
        healthUpgradeCount = 0;
        switch (turretType)
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
        health = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = GetComponent<SpriteRenderer>().color;
        ResetTurret();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBar != null)
        {
            if (health > 0)
            {
                healthBar.health = health / maxHealth;
            } else
            {
                healthBar.health = 0;
            }
        }
        if(gameManager.IsGameActive()) health += Time.deltaTime / 30f;
        if (health > maxHealth) health = maxHealth;
        if(cooldownTimer <= 0 && !gameManager.IsGameOver())
        {
            switch (turretType)
            {
                case 0:
                    {
                        List<Enemy> closestEnemies = new List<Enemy>();
                        foreach (Enemy enemy in gameManager.enemies)
                        {
                            if (enemy.transform.position.magnitude < 4)
                            {
                                closestEnemies.Add(enemy);
                            }
                        }
                        if (closestEnemies.Count == 0) break;
                        cooldownTimer = cooldown;
                        while (closestEnemies.Count > 3)
                        {
                            Enemy furthermostEnemy = null;
                            float biggestDistance = 0;
                            foreach (Enemy enemy in closestEnemies)
                            {
                                if (enemy.transform.position.magnitude >= biggestDistance)
                                {
                                    furthermostEnemy = enemy;
                                    biggestDistance = enemy.transform.position.magnitude;
                                }
                            }
                            closestEnemies.Remove(furthermostEnemy);
                        }
                        foreach (Enemy enemy in closestEnemies)
                        {
                            Vector3 position = enemy.transform.position.normalized * 1f;
                            BasicProjectile projectile = Instantiate(basicProjectile, position, Quaternion.FromToRotation(new Vector3(1, 0, 0), position));
                            projectile.target = enemy;
                            projectile.demage = demage;
                            projectile.gameManager = gameManager;
                        }
                        break;
                    }
                case 1:
                    {
                        Enemy closestEnemy = null;
                        foreach (Enemy enemy in gameManager.enemies)
                        {
                            if ((enemy.transform.position - transform.position).magnitude < 4)
                            {
                                closestEnemy = enemy;
                            }
                        }
                        if (closestEnemy == null) break;
                        cooldownTimer = cooldown;
                        Vector3 offset = (closestEnemy.transform.position - transform.position).normalized * 0.5f;
                        Vector3 position = transform.position + offset;
                        BasicProjectile projectile = Instantiate(basicProjectile, position, Quaternion.FromToRotation(new Vector3(1, 0, 0), offset));
                        projectile.target = closestEnemy;
                        projectile.demage = demage;
                        projectile.gameManager = gameManager;
                        break;
                    }
                case 2:
                    {
                        Enemy closestEnemy = null;
                        foreach (Enemy enemy in gameManager.enemies)
                        {
                            if ((enemy.transform.position - transform.position).magnitude < 4)
                            {
                                closestEnemy = enemy;
                            }
                            if (closestEnemy == null) break;
                            cooldownTimer = cooldown;
                            Vector3 offset = (closestEnemy.transform.position - transform.position).normalized * 0.5f;
                            Vector3 position = transform.position + offset;
                            AreaProjectile projectile = Instantiate(areaProjectile, position, Quaternion.FromToRotation(new Vector3(1, 0, 0), offset));
                            projectile.target = closestEnemy.transform.position - closestEnemy.transform.position.normalized * (closestEnemy.transform.position - transform.position).magnitude / projectile.speed * enemy.speed;
                            projectile.demage = demage;
                            projectile.gameManager = gameManager;
                        }
                        break;
                    }
                case 3:
                    {
                        cooldownTimer = cooldown;
                        EnergyFieldProjectile projectile = Instantiate(energyFieldProjectile, transform);
                        projectile.gameManager = gameManager;
                        projectile.demage = demage;
                        break;
                    }
            }
        }
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
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
        demageUpgradeCount++;
    }

    public void UpgradeAttackSpeed()
    {
        attackSpeedUpgradeCount++;
    }

    public void UpgradeHealth()
    {
        healthUpgradeCount++;
        health += 0.3f;
        maxHealth += 0.3f;
    }

    public bool IsGeneralTurret()
    {
        return turretType != 0;
    }
}
