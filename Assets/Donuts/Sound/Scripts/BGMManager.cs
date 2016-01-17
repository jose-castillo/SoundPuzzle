using UnityEngine;
using System.Collections;

/**************************************************************************************************/
/*!
 *    \file    BGMManager.cs
 *             BGMマネージャー
 *    \remarks 再生は EasyPlay の BGM 再生を利用
 *             SoundEasyPlayRoot 以下ではなく、BGMManager 以下に再生している EasyPlay ゲームオブジェクトを作成します
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
namespace Donuts
{
	namespace Sound
	{
    public class BGMManager : MonoBehaviour {

      static BGMManager instance;		//!< インスタンス

      /**************************************************************************************************/
      /*!
       *    \fn      public static BGMManager Instance
       *             インスタンスの取得（生成）
       *    \return  インスタンス
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static BGMManager Instance {
				get {
					Create();
					return instance;
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static void Create()
       *             インスタンスの生成
       *    \return  インスタンス
       *    \remakrs SoundMangaer 下に配置
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static void Create() {
				if (instance == null) {
					Donuts.Sound.Manager.Create();
					GameObject obj 				= new GameObject();
					obj.name 							= "BGMManager";
					GameObject managerObj = Donuts.Sound.Manager.GetManagerObj();
					obj.transform.parent  = managerObj.transform;
					instance 							= obj.AddComponent<BGMManager>();
				}
			}

			public string 	nextBgmName = "";		//!< 次の曲名(""の場合は登録されてないと判断します)
			public string 	nextBgmPath = "";		//!< 次の曲のパス名
			public float 		nextFadeTime;				//!< 次の曲再生時のフェードイン時間（秒）
			public EasyPlay nowPlayer;					//!< 再生している曲のEasyPlayクラス
			public string 	nowPlayName;				//!< 再生している曲名(path + name)
			// public bool 		isLoop;							//!< ループスイッチ
			// private bool 		isFadeOut = false;	//!< フェードアウト中はtrueになる

      /**************************************************************************************************/
      /*!
       *    \fn      public bool IsEnd()
       *             曲が最後まで再生されたか？
       *    \return  true : 最後まで再生された or 何も再生していない, false : 再生中
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public bool IsEnd() {
				if (nowPlayer != null) {
					return nowPlayer.IsEnd();
				}
				else {
					return true;
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public bool IsPlay()
       *             再生中か？
       *    \return  true : 再生中, false : 再生していない
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public bool IsPlay() {
				if (nowPlayer != null) {
					return nowPlayer.IsPlay();
				}
				else {
					return false;
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public float GetNowTime()
       *             現在の再生位置取得（秒）
       *    \return  再生位置（秒）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public float GetNowTime() {
				if (nowPlayer != null) {
					return nowPlayer.GetNowTime();
				}
				else {
					return 0.0f;
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public void Stop()
       *             停止
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void Stop() {
				if (nowPlayer != null) {
					nowPlayer.Stop();
					Destroy(nowPlayer.gameObject);
					nowPlayer = null;
				}
				nowPlayName = "";
				//isFadeOut = false;
				Donuts.Sound.Utility.debPrint("BGMManager STOP");		
	    }
	
      /**************************************************************************************************/
      /*!
       *    \fn      private void Play(string path, string name, bool isLoop, bool bFadeIn, float fadeInTime, float startTime)
       *             再生開始
       *    \param   path      : パス
       *    \param   name      : 再生ファイル名
       *    \param   isloop    : ループ再生するか（true : ループ再生）
       *    \param   isFadeIn  : フェードインするか（true : フェードインする）
       *    \param   fadeTime  : フェードイン時間（秒）
       *    \param   startTime : 再生開始位置（秒）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			private void Play(string path, string name, bool isLoop = true, bool isFadeIn = true, float fadeTime = 2.0f, float startTime = 0.0f) {
				Donuts.Sound.Utility.debPrint("BGMManager:Play:" + name + ", isLoop:" + isLoop + ", isFadeIn:" + isFadeIn + ", fadeTime:" + fadeTime + ", startTime:" + startTime);

				// 上書きで停止する場合は EncCallback は破棄する
				if (nowPlayer != null) {
					nowPlayer.SetEndCallback(null);
				}
				Stop();

				nowPlayer = EasyPlay.CreateAndBGMPlay(path, name, isLoop, isFadeIn, fadeTime, startTime);
				if (nowPlayer != null) {
			    Donuts.Sound.Utility.debPrint("BGMManager.Play:" + name);
			    nowPlayer.SetEndCallback(callbackPlayerEnd);
			    nowPlayName = Donuts.Sound.Utility.MakeFullname(path, name);
			    // isLoop = _isLoop;
			    // isFadeOut = false;

			    nowPlayer.gameObject.transform.parent = this.gameObject.transform;
			  }
		  }

      /**************************************************************************************************/
      /*!
       *    \fn      public void callbackPlayerEnd(EasyPlay handle)
       *             再生終了時コールバック
       *    \param   handle : EasyPlay クラス
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void callbackPlayerEnd(EasyPlay handle) {
				Donuts.Sound.Utility.debPrint("callbackPlayerEnd");
				nowPlayer   = null;
				nowPlayName = "";		// これやらないと同じ曲を再生しようとするとキャンセルしてしまう
			}


      /**************************************************************************************************/
      /*!
       *    \fn      public void PlayBGM(string path, string name, bool isFadeIn, float fadeTime)
       *             BGM再生開始（現在と同じ曲を再生する場合は何もしない）
       *    \param   path     : パス
       *    \param   name     : ファイル名
       *    \param   isFadeIn : フェードインするか
       *    \param   fadeTime : フェードイン時間（秒）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void PlayBGM(string path, string name, bool isFadeIn=true, float fadeTime=2.0f) {
				string bgmName = Donuts.Sound.Utility.MakeFullname(path, name);
				Donuts.Sound.Utility.debPrint("PlayMenuBGM:" + bgmName);
				if (nowPlayName != bgmName) {
					// 変更があったら再生します
					Play(path, name, true, isFadeIn, fadeTime, 0.0f);
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public void SetNextBgmName(string path, string name, float fadeTime)
       *             次の曲の登録（１曲だけ登録可能）
       *    \param   path     : パス
       *    \param   name     : ファイル名
       *    \param   fadeTime : フェードイン時間（秒）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public void SetNextBgmName(string path, string name, float fadeTime=2.0f) {
				nextBgmName  = name;
				nextBgmPath  = path;
				nextFadeTime = fadeTime;
			}
	
      /**************************************************************************************************/
      /*!
       *    \fn      public void callbackFadeOut(EasyPlay handle)
       *             フェードアウト時のコールバック（次の曲が登録されていたら再生開始する）
       *    \param   handle : EasyPlay クラス
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void callbackFadeOut(EasyPlay handle) {
				// 次のがあれば再生します
				if (nextBgmName.Length > 0) {
					Donuts.Sound.Utility.debPrint("callbackFadeOut");
					nowPlayName = "";		// わざわざフェードアウトして次のにつなぐので同じ曲だとしても再生するように現在再生していた曲の情報はクリアする
					PlayBGM(nextBgmPath, nextBgmName, true, nextFadeTime);
					nextBgmName = "";
					nextBgmPath = "";
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public void StartFadeOut(float sec)
       *             フェードアウト開始
       *    \param   fadeTime : フェードアウト時間（秒）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void StartFadeOut(float fadeTime) {
				if (nowPlayer != null) {
				  nowPlayer.StartFadeOut(fadeTime, callbackFadeOut);
				  //isFadeOut = true;
			  }		
		  }

      /**************************************************************************************************/
      /*!
       *    \fn      public void Pause(bool sw)
       *             ポーズ設定
       *    \param   sw : ポーズ設定（true : ポーズ）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void Pause(bool sw) {
				if (nowPlayer != null) {
					nowPlayer.Pause(sw);
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      void Awake()
       *             起動時処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			void Awake () {
				nextBgmName  = "";
				nextBgmPath  = "";
				nextFadeTime = 0.0f;
			}

			void OnDestroy() {
	    }

			void Update() {
			}
		}

	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
