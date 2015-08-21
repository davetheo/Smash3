using UnityEngine;
using System.Collections;

public class BoardCell
{
	public const int NoId = -1;

	public int GemId;
	public int BackgroundId;
	public int ForegroundId;

	#region Constructors

	public BoardCell(int gem, int bg, int fg)
	{
		GemId = gem;
		BackgroundId = bg;
		ForegroundId = fg;
	}

	public BoardCell(int gem)
	{
		GemId = gem;
		BackgroundId = NoId;
		ForegroundId = NoId;
	}

	public BoardCell()
	{
		GemId = NoId;
		BackgroundId = NoId;
		ForegroundId = NoId;
	}

	#endregion


}

