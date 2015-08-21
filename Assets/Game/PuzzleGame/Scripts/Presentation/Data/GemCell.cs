using UnityEngine;
using System.Collections;
using System;

public class GemCell : MonoBehaviour 
{
	public enum State
	{
		idle,
		dropping,
		selected,
		swapping,
		exploding
	};


	#region public variables

	public GridCell GridCell { get; set; }

	#endregion

	#region gem properties

	public GemBg GemBg { get; set; }
	public Gem Gem { get; set; }
	public GemFg GemFg { get; set; }

	#endregion


	public State cellState { get; set; }

	private Vector2 velocity;
	private float gravity;			// multiplied by time delta
	private float maxFall;

	private bool isBouncing;
	private float dropDelay;


	public int Type 
	{ 
		get
		{
			if (Gem == null)
				return GemConverter.NoGem;
			return Gem.Type;
		}
		set
		{
			throw new UnityException("Should not be setting the type here.");
		}
	}

	public event EventHandler<EventArgs> Touched;
	public event EventHandler<EventArgs> TouchReleased;
	public event EventHandler<SwipeEventArgs> Swiped;
	public event EventHandler<EventArgs> StateChanged;

	private BoxCollider2D BoxCollider;

	public bool IsBusy
	{
		get
		{
			return (cellState != State.idle && cellState != State.selected);
		}
	}

	#region Activate methods

	void Awake()
	{
		BoxCollider = GetComponent<BoxCollider2D>();
	}

	void Update()
	{
		switch (cellState)
		{
			case State.idle:
				//SetCellPos();
				break;

			case State.dropping:
				if (dropDelay > 0f)
				{
					dropDelay -= Time.deltaTime;
					return;
				}
				velocity.y += Time.deltaTime * gravity;
				if (velocity.y > maxFall)
					velocity.y = maxFall;

				Vector3 vec = gameObject.transform.localPosition;
				vec.y -= velocity.y;

				if (vec.y < GridCell.Position.y)
				{
					vec.y = GridCell.Position.y;
					if (isBouncing)
						SetStateIdle();
					else
					{
						velocity.y = -PuzzleGameConfig.GemBounceVelocity;
						isBouncing = true;
					}
				}
				gameObject.transform.localPosition = vec;
				break;

			case State.selected:
				//DoGemSelectedJiggle();
				break;
		}
	}

	public void OnActivate()
	{
	}

	public void OnDeActivate()
	{
		if (Touched != null || TouchReleased != null || Swiped != null)
			throw new UnityException("Handlers were not null on exit");
		StopAllCoroutines();
		iTween.Stop(gameObject);
	}

	private void SetCellPos()
	{
		if (GridCell == null)
			return;
		Vector3 vec = gameObject.transform.localPosition;
		vec.x = GridCell.Position.x;
		vec.y = GridCell.Position.y;
		gameObject.transform.localPosition = vec;
	}

	#endregion

	#region public gem related methods

	public void InitGem()
	{
		if (Gem == null)
		{
			Gem = GetComponentInChildren<Gem>();
			Gem.GemCell = this;
			Gem.Init();
		}
	}

	public void InitBgGem(int bgGem)
	{
		GemBg = null;
	}

	public void AddBg(GemBg gem)
	{
		if (GemBg != null)
			throw new UnityException("GemBg is not empty");
		GemBg = gem;
	}

	public void InitFgGem(int fgGem)
	{
		GemFg = null;
		if (fgGem == GemFGConverter.NoFgGem)
			return;
		GemFg = GemFgPool.Instance.GetOneGemFg(fgGem);
		GemFg.transform.SetParent(this.gameObject.transform);
	}

	public void AddFg(GemFg gem)
	{
		if (GemBg != null)
			throw new UnityException("GemFg is not empty");
		GemFg = gem;
	}

	/// <summary>
	/// Blindly clears the gem from the cell with no checking
	/// </summary>
	public void ClearGem()
	{
		if (Gem != null)
			Gem.GemCell = null;
		Gem = null;
	}

	/// <summary>
	/// Pulls a gem out of a cell and returns it
	/// </summary>
	/// <returns></returns>
	public Gem RetrieveGem()
	{
		if (Gem == null)
		{
			throw new UnityException("Trying to retrieve a gem when there is none in the cell");
		}
		Gem retGem = Gem;
		ClearGem();
		return retGem;
	}

	#endregion

	#region state methods

	public void SetStateIdle()
	{
		StopAllCoroutines();
		velocity.y = 0;
		SetCellPos();
		cellState = State.idle;

		var handler = StateChanged;
		if (handler != null)
		{
			handler(this, new EventArgs());
		}
	}

	public void SetStateSwapping()
	{
		cellState = State.swapping;

		var handler = StateChanged;
		if (handler != null)
		{
			handler(this, new EventArgs());
		}
	}


	public void SetStateSelected()
	{
		cellState = State.selected;
	}

	public void SetGemToDrop(float delay, float gravity, float maxFall)
	{
		isBouncing = false;
		if (cellState == State.dropping)
			return;
		cellState = State.dropping;
		velocity.y = 0;
		this.gravity = gravity;
		this.maxFall = maxFall;
		dropDelay = delay;
		var handler = StateChanged;
		if (handler != null)
		{
			handler(this, new EventArgs());
		}
	}


	#endregion

	#region Collision methods

	void OnMouseDown()
	{
		if (IsBusy)
		{
			return;
		}
		var handler = Touched;
		if (handler != null)
		{
			handler(this, new EventArgs());
		}
	}

	void OnMouseUp()
	{
		if (IsBusy)
		{
			return;
		}

		var handler = TouchReleased;
		if (handler != null)
		{
			handler(this, new EventArgs());
		}
	}

	void OnMouseDrag()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (BoxCollider.OverlapPoint(pos) == true)
			return;
		bool isOver = true;
		Vector2 offset = new Vector2();
		offset.x = pos.x - BoxCollider.bounds.center.x;
		offset.y = pos.y - BoxCollider.bounds.center.y;

		var handler = Swiped;
		if (handler != null)
		{
			handler(this, new SwipeEventArgs(isOver, offset));
		}
	}

	#endregion

}
