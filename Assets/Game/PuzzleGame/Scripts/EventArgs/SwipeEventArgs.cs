using UnityEngine;
using System.Collections;
using System;

public class SwipeEventArgs : EventArgs 
{

	public SwipeEventArgs(bool isOver, Vector2 offset)
    {
		this.isOver = isOver;
		this.offset = offset;
    }

	public bool isOver { get; set; }
	public Vector2 offset { get; set; }

	public bool swipedRight()
	{
		return offset.x > 0 && offset.x > Math.Abs(offset.y);
	}

	public bool swipedLeft()
	{
		return offset.x < 0 && Math.Abs(offset.x) > Math.Abs(offset.y);
	}

	public bool swipedUp()
	{
		return offset.y > 0 && offset.y > Math.Abs(offset.x);
	}

	public bool swipedDown()
	{
		return offset.y < 0 && Math.Abs(offset.y) > Math.Abs(offset.x);
	}
}
