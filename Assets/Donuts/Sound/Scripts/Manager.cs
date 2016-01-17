// デバイスのミュージックプレイヤーが再生中かどうかをチェックするプラグインを使用する場合はデファイン
#define USE_MUSICPLAYER_ONTIME_STAT

using UnityEngine;
using System.Collections;

/**************************************************************************************************/
/*!
 *    \file    Manager.cs
 *             サウンドマネージャー
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
namespace Donuts
{
  namespace Sound
  {
    public class Manager : MonoBehaviour
    {
			static GameObject myObj;			//!< 自分のGameObject
			static Manager 		instance;		//!< 自分のインスタンス

      /**************************************************************************************************/
      /*!
       *    \fn      public static Manager Instance
       *             インスタンスの取得（生成）
       *    \return  インスタンス
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static Manager Instance {
				get {
					Create();
					return instance;
				}
			}

			private bool isOnTimePlaying;	//!< ミュージックプレイヤーが再生中かどうかのフラグ(iOS時のみ動作)
		
      /**************************************************************************************************/
      /*!
       *    \fn      bool IsOnTimePlaying()
       *             デバイスのミュージックプレイヤーが再生中か？
       *    \return  true : 再生中
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public bool IsOnTimePlaying() {
				return isOnTimePlaying;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      GameObject GetManagerObj()
       *             Manager の GameObject を取得
       *    \return  Manager の GameObject
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static GameObject GetManagerObj() {
				return myObj;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      void Create()
       *             Manager の生成
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static void Create()	{
				if (instance == null) {
					myObj      = new GameObject();
					myObj.name = "_SoundManager";
					instance   = myObj.AddComponent<Manager>();
				}
			}

			// オーディオリスナー
			public AudioListener myAudioListener;

      /**************************************************************************************************/
      /*!
       *    \fn      void Awake()
       *             起動時処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			void Awake () {
				isOnTimePlaying = false;
		
				if (myAudioListener == null) {
					// AudioListenerが設定されてないなら、このGameObjectにAudioListenerがいるかチェック
					myAudioListener = this.gameObject.GetComponent<AudioListener>();
					if (myAudioListener == null) {
						// AudioListenerが居ないので追加する
						myAudioListener = this.gameObject.AddComponent<AudioListener>();
					}
					DontDestroyOnLoad(this.gameObject);			
				}
	
	#if USE_MUSICPLAYER_ONTIME_STAT
				Donuts.Sound.MusicPlayerOnTimeStat.Create();
	#endif	// USE_MUSICPLAYER_ONTIME_STAT
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
				Donuts.Sound.AudioClipCache.AllClear();
				Donuts.Sound.Status.ClearStatus();
	#if USE_MUSICPLAYER_ONTIME_STAT
				Donuts.Sound.MusicPlayerOnTimeStat.Release();
	#endif	// USE_MUSICPLAYER_ONTIME_STAT
	    }

      /**************************************************************************************************/
      /*!
       *    \fn      void Update()
       *             フレームワーク
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			void Update() {
			}

      /**************************************************************************************************/
      /*!
       *    \fn      void CallbackMusicPlayerOnTimeStat(string stat)
       *             デバイスのミュージックプレイヤーの再生状態が変更された時にコールされるメソッド
       *    \param   stat : 変更後のステータス（"Playing" = 再生中
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public void CallbackMusicPlayerOnTimeStat(string stat) {
				Donuts.Sound.Utility.debPrint("*********** CallbackMusicPlayerOnTimeStat Stat=" + stat);
				if (stat == "Playing") {
					isOnTimePlaying = true;
					Donuts.Sound.Utility.debPrint("*********** CallbackMusicPlayerOnTimeStat isOnTimePlaying="+isOnTimePlaying);
				}
				else {
					isOnTimePlaying = false;
					Donuts.Sound.Utility.debPrint("*********** CallbackMusicPlayerOnTimeStat isOnTimePlaying="+isOnTimePlaying);
				}
			}
		}

	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
