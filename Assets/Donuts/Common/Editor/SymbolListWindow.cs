using UnityEngine;
using System;
using UnityEditor;

/**************************************************************************************************/
/*!
 *    \file    SymbolListWindow
 *             #define で定義されているシンボルを一覧で表示するウィンドウ
 *    \date    2015. 1.30(Fri)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

public sealed class SymbolListWindow : EditorWindow {
  private Vector2 mScrollPos; // スクロールの座標

  /**************************************************************************************************/
  /*!
   *    \fn      private static void Open()
   *             ウィンドウを開く
   *    \date    2015. 1.30(Fri)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  [MenuItem("Donuts/Symbol List Window")]
  private static void Open() {
    GetWindow<SymbolListWindow>("Symbol List");
  }

  /**************************************************************************************************/
  /*!
   *    \fn      private void OnGUI()
   *             GUI を表示
   *    \date    2015. 1.30(Fri)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  private void OnGUI() {
    // スクロールビューの表示を開始します
    mScrollPos = EditorGUILayout.BeginScrollView(mScrollPos, GUILayout.Height(position.height));

    // 定義されているシンボルを取得します
    var defines = EditorUserBuildSettings.activeScriptCompilationDefines;

    // 取得したシンボルを名前順でソートします
    Array.Sort(defines);

    // 定義されているシンボルの一覧を表示します
    foreach (var define in defines) {
      EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

      // Copy ボタンが押された場合
      if (GUILayout.Button("Copy", GUILayout.Width(50), GUILayout.Height(20))) {
        // クリップボードにシンボル名を登録します
        EditorGUIUtility.systemCopyBuffer = define;
      }

      // 選択可能なラベルを使用してシンボル名を表示します
      EditorGUILayout.SelectableLabel(define, GUILayout.Height(20));

      EditorGUILayout.EndHorizontal();
    }
    
    // スクロールビューの表示を終了します
    EditorGUILayout.EndScrollView();
  }
}

/********************************************** End of File *********************************************/
