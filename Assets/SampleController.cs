using UnityEngine;
using System.Collections;
using Donuts.Common;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class SampleController : SingletonMonoBehaviour<SampleController> {

	public Button[] tones;
	private static readonly System.Random random = new System.Random();
	public bool isReady = false;
	bool isShowingTone = false;

	Color redDark    = new Color(0.63f, 0.03f, 0.03f, 0.59f);
	Color greenDark  = new Color(0.03f, 0.63f, 0.03f, 0.59f);
	Color blueDark   = new Color(0.03f, 0.03f, 0.63f, 0.59f);
	Color yellowDark = new Color(0.63f, 0.63f,    0f, 0.59f);
	Color purpleDark = new Color(0.63f,    0f, 0.63f, 0.59f);

	private List<string> sequence;
	// Use this for initialization
	void Start () {
		sequence= new List<string>();
		int i = 1;
		foreach(var btn in tones){
			btn.image.color = ChangeActiveButtonColor(i, false);
			i++;
		}
	}

	public List<string> GenerateTones(int numberOfTones){
		sequence.Clear();
		StartCoroutine(ShowTones(numberOfTones));
		return sequence;
	}

	IEnumerator ShowTones(int numberOfTones){
		for (int i = 0; i < numberOfTones; i++){
			isShowingTone = false;
			int tone = random.Next(1, 6);
			Debug.Log("Tone "+tone+" button "+(tone-1));
			
			StartCoroutine(ShowClick(tone));
			while(!isShowingTone){
				yield return null;
			}
		}
		isReady = true;
	}

	IEnumerator ShowClick(int tone){
		PlayTone(tone);
		tones[tone - 1].image.color = ChangeActiveButtonColor(tone, true); // true means changes to bright color
		yield return new WaitForSeconds(2);
		tones[tone - 1].image.color = ChangeActiveButtonColor(tone, false); // true means changes to bright color
		sequence.Add(tone.ToString());
		isShowingTone = true;
	}

	Color ChangeActiveButtonColor(int tone, bool isActive){
		Color btnColor = Color.white;
		switch(tone){
		case 1:
			btnColor = isActive ? Color.red     : redDark;
			break;
		case 2:
			btnColor = isActive ? Color.magenta : purpleDark;
			break;
		case 3:
			btnColor = isActive ? Color.blue    : blueDark;
			break;
		case 4:
			btnColor = isActive ? Color.green   : greenDark;
			break;
		case 5:
			btnColor = isActive ? Color.yellow  : yellowDark;
			break;
		}
		return btnColor;
	}

	void PlayTone(int toneNumber){
		Donuts.Sound.EasyPlay.CreateAndPlay("Sounds", toneNumber.ToString());
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
