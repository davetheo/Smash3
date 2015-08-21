using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GemFGConverter : MonoBehaviour 
{
	public static GemFGConverter Instance { get; private set; }

	public static int NoFgGem = -1;
	public static string NoFgGemString = "NoFgGem";

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
		return NoFgGem;
	}

	public static string GetGemName(int id)
	{
		if (id == NoFgGem)
			return NoFgGemString;
		return Instance.gemTypeList[id];
	}
}
