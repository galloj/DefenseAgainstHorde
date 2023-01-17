using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject foreground;
    public float health = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreground.GetComponent<SpriteRenderer>().color = new Color((1-health)/Mathf.Max(health,1-health), health / Mathf.Max(health, 1 - health), 0);
        Vector3 scale = foreground.transform.localScale;
        scale.x = health * 0.98f;
        foreground.transform.localScale = scale;
        Vector3 position = foreground.transform.localPosition;
        position.x = (health - 1) * 0.5f * 0.98f;
        foreground.transform.localPosition = position;
    }
}
