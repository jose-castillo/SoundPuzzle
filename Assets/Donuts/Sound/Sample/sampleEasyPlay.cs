using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sampleEasyPlay : MonoBehaviour {

	public AudioSource SEObject;

	public Toggle 			loopToggle;
	public Toggle 			fadeInToggle;
	public ToggleGroup 	myToggleGroup;

	private bool swLoop;
	private bool swFadeIn;

	Donuts.Sound.EasyPlay handle;

	// Fade IN Toggle
	public void toggleFadeIn() {
		swFadeIn = fadeInToggle.isOn;
	}

	// Loop Toggle
	public void toggleLoop() {
		swLoop = loopToggle.isOn;
	}

	// Play Object Button
	public void pushPlayObj() {
		if (SEObject == null) return;
		float fadeSec = 0.0f;
		if (swFadeIn) {
			fadeSec = 3.0f;
		}
		Donuts.Sound.EasyPlay.PlayObject(SEObject, swLoop, swFadeIn, fadeSec);		
	}

	// Play SE Button
	public void pushPlay() {
		Text 	txt = myToggleGroup.GetActiveText();	// グループ内から選択された Toggle 下の Label.Text を取得
		if(txt != null) {
			//Debug.Log(txt.text);

		  float fadeSec = 0.0f;
		  if (swFadeIn) {
			  fadeSec = 3.0f;
		  }
			Donuts.Sound.EasyPlay.CreateAndPlay("Sound", txt.text, swLoop, swFadeIn, fadeSec);
		}
	}

	// Stop All Button
	public void pushStopAll() {
		Donuts.Sound.EasyPlay.StopAll();
	}

	// BGM Fade OUT Button
	public void pushFadeOutBGM() {
		if (handle != null) {
			Debug.Log("pushFadeOutBGM");
			handle.StartFadeOut(3.0f, fadeOutCallback);
		}
	}

	// BGM Fade OUT の コールバック
	public void fadeOutCallback(Donuts.Sound.EasyPlay handel) {
		Debug.Log("SAMPLE:FadeOut Callback");
	}

	// Play BGM Button
	public void pushPlayBGM() {
		float fadeSec = 0.0f;
		if (swFadeIn) {
			fadeSec = 3.0f;
		}
		handle = Donuts.Sound.EasyPlay.CreateAndBGMPlay("BGM", "bgm_result_clear", swLoop, swFadeIn, fadeSec, 0.0f);
	}

	// Clear Cache Button
  public void pushClearCache() {
		Donuts.Sound.AudioClipCache.AllClear();
  }

  // ゲームオブジェクト破棄時にコール
	void OnApplicationQuit() {
		Debug.Log("Cache clear!");
		pushClearCache();
	}

}
