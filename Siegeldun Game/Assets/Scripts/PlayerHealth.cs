using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public bool isDamaged = false;
    private float health;
    private float lTimer;
    public float healthRegen;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image HealthbarF;
    public Image HealthbarB;

    private const float tTimer_max = .2f;
    private int tick;
    public int tick1;
    private float tTimer;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthRegen = 1f;
        tick = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(Random.Range(5,10));
            isDamaged = true;
        }

        if(health != maxHealth && isDamaged == false)
        {
            Regen(healthRegen);
        }

        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateTime();
        UpdateHealth();
    }
    
    public void UpdateHealth()
    {
        Debug.Log(health);
        float fillF = HealthbarF.fillAmount;
        float fillB = HealthbarB.fillAmount;
        float hFraction = health / maxHealth;
        if(isDamaged)
        {
            HealthbarF.fillAmount = hFraction;
            HealthbarB.color = Color.red;   
            lTimer += Time.deltaTime;
            float percentComplete = lTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            HealthbarB.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            HealthbarB.color = Color.green;
            HealthbarB.fillAmount = hFraction;
            lTimer += Time.deltaTime;
            float percentComplete = lTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            HealthbarF.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lTimer = 0f;
    }

    public void Regen(float regenerate)
    {
       health += regenerate * Time.deltaTime;
       lTimer = 0.15f; 
    }

    private void UpdateTime()
    {
        tTimer += Time.deltaTime;
        if (tTimer >= tTimer_max)
        {
            tTimer -= tTimer_max;
            tick++;
            if(tick == 15)
            {
                tick = 0;
                isDamaged = false;
                return;
            }
        }
    }
}
