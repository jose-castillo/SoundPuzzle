// このデファインが有るときは、BGMであるなら UserDataInfo.Instance.muteBGM をみる (isBGMじゃないならmuteSE)
#define	CHECK_USER_MUTE_FLAG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**************************************************************************************************/
/*!
 *    \file    OneShotPlay.cs
 *             OneShot再生クラス(SE用)
 *    \remarks OneShot のため再生後は、停止・ポーズ・ボリューム変更等できません
 *             同じAudioSourceで再生しますので、位置情報は無いモノと思ってください
 *             ループ時は Stop しない限り再生されつつけますので、再生時の EasyPlay クラスを保存して Stop するようにしてください
 *             Inspector に再生履歴生存リストを表示しています (Inspector をアクティブにしないと値が変更しませんが…)
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

namespace Donuts
{
	namespace Sound
	{
  public class OneShotPlay : MonoBehaviour
  {

		static GameObject  myObj;			//!< 自分のGameObject
		static OneShotPlay instance;	//!< 自分のインスタンス

      /**************************************************************************************************/
      /*!
       *    \fn      public static OneShotPlay Instance
       *             インスタンスの取得（生成）
       *    \return  インスタンス
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static OneShotPlay Instance {
				get {
					Create();
					return instance;
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static GameObject GetObj()
       *             自分のゲームオブジェクトを返す
       *    \return  自身の GameObject
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static GameObject GetObj() {
				return myObj;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public void Play(string path, string name, bool sameCheck, float livingTime, float volume)
       *             SE としての OneShot 再生 (Resources の path と name指定版)(AudioClipCacheも使用)
       *    \remarks 動的にデータを Resources からロードして再生します
       *             先に AudioClipCache にロードしておくと再生時負荷が削減できます
       *             Donuts.Sound.Status.IsMuteSE や Donuts.Sound.Status.IsPause が true の場合は再生しません(再生履歴生存リストにも登録しません)
       *             再生した場合は、AudioClip の名前を再生履歴生存リストにも登録して、指定した秒数リストに残り続けて、同じ音を連続再生する場合に後に再生した再生をキャンセルするようにしています
       *    \param   path       : パス
       *    \param   name       : ファイル名
       *		\param   sameCheck  : 連続再生時除外チェックをするか
       *    \param   livingTime : 再生履歴生存時間(この秒数リストに残り続けて、sameCheck が ON の時にリストにあったら再生しないようにします)
       *    \param   volume     : 再生ボリューム、特に無い場合は 1.0f を指定してください
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void Play(string path, string name, bool sameCheck=false, float livingTime=0.0f, float volume = 1.0f) {
				AudioClip clip = Donuts.Sound.AudioClipCache.LoadAndRegist(path, name);
				if (clip == null) {
					Donuts.Sound.Utility.debPrintError("ERROR:Sound.OneShotPlay.Play not found audioClip. path="+path+" name="+name);
					return ;
				}
				Play(clip, sameCheck, livingTime, volume);
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public void Play(AudioClip audioClip, bool sameCheck, float livingTime, float volume)
       *             SE としての OneShot 再生 (AudioClip指定版)
       *    \remarks Donuts.Sound.Status.IsMuteSE や Donuts.Sound.Status.IsPause が true の場合は再生しません(再生履歴生存リストにも登録しません)
       *             再生した場合は、AudioClip の名前を再生履歴生存リストにも登録して、指定した秒数リストに残り続けて、同じ音を連続再生する場合に後に再生した再生をキャンセルするようにしています
       *    \param   audioClip  : 再生する AudioClip
       *		\param   sameCheck  : 連続再生時除外チェックをするか
       *    \param   livingTime : 再生履歴生存時間(この秒数リストに残り続けて、sameCheck が ON の時にリストにあったら再生しないようにします)
       *    \param   volume     : 再生ボリューム、特に無い場合は 1.0f を指定してください
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void Play(AudioClip audioClip, bool sameCheck=false, float livingTime=0.0f, float volume = 1.0f) {
				if (audioClip == null) {
					Donuts.Sound.Utility.debPrintError("ERROR:Sound.OneShotPlay.Play audioClip is null.");
					return;
				}
				Create();

				// ポーズ中なら抜ける
				if (Donuts.Sound.Status.IsPause) return;
				// ミュート中なら抜ける
				if (Donuts.Sound.Status.IsMuteSE) return;	

				// 連続再生時除外チェックをするか？		
				if (sameCheck) {
					// 再生履歴生存リストに、その音はいるか？
					if (searchPlayInfoList(audioClip.name) != null) {
						// ちょっと前に同じ音が再生されていたので、追加で再生しないで抜けます
						Donuts.Sound.Utility.debPrint("Sound.OneShotPlay.Play already play same SE. "+audioClip.name);
						return;
					}
				}

				// 再生履歴生存時間が0.0fより大きいなら、再生履歴生存リストに登録します
				if (livingTime > 0.0f) {
					PlayInfo info = new PlayInfo(audioClip.name, livingTime);
					playInfoList.Add(info);
				}

				// 再生
				audioSrcObj.PlayOneShot(audioClip, volume * Donuts.Sound.Status.BaseVolumeSE);
				Donuts.Sound.Utility.debPrint("OneShotPlay.Play:"+audioClip.name);			
			}

      /**************************************************************************************************/
      /*!
       *    \fn      private static void Create()
       *             SoundOneShotCtrl が無い場合に作成する
       *    \remarks SoundManager の子供になります
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			private static void Create() {
				Donuts.Sound.Manager.Create();
				if (instance == null) {
					myObj = new GameObject();
					myObj.name = "SoundOneShotCtrl";
					GameObject managerObj = Donuts.Sound.Manager.GetManagerObj();
					myObj.transform.parent = managerObj.transform;
					instance = myObj.AddComponent<OneShotPlay>();
					instance.audioSrcObj 						 = myObj.AddComponent<AudioSource>();
					instance.audioSrcObj.playOnAwake = false;
					instance.playInfoList 					 = new List<PlayInfo>();
				}
			}

			//!< AudioSourceコンポーネント
			private AudioSource audioSrcObj;

      // 
			// 再生履歴情報クラス
			// 近々に再生されたAudioClipの名前を保存
			//
			public class PlayInfo {
				public PlayInfo(string _name, float _livingTime) {
					name 				= _name;
					livingTime 	= _livingTime;
				}
				public string name;				//!< AudioClip.nameが入る
				public float 	livingTime;	//!< 削除までの残り秒数が入る
			};
			private List<PlayInfo> playInfoList;	//!< 再生履歴生存リスト

      /**************************************************************************************************/
      /*!
       *    \fn      public List<PlayInfo> GetPlayInfoList()
       *             再生履歴生存リストの取得
       *    \return  true : 成功, false : 失敗
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public List<PlayInfo> GetPlayInfoList() {
				return playInfoList;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      PlayInfo searchPlayInfoList(string name)
       *             再生履歴生存リストから任意の名前を探す
       *    \param   name : 検索名（AudioClip.name）
       *    \return  PlayInfo （なければ null）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			PlayInfo searchPlayInfoList(string name) {
				if (playInfoList == null) return null;

				for (int i=0; i<playInfoList.Count; i++) {
					if (playInfoList[i].name == name) {
						return playInfoList[i];
					}
				}
				return null;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      void updatePlayInfo()
       *             再生履歴生存リストでの生存時間のカウントダウン
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			void updatePlayInfo() {
				if (playInfoList == null) return;
				for (int i=playInfoList.Count-1; i>=0; i--) {
					playInfoList[i].livingTime -= Time.deltaTime;
					if (playInfoList[i].livingTime <= 0.0f) {
						playInfoList.Remove(playInfoList[i]);
					}
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      void update()
       *             フレームワーク
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			void Update () {
				updatePlayInfo();
			}

      /**************************************************************************************************/
      /*!
       *    \fn      void OnDestroy()
       *             終了時処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			void OnDestroy() {
				playInfoList.Clear();
				playInfoList = null;
			}

		}

	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
