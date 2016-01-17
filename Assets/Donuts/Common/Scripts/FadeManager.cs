using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Donuts.Common;

/**************************************************************************************************/
/*!
 *    \file    FadeManager.cs
 *             フェード制御
 *    \date    2015. 2.12(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

namespace Donuts
{
  namespace Common
  {

    public class FadeManager : MonoBehaviour {

      //!< フェード状態
      public enum FadeStat {
        FadeIn = 0,   //!< フェードイン
        FadeOut,      //!< フェードアウト
        Fading        //!< フェード中
      };

      //!< インスタンス
      static FadeManager  instance = null;

      // フェード
      public   Image      fadePanel;                    //!< フェード用 Panel
      private  float      fadeAlpha = 0;                //!< フェード中の透明度
      public   float      interval  = 1f;               //!< フェード時間（秒）
      public   FadeStat   stat      = FadeStat.FadeIn;  //!< フェード状態

      // インジケータ
      // todo : とりあえず表示だけ  2015.2.12(Thu) M.Matsuyama@Donuts
      public   Transform  indicator;                    //!< Transform
      public   Image      indicatorImg;                 //!< イメージ
      private  bool       indicatorAct = false;         //!< アクティブ状態

      // プログレス
      public   GameObject progressObj;
      public   Text       progressText;

      /**************************************************************************************************/
      /*!
       *    \fn      public static FadeManager Instance
       *             インスタンスの取得（生成）
       *    \date    2015. 2.12(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static FadeManager Instance {
        get {
          return Create();
        }
      }

      /**************************************************************************************************/
      /*!
       *    \fn      FadeManager Create()
       *             インスタンス生成
       *    \return  インスタンス
       *    \date    2015. 2.12(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static FadeManager Create() {
        if(instance == null) {
          // インスタンス生成       
          GameObject obj = Instantiate(Resources.Load("CanvasFade")) as GameObject;
          obj.name = "_FadeManager";
          Debug.Log("create " + obj.name);
        }
        return instance;
      }

      /**************************************************************************************************/
      /*!
       *    \fn      void Release()
       *             解放
       *    \date    2015. 2.12(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public void Release() {
        if(instance != null) {
          Destroy(instance.gameObject);
          instance = null;
        }
      }

      /**************************************************************************************************/
      /*!
       *    \fn      void Awake()
       *             起動時処理
       *    \date    2015. 2.12(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      void Awake() {
        if(instance != null) {
          // 既にある場合は自身を破棄
          Destroy(this.gameObject);
        }
        else{
          instance = this;
          DontDestroyOnLoad(this.gameObject);

        }

        gameObject.SetActive(false);    // デフォルトは非アクティブ
      }

      /**************************************************************************************************/
      /*!
       *    \fn      public void FadeStart(FadeStat type)
       *             フェード開始
       *    \param   type : フェードタイプ
       *    \date    2015. 2.12(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public void FadeStart(FadeStat type) {
        switch(type) {
        case FadeStat.FadeIn:
          StartCoroutine(fadeIn());
          break;
        case FadeStat.FadeOut:
          gameObject.SetActive(true);   // フェードアウト開始時にアクティブへ切り替え
          StartCoroutine(fadeOut());
          break;
        case FadeStat.Fading:
          return;                       // フェード中は受付けない
        }
        stat = FadeStat.Fading;
      }

      /**************************************************************************************************/
      /*!
       *    \fn      public void SetBlack()
       *             フェードアウト状態にする
       *    \date    2015. 2.12(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public void SetBlack() {
        if(stat == FadeStat.FadeIn) {
          gameObject.SetActive(true);   // フェードアウト開始時にアクティブへ切り替え
          fadeAlpha = 1f;
          setAlpha();
          stat = FadeStat.FadeOut;
          // インジケータ開始
          StartCoroutine(updateIndicator());
        }
      }

      /**************************************************************************************************/
      /*!
       *    \fn      private IEnumerator fadeIn()
       *             フェードイン処理
       *    \date    2015. 2.12(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      private IEnumerator fadeIn() {
        // Debug.Log("Start Fade In");
        float time = 0;
        while (time <= interval) {
         fadeAlpha = Mathf.Lerp (1f, 0f, time / interval);
         time += Time.deltaTime;
         setAlpha();
         yield return 0;
        }
        stat = FadeStat.FadeIn;

        // インジケータ停止
        indicatorAct = false;

        // フェードイン完了時に非アクティブに
        yield return 0;
        gameObject.SetActive(false);
      }

      /**************************************************************************************************/
      /*!
       *    \fn      private IEnumerator fadeOut()
       *             フェードアウト処理
       *    \date    2015. 2.12(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      private IEnumerator fadeOut() {
        // インジケータ開始
        StartCoroutine(updateIndicator());

        // Debug.Log("Start Fade Out");
        float time = 0;
        while (time <= interval) {
         fadeAlpha = Mathf.Lerp (0f, 1f, time / interval);
         time += Time.deltaTime;
         setAlpha();
         yield return 0;
        }
        stat = FadeStat.FadeOut;
      }

      /**************************************************************************************************/
      /*!
       *    \fn      private void setAlpha()
       *             α値設定
       *    \date    2015. 2.12(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      private void setAlpha() {
        Color col = fadePanel.color;
        col.a = fadeAlpha;
        fadePanel.color = col;
      }

      /**************************************************************************************************/
      /*!
       *    \fn      private void updateIndicator()
       *             インジケータ制御
       *    \date    2015. 2.12(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      private IEnumerator updateIndicator() {
        // Debug.Log("Indicator Start");
        indicatorAct = true;

        while(indicatorAct) {
          // todo : とりあえず、回すだけ  2015.2.12(Thu) M.Matsuyama@Donuts
          indicator.AddEulerAnglesZ(-3f);

          // α値
          Color col = indicatorImg.color;
          col.a = fadeAlpha;
          indicatorImg.color = col;

          yield return 0;
        }
        // Debug.Log("Indicator Stop");
      }

      /**************************************************************************************************/
      /*!
       *    \fn      public void ShowProgress(bool isShow)
       *             プログレスの表示
       *    \param   isShow : TRUE で表示（デフォルトは非表示）
       *    \date    2015. 2.17(Tue)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public void ShowProgress(bool isShow) {
        progressObj.SetActive(isShow);
      }

      /**************************************************************************************************/
      /*!
       *    \fn      public void SetProgress(float progress)
       *             プログレス値の更新
       *    \param   progress : プログレス値（0f ～ 1f）
       *    \date    2015. 2.17(Tue)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public void SetProgress(float progress) {
        if(!progressObj.activeSelf) return;

        float val = progress * 100.0f;
        progressText.text = val.ToString("F2") + "%";
      }

    } // FadeManager
  } // Common
} // Donuts

/********************************************** End of File *********************************************/
