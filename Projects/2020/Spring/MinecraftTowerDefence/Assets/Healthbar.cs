using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
	[Header("Fill settings")]
	[Range(0f, 1f)]
	public float currentHealthFill = 1f;
	[Range(0f, 1f)]
	public float lostHealthFill = 1f;

	public float fillChangeSpeed = 1f;

	private Image currentHealth;
	private Image lostHealth;

	void Awake()
	{
		currentHealth = transform.Find("Mask_CurrentHealth").GetComponent<Image>();
		lostHealth = transform.Find("Mask_LostHealth").GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
	{
		AdjustCurrentHealthFill();
		AdjustLostHealthFill();
	}

	public void SetCurrenthealthFill(float fillAmount)
	{
		currentHealthFill = fillAmount;
	}

	public void AdjustCurrentHealthFill()
	{
		currentHealth.fillAmount = currentHealthFill;
	}

	private void AdjustLostHealthFill()
	{
		if (lostHealthFill > currentHealthFill)
		{
			lostHealthFill = Mathf.Lerp(lostHealthFill, currentHealthFill, Time.deltaTime * fillChangeSpeed);
		}
		else if (lostHealthFill < currentHealthFill)
		{
			lostHealthFill = currentHealthFill;
		}

		lostHealth.fillAmount = lostHealthFill;
	}
}
