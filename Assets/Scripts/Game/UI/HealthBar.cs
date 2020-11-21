using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance { get; private set; }

    [SerializeField]
    private UnityEngine.UI.Slider healthBar;
    private float health = 0;

	private void Awake()
	{
		Instance = this;
		ChangeHealth(1);
	}

	//Decrease fixed amount regardless of situation
	public void DecreaseHealth()
	{
		ChangeHealth(-0.05f);
	}

	private void ChangeHealth(float amount)
	{
		health += amount;
		healthBar.value = health;

		if (health >= 0.75f)
		{
			healthBar.fillRect.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1);
		}
		else if (health >= 0.5f)
		{
			healthBar.fillRect.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1,0);
		}
		else if (health >= 0.25f)
		{
			healthBar.fillRect.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 0.372f, 0);
		}
		else
		{
			healthBar.fillRect.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 0, 0);
		}

		if (health <= 0)
		{
			SongPlayer.Instance.Pause();
			//TOdO Game over message
		}
	}
}
