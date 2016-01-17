using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Collections.Generic;

// Find 系に BaseHierarchySort を使ってソートする
#if UNITY_EDITOR
using UnityEditor;
#endif

// ランタイム時はこちらの BaseHierarchySort を使う
#if !UNITY_EDITOR
public class BaseHierarchySort : System.Collections.Generic.IComparer<GameObject>
{
  public virtual int Compare (GameObject lhs, GameObject rhs) {
    return 0;
  }

  public virtual GUIContent content { get { return GUIContent.none; } }
}
#endif  // UNITY_EDITOR

/**************************************************************************************************/
/*!
 *    \file    GameObjectExtensions
 *             GameObject 拡張
 *    \remarks 元ネタ : UNIBOOK-1.0.0
 *    \date    2015. 1.23(Fri)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

public static class GameObjectExtensions {

  private class BaseHierarchySortCompare : IComparer<GameObject> {
    private readonly BaseHierarchySort _hierarchySort;

    public BaseHierarchySortCompare (Type hierarchySortType) {
      _hierarchySort = (BaseHierarchySort)Activator.CreateInstance (hierarchySortType);
    }

    public int Compare (GameObject x, GameObject y) {
      return _hierarchySort.Compare (x, y);
    }
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static GameObject[] GetChildren(this GameObjcect self, bool includeInactive = false)
   *             すべての子オブジェクトを返します
   *    \param   includeInactive : 非アクティブなオブジェクトも取得する場合 true
   *    \return  すべての子オブジェクトを管理する配列
   *    \remarks UNIBOOK からの転用ではない
   *    \date    2015. 1.23(Fri)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static GameObject[] GetChildren( this GameObject self, bool includeInactive = false ) {
    return self.GetComponentsInChildren<Transform>( includeInactive )
        .Where( c => c != self.transform )
        .Select( c => c.gameObject )
        .ToArray();
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static GameObject[] FindGameObjectsWithTag<T> (string tag) where T : BaseHierarchySort
   *             Tag名からゲームオブジェクトをすべて取得し，BaseHierarchySortで比較してソートする
   *    \param   T   : 型
   *    \param   tag : Tag 名
   *    \return  一致するゲームオブジェクト
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static GameObject[] FindGameObjectsWithTag<T> (string tag) where T : BaseHierarchySort {
    var gameObjects = GameObject.FindGameObjectsWithTag (tag).ToList ();
    gameObjects.Sort (new BaseHierarchySortCompare (typeof(T)));
    return gameObjects.ToArray ();
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static GameObject[] FindGameObjectsWithTag (string tag, Predicate<GameObject> condition)
   *             Tag名かつ条件に合うゲームオブジェクトをすべて取得
   *    \param   tag       : Tag 名
   *    \param   condition : 条件
   *    \return  一致するゲームオブジェクト
   *    \remark  Rigidbodyがアタッチされているゲームオブジェクトをすべて取得
   *             var gameObjects = GameObjectExtension.FindGameObjectsWithTag("Player", go => go.GetComponent<Rigidbody>());
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static GameObject[] FindGameObjectsWithTag (string tag, Predicate<GameObject> condition) {
    return GameObject.FindGameObjectsWithTag (tag).Where (go => condition (go)).ToArray ();
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static GameObject FindGameObjectsWithTag (string tag, Predicate<GameObject> condition)
   *             Tag名かつ条件に合うゲームオブジェクトを取得
   *    \param   tag       : Tag 名
   *    \param   condition : 条件
   *    \return  一致するゲームオブジェクト
   *    \remark  全取得した後，条件に合うゲームオブジェクトを取得するので速度は遅い。
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static GameObject FindGameObjectWithTag (string tag, Predicate<GameObject> condition) {
    return GameObject.FindGameObjectsWithTag (tag).FirstOrDefault (go => condition (go));
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static TSource[] FindObjectsOfType<TSource, TKey> ()
   *             タイプのオブジェクトをすべて取得し，BaseHierarchySortで比較してソートする
   *    \param   TSource : 型
   *    \param   TKey    : キー
   *    \return  一致するオブジェクト
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static TSource[] FindObjectsOfType<TSource, TKey> ()
      where TSource : Component
      where TKey : BaseHierarchySort
  {
    var objs = Object.FindObjectsOfType<TSource> ();
    return objs.OrderBy (obj => obj.gameObject, new BaseHierarchySortCompare (typeof(TKey))).ToArray ();
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static T[] FindObjectsOfType<T> (Predicate<T> condition) where T : Object
   *             条件に合うタイプのオブジェクトをすべて取得
   *    \param   T         : 型
   *    \param   condition : 条件
   *    \return  一致するオブジェクト
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static T[] FindObjectsOfType<T> (Predicate<T> condition) where T : Object {
    return Object.FindObjectsOfType<T> ().Where (o => condition (o)).ToArray ();
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static T FindObjectsOfType<T> (Predicate<T> condition) where T : Object
   *             条件に合うタイプのオブジェクトを取得
   *    \param   T         : 型
   *    \param   condition : 条件
   *    \return  一致するオブジェクト
   *    \remarks 全取得した後，条件に合うオブジェクトを取得するので速度は遅い
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static T FindObjectOfType<T> (Predicate<T> condition) where T : Object {
    return Object.FindObjectsOfType<T> ().FirstOrDefault (o => condition (o));
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static T Instantiate<T> (T original, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion)) where T : Object
   *             オブジェクトの複製
   *    \param   T        : 型
   *    \param   original : 複製するオブジェクト
   *    \param   position : 位置
   *    \param   rotation : 回転
   *    \return  複製されたオブジェクト
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static T Instantiate<T> (T original, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion)) where T : Object {
    return (T)Object.Instantiate (original, position, rotation);
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static void SetTagRecursively (this GameObject gameObject, string tag)
   *             Tag 名を設定（自分も含む子オブジェクト全て）
   *    \param   tag : Tag 名
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static void SetTagRecursively (this GameObject gameObject, string tag) {
    gameObject.tag = tag;
    foreach (Transform child in gameObject.transform)
      child.gameObject.tag = tag;
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static void SetLayerRecursively (this GameObject gameObject, int layer)
   *             レイヤーの設定（自分も含む子オブジェクト全て）
   *    \param   layer : layer 番号
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static void SetLayerRecursively (this GameObject gameObject, int layer) {
    gameObject.layer = layer;
    foreach (Transform child in gameObject.transform)
      child.gameObject.layer = layer;
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static void SetLayerRecursively (this GameObject gameObject, string layerName)
   *             レイヤーの設定（自分も含む子オブジェクト全て）
   *    \param   layer : layer 名
   *    \date    2015. 1.29(Thu)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static void SetLayerRecursively (this GameObject gameObject, string layerName) {
    gameObject.SetLayerRecursively (LayerMask.NameToLayer (layerName));
  }
    
}

/********************************************** End of File *********************************************/
