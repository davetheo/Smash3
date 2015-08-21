using UnityEngine;
using System.Collections;

public class PuzzleBoard
{
	private BoardCell[] Board;
	private int Width;
	private int Height;
	private int BoardSize;
	private BoardConfig BoardConfig;

	public PuzzleBoard(BoardConfig config)
	{
		BoardConfig = config;
		Board = new BoardCell[config.Width * config.Height];
		for (int index = 0; index < config.Width * config.Height; index++)
		{
			var cell = new BoardCell();
			Board[index] = cell;
		}
	}

	//Cell manipulators
	//Get, set, retrieve, remove

	public void SetCellAtPos(int x, int y, BoardCell cell)
	{
		SetCellAtIndex(BoardConfig.GetIndex(x, y), cell);
	}

	public void SetCellAtIndex(int index, BoardCell cell)
	{
		BoardConfig.ValidateIndex(index);
		Board[index] = cell;
	}

	public BoardCell GetCellAtPos(int x, int y)
	{
		return GetCellAtIndex(BoardConfig.GetIndex(x, y));
	}

	public BoardCell GetCellAtIndex(int index)
	{
		BoardConfig.ValidateIndex(index);
		return Board[index];
	}

	public void SwapTwoGems(int indexA, int indexB)
	{
		var cellA = GetCellAtIndex(indexA);
		var cellB = GetCellAtIndex(indexB);
		SetCellAtIndex(indexA, cellB);
		SetCellAtIndex(indexB, cellA);
	}


	#region Simple gem getters

	public int GetGemAtPos(int x, int y)
	{
		// no need to validate as the BoardConfig.GetIndex will validate
		return GetGemAtIndex(BoardConfig.GetIndex(x, y));
	}

	public int GetGemAtIndex(int index)
	{
		BoardConfig.ValidateIndex(index);
		return Board[index].GemId;
	}

	public int GetBgGemAtPos(int x, int y)
	{
		// no need to validate as the BoardConfig.GetIndex will validate
		return GetBgGemAtIndex(BoardConfig.GetIndex(x, y));
	}

	public int GetBgGemAtIndex(int index)
	{
		BoardConfig.ValidateIndex(index);
		return Board[index].BackgroundId;
	}

	public int GetFgGemAtPos(int x, int y)
	{
		// no need to validate as the BoardConfig.GetIndex will validate
		return GetFgGemAtIndex(BoardConfig.GetIndex(x, y));
	}

	public int GetFgGemAtIndex(int index)
	{
		BoardConfig.ValidateIndex(index);
		return Board[index].ForegroundId;
	}

	#endregion


}
