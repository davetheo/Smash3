using UnityEngine;
using System.Collections;
using System;

public class Gem : MonoBehaviour 
{
	public string Id;
	public bool AllowBG = true;
	public bool AllowFG = true;
	public int boardLimit = 0;
	public int Type { get; set; }

	/// <summary>
	/// The GridCell this gem is in if any
	/// </summary>
	public GemCell GemCell { get; set; }

	#region Private variables

	private float velocityY;
	private float gravity;			// multiplied by time delta
	private float maxFall;

	#endregion


	// Use this for initialization
	public void Init() 
	{
		Type = GemConverter.GetGemInt(Id);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	public void OnActivate()
	{
	}

	public void OnDeActivate()
	{
	}




}
