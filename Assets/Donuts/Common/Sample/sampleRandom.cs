using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**************************************************************************************************/
/*!
 *    \file    sampleRandom.cs
 *             ランダムのサンプル
 *    \date    2014.12.22(Mon)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
public class sampleRandom : MonoBehaviour {
		
  void Start () {
    const int RndWidth = 10;
    Donuts.Common.Random.SetAutoSeed();
    Donuts.Common.OnceChoiceRandom once = new Donuts.Common.OnceChoiceRandom(0, RndWidth);
    for(int i=0 ; i<RndWidth*2 ; i++) {
      Debug.Log("[" + i + "]:" + once.Choice());
    }    
  }
			
}

/********************************************** End of File *********************************************/
