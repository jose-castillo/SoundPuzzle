using UnityEngine;
using System.Collections;
using System.Linq;

/**************************************************************************************************/
/*!
 *    \file    ComponentExtensions
 *             Component 拡張
 *    \date    2015. 1.23(Fri)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

public static class ComponentExtensions {

  /**************************************************************************************************/
  /*!
   *    \fn      public static GameObject[] GetChildren( this Component self, bool includeInactive = false )
   *             すべての子オブジェクトを返します
   *    \param   self            : Component 型のインスタンス
   *    \param   includeInactive : 非アクティブなオブジェクトも取得する場合 true
   *    \return  すべての子オブジェクトを管理する配列
   *    \date    2015. 1.23(Fri)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static GameObject[] GetChildren( this Component self, bool includeInactive = false ) {
    return self.GetComponentsInChildren<Transform>( includeInactive )
        .Where( c => c != self.transform )
        .Select( c => c.gameObject )
        .ToArray();
  }
  
}

/********************************************** End of File *********************************************/
