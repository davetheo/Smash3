using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PuzzlePresentation : MonoBehaviour 
{
	public static PuzzlePresentation Instance { get; private set; }
	public GridCell[] Grid;

	public event EventHandler<EventArgs> GemCellAdded;
	public event EventHandler<EventArgs> GemCellRemoved;

	private BoardConfig boardConfig;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		// Furthermore we make sure that we don't destroy between scenes (this is optional)
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update()
	{

	}


	// Use this for initialization
	public void InitialiseBoard()
	{
		boardConfig = BoardConfig.Instance;
		Grid = new GridCell[boardConfig.Width * boardConfig.Height];
		for (int index = 0; index < boardConfig.Width * boardConfig.Height; index++)
		{
			var pos = new Vector3(boardConfig.GetPosXFromIndex(index), boardConfig.GetPosYFromIndex(index), 0f);
			var cell = new GridCell(boardConfig.GetGridXFromIndex(index), boardConfig.GetGridYFromIndex(index), index, pos);
			Grid[index] = cell;
		}
	}

	public bool IsBusy()
	{
		for (int index = 0; index < boardConfig.Width * boardConfig.Height; index++ )
		{
			var gemCell = GetGemCellAtIndex(index);
			if (gemCell == null || gemCell.IsBusy)
				return true;
		}
		return false;
	}

	public void RemoveGems(List<int> markedRemove)
	{
		foreach (int index in markedRemove)
		{
			RemoveGemCellAtIndex(index);
		}
	}

	public void CheckForGemsToDrop()
	{
		for (int x = 0; x < boardConfig.Width; x++)
		{
			var newGems = CheckForDropColumn(x);
			AddGemsToColumn(x, newGems);
		}
	}

	// return the nuber of gems that need to be added
	private int CheckForDropColumn(int x)
	{
		int numClear = 0;
		for (int y = boardConfig.Height - 1; y > -1; y--)
		{
			var startCell = GetGemCellAtPos(x, y);
			if (startCell == null)
			{
				numClear++;
			}
			else
			{
				if (numClear > 0)
				{

					var endCell = GetGemCellAtPos(x, y + numClear);
					DropOneGem(startCell, y + numClear);
				}
			}
		}
		return numClear;
	}

	private void AddGemsToColumn(int column, int  gemCount)
	{
		//we may need to modify this later so it checks the top of the currentr gems to see if we need to go up further

		Vector3 pos = new Vector3(boardConfig.GetPosXFromColumn(column), boardConfig.GetPosYFromRow(0), 0f);
		var row = gemCount;
		while (row > 0)
		{
			row--;
			pos.y += boardConfig.CellSpacing.y;
			var newGemCell = Spawner.Instance.GemCellSpawn();
			newGemCell.transform.localPosition = pos;
			var index = boardConfig.GetIndex(column, row);

			var cell = Grid[index];
			newGemCell.transform.parent = this.gameObject.transform;
			cell.InitGemCell(newGemCell, false);
			OnGemCellAdded(newGemCell);
			newGemCell.SetGemToDrop(PuzzleGameConfig.GemDropWait, PuzzleGameConfig.GemDropGravity, PuzzleGameConfig.GemMaximumFall);
		}
	}


	private void DropOneGem(GemCell gemCell, int targetY)
	{
		var sGridCell = gemCell.GridCell;
		var dGridCell = GetGridCellAtPos(sGridCell.BoardX, targetY);
		sGridCell.ClearGemCell();
		dGridCell.ClearGemCell();
		dGridCell.InitGemCell(gemCell, false);
		gemCell.SetGemToDrop(PuzzleGameConfig.GemDropWait, PuzzleGameConfig.GemDropGravity, PuzzleGameConfig.GemMaximumFall);
	}


	public void InitGemCell(int x, int y, GemCell newGemCell)
	{
		//need to get the gridcell for this and then 
		var index = boardConfig.GetIndex(x, y);
		var cell = Grid[index];
		newGemCell.transform.parent = this.gameObject.transform;
		cell.InitGemCell(newGemCell, true);
		OnGemCellAdded(newGemCell);
	}

	public void ChangeGemCellAtPos(int x, int y, GemCell gemCell)
	{
		ChangeGemCellAtIndex(boardConfig.GetIndex(x, y), gemCell);
	}

	public void ChangeGemCellAtIndex(int index, GemCell gemCell)
	{
		boardConfig.ValidateIndex(index);
		RemoveGemCellAtIndex(index);
		var cell = Grid[index];
		gemCell.transform.parent = this.gameObject.transform;
		cell.InitGemCell(gemCell, true);
		OnGemCellAdded(gemCell);
	}

	public void RemoveGemCellAtIndex(int index)
	{
		var gridCell = GetGridCellAtIndex(index);
		var gem = gridCell.GemCell;
		if (gem != null)
		{
			gem = gridCell.RetrieveGemCell();
			if (gem == null)
				throw new UnityException("RemoveGemAtIndex is null: " + index.ToString());
			OnGemCellRemoved(gem);
			GemCellPool.Instance.RemoveOneGemCell(gem);
			//GemPool.Instance.RemoveOneGem(gem);
		}
	}

	public GridCell GetGridCellAtPos(int x, int y)
	{
		return GetGridCellAtIndex(boardConfig.GetIndex(x, y));
	}

	public GridCell GetGridCellAtIndex(int index)
	{
		if (boardConfig.IsValidateIndex(index) == false)
			return null;
		return Grid[index];
	}

	public GemCell GetGemCellAtPos(int x, int y)
	{
		return GetGemCellAtIndex(boardConfig.GetIndex(x, y));
	}

	public GemCell GetGemCellAtIndex(int index)
	{
		if (boardConfig.IsValidateIndex(index) == false)
			return null;
		return Grid[index].GemCell;
	}

	public void SwapTwoGridCellsStraight(int indexA, int indexB)
	{
		var cellA = GetGridCellAtIndex(indexA);
		var cellB = GetGridCellAtIndex(indexB);

		var gem1 = cellA.GemCell;
		var gem2 = cellB.GemCell;
		cellA.ClearGemCell();
		cellB.ClearGemCell();
		cellA.InitGemCell(gem2, true);
		cellB.InitGemCell(gem1, true);
	}

	public void OnGemCellAdded(GemCell gemCell)
	{
		var handler = GemCellAdded;
		if (handler != null)
		{
			handler(gemCell, new EventArgs());
		}
	}

	public void OnGemCellRemoved(GemCell gemCell)
	{
		var handler = GemCellRemoved;
		if (handler != null)
		{
			handler(gemCell, new EventArgs());
		}
	}


}
