using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance { get; private set; }

    [SerializeField]
    private UnityEngine.UI.Slider healthBar = null;
    private float health = 0;

	private void Awake()
	{
		Instance = this;
		ChangeHealth(1);
	}

	//Decrease fixed amount regardless of situation
	public void DecreaseHealth()
	{
		ChangeHealth(-0.00f);
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
			SelectionWindow.Instance.Show("G A M E   O V E R", "Your flight health reached 0.\nYour flight cannot continue the mission", new List<SelectionWindow.ButtonInfo>
			{
				new SelectionWindow.ButtonInfo("RESTART", SongPlayer.Instance.Replay),
				new SelectionWindow.ButtonInfo("RETREAT", ()=>{ AsyncSceneLoader.LoadAsyncAdditive("Base", this); Time.timeScale = 1.0f; })
			});
			SongPlayer.Instance.Pause();
		}
	}
}
