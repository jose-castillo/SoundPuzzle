using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/**************************************************************************************************/
/*!
 *    \file    InspectorExtraOneShotPlay.cs
 *             Donuts.Sound.OneShotPlay の Inspector拡張
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

namespace Donuts
{
	namespace Sound
	{
    [CustomEditor( typeof(OneShotPlay) )]
    public class InspectorExtraOneShotPlay : Editor
    {
	    public override void OnInspectorGUI()
	    {
		    OneShotPlay myRef = target as OneShotPlay;
		    if (myRef != null) {
			    // 再生履歴生存リストの表示
			    List<OneShotPlay.PlayInfo> pList = myRef.GetPlayInfoList();
			    if (pList != null) {
				    GUILayout.Label("ListCount : "+pList.Count);
				    EditorGUI.indentLevel++;
				    for (int i=0; i<pList.Count; i++) {
					    EditorGUILayout.LabelField(pList[i].name, pList[i].livingTime.ToString());
				    }
				    EditorGUI.indentLevel--;
			    }
		    }
		    // デフォルト表示
		    DrawDefaultInspector();
	    }
    }

	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
