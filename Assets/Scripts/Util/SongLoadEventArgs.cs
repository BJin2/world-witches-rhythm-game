using System;
using System.Collections.Generic;

public class SongLoadEventArgs : EventArgs
{
	public SongInfo Info { get; private set; }

	public SongLoadEventArgs(SongInfo info)
	{
		Info = info;
	}
}
