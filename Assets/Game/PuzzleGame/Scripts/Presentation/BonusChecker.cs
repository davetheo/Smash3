using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BonusChecker 
{
	public void CalculateBonusMatches(List<MatchData> matches)
	{
		// firstly check if there are any overlaps
		for (int i = 0; i < matches.Count - 1; i++)
		{
			for (int j = i + 1; j < matches.Count; j++)
			{
				if (DoMatchesOverlap(matches[i], matches[j]))
				{
					ConsolidateMatches(matches[i], matches[j]);
				}
			}
		}

		var zeroIndex = MatchContainsZero(matches);
		while (zeroIndex != -1)
		{
			matches.RemoveAt(zeroIndex);
			zeroIndex = MatchContainsZero(matches);
		}
	}

	private int MatchContainsZero(List<MatchData> matches)
	{
		foreach (var match in matches)
		{
			if (match.indexList.Count == 0)
				return matches.IndexOf(match);
		}
		return -1;
	}

	private void ConsolidateMatches(MatchData match1, MatchData match2)
	{
		while (match2.indexList.Count > 0)
		{
			var newIndex = match2.indexList[0];
			if (match1.indexList.Contains(newIndex) == false)
				match1.AddIndex(match2.indexList[0]);
			match2.indexList.RemoveAt(0);
		}
	}

	private bool DoMatchesOverlap(MatchData match1, MatchData match2)
	{
		if (match1.gemType != match2.gemType)
			return false;
		foreach (int val in match1.indexList)
		{
			if (match2.indexList.Contains(val))
				return true;
		}
		return false;
	}


}
