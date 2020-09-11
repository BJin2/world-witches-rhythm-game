using System.Collections.Generic;

public static class CharacterInfo
{
	static CharacterInfo()
	{
		characterName = new List<string>
		{
			"Miyafuji",
			"Sakamoto",
			"Lynette",
			"Perrine",
			"Minna",
			"Barkhorn",
			"Hartmann",
			"Charlotte",
			"Lucchini",
			"Sanya",
			"Eila"
		};
		Init();
	}
	public static readonly List<string> characterName;
	public static List<string> flightMember;
	public static int currentMemberIndex;

	public static void Init()
	{
		flightMember = new List<string>();
		flightMember.Clear();
		for (int i = 0; i < 5; i++)
		{
			flightMember.Add("");
		}
		currentMemberIndex = -1;
	}
}
