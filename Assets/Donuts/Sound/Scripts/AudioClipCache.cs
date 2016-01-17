using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**************************************************************************************************/
/*!
 *    \file    AudioClipCache.cs
 *             AudioClip のキャッシュ
 *    \remarks キャッシュリストの解放はアプリ終了時等で意図的に行ってください
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

namespace Donuts
{
	namespace Sound
	{
    public class AudioClipCache
    {
			// キャッシュ最大数
			static int LIMIT_COUNT = 10;

			//
			//	キャッシュ情報クラス
			//
			public class Info {
				public Info(string _name, AudioClip _clip) {
					name = _name;
					clip = _clip;
				}
				public string 	 name;		//<! 名前
				public AudioClip clip;		//<! AudioClipデータ
			};

			static List<Info> cacheList;	//<! キャッシュリスト

      /**************************************************************************************************/
      /*!
       *    \fn      static Info FindInfo(string name)
       *             任意の名前をもつものをキャッシュリストから取得
       *    \param   name : 検索する名称
       *    \return  見つかったらキャッシュ情報クラスを返す (無い場合は null)
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			static Info FindInfo(string name) {
				if (cacheList == null) {
					return null;
				}
				for (int i=0; i<cacheList.Count; i++) {
					if (cacheList[i].name == name) {
						return cacheList[i];
					}
				}
				return null;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      static Info FindInfo(string path, string name)
       *             任意の名前をもつものをキャッシュリストから取得(path,name指定版)
       *    \param   path : パス
       *    \param   name : 検索する名称
       *    \return  見つかったらキャッシュ情報クラスを返す (無い場合は null)
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			static Info FindInfo(string path, string name) {
				string fullname = Donuts.Sound.Utility.MakeFullname(path, name);
				if (fullname == null) {
					return null;
				}
				return FindInfo(fullname);
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static AudioClip SearchClip(string path, string name)
       *             パス名と名前から AudioClipを取得する
       *    \param   path : パス
       *    \param   name : 名前
       *    \return  AudioClip （エラー時は null）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static AudioClip SearchClip(string path, string name) {
				Info info = FindInfo(path, name);
				if (info == null) {
					return null;
				}
				return info.clip;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static List<Info> GetList()
       *             キャッシュリストの取得（デバッグ用）
       *    \return  キャッシュリスト
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static List<Info> GetList() {
				return cacheList;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static int GetLimitNum()
       *             キャッシュ登録最大数の取得（デバッグ用）
       *    \return  キャッシュ登録最大数
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static int GetLimitNum() {
				return LIMIT_COUNT;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static void AllClear()
       *             キャッシュリストの解放
       *    \remarks キャッシュしたAudioClipの解放はしてません
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static void AllClear() {
				if (cacheList != null) {
					cacheList.Clear();
					cacheList = null;
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static void Remove(string path, string name)
       *             パス名と名前を元にキャッシュから解放
       *    \param   path : パス
       *    \param   name : 名前
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static void Remove(string path, string name) {
				Info info = FindInfo(path, name);
				if (info == null) {
					return;
				}
				cacheList.Remove(info);
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static void Add(string path, string name, AudioClip clip)
       *             パス名と名前を元にキャッシュに AudioClip を追加
       *    \param   path : パス
       *    \param   name : 名前
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static void Add(string path, string name, AudioClip clip) {
				string fullname = Donuts.Sound.Utility.MakeFullname(path, name);
				if (fullname == null) {
					return;
				}

				if (cacheList == null) {
					cacheList = new List<Info>();
				}

				Info info = FindInfo(fullname);
				if (info == null) {
				  // 新規
					if (cacheList.Count >= LIMIT_COUNT) {
						cacheList.RemoveAt(0);
					}
					info = new Info(fullname, clip);
					cacheList.Add(info);
				}
				else {
					// 存在するので一端削除して先頭に登録しなおす
					cacheList.Remove(info);
					cacheList.Add(info);
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static AudioClip LoadAndRegist(string path, string name)
       *             リソースからAudioClipをロードしてキャッシュする
       *    \remarks 予め登録しておくと再生時のロード負荷を回避できます
       *    \param   path : パス
       *    \param   name : 名前
       *    \return  AudioClip （エラー時は null）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static AudioClip LoadAndRegist(string path, string name) {
				AudioClip clip = SearchClip(path, name);
				if (clip != null) {
					// 既にあるので再登録して返す
					Add(path, name, clip);
					return clip;
				}
		
				string fullname = Donuts.Sound.Utility.MakeFullname(path, name);
				clip = (AudioClip)Resources.Load(fullname);

				if (clip == null) {
				  Donuts.Sound.Utility.debPrintError("ERROR:AudioClipCache:Regist:Not found data. path="+path+" name="+name);
					return null;
				}
				else {
					// 読み込めたので登録します
					Donuts.Sound.AudioClipCache.Add(path, name, clip);
				}
				return clip;		
			}

		}

	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
