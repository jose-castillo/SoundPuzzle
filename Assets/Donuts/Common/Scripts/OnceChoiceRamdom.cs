using UnityEngine;
using System;

/**************************************************************************************************/
/*!
 *    \file    OnceChoiceRandom.cs
 *             今まで選択されてないモノだけランダムで取得する乱数
 *    \date    2014.12.22(Mon)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
namespace Donuts
{
  namespace Common
  {
    public class OnceChoiceRandom
    {
      private int       startNo;
      private int       totalNum;
	    private int       remainNum;
	    private bool[]    flagTbl;

      /**************************************************************************************************/
      /*!
       *    \fn      OnceChoiceRandom(int sno, int num)
       *             コンストラクタ（sno 〜 sno+(num-1) の範囲）
       *    \param   sno : 乱数の開始数
       *    \param   num : 乱数の幅
       *    \date    2014.12.22(Mon)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public OnceChoiceRandom(int sno, int num) {
        startNo   = sno;
        totalNum  = num;
        remainNum = num;
        flagTbl = new bool[totalNum];
        ClearAllFlag();
      }      

      /**************************************************************************************************/
      /*!
       *    \fn      void ClearFlag(int index)
       *             フラグクリア
       *    \date    2014.12.22(Mon)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public void ClearAllFlag() {
        for (int i=0 ; i<totalNum ; i++) flagTbl[i] = false;		
      }

      /**************************************************************************************************/
      /*!
       *    \fn      int[] makeRemainTable()
       *             まだ取得されていない値をテーブル化
       *    \return  取得されていない値のテーブル
       *    \date    2014.12.22(Mon)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      private int[] makeRemainTable() {
        if (remainNum <= 0) return null;
        int[] remTbl = new int[remainNum];
        int remId = 0;
        for (int i=0 ; i<totalNum ; i++) {
          if (!flagTbl[i]) {
            remTbl[remId] = i;
            remId++;
          }
        }
        return remTbl;
      }

      /**************************************************************************************************/
      /*!
       *    \fn      int Choice()
       *             ランダム値を取得する
       *    \return  ランダム値
       *    \date    2014.12.22(Mon)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public int Choice() {
        int[] remTbl = makeRemainTable();
        if (remTbl == null) return 0;

        int rndIdx = Donuts.Common.Random.GetInt(remTbl.Length);
        int selNo  = remTbl[rndIdx];
        flagTbl[selNo] = true;
        remTbl = null;
        remainNum--;
        if (remainNum <= 0) {
          remainNum = totalNum;
          ClearAllFlag();
        }
        return (selNo + startNo);
      }
    }

  }  // Common
}  // Donuts


/********************************************** End of File *********************************************/
