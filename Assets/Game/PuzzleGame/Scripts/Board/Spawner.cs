using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public static Spawner Instance { get; private set; }

	#region Inspector properties

	public string Name;
	public List<SpawnItem> SpawnList;
	public bool SeparateFgSpawn = true;
	public List<FGSpawnItem> FgSpawnList;

	#endregion

	#region private variables

	private float TotalProbability;
	private float TotalFgProbability;
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
		CalculateFgProbability();
	}

	public GemCell GemCellSpawn()
	{
		var spawn = GetSpawnItem();
		spawn.FgName = FgGemSpawn();
		return ConvertToGemCell(spawn);
	}

	private string FgGemSpawn()
	{
		float value = Random.Range(0, TotalFgProbability);
		// get rid of the complete edge case first
		while (value == TotalFgProbability)
			value = Random.Range(0, TotalFgProbability);
		foreach (var spawn in FgSpawnList)
		{
			if (value < spawn.Probability)
			{
				return spawn.FgName;
			}
			value -= spawn.Probability;
		}
		return GemFGConverter.NoFgGemString;
	}

	private GemCell ConvertToGemCell(SpawnItem spawn) 
	{
		int bg = GemConverter.GetGemInt(spawn.BgName);
		int gem = GemConverter.GetGemInt(spawn.GemName);
		int fg = GemFGConverter.GetGemInt(spawn.FgName);

		GemCell newGemCell = GemCellPool.Instance.GetOneGemCell(gem);
		newGemCell.InitGem();
		newGemCell.InitBgGem(bg);
		newGemCell.InitFgGem(fg);

		return newGemCell;
	}



	private SpawnItem GetSpawnItem()
	{
		float value = Random.Range(0, TotalProbability);
		// get rid of the complete edge case first
		while (value == TotalProbability)
			value = Random.Range(0, TotalProbability);
		foreach (var spawn in SpawnList)
		{
			if (value < spawn.Probability)
			{
				return spawn;
			}
			value -= spawn.Probability;
		}
		Debug.LogError("Shouldn't get here....");
		return null;
	}


	private void CalculateProbability()
	{
		TotalProbability = 0f;
		foreach (var spawn in SpawnList)
		{
			TotalProbability += spawn.Probability;
		}
	}
	private void CalculateFgProbability()
	{
		TotalFgProbability = 0f;
		foreach (var spawn in FgSpawnList)
		{
			TotalFgProbability += spawn.Probability;
		}
	}
}
