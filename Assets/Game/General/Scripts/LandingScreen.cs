using UnityEngine;
using System.Collections;

public class LandingScreen : MonoBehaviour 
{

	public GameObject LoadingObject;
	public GameObject ConnectObject;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickStart()
	{
		SceneManager.Instance.ChangeScene(SceneManager.Scenes.MainMenu);
	}
}
