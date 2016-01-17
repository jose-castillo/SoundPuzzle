using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sampleOneShotPlay : MonoBehaviour {

	public AudioClip 		selectClip;
	public Toggle    		sameToggle;
	public ToggleGroup 	myToggleGroup;

	private bool swSameCheck;

	// Same Check Toggle
	public void toggleSameCheck() {
		swSameCheck = sameToggle.isOn;
	}

	// Play SE Button （SE List と連動）
	public void pushPlayAudioClipList() {
		Text 	txt = myToggleGroup.GetActiveText();	// グループ内から選択された Toggle 下の Label.Text を取得
		if(txt != null) {
			//Debug.Log(txt.text);			
		  // OneShot 再生　　引数：パス（Resources 以下）、ファイル名、連続再生チェック、連続再生インターバル、ボリューム
			Donuts.Sound.OneShotPlay.Instance.Play("Sound", txt.text, swSameCheck, 3.0f, 1.0f);
		}
	}

	// Play AudioClip Button
	public void pushPlayAudioClip() {
		// OneShot 再生　　引数：オーディオクリップ、連続再生チェック、連続再生インターバル、ボリューム
		Donuts.Sound.OneShotPlay.Instance.Play(selectClip, swSameCheck, 3.0f, 1.0f);
	}


}
