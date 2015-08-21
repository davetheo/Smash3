using UnityEngine;
using System.Collections;

public class ComponentContainer : MonoBehaviour 
{
	public PuzzleGameController PuzzleGameController;
	public PuzzleBoardController PuzzleBoardController;

	public PuzzlePresentationController PuzzlePresentationController;
	public PuzzlePresentation PuzzlePresentation;

	#region Unity methods
	void Awake()
	{
	}

	#endregion

}
