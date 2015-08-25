using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GemBGConverter : MonoBehaviour
{
	public static GemBGConverter Instance { get; private set; }

	public static int NoBgGem = -1;
	public static string NoBgGemString = "NoBgGem";

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
		return NoBgGem;
	}

	public static string GetGemName(int id)
	{
		if (id == NoBgGem)
			return NoBgGemString;
		return Instance.gemTypeList[id];
	}
}