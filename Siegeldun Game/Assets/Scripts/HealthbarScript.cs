using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript: MonoBehaviour
{

	public float maxHealth = 100f;
	public float currentHealth;
	public float healthRegen;

	public HealthbarColorScript healthBar;

	// Start is called before the first frame update
	void Start()
	{
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
		healthRegen = 1f;
	}

	// Update is called once per frame
	void Update()
	{
		if(currentHealth > maxHealth)
        {
			currentHealth = 100f;
        }
		if(currentHealth < 0)
        {
			currentHealth = 0;
        }
		healthBar.SetHealth(currentHealth += healthRegen * Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.P))
		{
			TakeDamage(10);
		}
	}

	void TakeDamage(float damage)
	{
		currentHealth -= damage;

		healthBar.SetHealth(currentHealth);
	}
}	