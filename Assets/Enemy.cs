using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    internal GameManager gameManager;

    public HealthBar healthBar;

    public float maxHealth = 1;
    internal float health;
    private float cooldown = 0;
    internal float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.health = health/maxHealth;
        if(cooldown <= 0)
        {
            if (transform.position.magnitude <= 1.4f)
            {
                gameManager.baseTurret.health -= 0.1f;
                cooldown = 0.7f;
            }
        } else
        {
            cooldown -= Time.deltaTime;
        }
        if (transform.position.magnitude > 1.4f)
        {
            transform.position -= transform.position.normalized * Time.deltaTime * speed;
        }
        if(health <= 0)
        {
            gameManager.KillEnemy(this);
        }
    }
}
