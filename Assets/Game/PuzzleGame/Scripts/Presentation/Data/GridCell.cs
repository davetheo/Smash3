using UnityEngine;
using System.Collections;

public class GridCell
{

	#region public variables

	/// <summary>
	/// The transform local position on the board
	/// </summary>
	public Vector3 Position;

	/// <summary>
	/// The x position in the board
	/// </summary>
	public int BoardX;

	/// <summary>
	/// The y position in the board
	/// </summary>
	public int BoardY;

	/// <summary>
	/// The index into the board array of this cell
	/// </summary>
	public int Index;

	#endregion

	#region private variables

	public GemCell GemCell { get; set; }

	#endregion

	#region Constructor

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="x"> The x position in the board </param>
	/// <param name="y"> The y position in the board </param>
	/// <param name="index"> The index in the board array </param>
	/// <param name="pos"> The transform position of the board position </param>
	public GridCell(int x, int y, int index, Vector3 pos)
	{
		BoardX = x;
		BoardY = y;
		Index = index;
		Position = pos;
	}

	#endregion

	#region public methods

	public void InitGemCell(GemCell newCell, bool setPosition=true)
	{
		if (GemCell != null)
		{
			throw new UnityException("Trying to init a gem when one is already in the cell");
		}
		if (setPosition)
		{
			newCell.transform.localPosition = Position;
		}
		newCell.GridCell = this;
		GemCell = newCell;
	}

	public void ClearGemCell()
	{
		if (GemCell != null)
		{
			GemCell.GridCell = null;
		}
		GemCell = null;
	}

	public GemCell RetrieveGemCell()
	{
		GemCell.GridCell = null;
		GemCell gemCell = GemCell;
		GemCell = null;
		return gemCell;
	}

	#endregion
}
