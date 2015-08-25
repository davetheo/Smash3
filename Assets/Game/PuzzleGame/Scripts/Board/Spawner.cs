using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	#region Inspector properties

	public string Name;
	public List<SpawnItem> SpawnList;
	public bool SeparateFgSpawn = true;
	public List<FGSpawnItem> FgSpawnList;
	public bool SeparateBgSpawn = true;
	public List<BGSpawnItem> BgSpawnList;

	#endregion

	#region private variables

	private float TotalBgProbability;
	private float TotalProbability;
	private float TotalFgProbability;

	#endregion

	void Awake()
	{
	}


	// Use this for initialization
	void Start () 
	{
		CalculateBgProbability();
		CalculateProbability();
		CalculateFgProbability();
	}

	public GemCell GemCellSpawn()
	{
		var spawn = GetSpawnItem();
		spawn.BgName = BgGemSpawn();
		spawn.FgName = FgGemSpawn();
		return ConvertToGemCell(spawn);
	}

	private string BgGemSpawn()
	{
		if (BgSpawnList.Count == 0)
			return GemBGConverter.NoBgGemString;
		float value = Random.Range(0, TotalBgProbability);
		// get rid of the complete edge case first
		while (value == TotalBgProbability)
			value = Random.Range(0, TotalBgProbability);
		foreach (var spawn in BgSpawnList)
		{
			if (value < spawn.Probability)
			{
				return spawn.BgName;
			}
			value -= spawn.Probability;
		}
		return GemBGConverter.NoBgGemString;
	}

	private string FgGemSpawn()
	{
		if (FgSpawnList.Count == 0)
			return GemFGConverter.NoFgGemString;
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
		int bg = GemBGConverter.GetGemInt(spawn.BgName);
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
	private void CalculateBgProbability()
	{
		TotalBgProbability = 0f;
		foreach (var spawn in BgSpawnList)
		{
			TotalBgProbability += spawn.Probability;
		}
	}
}
