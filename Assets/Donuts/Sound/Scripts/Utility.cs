// デファインあるとデバッグログ出します
// #define SOUND_USE_DEBUGLOG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**************************************************************************************************/
/*!
 *    \file    Utillity.cs
 *             サウンド関係全体でのユーティリティー
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
namespace Donuts
{
	namespace Sound
	{
    public class Utility
    {

      /**************************************************************************************************/
      /*!
       *    \fn      public static string MakeFullname(string path, string name)
       *             パス名とファイル名からフルネームを作成する
       *    \param   path : パス
       *    \param   name : ファイル名
       *    \return  フルネーム（エラー時は null）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static string MakeFullname(string path, string name) {
				if ((name == null) || (name.Length <= 0)) {
					return null;
				}
				if (path == null) {
					path = "";
				}
				if ((path[path.Length-1] != '/') && (name[0] != '/')) {
					path = path + "/";
				}
				return (path + name);
			}
	
      /**************************************************************************************************/
      /*!
       *    \fn      public static void debPrintError(string str)
       *             エラーログの出力
       *    \param   str : ログメッセージ 
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static void debPrintError(string str) {
				Debug.LogError(str);	
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static void debPrint(string str)
       *             デバッグログの出力
       *    \remarks SOUND_USE_DEBUGLOG がデファインされている場合ログ出力を行う
       *    \param   str : ログメッセージ 
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static void debPrint(string str) {
#if	SOUND_USE_DEBUGLOG
				Debug.Log(str);	
#endif // SOUND_USE_DEBUGLOG
			}
		}

	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
