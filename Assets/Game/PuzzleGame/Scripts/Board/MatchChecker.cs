using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchChecker : MonoBehaviour
{

	#region component connectors

	private BoardConfig BoardConfig;

	#endregion

	public List<PossibleMoveData> PossibleMoves;
	public BoardMatchContainer BoardMatchData;

	private PuzzleBoard PuzzleBoard;
	private bool allowAllSwaps;

	// Use this for initialization
	void Awake()
	{
		BoardMatchData = new BoardMatchContainer();

	}

	void Start () 
	{
		BoardConfig = GetComponentInParent<BoardConfig>() as BoardConfig;
		if (BoardConfig == null)
		{
			throw new UnityException("MatchChecker must have a BoardConfig component attached or as a parent.");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetAllowAllSwaps(bool allow)
	{
		allowAllSwaps = allow;
	}

	public void UseBoard(PuzzleBoard board)
	{
		PuzzleBoard = board;
	}

	public bool CheckMatchAtCell(int x, int y)
	{
		int baseGem = PuzzleBoard.GetGemAtPos(x, y);
		int leftMatch = MatchLeftCount(x, y, baseGem);
		int rightMatch = MatchRightCount(x, y, baseGem);
		int topMatch = MatchUpCount(x, y, baseGem);
		int bottomMatch = MatchDownCount(x, y, baseGem);

		if ((leftMatch + rightMatch) > 1)
			return true;
		if ((topMatch + bottomMatch) > 1)
			return true;

		return false;
	}

	public bool CheckForValidSwap(GemSwapData data)
	{
		if (allowAllSwaps == true)
			return true;
		//don't check the possible moves, just check the board
		PuzzleBoard.SwapTwoGems(data.indexA, data.indexB);
		CheckBoardForMatches();
		PuzzleBoard.SwapTwoGems(data.indexA, data.indexB);

/*		foreach (PossibleMoveData move in PossibleMoves)
		{
			if (move.IsValidSwap(data))
				return true;
		}*/
		return BoardMatchData.MatchedCount() > 0;
	}

	#region Matching methods

	public bool CheckBoardForMatches()
	{
		BoardMatchData.Clear();
		List<MatchData> horizontalMatches = new List<MatchData>();
		List<MatchData> verticalMatches = new List<MatchData>();

		// firstly go through the board and find the horizontal matches
		for (int y = 0; y < BoardConfig.Height; y++)
		{
			int xIndex = 0;
			while (xIndex < BoardConfig.Width)
			{
				var baseGem = PuzzleBoard.GetGemAtPos(xIndex, y);
				var xMatch = MatchRightCount(xIndex, y, baseGem) + 1;
				if (xMatch > 2)
				{
					MatchData match = new MatchData(baseGem);
					while (xMatch > 0)
					{
						match.AddIndex(BoardConfig.GetIndex(xIndex, y));
						xMatch--;
						xIndex++;
					}
					horizontalMatches.Add(match);
				}
				else
				{
					xIndex += xMatch;
				}
			}
		}

		//now the vertical matches
		for (int x = 0; x < BoardConfig.Width; x++)
		{
			int yIndex = 0;
			while (yIndex < BoardConfig.Height)
			{
				var baseGem = PuzzleBoard.GetGemAtPos(x, yIndex);
				var yMatch = MatchDownCount(x, yIndex, baseGem) + 1;
				if (yMatch > 2)
				{
					MatchData match = new MatchData(baseGem);
					while (yMatch > 0)
					{
						match.AddIndex(BoardConfig.GetIndex(x, yIndex));
						yMatch--;
						yIndex++;
					}
					verticalMatches.Add(match);
				}
				else
				{
					yIndex += yMatch;
				}
			}
		}

		if (horizontalMatches.Count == 0 && verticalMatches.Count == 0)
			return false;

		if (horizontalMatches.Count > 0 && verticalMatches.Count > 0)
		{
			//horizontalMatches.Sort
			// go through all the horizontal matches and see if there's any vertical matches that cross
			// and if so it grabs the largest one and tags it as to be used with the horizontal and move on.
		}
		BoardMatchData.matchesFound.AddRange(horizontalMatches);
		BoardMatchData.matchesFound.AddRange(verticalMatches);

		foreach (var match in BoardMatchData.matchesFound)
		{
			foreach (var index in match.indexList)
			{
				if (BoardMatchData.markedDestroy.Contains(index) == false)
					BoardMatchData.markedDestroy.Add(index);
			}
		}

		return BoardMatchData.matchesFound.Count > 0;
	}

	#endregion

	#region possible move methods

	public int GetAllPossibleMoves()
	{
		PossibleMoves.Clear();
		//clear out our match list
		for (int y = 0; y < BoardConfig.Height; y++)
		{
			for (int x = 0; x < BoardConfig.Width; x++)
			{
				AddOnePossibleMatch(x, y);
			}
		}

		foreach (var possibleMove in PossibleMoves)
		{
		//	BonusChecker.CalculateBonusMatches(possibleMove.matchList);
		}

		return PossibleMoves.Count;
	}

	private void AddOnePossibleMatch(int x, int y)
	{
		//all checks are to the right and down and both are valid when they get here
		var index = BoardConfig.GetIndex(x, y);

		//only check to the right if we're not at the right edge
		if (x < BoardConfig.Width - 1)
		{

			var indexr = BoardConfig.GetIndex(x + 1, y);
			//int baseGemr = PuzzleBoard.GetGemAtIndex(indexr);

			PuzzleBoard.SwapTwoGems(index, indexr);
			var match1x = GetOneIndexMatchData(x, y);
			var match2x = GetOneIndexMatchData(x + 1, y);
			if (match1x != null || match2x != null)
			{
				var possible = new PossibleMoveData();
				possible.SetMove(index, indexr);
				if (match1x != null)
					possible.AddMatch(match1x);
				if (match2x != null)
					possible.AddMatch(match2x);
				PossibleMoves.Add(possible);
			}
			PuzzleBoard.SwapTwoGems(index, indexr);
		}

		//only check down if we're not at the bottom edge
		if (y < BoardConfig.Height - 1)
		{
			var indexd = BoardConfig.GetIndex(x, y + 1);
			//int baseGemd = PuzzleBoard.GetGemAtIndex(indexd);

			PuzzleBoard.SwapTwoGems(index, indexd);
			var match1y = GetOneIndexMatchData(x, y);
			var match2y = GetOneIndexMatchData(x, y + 1);
			if (match1y != null || match2y != null)
			{
				var possible = new PossibleMoveData();
				possible.SetMove(index, indexd);
				if (match1y != null)
					possible.AddMatch(match1y);
				if (match2y != null)
					possible.AddMatch(match2y);
				PossibleMoves.Add(possible);
			}
			PuzzleBoard.SwapTwoGems(index, indexd);
		}
	}

	private MatchData GetOneIndexMatchData(int x, int y)
	{
		var index = BoardConfig.GetIndex(x, y);
		int baseGem = PuzzleBoard.GetGemAtPos(x, y);

		if (baseGem == GemConverter.NoGem)
			return null;

		var leftMatch = MatchLeftCount(x, y, baseGem);
		var rightMatch = MatchRightCount(x, y, baseGem);
		var upMatch = MatchUpCount(x, y, baseGem);
		var downMatch = MatchDownCount(x, y, baseGem);

		var horizontalMatch = 1 + leftMatch + rightMatch;
		var verticalMatch = 1 + upMatch + downMatch;

		if (horizontalMatch > 2 && verticalMatch > 2)
		{
			//corner match
			var matchData = new MatchData(index);
			matchData.AddIndex(BoardConfig.GetIndex(x, y));
			while (leftMatch > 0)
			{
				leftMatch--;
				matchData.AddIndex(BoardConfig.GetIndex(x - leftMatch, y));
			}
			while (rightMatch > 0)
			{
				rightMatch--;
				matchData.AddIndex(BoardConfig.GetIndex(x + rightMatch, y));
			}
			while (upMatch > 0)
			{
				upMatch--;
				matchData.AddIndex(BoardConfig.GetIndex(x, y - upMatch));
			}
			while (downMatch > 0)
			{
				downMatch--;
				matchData.AddIndex(BoardConfig.GetIndex(x, y + downMatch));
			}
			return matchData;
		}
		else
		if (horizontalMatch > 2)
		{
			var startx = x + rightMatch;
			// simple side match 
			var matchData = new MatchData(index);
			while (horizontalMatch > 0)
			{
				horizontalMatch--;
				matchData.AddIndex(BoardConfig.GetIndex(startx - horizontalMatch, y));
			}
			return matchData;
		}
		else
		if (verticalMatch > 2)
		{
			var starty = y + downMatch;
			// vertical
			var matchData = new MatchData(index);
			while (verticalMatch > 0)
			{
				verticalMatch--;
				matchData.AddIndex(BoardConfig.GetIndex(x, starty - verticalMatch));
			}
			return matchData;
		}
		return null;
	}


	#endregion

	#region directional match methods

	// All match counts return the number that match with the specified gem, so 2 will mean 3 (original plus 2)
	private int MatchLeftCount(int x, int y, int type)
	{
		int matches = 0;
		var remaining = GetValidCellsToLeft(x);
		while (remaining > 0)
		{
			x--;
			if (PuzzleBoard.GetGemAtPos(x, y) == type)
				matches++;
			else
				return matches;
			remaining--;
		}
		return matches;
	}

	private int MatchRightCount(int x, int y, int type)
	{
		int matches = 0;
		var remaining = GetValidCellsToRight(x);
		while (remaining > 0)
		{
			x++;
			if (PuzzleBoard.GetGemAtPos(x, y) == type)
				matches++;
			else
				return matches;
			remaining--;
		}
		return matches;
	}

	private int MatchUpCount(int x, int y, int type)
	{
		int matches = 0;
		var remaining = GetValidCellsToTop(y);
		while (remaining > 0)
		{
			y--;
			if (PuzzleBoard.GetGemAtPos(x, y) == type)
				matches++;
			else
				return matches;
			remaining--;
		}
		return matches;
	}

	private int MatchDownCount(int x, int y, int type)
	{
		int matches = 0;
		var remaining = GetValidCellsToBottom(y);
		while (remaining > 0)
		{
			y++;
			if (PuzzleBoard.GetGemAtPos(x, y) == type)
				matches++;
			else
				return matches;
			remaining--;
		}
		return matches;
	}

	#endregion

	#region Checks for valid cells
	// also need to check for state of those

	private int GetValidCellsToRight(int x)
	{
		return BoardConfig.Width - x - 1;
	}

	private int GetValidCellsToLeft(int x)
	{
		return x;
	}

	private int GetValidCellsToTop(int y)
	{
		return y;
	}

	private int GetValidCellsToBottom(int y)
	{
		return BoardConfig.Height - y - 1;
	}

	#endregion

}
