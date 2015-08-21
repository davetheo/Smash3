using UnityEngine;
using System.Collections;

public class PuzzleGameConfig : MonoBehaviour 
{
	public static PuzzleGameConfig Instance { get; private set; }

	#region Inspector properties

	[Space(6)]
	[Header("Gem variables")]
	public float GemDropWaitI = 0.1f;
	public static float GemDropWait
	{
		get { return Instance.GemDropWaitI; }
		set { Instance.GemDropWaitI = value; }
	}

	public float GemDropGravityI = 48.0f;
	public static float GemDropGravity
	{
		get { return Instance.GemDropGravityI; }
		set { Instance.GemDropGravityI = value; }
	}

	public float GemMaximumFallI = 256f;
	public static float GemMaximumFall
	{
		get { return Instance.GemMaximumFallI; }
		set { Instance.GemMaximumFallI = value; }
	}

	//[Range(0f, 10.0f)]
	public float GemBounceVelocityI = 3.6f;
	public static float GemBounceVelocity
	{
		get { return Instance.GemBounceVelocityI; }
		set { Instance.GemBounceVelocityI = value; }
	}

	public float GemSwapTimeI = 0.25f;
	public static float GemSwapTime
	{
		get { return Instance.GemSwapTimeI; }
		set { Instance.GemSwapTimeI = value; }
	}

	#endregion



	void Awake()
	{
		// First we check if there are any other instances conflicting
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		Instance = this;
		// Furthermore we make sure that we don't destroy between scenes (this is optional)
		DontDestroyOnLoad(gameObject);
	}


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}


}
