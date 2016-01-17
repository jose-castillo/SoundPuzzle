using UnityEngine;
using System.Collections;

public class sampleTransformExtension : MonoBehaviour {

  public Transform  cube;
  public Transform  plate;

  // Lerp 検証用
  public Transform  tgtMarker;
  private bool      isLerp = false;

  // SmoothStep 検証用
  private bool      isSmoothStep = false;


  // SetPosition
  public void BtnSetPosition() {
    cube.SetPosition(-5, 5, 0);
  }

  // SetScale
  public void BtnSetScale() {
    cube.SetLocalScale(2);
  }

  // SetEulerAngles
  public void BtnSetAngles() {
    cube.SetEulerAnglesY(45);
  }

  // Lerp
  public void BtnLerp() {
    if(!isLerp) {
      StartCoroutine(LerpUpdate());
      isLerp = true;
    }
  }

  // SmoothStep
  public void BtnSmoothStep() {
    if(!isSmoothStep) {
      StartCoroutine(SmoothStepUpdate());
      isSmoothStep = true;
    }
  }

  // Clamp
  public void BtnClamp() {
    cube.ClampPositionX(-1, 1);
  }

  // HasChanged
  public void BtnHasChanged() {
    cube.HasChanged( () => Debug.Log("Changed!!") );
  }

  // LookAt2D
  public void BtnLookAt2D() {
    plate.LookAt2D(cube);
  }

  // リセット
  public void BtnReset() {
    cube.Reset();

    if(isLerp) {
      isLerp = false;
    }
    if(isSmoothStep) {
      isSmoothStep = false;
    }
  }

  // Lerp Update 処理
  private IEnumerator LerpUpdate() {
    Debug.Log("Lerp Update Start");
    while(true) {
      cube.Lerp(tgtMarker, 0.2f);
      yield return null;

      // 終点
      if(cube.position == tgtMarker.position) break;

      // 中断
      if(!isLerp) break;
    }
    isLerp = false;
    Debug.Log("Lerp Update Finish");
  }

  // SmoothStep Update 処理
  private IEnumerator SmoothStepUpdate() {
    Debug.Log("SmoothStep Update Start");
    while(true) {
      cube.SmoothStep(tgtMarker, 0.2f);
      yield return null;

      // 終点
      if(cube.position == tgtMarker.position) break;

      // 中断
      if(!isSmoothStep) break;
    }
    isSmoothStep = false;
    Debug.Log("SmoothStep Update Finish");
  }

}
