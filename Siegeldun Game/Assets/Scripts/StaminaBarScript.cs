using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{

	public float maxStam = 100f;
	public float currentStam;
	public float stamRegen;

	public StaminaScript stamBar;

	private bool attacking = false;

	// Start is called before the first frame update
	void Start()
	{
		currentStam = maxStam;
		stamBar.SetMaxStam(maxStam);
		stamRegen = 1f;
	}

	// Update is called once per frame
	void Update()
	{
		if (currentStam > maxStam)
		{
			currentStam = 100f;
		}
		if (currentStam < 0)
		{
			currentStam = 0;
		}
		stamBar.SetStam(currentStam += stamRegen * Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.Mouse0) && attacking == false)
		{
			ReduceStamina(10);
		}
	}

	void ReduceStamina(float damage)
	{
		currentStam -= damage;

		stamBar.SetStam(currentStam);
	}
}