using UnityEngine;
using System.Collections;

/**************************************************************************************************/
/*!
 *    \file    MaterialExtensions
 *             Material 拡張
 *    \date    2015. 1.29(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

public static class MaterialExtensions {

  /**************************************************************************************************/
  /*!
   *    \fn      public static void SetAlpha(this Material self, float a)
   *             α値の設定
   *    \param   a : α値
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static void SetAlpha(this Material self, float a) {
    var color = self.color;
    color.a = a;
    self.color = color;
  }

}

/********************************************** End of File *********************************************/
