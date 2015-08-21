using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public class GemConverter : MonoBehaviour 
{
	public static GemConverter Instance { get; private set; }

	public static int NoGem = -1;
	public static string NoGemString = "NoGem";

	public List<string> gemTypeList;

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

	public static int GetGemInt(string name)
	{
		if (Instance.gemTypeList.Contains(name))
			return Instance.gemTypeList.IndexOf(name);
		return NoGem;
	}

	public static string GetGemName(int id)
	{
		if (id == NoGem)
			return NoGemString;
		return Instance.gemTypeList[id];
	}

}
