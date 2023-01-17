using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile : MonoBehaviour
{
    internal GameManager gameManager;
    internal Vector3 target;
    internal float demage;
    public AreaDemager areaDemager;
    internal float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += (target - this.transform.position).normalized * Time.deltaTime * speed;
        if ((target - this.transform.position).magnitude < 0.1)
        {
            AreaDemager demager = Instantiate(areaDemager, target, Quaternion.identity);
            demager.gameManager = gameManager;
            demager.demage = demage;
            Destroy(this.gameObject);
        }
    }
}
