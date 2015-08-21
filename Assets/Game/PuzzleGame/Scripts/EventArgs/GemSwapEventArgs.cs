using UnityEngine;
using System.Collections;
using System;

public class GemSwapEventArgs : EventArgs {

	public int indexA { get; set; }
	public int indexB { get; set; }
	public bool isValid { get; set; }

	public GemSwapEventArgs(int indexA, int indexB, bool isValid)
	{
		this.indexA = indexA;
		this.indexB = indexB;
		this.isValid = isValid;
	}

}
