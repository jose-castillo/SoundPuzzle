// IOS/Android時にそれぞれのデバイスプラグインをコールする場合はデファインする事
#define USE_DEVICE_CODE

using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Donuts
{
  namespace Sound
  {

    // BuildTarget 毎のクラス定義
#if UNITY_EDITOR
    using musicPlayerOnTimeStatPlatform = musicPlayerOnTimeStatDefault;
#elif (UNITY_ANDROID && USE_DEVICE_CODE)
    using musicPlayerOnTimeStatPlatform = musicPlayerOnTimeStatAndroid;
#elif (UNITY_IPHONE && USE_DEVICE_CODE)
    using musicPlayerOnTimeStatPlatform = musicPlayerOnTimeStatIOS;
#else
    using musicPlayerOnTimeStatPlatform = musicPlayerOnTimeStatDefault;
#endif

    /**************************************************************************************************/
    /*!
     *    \class   MusicPlayerOnTimeStat
     *    \brief   アプリ実行中のデバイスのミュージックプレイヤーの状態変化を受け取って Unity 側に通知するクラス
     *             アプリ実行中に純正ヘッドフォンなどのリモコンでプレイボタンを押した時の状態を取得するのに使います
     *             前提条件として、Unity のヒエラルキー上に 「BGMManager」 というゲームオブジェクトが１つだけ存在していて、
     *             その中のスクリプトにて「void CallbackMusicPlayerOnTimeStat(string)」というメソッドがある事が条件になります。
     *             CallbackMusicPlayerOnTimeStat メソッドの第一引数は状態の文字列が入ります。(Paused/Playing/Stopped)
     *    \date    2014.12.25(Thu)
     *    \author  Masayoshi.Matsuyama@Donuts
     */
    /**************************************************************************************************/
    public class MusicPlayerOnTimeStat
    {
      static IntPtr pluginInstance = IntPtr.Zero;

      /**************************************************************************************************/
      /*!
       *    \fn      public static void Create()
       *             作成（外部から呼び出し用）
       *    \remarks BuildTargetによって内部で呼ぶクラスが変更されます
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static void Create() {
		// Debug.Log("MusicPlayerOnTimeStat:Create START");
		if (pluginInstance == IntPtr.Zero) {
		  // Debug.Log("MusicPlayerOnTimeStat:Create call");
		  pluginInstance = musicPlayerOnTimeStatPlatform.Create();
		}
		// Debug.Log("MusicPlayerOnTimeStat:Create END");
	  }

      /**************************************************************************************************/
      /*!
       *    \fn      public static void Release()
       *             解放（外部から呼び出し用）
       *    \remarks BuildTargetによって内部で呼ぶクラスが変更されます
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static void Release() {
		// Debug.Log("MusicPlayerOnTimeStat:Release START");
        if (pluginInstance != IntPtr.Zero) {
		  // Debug.Log("MusicPlayerOnTimeStat:Release call");
		  musicPlayerOnTimeStatPlatform.Release(pluginInstance);
		  pluginInstance = IntPtr.Zero;
		}
		// Debug.Log("MusicPlayerOnTimeStat:Release END");
	  }
    }


#if UNITY_EDITOR
    /**************************************************************************************************/
    /*!
     *    \class   MusicPlayerOnTimeStatDefault
     *    \brief   UnityEditor時のクラス
     *    \date    2014.12.25(Thu)
     *    \author  Masayoshi.Matsuyama@Donuts
     */
    /**************************************************************************************************/
    public class musicPlayerOnTimeStatDefault
    {
      /**************************************************************************************************/
      /*!
       *    \fn      public static IntPtr Create()
       *             作成
       *    \return  クラスポインタ（失敗時は IntPtr.Zero）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static IntPtr Create() {
        // Debug.Log("musicPlayerOnTimeStatDefault:Create");
		return IntPtr.Zero;
      }

      /**************************************************************************************************/
      /*!
       *    \fn      public static void Release(IntPtr instance)
       *             解放
       *    \param   解放するインスタンス
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static void Release(IntPtr _instance) {
        // Debug.Log("musicPlayerOnTimeStatDefault:Release");
      }
    }

#elif (UNITY_ANDROID && USE_DEVICE_CODE)
    /**************************************************************************************************/
    /*!
     *    \class   MusicPlayerOnTimeStatAndroid
     *    \brief   Android時のクラス
     *    \date    2014.12.25(Thu)
     *    \author  Masayoshi.Matsuyama@Donuts
     */
    /**************************************************************************************************/
    public class musicPlayerOnTimeStatAndroid
    {
      /**************************************************************************************************/
      /*!
       *    \fn      public static IntPtr Create()
       *             作成
       *    \return  クラスポインタ（失敗時は IntPtr.Zero）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static IntPtr Create() {
        if ( Application.platform == RuntimePlatform.Android )
        {
        }
        // Debug.Log("musicPlayerOnTimeStatAndroid:Create");
		return IntPtr.Zero;        
      }

      /**************************************************************************************************/
      /*!
       *    \fn      public static void Release(IntPtr instance)
       *             解放
       *    \param   解放するインスタンス
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static void Release(IntPtr _instance) {
        if ( Application.platform == RuntimePlatform.Android ) {
        }
        // Debug.Log("musicPlayerOnTimeStatAndroid:Release");
      }
    }

#elif (UNITY_IPHONE && USE_DEVICE_CODE)
    /**************************************************************************************************/
    /*!
     *    \class   MusicPlayerOnTimeStatIOS
     *    \brief   iOS 時のクラス
     *    \date    2014.12.25(Thu)
     *    \author  Masayoshi.Matsuyama@Donuts
     */
    /**************************************************************************************************/
    public class musicPlayerOnTimeStatIOS
    {
	  [DllImport("__Internal")]
	  private static extern IntPtr _MusicPlayerOnTimeStat_Init();
	  [DllImport("__Internal")]
	  private static extern int _MusicPlayerOnTimeStat_Destroy(IntPtr instance);

      /**************************************************************************************************/
      /*!
       *    \fn      public static IntPtr Create()
       *             作成
       *    \return  クラスポインタ（失敗時は IntPtr.Zero）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static IntPtr Create() {
        // Debug.Log("musicPlayerOnTimeStatIOS:Create");
        if ( Application.platform == RuntimePlatform.IPhonePlayer ) {
			return _MusicPlayerOnTimeStat_Init();
	    }
	    return IntPtr.Zero;
      }

      /**************************************************************************************************/
      /*!
       *    \fn      public static void Release(IntPtr instance)
       *             解放
       *    \param   解放するインスタンス
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static void Release(IntPtr instance) {
        // Debug.Log("musicPlayerOnTimeStatIOS:Release");
        if ( Application.platform == RuntimePlatform.IPhonePlayer ) {
			_MusicPlayerOnTimeStat_Destroy(instance);
	    }
      }
    }

#else
    public class musicPlayerOnTimeStatDefault
    {
	  public static IntPtr Create() {
        Debug.Log("musicPlayerOnTimeStatDefault:Create");
        return IntPtr.Zero;
      }
	  public static void Release(IntPtr _instance) {
        Debug.Log("musicPlayerOnTimeStatDefault:Release");
      }
    }
#endif


  }	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/

