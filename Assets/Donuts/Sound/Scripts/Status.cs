using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**************************************************************************************************/
/*!
 *    \file    Status.cs
 *             サウンド関係全体でのステータス管理
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

namespace Donuts
{
	namespace Sound
	{
    public class Status  {
			static private bool 	isMuteBGM;						//!< BGMミュートスイッチ
			static private bool 	isMuteSE;							//!< SEミュートスイッチ
			static private bool 	isPause;							//!< Pauseスイッチ
			static private float 	bgmBaseVolume = 1.0f;	//!< BGM用ベースボリューム
			static private float 	seBaseVolume  = 1.0f;	//!< BGM用ベースボリューム

      /**************************************************************************************************/
      /*!
       *    \fn      static public float BaseVolumeBGM
       *             BGM 用ベースボリューム（デフォルト 1.0f）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			static public float BaseVolumeBGM {
				get {
					return bgmBaseVolume;
				}
				set {
					if( (value >= 0.0f) && (value <= 1.0f) && (value != bgmBaseVolume) ) {
						bgmBaseVolume = value;
					}
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      static public float BaseVolumeSE
       *             SE 用ベースボリューム（デフォルト 1.0f）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			static public float BaseVolumeSE {
				get {
					return seBaseVolume;
				}
				set {
					if( (value >= 0.0f) && (value <= 1.0f) && (value != seBaseVolume) ) {
						seBaseVolume = value;
					}
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      static public void ClearStatus()
       *             ステータスクリア
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			static public void ClearStatus() {
				isPause   		= false;
				isMuteBGM 		= false;
				isMuteSE  		= false;
				bgmBaseVolume = 1.0f;
				seBaseVolume  = 1.0f;
			}

      /**************************************************************************************************/
      /*!
       *    \fn      static public bool IsPause
       *             ポーズ状態のプロパティ
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			static public bool IsPause {
				get {
					return isPause;
				}
				set {
					// 動作中の時は各管理クラスにポーズステータス変更の通知
					if (Donuts.Sound.Manager.GetManagerObj() != null) {
						if (isPause) {
							if (!value) {
								// ポーズを解除する
								Donuts.Sound.EasyPlay.SetAllPause(false);
								Donuts.Sound.BGMManager.Instance.Pause(false);
							}
						}
						else {
							if (value) {
								// ポーズする
								Donuts.Sound.EasyPlay.SetAllPause(true);
								Donuts.Sound.BGMManager.Instance.Pause(true);
							}
						}
					}
					isPause = value;
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      static public bool IsMuteBGM
       *             BGM ミュート状態のプロパティ
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			static public bool IsMuteBGM {
				get {
					return isMuteBGM;
				}
				set {
					isMuteBGM = value;
				}
			}

      /**************************************************************************************************/
      /*!
       *    \fn      static public bool IsMuteSE
       *             SE ミュート状態のプロパティ
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
			static public bool IsMuteSE {
				get {
					return isMuteSE;
				}
				set {
					isMuteSE = value;
				}
			}

		}

	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
