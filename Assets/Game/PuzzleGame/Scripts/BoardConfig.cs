using UnityEngine;
using System.Collections;

public class BoardConfig : MonoBehaviour 
{
	public static BoardConfig Instance { get; private set; }

	/// <summary>
	/// The width of the board in cells.
	/// </summary>
	public int Width = 9;

	/// <summary>
	/// The height of the board in cells.
	/// </summary>
	public int Height = 9;

	/// <summary>
	/// The transform position spacing of the board cell positions
	/// </summary>
	public Vector2 CellSpacing;

	private float[] CellXPositions;
	private float[] CellYPositions;
	private float gridWorldStartX;
	private float gridWorldStartY;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		// Furthermore we make sure that we don't destroy between scenes (this is optional)
		DontDestroyOnLoad(gameObject);


		CellXPositions = new float[Width];
		CellYPositions = new float[Height];

		float gridWidth = Width * CellSpacing.x;
		gridWorldStartX = 0 - (gridWidth * 0.5f) + (CellSpacing.x * 0.5f);
		for (int x = 0; x < Width; x++)
		{
			CellXPositions[x] = gridWorldStartX + (CellSpacing.x * x);
		}

		float gridHeight = Height * CellSpacing.y;
		gridWorldStartY = (gridHeight * 0.5f) - (CellSpacing.y * 0.5f);
		for (int y = 0; y < Height; y++)
		{
			CellYPositions[y] = gridWorldStartY - (CellSpacing.y * y);
		}
	}

	#region public board transform position methods

	public float GetPosXFromIndex(int index)
	{
		var x = GetGridXFromIndex(index);
		return CellXPositions[x];
	}

	public float GetPosYFromIndex(int index)
	{
		var y = GetGridYFromIndex(index);
		return CellYPositions[y];
	}

	public float GetPosXFromColumn(int x)
	{
		return CellXPositions[x];
	}

	public float GetPosYFromRow(int y)
	{
		return CellYPositions[y];
	}

	#endregion

	#region public grid and index helpers

	public int GetIndex(int x, int y)
	{
		ValidatePos(x, y);
		return (y * Width) + x;
	}

	public int GetGridXFromIndex(int index)
	{
		ValidateIndex(index);
		return index % Width;
	}

	public int GetGridYFromIndex(int index)
	{
		ValidateIndex(index);
		return index / Width;
	}

	#endregion

	#region public board checkers

	public bool IsValidateIndex(int index)
	{
		if (index < 0 || index >= Width * Height)
		{
			return false;
		}
		return true;
	}

	public bool IsValidPos(int x, int y)
	{
		if (x < 0 || x >= Width)
		{
			return false;
		}
		if (y < 0 || y >= Height)
		{
			return false;
		}
		return true;
	}

	#endregion

	#region private grid validators

	public void ValidatePos(int x, int y)
	{
		if (x < 0 || x >= Width)
		{
			throw new UnityException("X value is invalid: " + x.ToString());
		}
		if (y < 0 || y >= Height)
		{
			throw new UnityException("Y value is invalid: " + y.ToString());
		}
	}

	public void ValidateIndex(int index)
	{
		if (index < 0 || index >= Width * Height)
		{
			throw new UnityException("index value is invalid: " + index.ToString());
		}
	}

	#endregion

}
