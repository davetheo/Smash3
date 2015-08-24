using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour 
{
	public static SpawnController Instance { get; private set; }

	public Spawner CurrentSpawner;

	public string CurrentSpawnerName {get; set; }

	//private 
	// Use this for initialization
	void Awake () 
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		// Furthermore we make sure that we don't destroy between scenes (this is optional)
		//DontDestroyOnLoad(gameObject);
		CurrentSpawnerName = CurrentSpawner.Name;
	}
	


	public bool ChangeSpawner(string spawnerName)
	{
		var blah = GetComponentsInChildren<Spawner>();
		for (int i = 0; i < blah.Length; i++)
		{
			if (blah[i].Name == spawnerName)
			{
				CurrentSpawner = blah[i];
				CurrentSpawnerName = CurrentSpawner.Name;
				return true;
			}
		}
		return false;
	}
}
