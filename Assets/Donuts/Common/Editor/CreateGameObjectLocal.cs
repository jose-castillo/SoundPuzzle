using UnityEngine;
using UnityEditor;

/**************************************************************************************************/
/*!
 *    \file    CreateGameObjectLocal.cs
 *             Hierarchy で選択している GameObject の子供の GameObject を作成するエディタ拡張
 *    \remarks 参考：http://wiki.unity3d.com/index.php/CreateGameObjectLocal
 *    \date    2014.12.22(Mon)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
namespace Donuts
{
  namespace Common
  {
    public class CreateGameObjectLocal : Editor
    {
      [MenuItem ("GameObject/Create Empty Local #&n")]

      static void CreateEmptyLocal() {
        if(Selection.activeTransform != null)
        { 
          // Check if the selected object is a prefab instance and display a warning
          PrefabType type = PrefabUtility.GetPrefabType( Selection.activeGameObject );
          if(type == PrefabType.PrefabInstance) {
            if(!EditorUtility.DisplayDialog("Losing prefab", 
                                            "This action will lose the prefab connection. Are you sure you wish to continue?", 
                                            "Continue", "Cancel"))
            {
              return; // The user does not want to break the prefab connection so do nothing.
            }
          }
        }

        // Create our new GameObject
        GameObject newGameObject = new GameObject();
        newGameObject.name = "GameObject";
 
        // Undo 関数変更の為、GameObject 作成後に登録するように変更
        Undo.RegisterCreatedObjectUndo(newGameObject, "Create Empty Local"); 
 
        // If there is a selected object in the scene then make the new object its child.
        if(Selection.activeTransform != null) {
          newGameObject.transform.parent = Selection.activeTransform;
          newGameObject.name             = Selection.activeTransform.gameObject.name + "Child";

          // layer も継承するように修正
          newGameObject.layer = Selection.activeTransform.gameObject.layer;
 
          // Place the new GameObject at the same position as the parent.
          newGameObject.transform.localPosition = Vector3.zero;
          newGameObject.transform.localRotation = Quaternion.identity;
          newGameObject.transform.localScale    = Vector3.one;
        }
 
        // Select our newly created GameObject
        Selection.activeTransform = newGameObject.transform;
      }
    }

  }  // Common		
}  // Donuts

/********************************************** End of File *********************************************/

