using UnityEngine;
using System.Collections.Generic;

public class PossibleMoveData
{
	public int index1;
	public int index2;
	public List<MatchData> matchList = new List<MatchData>();

	public PossibleMoveData()
	{
		matchList = new List<MatchData>();
	}

	public void SetMove(int index1, int index2)
	{
		this.index1 = index1;
		this.index2 = index2;
		matchList.Clear();
	}

	public bool IsValidSwap(GemSwapData data)
	{
		if ((data.indexA == index1 && data.indexB == index2) || (data.indexA == index2 && data.indexB == index1))
		{
			return true;
		}
		return false;
	}

	public void AddMatch(MatchData match)
	{
		matchList.Add(match);
	}

}
