using UnityEngine;
using System.Collections;
using System;

public class GemSwapper : MonoBehaviour 
{

	public event EventHandler<GemSwapEventArgs> Swapped;

	private ComponentContainer ComponentContainer;
	private PuzzlePresentation PuzzlePresentation;
	private PuzzleBoard PuzzleBoard;
	private GemCell selectedGemCell;

	private bool waitRelease;

	void Awake ()
	{
		ComponentContainer = GetComponentInParent<ComponentContainer>() as ComponentContainer;
		if (ComponentContainer == null)
		{
			throw new UnityException("GemSwapper must have a ComponentContainer component attached to a parent.");
		}
	}

	// Use this for initialization
	void Start () 
	{
		PuzzleBoard = ComponentContainer.PuzzleBoardController.PuzzleBoard;
		PuzzlePresentation = ComponentContainer.PuzzlePresentation;
		PuzzlePresentation.Instance.GemCellAdded += Grid_GemCellAdded;
		PuzzlePresentation.Instance.GemCellRemoved += Grid_GemCellRemoved;
		selectedGemCell = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (waitRelease)
		{
			if (Input.GetMouseButton(0) == false)
				waitRelease = false;
		}
	}

	#region Gem Add Remove Event handlers

	void Grid_GemCellAdded(object sender, System.EventArgs e)
	{
		var gem = sender as GemCell;
		gem.Touched += gem_Touched;
		gem.TouchReleased += gem_TouchReleased;
		gem.Swiped += gem_Swiped;
	}

	void Grid_GemCellRemoved(object sender, System.EventArgs e)
	{
		var gem = sender as GemCell;
		gem.Touched -= gem_Touched;
		gem.TouchReleased -= gem_TouchReleased;
		gem.Swiped -= gem_Swiped;
	}

	#endregion

	#region Manipulation methods

	private void SetSelectedGemCell(GemCell gemCell)
	{
		if (selectedGemCell != null)
		{
			selectedGemCell.SetStateIdle();
		}
		selectedGemCell = gemCell;
		if (selectedGemCell != null)
			gemCell.SetStateSelected();
	}

	#endregion

	#region Gem Manipulation Event Handlers

	void gem_Touched(object sender, System.EventArgs e)
	{
		if (waitRelease)
			return;
		var gemCell = sender as GemCell;

		if (selectedGemCell != null)
		{
			if (AreGemCellsAdjacent(gemCell, selectedGemCell))
			{
				gemCell.StopAllCoroutines();
				selectedGemCell.StopAllCoroutines();
				RaiseSwappedEvent(gemCell, selectedGemCell);
				selectedGemCell = null;
				waitRelease = true;
			}
			else
			{
				SetSelectedGemCell(null);
				waitRelease = true;
			}
		}
	}

	void gem_TouchReleased(object sender, System.EventArgs e)
	{
		if (waitRelease)
			return;
		var gemCell = sender as GemCell;
		if (selectedGemCell == null)
			SetSelectedGemCell(gemCell);
	}

	void gem_Swiped(object sender, SwipeEventArgs e)
	{
		if (waitRelease)
			return;
		var gemCell = sender as GemCell;
		GridCell otherCell = null;
		if (e.swipedDown())
			otherCell = PuzzlePresentation.GetGridCellAtPos(gemCell.GridCell.BoardX, gemCell.GridCell.BoardY + 1);
		if (e.swipedUp())
			otherCell = PuzzlePresentation.GetGridCellAtPos(gemCell.GridCell.BoardX, gemCell.GridCell.BoardY - 1);
		if (e.swipedLeft())
			otherCell = PuzzlePresentation.GetGridCellAtPos(gemCell.GridCell.BoardX - 1, gemCell.GridCell.BoardY);
		if (e.swipedRight())
			otherCell = PuzzlePresentation.GetGridCellAtPos(gemCell.GridCell.BoardX + 1, gemCell.GridCell.BoardY);
		
		if (otherCell == null)
			return;
		var otherGemCell = otherCell.GemCell;
		if (AreGemCellsAdjacent(gemCell, otherGemCell))
		{
			gemCell.StopAllCoroutines();
			otherGemCell.StopAllCoroutines();
			RaiseSwappedEvent(gemCell, otherGemCell);
			selectedGemCell = null;
		}
		waitRelease = true;
	}

	private void RaiseSwappedEvent(GemCell cell1, GemCell cell2)
	{
		var handler = Swapped;
		if (handler != null)
		{
			handler(this, new GemSwapEventArgs(cell1.GridCell.Index, cell2.GridCell.Index, true));
		}
	}


	#endregion


	#region private check methods

	private bool AreGemCellsAdjacent(GemCell cell1, GemCell cell2)
	{
		var xDif = Math.Abs(cell1.GridCell.BoardX - cell2.GridCell.BoardX);
		var yDif = Math.Abs(cell1.GridCell.BoardY - cell2.GridCell.BoardY);
		if (xDif == 1 && yDif == 0)
			return true;
		if (xDif == 0 && yDif == 1)
			return true;
		return false;
	}


	#endregion

}
