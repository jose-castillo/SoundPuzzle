using UnityEngine;
using System.Collections;
using Donuts.Common;

public class sampleFade : MonoBehaviour {

  FadeManager   fm;
  private float progress = 0f;

	// Use this for initialization
	void Start () {
    fm = FadeManager.Create();
	}

  void Update() {
    if(Input.GetMouseButtonDown(0)) {
      switch(fm.stat) {
        case FadeManager.FadeStat.FadeIn:
          FadeManager.Instance.FadeStart(FadeManager.FadeStat.FadeOut);
          fm.ShowProgress(true);
          break;
        case FadeManager.FadeStat.FadeOut:
          FadeManager.Instance.FadeStart(FadeManager.FadeStat.FadeIn);
          fm.ShowProgress(false);
          break;
      }
    }

    progress += 0.0001f;
    if(progress >= 1f) progress = 0f;
    fm.SetProgress(progress);
  }	

}
