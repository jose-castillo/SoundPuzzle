using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Donuts.Common;

/**************************************************************************************************/
/*!
 *    \file    sampleUtility
 *             Utility のサンプル
 *    \date    2014.12.22(Mon)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
public class sampleUtility : MonoBehaviour {
		
  void Start () {
    Debug.Log("IsSmartPhone=" + Utility.IsSmartPhone());
    Dictionary<string, string> deviceDict = Utility.GetDeviceInfoToDictionary();
    Debug.Log("DeviceInfoDict\n");
    foreach (KeyValuePair<string, string> kvp in deviceDict) {
      Debug.Log("    key="+kvp.Key+" value="+kvp.Value);
    }
  }			
}

/********************************************** End of File *********************************************/
