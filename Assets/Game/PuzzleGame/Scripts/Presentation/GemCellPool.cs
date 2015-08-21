using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GemCellPrefabItem
{
	public string Name;
	public GameObject Prefab;
}

public class GemCellPool : MonoBehaviour 
{
	public static GemCellPool Instance { get; private set; }

	public List<GemCellPrefabItem> PrefabList;

	public class GemCellPoolItem
	{
		public int Id;
		public List<GameObject> GemCells;
	}

	private List<GemCellPoolItem> GemCellPoolItems;

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
		if (GemCellPoolItems == null)
		{
			GemCellPoolItems = new List<GemCellPoolItem>();
		}
	
	}

	public GemCell GetOneGemCell(int id)
	{
		GemCell gemCellObject = null;
		var poolItem = GemCellPoolItems.Find(pool => pool.Id == id);
		if (poolItem != null && poolItem.GemCells.Count > 0)
		{
			var gemCell = poolItem.GemCells[0];
			gemCellObject = gemCell.GetComponent<GemCell>();
			gemCellObject.gameObject.transform.parent = null;
			gemCellObject.gameObject.transform.localPosition = Vector3.zero;
			poolItem.GemCells.RemoveAt(0);
			gemCellObject.gameObject.SetActive(true);
		}
		else
		{
			var name = GemConverter.GetGemName(id);
			var prefabItem = PrefabList.Find(a => a.Name.Equals(name));
			var gObject = Instantiate(prefabItem.Prefab) as GameObject;
			gemCellObject = gObject.GetComponent<GemCell>();
			gemCellObject.gameObject.transform.parent = null;
			gemCellObject.gameObject.transform.localPosition = Vector3.zero;
		}
		gemCellObject.OnActivate();
		return gemCellObject;
	}

	public void RemoveOneGemCell(GemCell gemCell)
	{
		gemCell.OnDeActivate();
		var id = gemCell.Type;
		var poolItem = GemCellPoolItems.Find(pool => pool.Id == id);
		if (poolItem == null)
		{
			poolItem = new GemCellPoolItem { Id = id, GemCells = new List<GameObject>() };
			GemCellPoolItems.Add(poolItem);
		}
		gemCell.gameObject.transform.parent = transform;
		gemCell.gameObject.transform.localPosition = Vector3.zero;
		gemCell.gameObject.SetActive(false);
		poolItem.GemCells.Add(gemCell.gameObject);
	}
}
