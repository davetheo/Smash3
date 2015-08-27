using UnityEngine;
using System.Collections;

public class PuzzleGameController : MonoBehaviour
{
	public static PuzzleGameController Instance { get; private set; }

	#region component connectors

	private PuzzleBoardController PuzzleBoardController;
	private PuzzlePresentationController PuzzlePresentationController;
	private GemSwapper GemSwapper;
	private MatchChecker MatchChecker;

	private bool waitForSettle;
	private int cascadeCount;

	#endregion

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
		GetAllComponents();
		SetupGemSwapHandler();
		//setup callbackd for the presentation to let us know when things have finished 
		MatchChecker = PuzzleBoardController.MatchChecker;
		//for now start a coroutine and then start a game
		StartCoroutine(TestInitGame());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (waitForSettle)
		{
			if (PuzzlePresentation.Instance.IsBusy() == false)
			{
				waitForSettle = false;
				AllGemsHaveSettled();
			}
		}
	}

	private IEnumerator TestInitGame()
	{
		yield return new WaitForSeconds(1.0f);
		PuzzlePresentationController.InitialPopulateBoard();
		Debug.Log("Found possible matches: " + PresentationMatchChecker.Instance.PossibleMoves.Count);

	//	PuzzleBoardController.InitialPopulateBoard();
	//	yield return new WaitForSeconds(0.1f);
	//	PuzzlePresentationController.PopulateGrid(PuzzleBoardController.PuzzleBoard);
	}


	private void GetAllComponents()
	{
		PuzzleBoardController = GetComponentInChildren<PuzzleBoardController>() as PuzzleBoardController;
		if (PuzzleBoardController == null)
		{
			throw new UnityException("PuzzleGameController must have a PuzzleBoardController component as a child.");
		}
		PuzzlePresentationController = GetComponentInChildren<PuzzlePresentationController>() as PuzzlePresentationController;
		if (PuzzlePresentationController == null)
		{
			throw new UnityException("PuzzleGameController must have a PuzzlePresentationController component as a child.");
		}
		GemSwapper = GetComponentInChildren<GemSwapper>() as GemSwapper;
		if (GemSwapper == null)
		{
			throw new UnityException("PuzzleGameController must have a GemSwapper component as a child.");
		}
	}

	private void AllGemsHaveSettled()
	{
		if (PresentationMatchChecker.Instance.CheckBoardForMatches())
		{
			PuzzlePresentation.Instance.RemoveGems(PresentationMatchChecker.Instance.BoardMatchContainer.markedDestroy);

			PresentationMatchChecker.Instance.CalculateBonusMatches(PresentationMatchChecker.Instance.BoardMatchContainer.matchesFound);
			cascadeCount++;
			foreach (var match in PresentationMatchChecker.Instance.BoardMatchContainer.matchesFound)
			{
				//maybe raise a clear event for each gem
				RaiseMatchEvent(match);
			}
			RaiseCascadeEvent(cascadeCount);
			//raise an event for cascade

			PuzzlePresentation.Instance.CheckForGemsToDrop();
			waitForSettle = true;
			//PuzzleValidator.WaitForBoardSettle(() => AllGemsHaveSettled());			
		}
		else
		if (PresentationMatchChecker.Instance.PossibleMoves.Count == 0)
		{
			//DoBoardReshuffle();
		}
	}

	private void RaiseMatchEvent(MatchData match)
	{
		Debug.Log("Match!!  " + match.indexList.Count);
	}

	private void RaiseCascadeEvent(int count)
	{
		if (count > 1)
			Debug.Log("Cascade!!  " + count);
	}

	#region Gem Swap Methods

	private void SetupGemSwapHandler()
	{
		GemSwapper.Swapped += GemSwapper_Swapped;
		PuzzlePresentationController.GemSwapFinished += PuzzlePresentationController_GemSwapFinished;
	}

	void PuzzlePresentationController_GemSwapFinished(object sender, System.EventArgs e)
	{
		cascadeCount = 0;
		AllGemsHaveSettled();
	}

	void GemSwapper_Swapped(object sender, GemSwapEventArgs e)
	{
		Debug.Log("PuzzleGameController Swapped Event");
		GemSwapData swap = new GemSwapData(e.indexA, e.indexB, e.isValid);
		e.isValid = PresentationMatchChecker.Instance.CheckForValidSwap(swap);
		PuzzlePresentationController.SwapTwoGems(e.indexA, e.indexB, e.isValid);
	}

	#endregion

}
