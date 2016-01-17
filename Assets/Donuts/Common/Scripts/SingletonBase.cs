using UnityEngine;
using System.Collections;

/**************************************************************************************************/
/*!
 *    \file    SingletonBase.cs
 *             MonoBehaviourを継承しないシングルトンクラスのベース
 *    \remarks デフォルトコンストラクタが必要
 *    \date    2014.12.22(Mon)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
namespace Donuts
{
  namespace Common
  {
    public class SingletonBase<T> where T : new()
    {
      private static T instance;
      public static T Instance {
        get {
          if (instance == null) {
            instance = new T();
          }
          return instance;
        }
        set {
          instance = value;
        }
      }
    }

  } // Common
} // Donuts

/********************************************** End of File *********************************************/
