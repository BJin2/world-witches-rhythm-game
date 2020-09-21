using System;
using System.Collections.Generic;

[Serializable]
public class Note
{
	public float timing = 0;
	public int type = 0;

	public Note(float ti, int ty)
	{
		timing = ti;
		type = ty;
	}
}

[Serializable]
public class Part
{
	public float timing = 0;
	public int singer = 0;

	public Part(float ti, int si)
	{
		timing = ti;
		singer = si;
	}
}

[Serializable]
public class SongInfo
{
	public List<Note> note;
	public List<Part> part;
}