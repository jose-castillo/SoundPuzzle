using UnityEngine;
using System.Collections;
using UnityEditor;

/**************************************************************************************************/
/*!
 *    \file    PolygonCounter.cs
 *             ポリゴン数の表示
 *    \date    2014.12.29(Mon)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

[CustomEditor(typeof(MeshFilter))]
public class PolygonCounter : Editor {

  public override void OnInspectorGUI() {
    base.OnInspectorGUI();
    MeshFilter filter = target as MeshFilter;
    string polygons = "Triangles: " + filter.sharedMesh.triangles.Length / 3;
    EditorGUILayout.LabelField(polygons);
  }

}
