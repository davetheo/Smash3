using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GemPrefabItem
{
	public string Name;
	public GameObject Prefab;
}

public class GemPool : MonoBehaviour 
{
	public static GemPool Instance { get; private set; }

	#region Inspector Fields

	public GemConverter GemConverter;
	public List<GemPrefabItem> GemPrefabList;

	public class GemPoolItem
	{
		public int GemId;
		public List<GameObject> Gems;
	}

	#endregion

	private List<GemPoolItem> GemPoolItems;
	private bool usePool = true;
	private Vector3 offPos = new Vector3(720f, 0f, 120f);

	// Use this for initialization

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

	void Start () 
	{
		if (GemPoolItems == null)
		{
			GemPoolItems = new List<GemPoolItem>();
		}
	}

	public GemCell GetOneGemCell(int id)
	{
		Debug.LogError("Do this!");
		return null;
	}

	public void RemoveOneGem(GemCell gem)
	{
		Debug.LogError("Do this!");
	}

	public Gem GetOneGem(int id)
	{
		Gem gemObject = null;
		var poolItem = GemPoolItems.Find(pool => pool.GemId == id);
		if (usePool == true && poolItem != null && poolItem.Gems.Count > 0)
		{
			var gem = poolItem.Gems[0];
			gemObject = gem.GetComponent<Gem>();
			gem.gameObject.transform.parent = null;
			gem.gameObject.transform.localPosition = Vector3.zero;
			poolItem.Gems.RemoveAt(0);
			gem.gameObject.SetActive(true);
		}
		else
		{
			var name = GemConverter.GetGemName(id);
			var prefabItem = GemPrefabList.Find(a => a.Name.Equals(name));
			var gObject = Instantiate(prefabItem.Prefab) as GameObject;
			gemObject = gObject.GetComponent<Gem>();
		}
		gemObject.OnActivate();
		return gemObject;
	}

	public void RemoveOneGem(Gem gem)
	{
		gem.OnDeActivate();
		if (usePool == true)
		{
			var id = GemConverter.GetGemInt(gem.Id);
			var poolItem = GemPoolItems.Find(pool => pool.GemId == id);
			if (poolItem == null)
			{
				poolItem = new GemPoolItem { GemId = id, Gems = new List<GameObject>() };
				GemPoolItems.Add(poolItem);
			}
			gem.gameObject.transform.parent = transform;
			gem.gameObject.transform.localPosition = offPos;	// Vector3.zero;
			gem.gameObject.SetActive(false);
			poolItem.Gems.Add(gem.gameObject);
		}
		else
		{
			Destroy(gem.gameObject);
		}
	}

}
