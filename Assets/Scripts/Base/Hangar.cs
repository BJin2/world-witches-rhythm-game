using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Hangar : KeyAction
{
	[SerializeField]
	private GameObject hangarUI = null;
	[SerializeField]
	private GameObject characterUI = null;
	[SerializeField]
	private List<MonoBehaviour> listToDiable = null;
	private Dictionary<string, Sprite> profiles = null;

	private void Start()
	{
		profiles = new Dictionary<string, Sprite>();
		foreach (string name in CharacterInfo.characterName)
		{
			profiles.Add(name, Resources.Load("Sprites/" + name, typeof(Sprite)) as Sprite);
		}
	}

	protected override void OnTriggerEnter(Collider col)
	{
		InstructionTrigger(col.GetComponent<BaseCharacter>(), true, new TriggeredAction(TurnOnFormation), KeyCode.E, "Form a flight");
	}

	protected override void OnTriggerExit(Collider col)
	{
		InstructionTrigger(col.GetComponent<BaseCharacter>(), false, new TriggeredAction(TurnOnFormation));
	}

	//This function must be passed by delegate to target script
	private void TurnOnFormation()
	{
		UIEnable(true);
	}

	public void TurnOffFormation()
	{
		UIEnable(false);
	}

	private void UIEnable(bool enable)
	{
		//Diable other scripts that processes input
		foreach (MonoBehaviour toDiable in listToDiable)
		{
			toDiable.enabled = !enable;
		}

		//Turn on hangar UI
		hangarUI.SetActive(enable);
		CharacterInfo.Init();
		for (int i = 0; i < 5; i++)
		{
			UpdateProfile(i);
		}
	}

#region Functions for member selection

	public void ConfirmMember()
	{
		//Check all slots are filled
		foreach (string member in CharacterInfo.flightMember)
		{
			if (member == "")
				return;
		}
		AsyncSceneLoader.LoadAsyncAdditive("Gameplay", this);
	}

	public void SelectMember(int index)
	{
		CharacterInfo.currentMemberIndex = index;
		characterUI.SetActive(true);
	}

	public void SelectCharacter(string name)
	{
		if (name != "")
		{
			//Current slot is already occupied or selected character already exist
			for (int i = 0; i < CharacterInfo.flightMember.Count; i++)
			{
				if (CharacterInfo.flightMember[i] == name)
				{
					//Swap
					CharacterInfo.flightMember[i] = CharacterInfo.flightMember[CharacterInfo.currentMemberIndex];
					UpdateProfile(i);
					break;
				}
			}
		}

		CharacterInfo.flightMember[CharacterInfo.currentMemberIndex] = name;
		UpdateProfile(CharacterInfo.currentMemberIndex);

		//Close Character selection window
		GameObject.Find("CharacterList").SetActive(false);
	}

	public void CloseWindow(GameObject toClose)
	{
		toClose.SetActive(false);
	}

	private void UpdateProfile(int index)
	{
		string name = CharacterInfo.flightMember[index];
		Transform slotBG = hangarUI.transform.Find("BG").Find("SlotBG");
		Image profile = slotBG.GetChild(index).Find("Profile").GetComponent<Image>();
		if (name != "")
		{
			profile.sprite = profiles[CharacterInfo.flightMember[index]];
			profile.color = new Color(255, 255, 255, 1.0f);
		}
		else
		{
			profile.color = new Color(255, 255, 255, 0.0f);
		}
	}

#endregion
}
