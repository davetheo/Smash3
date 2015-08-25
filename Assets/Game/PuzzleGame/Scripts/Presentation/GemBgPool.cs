using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemBgPool : MonoBehaviour 
{
	public static GemBgPool Instance { get; private set; }

	#region Inspector Fields

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

	void Start()
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

	public GemBg GetOneGemBg(int id)
	{
		GemBg gemObject = null;
		var poolItem = GemPoolItems.Find(pool => pool.GemId == id);
		if (usePool == true && poolItem != null && poolItem.Gems.Count > 0)
		{
			var gem = poolItem.Gems[0];
			gemObject = gem.GetComponent<GemBg>();
			gem.gameObject.transform.parent = null;
			gem.gameObject.transform.localPosition = Vector3.zero;
			poolItem.Gems.RemoveAt(0);
			gem.gameObject.SetActive(true);
		}
		else
		{
			var name = GemBGConverter.GetGemName(id);
			var prefabItem = GemPrefabList.Find(a => a.Name.Equals(name));
			var gObject = Instantiate(prefabItem.Prefab) as GameObject;
			gemObject = gObject.GetComponent<GemBg>();
		}
		gemObject.OnActivate();
		return gemObject;
	}

	public void RemoveOneGemBg(GemBg gem)
	{
		gem.OnDeActivate();
		if (usePool == true)
		{
			var id = GemBGConverter.GetGemInt(gem.Id);
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
