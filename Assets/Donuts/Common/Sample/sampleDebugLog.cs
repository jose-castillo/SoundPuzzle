using UnityEngine;
using System.Collections;

public class sampleDebugLog : MonoBehaviour {

	// Use this for initialization
	void Start () {
    StartCoroutine(logMessage());	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


  private IEnumerator logMessage() {
    int cnt = 0;
    while(true) {
      Debug.Log("Log:" + cnt);
      cnt++;
      yield return new WaitForSeconds(1);
    }
  }

}
