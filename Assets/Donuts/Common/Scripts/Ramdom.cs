using UnityEngine;
using System;

/**************************************************************************************************/
/*!
 *    \file    Random.cs
 *             ランダム（Unity 内蔵ランダムのラッパー）
 *    \date    2014.12.22(Mon)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
namespace Donuts
{
  namespace Common
  {
    public class Random
    {
      /**************************************************************************************************/
      /*!
       *    \fn      void SetAutoSeed()
       *             日時を使用してシードを設定
       *    \date    2014.12.22(Mon)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static public void SetAutoSeed() {
        DateTime nowDate = DateTime.Now;
        string seedStr = "" + nowDate.Millisecond + nowDate.Hour+nowDate.Second + nowDate.Minute;
        int seedInt;
        if (!Int32.TryParse(seedStr, out seedInt)) {
          seedInt = 301301321;
        }
        // Debug.Log("SetAutoRandomSeed:"+seedStr+" INT:"+seedInt);
        UnityEngine.Random.seed = seedInt;
      }
	
      /**************************************************************************************************/
      /*!
       *    \fn      float GetFloat()
       *             Unity デフォルトのランダムで float (0.0f ～ 1.0f) を取得
       *    \return  0.0f ～ 1.0f ※ 1.0f を含む
       *    \date    2014.12.22(Mon)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static public float GetFloat() {
        return UnityEngine.Random.value;
      }

      /**************************************************************************************************/
      /*!
       *    \fn      int GetInt(int num)
       *             ランダムで整数値を取得
       *    \param   num : 乱数の範囲
       *    \return  0 〜 num ※ num 未満
       *    \date    2014.12.22(Mon)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static public int GetInt(int num) {
        if (num <= 0) return 0;
        return UnityEngine.Random.Range(0, num);
      }

    }
  }  // Common
}  // Donuts

/********************************************** End of File *********************************************/
