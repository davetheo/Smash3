using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public static Spawner Instance { get; private set; }

	#region Inspector properties

	public string Name;
	public List<SpawnItem> SpawnList;

	#endregion

	#region private variables

	private float TotalProbability;
	private GemConverter GemConverter;

	#endregion

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


	// Use this for initialization
	void Start () 
	{
		GemConverter = GameObject.Find("GameController").GetComponent<GemConverter>();
		CalculateProbability();
	}

	public GemCell GemCellSpawn()
	{
		float value = Random.Range(0, TotalProbability);
		// get rid of the complete edge case first
		while (value == TotalProbability)
			value = Random.Range(0, TotalProbability);
		foreach (var spawn in SpawnList)
		{
			if (value < spawn.Probability)
			{
				return ConvertToGemCell(spawn);
			}
			value -= spawn.Probability;
		}
		throw new UnityException("Shouldn't get here.");
	}

	private GemCell ConvertToGemCell(SpawnItem spawn) 
	{
		int bg = GemConverter.GetGemInt(spawn.BgName);
		int gem = GemConverter.GetGemInt(spawn.GemName);
		int fg = GemConverter.GetGemInt(spawn.FgName);

		GemCell newGemCell = GemCellPool.Instance.GetOneGemCell(gem);
		newGemCell.InitGem();
		newGemCell.InitBgGem(bg);
		newGemCell.InitFgGem(fg);

		return newGemCell;
	}

	private void CalculateProbability()
	{
		TotalProbability = 0f;
		foreach (var spawn in SpawnList)
		{
			TotalProbability += spawn.Probability;
		}
	}
}
