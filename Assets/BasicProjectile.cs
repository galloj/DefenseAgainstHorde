using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    internal GameManager gameManager;
    internal Enemy target;
    internal float demage;
    private Vector3 targetVector;

    // Start is called before the first frame update
    void Start()
    {
        targetVector = target.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            targetVector = target.transform.position - this.transform.position;
        }
        this.transform.position += (targetVector).normalized * Time.deltaTime * 2;
        if(targetVector.magnitude < 0.1 && target != null)
        {
            target.health -= demage;
            Destroy(this.gameObject);
        }
        if (this.transform.position.magnitude > 150)
        {
            Destroy(this.gameObject);
        }
    }
}
