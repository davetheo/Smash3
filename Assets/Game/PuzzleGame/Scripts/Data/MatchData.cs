using UnityEngine;
using System.Collections.Generic;

public class MatchData 
{
	public int gemType;
	public List<int> indexList;

	public MatchData()
	{
		gemType = -1;
		indexList = new List<int>();
	}

	public MatchData(int type)
	{
		gemType = type;
		indexList = new List<int>();
	}

	public void SetMatch(int type)
	{
		gemType = type;
		indexList.Clear();
	}

	public void AddIndex(int index)
	{
		indexList.Add(index);
	}

}
