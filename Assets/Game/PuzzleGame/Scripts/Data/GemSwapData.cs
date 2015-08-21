using UnityEngine;
using System.Collections;

public class GemSwapData
{

	public int indexA { get; set; }
	public int indexB { get; set; }
	public bool isValid { get; set; }


	public GemSwapData(int indexA, int indexB, bool isValid)
	{
		this.indexA = indexA;
		this.indexB = indexB;
		this.isValid = isValid;
	}

}
