using UnityEngine;
using System.Collections;

/**************************************************************************************************/
/*!
 *    \file    SingletonMonoBehaviour.cs
 *             MonoBehaviourを継承したシングルトンクラスのベース
 *    \remarks デフォルトコンストラクタが必要
 *    \date    2014.12.22(Mon)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
namespace Donuts
{
  namespace Common
  {
    /// <summary>
    /// MonoBehaviourを継承したシングルトンテンプレート
    /// </summary>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
      /// <summary>
      /// 自身のインスタンス
      /// </summary>
      protected static T instance;

      /// <summary>
      /// 自身のインスタンスへのアクセサ
      /// </summary>
      public static T Instance
      {
        get {
          if(instance == null) {
            instance = (T)FindObjectOfType(typeof(T));

            if(instance == null) {
              //Debug.LogError( "Instance [ " + typeof(T) + " ] is none " );
              //GameObject go = new GameObject(typeof(T).Name);
              GameObject go = new GameObject();
              instance      = go.AddComponent<T>();
              go.name       = instance.GetType().ToString();
              DontDestroyOnLoad(go);
            }
          }
          return instance;
        }
      }
    }

  } // Common
} // Donuts
