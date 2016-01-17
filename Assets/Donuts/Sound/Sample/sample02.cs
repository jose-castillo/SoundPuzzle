using UnityEngine;
using System.Collections;

// サンプルシーン２
public class sample02 : MonoBehaviour {

	public void pushNextScene()
	{
		Application.LoadLevelAsync("sample01");
	}

  // Play BGM Button
  public void pushPlayBGM() {
    // BGM 再生　　引数：パス（Resoruces以下）、ファイル名、フェードイン、フェードイン時間
    Donuts.Sound.BGMManager.Instance.PlayBGM("BGM", "bgm_result_clear", true, 2.0f);
  }

  // BGM Stop Button
  public void pushStopBGM() {
    // BGM 停止　　備考：NextBGM が設定されている場合、停止後に再生開始
    Donuts.Sound.BGMManager.Instance.Stop();
  }

  // Play SE Button
  public void pushPlaySE() {
    // OneShot 再生　　引数：パス（Resources 以下）、ファイル名、連続再生チェック、連続再生インターバル、ボリューム
    Donuts.Sound.OneShotPlay.Instance.Play("Sound", "Buildup", true, 3.0f, 1.0f);
  }

}
