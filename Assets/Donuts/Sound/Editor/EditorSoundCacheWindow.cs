using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

/**************************************************************************************************/
/*!
 *    \file    EditorSoundCacheWindow.cs
 *             AudioClip キャッシュウィンドウ
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

namespace Donuts
{
	namespace Sound
	{

    public class EditorSoundCacheWindow : EditorWindow
    {

			Vector2 positionScroll;

			void OnGUI() {
		    List<Donuts.Sound.AudioClipCache.Info> list = Donuts.Sound.AudioClipCache.GetList();

				GUIStyle style = new GUIStyle();
				GUIStyleState stylestate = new GUIStyleState();
				stylestate.textColor = Color.white;
				style.fontSize = 20;
				style.normal = stylestate;
				GUILayout.Label("AudioClipCacheList", style);

				if (list == null) {
					GUILayout.Label("List is null");
				}
				else {
					GUILayout.Label("ListCount:"+list.Count+" Limit:"+Donuts.Sound.AudioClipCache.GetLimitNum());

        	positionScroll = GUILayout.BeginScrollView(positionScroll, GUILayout.Height(500));
        	GUILayout.BeginVertical();

					for (int i=0; i<list.Count; i++) {
						GUILayout.Label(list[i].name);
					}

    	    GUILayout.EndVertical();
	        GUILayout.EndScrollView();
				}
			}
		}


	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
