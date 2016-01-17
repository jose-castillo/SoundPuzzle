using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// BGM マネージャー（サンプル）
public class sampleBGMManager : MonoBehaviour {

	public Text 	isOnTimePlayingStatLabel;
	public Toggle fadeInToggle;
	private bool  swFadeIn;

	// Face IN Toggle
	public void toggleFadeIn() {
		// Debug.Log("toggle:" + fadeInToggle.isOn);
		swFadeIn = fadeInToggle.isOn;
	}

	// Play BGM 1 Button
	public void pushPlayBGM1() {
		float fadeSec = 0.0f;
		if (swFadeIn) {
			fadeSec = 3.0f;
		}
		// BGM 再生　　引数：パス（Resoruces以下）、ファイル名、フェードイン、フェードイン時間
		Donuts.Sound.BGMManager.Instance.PlayBGM("BGM", "Result", swFadeIn, fadeSec);
	}

	// Play BGM 2 Button
	public void pushPlayBGM2() {
		float fadeSec = 0.0f;
		if (swFadeIn) {
			fadeSec = 3.0f;
		}
		// BGM 再生　　引数：パス（Resoruces以下）、ファイル名、フェードイン、フェードイン時間		
		Donuts.Sound.BGMManager.Instance.PlayBGM("BGM", "bgm_result_clear", swFadeIn, fadeSec);
	}

	// Set Next 1 Button
	public void pushNext1() {
		// 次 BGM 設定　　引数：パス（Resoruces以下）、ファイル名、フェードイン、フェードイン時間		
		Donuts.Sound.BGMManager.Instance.SetNextBgmName("BGM", "Result", 3.0f);
	}

	// Set Next 2 Button
	public void pushNext2() {
		// 次 BGM 設定　　引数：パス（Resoruces以下）、ファイル名、フェードイン、フェードイン時間		
		Donuts.Sound.BGMManager.Instance.SetNextBgmName("BGM", "bgm_result_clear", 3.0f);
	}

	// BGM Stop Button
	public void pushStop() {
		// BGM 停止　　備考：NextBGM が設定されている場合、停止後に再生開始
		Donuts.Sound.BGMManager.Instance.Stop();
	}

	// BGM Fade OUT Button
	public void pushFadeOutBGM() {
		// BGM フェードアウト　　引数：フェードアウト時間　　備考：NextBGM が設定されている場合、フェードアウト後に再生開始
		Donuts.Sound.BGMManager.Instance.StartFadeOut(2.0f);
	}

	void Update() {
		// ミュージックプレイヤーの状態（iOS のみ）
		if (isOnTimePlayingStatLabel != null) {
			bool isOnTimePlaying = Donuts.Sound.Manager.Instance.IsOnTimePlaying();
			if (isOnTimePlaying) {
				isOnTimePlayingStatLabel.text = "isOnTimePlaying:TRUE";
			} else {
				isOnTimePlayingStatLabel.text = "isOnTimePlaying:FALSE";
			}
		}				
	}	

}
