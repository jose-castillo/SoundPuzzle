using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

/**************************************************************************************************/
/*!
 *    \file    EditorSoundStatus.cs
 *             ステータス表示
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

namespace Donuts
{
	namespace Sound
	{

    public class EditorSoundStatus : EditorWindow
    {

      void OnGUI() {
		    Donuts.Sound.Status.IsPause   		= EditorGUILayout.Toggle(    "Pause",   			Donuts.Sound.Status.IsPause);
		    Donuts.Sound.Status.IsMuteBGM 		= EditorGUILayout.Toggle(    "MuteBGM", 			Donuts.Sound.Status.IsMuteBGM);
		    Donuts.Sound.Status.IsMuteSE  		= EditorGUILayout.Toggle(    "MuteSE",  			Donuts.Sound.Status.IsMuteSE);
				Donuts.Sound.Status.BaseVolumeBGM = EditorGUILayout.FloatField("BaseVolumeBGM", Donuts.Sound.Status.BaseVolumeBGM);
				Donuts.Sound.Status.BaseVolumeSE 	= EditorGUILayout.FloatField("BaseVolumeSE",  Donuts.Sound.Status.BaseVolumeSE);		    
	    }
    }


	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
