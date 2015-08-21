using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class SmashPlayFabManager : MonoBehaviour 
{
	public static SmashPlayFabManager Instance { get; private set; }

	/// <summary>
	/// All the game news' reference.
	/// </summary>
	//public List<TitleNewsItem> gameNews { get; private set; }

	/// <summary>
	/// Callback to be called when PlayFab login process completes.
	/// </summary>
	//ProjectDelegates.PlayFabLoginCallback OnLoginCompletedCallback;

	/// <summary>
	/// Game ID given when the developer creates a new PlayFab game.
	/// </summary>
	[SerializeField]
	string playFabGameID;

	public string AppId = "40ada087-34dd-460d-899f-6014b051ce33";

	/// <summary>
	/// Reference of all the title data, stored in PlayFab setting.
	/// </summary>
	public Dictionary<string, string> titleData { get; private set; }

	/// <summary>
	/// User's unique PlayFab ID.
	/// </summary>
	string playerID;

	public string deviceId = "a931f26995380c5a";

	/// <summary>
	/// User's unique PlayFab username.
	/// </summary>
	public string playerUsername { get; private set; }

	/// <summary>
	/// User's display name (facebook name).
	/// </summary>
	public string playerDisplayName { get; private set; }

	/// <summary>
	/// User's facebook picture URL.
	/// </summary>
	string playerPictureURL;

	/// <summary>
	/// Callback to be called when getting leaderboards process completes.
	/// </summary>
	//ProjectDelegates.PlayFabLeaderboardCallback OnLeaderboardLoadedCallback;

	/// <summary>
	/// Callback to be called when getting store catalog process completes.
	/// </summary>
	//ProjectDelegates.PlayFabCatalogListCallback OnCatalogLoadedCallback;

	/// <summary>
	/// Callback to be called when buying item with virtual currency process completes.
	/// </summary>
	//ProjectDelegates.PlayFabItemBuyCallback OnBuySuccessCallback;

	/// <summary>
	/// Store's catalog items.
	/// </summary>
	//List<CatalogItem> catalogItems;

	/// <summary>
	/// Number to be added to the player's name, if there's another player with the same name.
	/// </summary>
	int userNameIndex;


	// Variables set by callbacks return data
	private string sessionTicket;
	private string playFabLoginId;


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
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Initialise()
	{
		PlayFabSettings.TitleId = playFabGameID;

	}



	public void LoginWithAndroidDeviceID()
	{
		Debug.Log("LoginWithAndroidDeviceID : ");
		LoginWithAndroidDeviceIDRequest request = new LoginWithAndroidDeviceIDRequest();
		request.AndroidDeviceId = deviceId;
		request.TitleId = playFabGameID;
		request.CreateAccount = true;
		PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginResult, OnLoginError);
	}

	void OnLoginResult(LoginResult result)
	{
		Debug.Log("Login result session ticket : " + result.SessionTicket);
		sessionTicket = result.SessionTicket;
		playFabLoginId = result.PlayFabId;

	//	GetUserStats();
	//	GetPhotonAuthenticationToken();

	//	if (loginFinished != null)
	//		loginFinished(true, result.SessionTicket);
	}

	void OnLoginError(PlayFabError error)
	{
		Debug.Log("Login error : " + error.ErrorMessage);

	//	if (loginFinished != null)
	//		loginFinished(false, error.ErrorMessage);
	}



}
