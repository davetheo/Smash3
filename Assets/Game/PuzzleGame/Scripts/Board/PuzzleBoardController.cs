using UnityEngine;
using System.Collections;

public class PuzzleBoardController : MonoBehaviour
{
	//Receives messages from the PuzzleGameController and carries those out on the board - init, swap gems, etc
	#region Component connectors

	private BoardConfig BoardConfig;

	#endregion

	public PuzzleBoard PuzzleBoard;
	public MatchChecker MatchChecker { get; set; }


	#region Unity Mono methods

	// Use this for initialization
	void Awake()
	{
		GetAllComponents();
	}

	// Use this for initialization
	void Start () 
	{
		PuzzleBoard = new PuzzleBoard(BoardConfig);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#endregion

	public void InitialPopulateBoard()
	{
		int possibleMatches = 0;
		MatchChecker.UseBoard(PuzzleBoard);
		while (possibleMatches == 0)
		{
			for (int y = 0; y < BoardConfig.Height; y++)
			{
				for (int x = 0; x < BoardConfig.Width; x++)
				{
				//	PuzzleBoard.SetCellAtPos(x, y, Spawner.CellSpawn());
					while (MatchChecker.CheckMatchAtCell(x, y) == true)
					{
				//		PuzzleBoard.SetCellAtPos(x, y, Spawner.CellSpawn());
					}
				}
			}
			Debug.Log("Need to check here for possible matches and redo the board if there is none.");
			// make sure there are more than 0 possible matches
			//possibleMatches = MatchChecker.GetAllPossibleMoves();
			possibleMatches = 1;
		}
	}

	#region Gem swapping

	public void SwapTwoGems(int indexA, int indexB)
	{
		PuzzleBoard.SwapTwoGems(indexA, indexB);
	}

	#endregion

	#region private init methods

	private void GetAllComponents()
	{
		BoardConfig = GetComponentInParent<BoardConfig>() as BoardConfig;
		if (BoardConfig == null)
		{
			throw new UnityException("PuzzleBoardController must have a BoardConfig component attached or as a parent.");
		}
		MatchChecker = GetComponent<MatchChecker>() as MatchChecker;
		if (MatchChecker == null)
		{
			throw new UnityException("PuzzleBoardController must have a MatchChecker component attached.");
		}
	}

	#endregion
}
