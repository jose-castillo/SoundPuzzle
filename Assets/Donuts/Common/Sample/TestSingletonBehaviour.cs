using UnityEngine;
using System.Collections;

public class TestSingletonBehaviour : Donuts.Common.SingletonMonoBehaviour<TestSingletonBehaviour> {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void TestLog() {
    Debug.Log(Donuts.Common.Random.GetFloat());
  }

}
