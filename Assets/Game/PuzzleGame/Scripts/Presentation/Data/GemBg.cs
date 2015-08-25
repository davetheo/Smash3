using UnityEngine;
using System.Collections;

public class GemBg : MonoBehaviour 
{

	public string Id;
	public int Type { get; set; }

	public GemCell GemCell { get; set; }

	public void Init()
	{
		Type = GemBGConverter.GetGemInt(Id);
	}


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnActivate()
	{
	}

	public void OnDeActivate()
	{
	}

}
