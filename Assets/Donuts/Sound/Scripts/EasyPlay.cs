// このデファインが有るときは、BGMであるなら UserDataInfo.Instance.muteBGM をみる(isBGM でないなら muteSE)
#define	CHECK_USER_MUTE_FLAG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**************************************************************************************************/
/*!
 *    \file    EasyPlay.cs
 *             簡易サウンド再生
 *    \remakrs 再生は PlayOneShot ではなく普通の Play を使用するため、同時複数発生したい場合は別の方法で行うようにしてください
 *             基本的に再生終了したら削除される事を設計思想としているため、Stop したら削除されます。また、音が最後まで再生されれば削除されます
 *						 ループ時は Stop しない限り再生され続けます。再生時の EasyPlay クラスを保存して Stop するようにしてください
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
namespace Donuts
{
	namespace Sound
	{
    public class EasyPlay : MonoBehaviour
    {

      /**************************************************************************************************/
      /*!
       *    \fn      public static EasyPlay PlayObject(AudioSource audioSrc, bool isLoop, bool isFadeIn, float fadeTime)
       *             SE としての再生(既に Hierarchy に作成済みの AudioSource(AudioClipも登録済み)を指定しての再生)
       *    \remarks Donuts.Sound.Status.IsMuteSE が true の場合は音量が 0 になります( false 時は 1.0 に再設定されます)
       *             返り値の EasyPlay クラスを使用して色々操作出来ます(ループ再生時は停止等)
       *             ループでない場合、再生が終了すると自動でオブジェクトを削除します
       *    \param   audioSrc : 再生する AudioSource(AudioClipも登録済みの事)
       *    \param   isLoop   : ループスイッチ (true=ループする false=ループしない)
       *    \param   isFadeIn : フェードインスイッチ(true=する false=しない)
       *    \param   fadeTime : フェード時間（秒）
       *    \return  EasyPlayクラス　(失敗時は　null)
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
	    public static EasyPlay PlayObject(AudioSource audioSrc, bool isLoop=false, bool isFadeIn=false, float fadeTime=0.0f) {
        if (audioSrc == null) {
					Donuts.Sound.Utility.debPrintError("ERROR:EasyPlay.PlayObject audioSrc is null.");
					return null;
				}

				if (audioSrc.clip == null) {
					Donuts.Sound.Utility.debPrintError("ERROR:EasyPlay.PlayObject audioSrc.clip is null.");
					return null;
				}

				EasyPlay handle = Create(false);
				if (handle != null) {
					handle.audioSrcObj     = audioSrc;
					handle.gameObject.name = "SndEzPlyObj:" + audioSrc.gameObject.name;
					if (isLoop) {
						handle.gameObject.name = handle.gameObject.name+" Loop";
					}
			
					handle.Play("", audioSrc.gameObject.name, isLoop, isFadeIn, fadeTime);
				}
				return handle;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static EasyPlay CreateAndPlay(string path, string name, bool isLoop, bool isFadeIn, float fadeTime)
       *             SE としての再生(BGMは CreateAndBGMPlay を使用してください)
       *    \remarks Donuts.Sound.Status.IsMuteSE が true の場合は音量が 0 になります( false 時は 1.0 に再設定されます)
       *             動的にデータを Resources からロードして再生します
       *             先に AudioClipCache にロードしておくと再生時負荷が削減できます
       *             返り値の EasyPlay クラスを使用して色々操作出来ます(ループ再生時は停止等)
       *             ループでない場合、再生が終了すると自動でオブジェクトを削除します
       *    \param   path     : パス
       *    \param   name     : ファイル名
       *    \param   isLoop   : ループスイッチ (true=ループする false=ループしない)
       *    \param   isFadeIn : フェードインスイッチ(true=する false=しない)
       *    \param   fadeTime : フェード時間（秒）
       *    \return  EasyPlayクラス　(失敗時は　null)
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static EasyPlay CreateAndPlay(string path, string name, bool isLoop=false, bool isFadeIn=false, float fadeTime=0.0f) {
				EasyPlay handle = Create(true);
				if (handle != null) {
					handle.gameObject.name = "SndEzPly:" + name;
					if (isLoop) {
						handle.gameObject.name = handle.gameObject.name + " Loop";
					}
					handle.Play(path, name, isLoop, isFadeIn, fadeTime);
				}
				return handle;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static EasyPlay CreateAndBGMPlay(string path, string name, bool isLoop, bool isFadeIn, float fadeTime, float startTime)
       *             BGM としての再生
       *    \remarks BGM でループする場合、Audio データ側の Gapless looping を ON にする事を推奨します(BuildTargetが iOS/Android になっていると変更可能になります)
       *             Donuts.Sound.Status.IsMuteSE が true の場合は音量が 0 になります( false 時は 1.0 に再設定されます)
       *             動的にデータを Resources からロードして再生します (AudioClipCacheは使用してません)
       *             返り値の EasyPlay クラスを使用して色々操作出来ます(ループ再生時は停止等)
       *             ループでない場合、再生が終了すると自動でオブジェクトを削除します
       *    \param   path      : パス
       *    \param   name      : ファイル名
       *    \param   isLoop    : ループスイッチ (true=ループする false=ループしない)
       *    \param   isFadeIn  : フェードインスイッチ(true=する false=しない)
       *    \param   fadeTime  : フェード時間（秒）
       *    \param   startTime : 再生開始位置（秒）
       *    \return  true : 成功、 false : 失敗
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static EasyPlay CreateAndBGMPlay(string path, string name, bool isLoop=true, bool isFadeIn=true, float fadeTime=2.0f, float startTime=0.0f) {
				EasyPlay handle = Create(true);
				if (handle != null) {
			    handle.gameObject.name = "SndEzBGMPly:" + name;
				  if (isLoop) {
				    handle.gameObject.name = handle.gameObject.name + " Loop";
				  }
				  handle.isBGM = true;
				  handle.SetStartTime(startTime);
				  handle.Play(path, name, isLoop, isFadeIn, fadeTime);
			  }
			  return handle;
		  }

      /**************************************************************************************************/
      /*!
       *    \fn      public static EasyPlay Create(bool intoAudioSource)
       *             EasyPlay の作成
       *    \remarks GameObject を作成後、EasyPlay を追加します
       *             作成した GameObject は SoundEasyPlayRoot の子供になります
       *             SoundEasyPlayRoot が無い場合は自動で作成します
       *             SoundEasyPlayRoot の子供として管理されるので、再生中の EasyPlay オブジェクトは SoundEasyPlayRoot の子供をサーチすれば出来るようになっています
       *    \param   intoAudioSource : AudioSource を内部に持つか？ (true:持つ , false:AudioSource は外部にあるので持たない)
       *    \return  作成した EasyPlay
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static EasyPlay Create(bool intoAudioSource) {
				makeSoundRootObj();
				GameObject obj 			 = new GameObject();
				obj.transform.parent = SoundRootObj.transform;

				EasyPlay easyPlay = obj.AddComponent<EasyPlay>();

				if (intoAudioSource) {
					easyPlay.audioSrcObj 							= obj.AddComponent<AudioSource>();
					easyPlay.audioSrcObj.playOnAwake 	= false;
					easyPlay.outSideAudioSource 			= false;
				}
				else {
					easyPlay.outSideAudioSource = true;
		    }
			  return easyPlay;
		  }

      /**************************************************************************************************/
      /*!
       *    \fn      public static GameObject GetRootObj()
       *             SoundEasyPlayRoot の取得
       *    \return  SoundEasyPlayRoot の GameObject (まだ作成してない場合は null)
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
	    public static GameObject GetRootObj() {
        return SoundRootObj;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public static void StopAll()
       *             管理している EasyPlay オブジェクト全てに対して再生停止を行う
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public static void StopAll() {
				if (SoundRootObj == null) {
				  return;
		    }
			  Transform rootTrans = SoundRootObj.transform;
			  int childNum = rootTrans.childCount;
			  for (int i=0; i<childNum; i++) {
			    Transform trans = rootTrans.GetChild(i);
			    EasyPlay handle = trans.gameObject.GetComponent<EasyPlay>();
			    handle.Stop();
		    }
	    }

      /**************************************************************************************************/
      /*!
       *    \fn      public static void SetAllPause(bool sw)
       *             管理している EasyPlay オブジェクトの全てにポーズ設定をします
       *    \param   sw : ポーズ状態 (true:ポーズ中, false=ポーズしてない）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static void SetAllPause(bool sw) {
				if (SoundRootObj == null) {
					return;
				}
				Transform rootTrans = SoundRootObj.transform;
				int childNum = rootTrans.childCount;
				for (int i=0; i<childNum; i++) {
					Transform trans = rootTrans.GetChild(i);
					//Donuts.Sound.Utility.debPrint("Child:"+i+" name="+trans.gameObject.name);
					EasyPlay handle = trans.gameObject.GetComponent<EasyPlay>();
					handle.Pause(sw);
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      private static void makeSoundRootObj()
       *             SoundEasyPlayRoot オブジェクトが作成されてないなら作成します
       *    				 SoundManager の子供になるようにしてます
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      private static void makeSoundRootObj() {
				Donuts.Sound.Manager.Create();
		
				if (SoundRootObj != null) {
					return;
				}
				SoundRootObj 									= new GameObject();
				SoundRootObj.name 						= "SoundEasyPlayRoot";
				GameObject managerObj 				= Donuts.Sound.Manager.GetManagerObj();
				SoundRootObj.transform.parent = managerObj.transform;
				// 残しつつ続けたいなら
				// DontDestroyOnLoad(SoundRootObj);
			}

			// 再生終了時 (GameObject を削除する時)のコールバックデリゲートタイプ
			public delegate void EndCallback(EasyPlay handle);
      /**************************************************************************************************/
      /*!
       *    \fn      public void SetEndCallback(EndCallback callback)
       *             再生終了時(GameObjectを削除する時)のコールバックの登録
       *    \remarks これは再生時に引数指定をしないため、やりたい場合は再 Create 後にコールしてください
       *    \param   callback : コールバックメソッド
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
	    public void SetEndCallback(EndCallback callback) {
				endCallbackFunc = callback;
			}

			// FadeOut時のコールバックデリゲートタイプ
			public delegate void FadeOutCallback(EasyPlay handle);
	
			private bool isBGM;	//!< BGMステータス
      /**************************************************************************************************/
      /*!
       *    \fn      public bool IsBGM
       *             BGMかどうか？
       *    \return  true:BGM, false:BGMでない
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
	    public bool IsBGM {
				get {
					return isBGM;
				}
				set {
					isBGM = value;
			  }		
			}

      private static GameObject SoundRootObj;					//!< SoundEasyPlayRoot
      public bool 							outSideAudioSource;		//!< AudioSrouce(AudioClip)も外部に有る場合はtrue
      private FadeOutCallback 	fadeOutCallbackFunc;	//!< フェードアウト時のコールバックデリゲート
      private EndCallback 			endCallbackFunc;			//!< 終了時のコールバックデリゲート	
      private AudioSource 			_audioSrcObj;					//!< このEasyPlayが使用するAudioSourceコンポーネント

      /**************************************************************************************************/
      /*!
       *    \fn      public AudioSource audioSrcObj
       *             AudioSourceのアクセサ （セット時はAudioSourceのvolumeとloopを保存）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public AudioSource audioSrcObj {
				get {
					return _audioSrcObj;
				}
				set {
					_audioSrcObj 		= value;
					standardVolume 	= GetVolume();
					orgLoopSw 			= _audioSrcObj.loop;
				}
			}
	
			public string		myName;							//!< 再生する曲名(path無し)
			public float 		startTime = 0.0f;		//!< 再生開始位置（秒）
			public float 		nowTime   = 0.0f;		//!< 現在の再生位置（秒）
			public float 		fadeSpeed = 2.0f;		//!< 音量フェードの速度（1/秒数の値で毎フレームTime.deltaTimeに掛けて加算されます）
			public float 		fadeRate  = 1.0f;		//!< 音量フェードのレート(1.0f=音量最大 0.0f=音量最小)
			public float 		standardVolume;			//!< このオブジェクトの標準音量(audioSrcObjに設定時にAudioSourceから取得します)
			public bool 		orgLoopSw;					//!< このAudioSourceの初期時のループ設定(audioSrcObjに設定時にAudioSourceから取得します)
			public float 		preVolume = -1.0f;

			private bool 		bPlayed;						//!< 一度でもPlayしたらtrue(Stop時にも戻らない)
			private bool 		isPause;						//!< ポーズ状態(true-ポーズ中 false=ポーズ中ではない)
			private bool 		bLoop;							//!< ループスイッチ状態(true-ループする false=ループしない)

			// 音量フェードモード
			public enum FadeMode {
				None,			//!< フェード中じゃない
				FadeOut,	//!< フェードイン中
				FadeIn,		//!< フェードアウト中
			};
			public FadeMode fadeMode;	//!< 音量フェードモード

      /**************************************************************************************************/
      /*!
       *    \fn      void Update()
       *             フレームワーク
       *             音量フェード処理を行います
       *             Muteスイッチに応じてボリューム設定をします
       *             音が最後まで再生されているか、Play 後に Stop されていると自動で削除されます
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
	    void Update() {
        nowTime = GetNowTime();
		
        switch(fadeMode) {
        case FadeMode.None:
          break;
        case FadeMode.FadeIn:
          fadeRate += (Time.deltaTime * fadeSpeed);
					if (fadeRate >= 1.0f) {
						fadeRate = 1.0f;
						fadeMode = FadeMode.None;
					}
					break;
				case FadeMode.FadeOut:
					fadeRate -= (Time.deltaTime * fadeSpeed);
					if (fadeRate <= 0.0f) {
						fadeRate = 0.0f;
						fadeMode = FadeMode.None;
						Stop();		// ここでコレを入れてしまうと、BGMManagerでのフェード終了が取得できない
						Donuts.Sound.Utility.debPrint("EasyPlay:FadeOut:END");

						// フェードアウトが終わったコールバック
						if (fadeOutCallbackFunc != null) {
							fadeOutCallbackFunc(this);
						}				
					}
					break;
				}

				if (IsPlay()) {
					float muteRate = 1.0f;
#if CHECK_USER_MUTE_FLAG
					if (IsMute()) {
						muteRate = 0.0f;
					}
#endif // CHECK_USER_MUTE_FLAG
					float tmpVol = standardVolume * fadeRate * muteRate * getBaseVolume();
					SetVolume(tmpVol);
				}

		 		if (IsEnd()) {
					Donuts.Sound.Utility.debPrint("Destroy:"+this.gameObject.name);
					if (endCallbackFunc != null) {
						endCallbackFunc(this);
					}
					audioSrcObj.volume = standardVolume;	// 戻しておかないとAudioSourceを外部に持ったときにボリュームが戻らなくなるので注意
					audioSrcObj.loop   = orgLoopSw;				// 念のためコレも戻しておく
					Destroy(this.gameObject);
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      private float getBaseVolume()
       *             ベースボリュームを取得
       *    \return  ベースボリューム
       *    \date    2014.12.26(Fri)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			private float getBaseVolume() {
				if (isBGM) {
					return Donuts.Sound.Status.BaseVolumeBGM;
				}
				else {
					return Donuts.Sound.Status.BaseVolumeSE;
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public void StartFadeOut(float sec, FadeOutCallback callback)
       *             音量フェードアウト開始
       *    \param   sec      : フェードアウト時間（秒）
       *    \param   callback : フェード完了時のコールバック（必要ない場合は null）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void StartFadeOut(float sec, FadeOutCallback callback = null) {
				Donuts.Sound.Utility.debPrint("EasyPlay:StartFadeOut:" + sec);
				switch(fadeMode) {
				case FadeMode.FadeIn:
					// フェードイン中なら現在の状況からフェードアウトを行う
					//fadeRate;
					// 時間はその分へらす
					if (sec > 0.0f) {
						sec = sec * fadeRate;
					}
					break;
				case FadeMode.None:
					fadeRate = 1.0f;
					break;
				case FadeMode.FadeOut:
					// フェードアウトを既にやってるなら無し
					Donuts.Sound.Utility.debPrint("ERROR:EasyPlay:already fading...");
					return;
				}
				if (sec > 0.0f) {
					fadeSpeed = 1.0f / sec;
				}
				else {
					fadeSpeed = 2.0f;
				}
				fadeMode 						= FadeMode.FadeOut;
				fadeOutCallbackFunc = callback;
				Donuts.Sound.Utility.debPrint("EasyPlay:StartFadeOut:nowMode=" + fadeMode);
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public float GetVolume()
       *             音量ボリューム取得
       *    \return  ボリューム(0.0f～1.0f)
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public float GetVolume() {
				return audioSrcObj.volume;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public void SetVolume(float vol)
       *             音量ボリューム設定
       *             ミュート時やフェード中のボリュームもそのまま返ってくるので注意(ミュート中なら 0.0f になる等)
       *    \return  ボリューム(0.0f～1.0f)
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void SetVolume(float vol) {
				// 前回と違うボリュームなら設定します
				if (!Donuts.Common.Utility.IsFloatEqual(preVolume, vol)) {
					audioSrcObj.volume = vol;
					preVolume = vol;
				}

			}

      /**************************************************************************************************/
      /*!
       *    \fn      public bool IsEnd()
       *             曲が最後まで再生されたか？
       *    \return  true : 最後まで再生, false : 再生中 or 再生前
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public bool IsEnd() {
				if (bPlayed) {
					// ポーズ中じゃない
					if (!isPause) {
						// Play/PlayMusicをしていて再生開始している
						if (!audioSrcObj.isPlaying) {
							// その上で再生中じゃなくなっているので最後まで再生したと判断する
							return true;
						}
					}
				}
				return false;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public bool IsPlay()
       *             現在再生中か？
       *    \return  true : 再生中, false : 再生していない
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public bool IsPlay() {
				return audioSrcObj.isPlaying;
			}


      /**************************************************************************************************/
      /*!
       *    \fn      public float GetNowTime()
       *             現在の再生位置（時間）を取得（秒）
       *    \return  再生位置（秒）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public float GetNowTime() {
				return audioSrcObj.time;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public float GetStartTime()
       *             再生開始位置の取得
       *    \return  再生開始位置（秒）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public float GetStartTime() {
				return startTime;
			}


      /**************************************************************************************************/
      /*!
       *    \fn      public void SetStartTime(float start)
       *             再生開始位置の設定
       *						 再生開始前に設定しないと意味がないです
       *    \param   start : 再生開始位置（秒）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void SetStartTime(float start) {
				Donuts.Sound.Utility.debPrint("SoundEZPlay:SetStartTime=" + start);
				startTime = start;
				audioSrcObj.time = startTime;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public void Stop()
       *             再生停止
       *						 再生停止後の Updeteでオブジェクトが削除されます
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void Stop() {
				fadeMode = FadeMode.None;
				audioSrcObj.Stop();
				//	bPlayed = false;	// これをクリアしちゃうとUpdateでDestroyしないので注意
				isPause = false;
			}
	
      /**************************************************************************************************/
      /*!
       *    \fn      public AudioClip LoadSound(string path, string name)
       *             音声データ(AudioClip)のロード
       *    \remarks BGM の時はキャッシュしません
       *             AudioClipCache を使用しています
       *    \param   path : パス
       *    \param   name : ファイル名
       *    \return  AudioClip（ロード失敗時は null）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public AudioClip LoadSound(string path, string name) {
				if (isBGM) {
					string fullname = Donuts.Sound.Utility.MakeFullname(path, name);
					AudioClip clip = (AudioClip)Resources.Load(fullname);
					return clip;
				}
				else {
					return Donuts.Sound.AudioClipCache.LoadAndRegist(path, name);
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public float Play(string path, string name, bool isLoop, bool isFadeIn, float fadeTime)
       *             再生開始
       *    \param   path     : パス
       *    \param   name     : ファイル名
       *    \param   isLoop   : ループ再生するか
       *    \param   isFadeIn : フェードインするか
       *    \param   fadeTime : フェード時間（秒）
       *    \return  この音声の時間（秒：最長時間）を返します、エラー時は0.0fが返ります
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public float Play(string path, string name, bool isLoop, bool isFadeIn, float fadeTime) {
				if (bPlayed) {
					Donuts.Sound.Utility.debPrintError("ERROR:EasyPlay already play. path="+path+" name="+name);
					return 0.0f;
				}
				myName = name;
				bLoop  = isLoop;
				AudioClip aclip;
				if (outSideAudioSource) {
					aclip = audioSrcObj.clip;
				}
				else {
					aclip = LoadSound(path, myName);
					if (aclip == null) {
						Donuts.Sound.Utility.debPrint("ERROR:EasyPlay:Play Could not load audioClip. path="+path+" name="+myName);
						return 0.0f;
					}
					audioSrcObj.clip = aclip;
				}

				audioSrcObj.time = startTime;
				audioSrcObj.loop = bLoop;

				Donuts.Sound.Utility.debPrint("SoundEZPlay:Play:StartTime="+startTime+" :"+audioSrcObj.time + ", isFadeIn:" + isFadeIn);

				if (isFadeIn) {
					fadeMode 			= FadeMode.FadeIn;
					fadeRate = 0.0f;

					if (fadeTime > 0.0f) {
						fadeSpeed = 1.0f / fadeTime;
					}
					else {
						fadeSpeed = 2.0f;
					}
				}
				else {
					fadeRate = 1.0f;
				}

				audioSrcObj.Play();
		
				bPlayed = true;
				if (Donuts.Sound.Status.IsPause) {
					Pause(true);
				}
				else {
					isPause = false;
				}

		    float muteRate = 1.0f;
#if CHECK_USER_MUTE_FLAG
				// Mute中ならボリュームを０にしておく
				if (IsMute()) {
					muteRate = 0.0f;
				}
#endif // CHECK_USER_MUTE_FLAG

		    preVolume = -1.0f;
   		  float tmpVol = standardVolume * fadeRate * muteRate * getBaseVolume();
		    SetVolume(tmpVol);

 				return aclip.length;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public void Pause(bool sw)
       *             ポーズ設定
       *    \param   sw : ポーズ設定（true:ポーズ中）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public void Pause(bool sw) {
				if (sw) {
					audioSrcObj.Pause();
					isPause = true;
				}
				else {
					isPause = false;
					audioSrcObj.Play();
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      public bool IsMute()
       *             ミュート中かどうか？
       *    \remarks SE or BGM かどうかを内部で判断して Donuts.Sound.Status.IsMuteSE or Donuts.Sound.Status.IsMuteBGM の値をチェックする
       *    \return  true : ミュート中
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			public bool IsMute() {
#if CHECK_USER_MUTE_FLAG
				if (isBGM) {
					// デバイスのミュージックプレイヤーが再生中だったらミュートとする
					Donuts.Sound.Manager manager = Donuts.Sound.Manager.Instance;
					if (manager != null) {
						if (manager.IsOnTimePlaying()) {
							return true;
						}
					}
					return Status.IsMuteBGM;
				}
				else {
					return Status.IsMuteSE;
				}
#else
				return false;
#endif // CHECK_USER_MUTE_FLAG
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
				isBGM = false;
				fadeMode = FadeMode.None;
				fadeRate = 1.0f;
				fadeSpeed = 2.0f;
				bPlayed = false;
				isPause = false;
			}
		}

	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
