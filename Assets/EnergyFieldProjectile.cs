using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyFieldProjectile : MonoBehaviour
{
    internal GameManager gameManager;
    internal float targetExistence = 0.5f;
    internal float targetSize = 6f;
    internal float demage;

    private float existence = 0;
    private List<Enemy> hitEnemies = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        existence += Time.deltaTime;
        float scale = existence / targetExistence * targetSize;
        foreach(Enemy enemy in gameManager.enemies)
        {
            if (hitEnemies.Contains(enemy)) continue;
            if((enemy.transform.position - transform.position).magnitude < scale * 0.5f)
            {
                hitEnemies.Add(enemy);
                enemy.health -= demage;
            }
        }
        transform.localScale = new Vector3(scale, scale, scale);
        if(existence >= targetExistence)
        {
            Destroy(this.gameObject);
        }
    }
}
