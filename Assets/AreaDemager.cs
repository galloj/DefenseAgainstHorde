using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDemager : MonoBehaviour
{
    internal GameManager gameManager;
    internal float demage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Live());
    }

    IEnumerator Live()
    {
        for(int i = 0; i<10; i++)
        {
            foreach (Enemy enemy in gameManager.enemies)
            {
                if ((enemy.transform.position - transform.position).magnitude < 1)
                {
                    enemy.health -= demage / 10;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(this.gameObject);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
