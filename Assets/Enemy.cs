using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    internal GameManager gameManager;
    internal float health = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1.4f)
        {
            transform.position -= transform.position.normalized * Time.deltaTime;
        }
        if(health <= 0)
        {
            gameManager.KillEnemy(this);
        }
    }
}
