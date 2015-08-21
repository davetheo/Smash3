using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PresentationMatchChecker : MonoBehaviour 
{
	public static PresentationMatchChecker Instance { get; private set; }

	public List<PossibleMoveData> PossibleMoves = new List<PossibleMoveData>();
	public BoardMatchContainer BoardMatchContainer;
	private BonusChecker BonusChecker;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		// Furthermore we make sure that we don't destroy between scenes (this is optional)
		DontDestroyOnLoad(gameObject);

		BoardMatchContainer = new BoardMatchContainer();
		BonusChecker = new BonusChecker();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool CheckBoardForMatches()
	{
		BoardMatchContainer.Clear();
		List<MatchData> horizontalMatches = new List<MatchData>();
		List<MatchData> verticalMatches = new List<MatchData>();

		// firstly go through the board and find the horizontal matches
		for (int y = 0; y < BoardConfig.Instance.Height; y++)
		{
			int xIndex = 0;
			while (xIndex < BoardConfig.Instance.Width)
			{
				var gemCell = PuzzlePresentation.Instance.GetGemCellAtPos(xIndex, y);

				var xMatch = MatchRightCount(xIndex, y, gemCell) + 1;
				if (xMatch > 2)
				{
					MatchData match = new MatchData(gemCell.Type);
					while (xMatch > 0)
					{
						match.AddIndex(BoardConfig.Instance.GetIndex(xIndex, y));
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
		for (int x = 0; x < BoardConfig.Instance.Width; x++)
		{
			int yIndex = 0;
			while (yIndex < BoardConfig.Instance.Height)
			{
				var gemCell = PuzzlePresentation.Instance.GetGemCellAtPos(x, yIndex);
				var yMatch = MatchDownCount(x, yIndex, gemCell) + 1;
				if (yMatch > 2)
				{
					MatchData match = new MatchData(gemCell.Type);
					while (yMatch > 0)
					{
						match.AddIndex(BoardConfig.Instance.GetIndex(x, yIndex));
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
		BoardMatchContainer.matchesFound.AddRange(horizontalMatches);
		BoardMatchContainer.matchesFound.AddRange(verticalMatches);

		foreach (var match in BoardMatchContainer.matchesFound)
		{
			foreach (var index in match.indexList)
			{
				if (BoardMatchContainer.markedDestroy.Contains(index) == false)
					BoardMatchContainer.markedDestroy.Add(index);
			}
		}

		return BoardMatchContainer.matchesFound.Count > 0;
	}

	public bool CheckMatchAtCell(int x, int y)
	{
		var gemCell = PuzzlePresentation.Instance.GetGemCellAtPos(x, y);
		int leftMatch = MatchLeftCount(x, y, gemCell);
		int rightMatch = MatchRightCount(x, y, gemCell);
		int topMatch = MatchUpCount(x, y, gemCell);
		int bottomMatch = MatchDownCount(x, y, gemCell);
		if ((leftMatch + rightMatch) > 1)
			return true;
		if ((topMatch + bottomMatch) > 1)
			return true;
		return false;
	}

	public void CalculateBonusMatches(List<MatchData> matches)
	{
		BonusChecker.CalculateBonusMatches(matches);
	}

	#region directional match methods

	// All match counts return the number that match with the specified gem, so 2 will mean 3 (original plus 2)
	private int MatchLeftCount(int x, int y, GemCell gemCell)
	{
		if (IsCellMatchable(gemCell) == false)
			return 0;
		int matches = 0;
		var remaining = GetValidCellsToLeft(x);
		while (remaining > 0)
		{
			x--;
			var cell = PuzzlePresentation.Instance.GetGemCellAtPos(x, y);
			if (IsMatch(gemCell, cell))
				matches++;
			else
				return matches;
			remaining--;
		}
		return matches;
	}

	private int MatchRightCount(int x, int y, GemCell gemCell)
	{
		if (IsCellMatchable(gemCell) == false)
			return 0;
		int matches = 0;
		var remaining = GetValidCellsToRight(x);
		while (remaining > 0)
		{
			x++;
			var cell = PuzzlePresentation.Instance.GetGemCellAtPos(x, y);
			if (IsMatch(gemCell, cell))
				matches++;
			else
				return matches;
			remaining--;
		}
		return matches;
	}

	private int MatchUpCount(int x, int y, GemCell gemCell)
	{
		if (IsCellMatchable(gemCell) == false)
			return 0;
		int matches = 0;
		var remaining = GetValidCellsToTop(y);
		while (remaining > 0)
		{
			y--;
			var cell = PuzzlePresentation.Instance.GetGemCellAtPos(x, y);
			if (IsMatch(gemCell, cell))
				matches++;
			else
				return matches;
			remaining--;
		}
		return matches;
	}

	private int MatchDownCount(int x, int y, GemCell gemCell)
	{
		if (IsCellMatchable(gemCell) == false)
			return 0;
		int matches = 0;
		var remaining = GetValidCellsToBottom(y);
		while (remaining > 0)
		{
			y++;
			var cell = PuzzlePresentation.Instance.GetGemCellAtPos(x, y);
			if (IsMatch(gemCell, cell))
				matches++;
			else
				return matches;
			remaining--;
		}
		return matches;
	}

	#endregion

	#region match helpers

	private bool IsCellMatchable(GemCell gemCell)
	{
		if (gemCell == null || gemCell.IsBusy)
			return false;
		return true;
	}

	private bool IsMatch(GemCell cell1, GemCell cell2)
	{
		if (IsCellMatchable(cell1) == false || IsCellMatchable(cell2) == false)
			return false;
		//could do a little more here maybe, but for now this is enough
		return cell1.Type == cell2.Type;
	}

	#endregion

	public int GetAllPossibleMoves()
	{
		PossibleMoves.Clear();
		//clear out our match list
		for (int y = 0; y < BoardConfig.Instance.Height; y++)
		{
			for (int x = 0; x < BoardConfig.Instance.Width; x++)
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
		var index = BoardConfig.Instance.GetIndex(x, y);
		//int baseGem = PuzzleBoard.GetGemAtIndex(index);

		//only check to the right if we're not at the right edge
		if (x < BoardConfig.Instance.Width - 1)
		{

			var indexr = BoardConfig.Instance.GetIndex(x + 1, y);

			PuzzlePresentation.Instance.SwapTwoGridCellsStraight(index, indexr);
			//PuzzleBoard.SwapTwoGems(index, indexr);
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
			PuzzlePresentation.Instance.SwapTwoGridCellsStraight(index, indexr);
		}

		//only check down if we're not at the bottom edge
		if (y < BoardConfig.Instance.Height - 1)
		{
			var indexd = BoardConfig.Instance.GetIndex(x, y + 1);
			//int baseGemd = PuzzleBoard.GetGemAtIndex(indexd);

			PuzzlePresentation.Instance.SwapTwoGridCellsStraight(index, indexd);
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
			PuzzlePresentation.Instance.SwapTwoGridCellsStraight(index, indexd);
		}
	}

	private MatchData GetOneIndexMatchData(int index)
	{
		var x = BoardConfig.Instance.GetGridXFromIndex(index);
		var y = BoardConfig.Instance.GetGridYFromIndex(index);
		return GetOneIndexMatchData(x, y);
	}


	private MatchData GetOneIndexMatchData(int x, int y)
	{
		var index = BoardConfig.Instance.GetIndex(x, y);
		var gemCell = PuzzlePresentation.Instance.GetGemCellAtPos(x, y);
		if (IsCellMatchable(gemCell) == false)
			return null;

		var leftMatch = MatchLeftCount(x, y, gemCell);
		var rightMatch = MatchRightCount(x, y, gemCell);
		var upMatch = MatchUpCount(x, y, gemCell);
		var downMatch = MatchDownCount(x, y, gemCell);

		var horizontalMatch = 1 + leftMatch + rightMatch;
		var verticalMatch = 1 + upMatch + downMatch;

		if (horizontalMatch > 2 && verticalMatch > 2)
		{
			//corner match
			var matchData = new MatchData(index);
			matchData.AddIndex(BoardConfig.Instance.GetIndex(x, y));
			while (leftMatch > 0)
			{
				leftMatch--;
				matchData.AddIndex(BoardConfig.Instance.GetIndex(x - leftMatch, y));
			}
			while (rightMatch > 0)
			{
				rightMatch--;
				matchData.AddIndex(BoardConfig.Instance.GetIndex(x + rightMatch, y));
			}
			while (upMatch > 0)
			{
				upMatch--;
				matchData.AddIndex(BoardConfig.Instance.GetIndex(x, y - upMatch));
			}
			while (downMatch > 0)
			{
				downMatch--;
				matchData.AddIndex(BoardConfig.Instance.GetIndex(x, y + downMatch));
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
				matchData.AddIndex(BoardConfig.Instance.GetIndex(startx - horizontalMatch, y));
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
				matchData.AddIndex(BoardConfig.Instance.GetIndex(x, starty - verticalMatch));
			}
			return matchData;
		}
		return null;
	}

	public bool CheckForValidSwap(GemSwapData swap)
	{
		var retval = false;
		PuzzlePresentation.Instance.SwapTwoGridCellsStraight(swap.indexA, swap.indexB);
		var match1x = GetOneIndexMatchData(swap.indexA);
		var match2x = GetOneIndexMatchData(swap.indexB);
		if (match1x != null || match2x != null)
		{
			retval = true;
		}
		PuzzlePresentation.Instance.SwapTwoGridCellsStraight(swap.indexA, swap.indexB);
		return retval;
	}

	#region Checks for valid cells
	// also need to check for state of those

	private int GetValidCellsToRight(int x)
	{
		return BoardConfig.Instance.Width - x - 1;
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
		return BoardConfig.Instance.Height - y - 1;
	}

	#endregion

}
