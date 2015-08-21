using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardMatchContainer
{
	// the matches that were found
	public List<MatchData> matchesFound = new List<MatchData>();
	// the gems marked for destroy
	public List<int> markedDestroy = new List<int>();
	// the column drops

	public void Clear()
	{
		matchesFound.Clear();
		markedDestroy.Clear();
	}

	public int MatchedCount()
	{
		return matchesFound.Count;
	}

}
