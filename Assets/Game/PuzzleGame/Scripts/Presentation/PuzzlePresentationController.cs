using UnityEngine;
using System.Collections;
using System;

public class PuzzlePresentationController : MonoBehaviour 
{
	public static PuzzlePresentationController Instance { get; private set; }


	private float GemSwapTime = 0.4f;
	#region Component connectors

	private GemPool GemPool;

	private bool isSwapping;

	#endregion

	private PuzzleBoard PuzzleBoard;

	public event EventHandler<EventArgs> GemSwapFinished;

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


	// Use this for initialization
	void Start () 
	{
		GemPool = GetComponentInChildren<GemPool>() as GemPool;
		if (GemPool == null)
		{
			throw new UnityException("PuzzlePresentationController must have a GemPool component attached or as a child.");
		}

		PuzzlePresentation.Instance.InitialiseBoard();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if needed we can run through and update whether the board is busy or not
	}

	public void InitialPopulateBoard()
	{
		//we now generate the gems
		int possibleMatches = 0;

		while (possibleMatches == 0)
		{
			for (int y = 0; y < BoardConfig.Instance.Height; y++)
			{
				for (int x = 0; x < BoardConfig.Instance.Width; x++)
				{
					PuzzlePresentation.Instance.ChangeGemCellAtPos(x, y, Spawner.Instance.GemCellSpawn());

					while (PresentationMatchChecker.Instance.CheckMatchAtCell(x, y) == true)
					{
						PuzzlePresentation.Instance.ChangeGemCellAtPos(x, y, Spawner.Instance.GemCellSpawn());
					}
				}
			}
			// make sure there are more than 0 possible matches
			possibleMatches = PresentationMatchChecker.Instance.GetAllPossibleMoves();
		}
	}

	public void PopulateGrid(PuzzleBoard board)
	{
		PuzzleBoard = board;
		for (int y = 0; y < BoardConfig.Instance.Height; y++)
		{
			for (int x = 0; x < BoardConfig.Instance.Width; x++)
			{
				var thisOne = PuzzleBoard.GetGemAtPos(x, y);
				var newGem = GemCellPool.Instance.GetOneGemCell(thisOne);
				newGem.transform.parent = this.gameObject.transform;

				PuzzlePresentation.Instance.InitGemCell(x, y, newGem);
				//OnGemAdded(newGem);
			}
		}
	}

	#region Gem swapping methods

	public void SwapTwoGems(int indexA, int indexB, bool isValid)
	{
		var cellA = PuzzlePresentation.Instance.GetGridCellAtIndex(indexA);
		var cellB = PuzzlePresentation.Instance.GetGridCellAtIndex(indexB);

		if (isValid == true)
		{
			StartCoroutine(DoSwapTwoGems(cellA, cellB));
		}
		else
		{
			StartCoroutine(InvalidSwapTwoGems(cellA, cellB));
		}
	}

	private IEnumerator DoSwapTwoGems(GridCell cellA, GridCell cellB)
	{
		isSwapping = true;
		var gem1 = cellA.GemCell;
		var gem2 = cellB.GemCell;

		gem1.SetStateSwapping();
		gem2.SetStateSwapping();

		cellA.ClearGemCell();
		cellB.ClearGemCell();

		cellA.InitGemCell(gem2, false);
		cellB.InitGemCell(gem1, false);

		var pos1 = cellA.Position;
		var pos2 = cellB.Position;

		iTween.MoveTo(gem1.gameObject, iTween.Hash("position", pos2, "islocal", true, "time", GemSwapTime));
		iTween.MoveTo(gem2.gameObject, iTween.Hash("position", pos1, "islocal", true, "time", GemSwapTime));
		yield return new WaitForSeconds(GemSwapTime);

		gem1.SetStateIdle();
		gem2.SetStateIdle();
		isSwapping = false;
		OnGemSwapFinished();

	}

	private IEnumerator InvalidSwapTwoGems(GridCell cellA, GridCell cellB)
	{
		isSwapping = true;
		var gem1 = cellA.GemCell;
		var gem2 = cellB.GemCell;

		gem1.SetStateSwapping();
		gem2.SetStateSwapping();

		var pos1 = cellA.Position;
		var pos2 = cellB.Position;

		iTween.MoveTo(gem1.gameObject, iTween.Hash("position", pos2, "islocal", true, "time", GemSwapTime));
		iTween.MoveTo(gem2.gameObject, iTween.Hash("position", pos1, "islocal", true, "time", GemSwapTime));
		yield return new WaitForSeconds(GemSwapTime);
		iTween.MoveTo(gem1.gameObject, iTween.Hash("position", pos1, "islocal", true, "time", GemSwapTime));
		iTween.MoveTo(gem2.gameObject, iTween.Hash("position", pos2, "islocal", true, "time", GemSwapTime));
		yield return new WaitForSeconds(GemSwapTime);

		gem1.SetStateIdle();
		gem2.SetStateIdle();
		isSwapping = false;
		OnGemSwapFinished();
	}


	#endregion

	#region Event handlers

	public void OnGemSwapFinished()
	{
		var handler = GemSwapFinished;
		if (handler != null)
		{
			handler(this, new EventArgs());
		}
	}

	#endregion

}
