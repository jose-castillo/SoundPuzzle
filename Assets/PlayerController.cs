using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Donuts.Common;

public class PlayerController : SingletonMonoBehaviour<PlayerController> {

	public Button[] tones;
	public Text     waitForPc;

	private List<string> sequence;
	// Use this for initialization
	void Start () {
		sequence= new List<string>();
	}

	public void Clicked(int buttonNumber){
		Debug.Log("[Player] Pressed Tone: " + buttonNumber + " Button: " + ( buttonNumber - 1 ));
		Donuts.Sound.EasyPlay.CreateAndPlay("Sounds", buttonNumber.ToString());
		sequence.Add(buttonNumber.ToString ());
	}

	public void DisableButtons(){
		waitForPc.gameObject.SetActive(true);
		foreach(var btn in tones){
			btn.gameObject.SetActive(false);
		}
	}

	public void EnableButtons(){
		waitForPc.gameObject.SetActive(false);
		foreach(var btn in tones){
			btn.gameObject.SetActive(true);
		}
	}

	public List<string> GetPlayerSequence(){
		return sequence;
	}

	public void RestartPlayerSequence(){
		sequence.Clear();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
