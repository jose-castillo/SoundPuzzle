using UnityEngine;
using System.Collections;

public class sampleSingleton : MonoBehaviour {

	// Use this for initialization
	void Start () {
    // 普通のシングルトン
    TestSingleton   test1 = TestSingleton.Instance;
    TestSingleton   test2 = TestSingleton.Instance;
    if(test1 == test2) Debug.Log("test1 と test2 は同じインスタンスです");
    else               Debug.Log("test1 と test2 は同じインスタンスではない");
    test1.TestLog();

    // MonoBehaviour を継承するシングルトン
    TestSingletonBehaviour  test3 = TestSingletonBehaviour.Instance;
    TestSingletonBehaviour  test4 = TestSingletonBehaviour.Instance;
    if(test3 == test4) Debug.Log("test3 と test4 は同じインスタンスです");
    else               Debug.Log("test3 と test4 は同じインスタンスではない");
    test3.TestLog();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
