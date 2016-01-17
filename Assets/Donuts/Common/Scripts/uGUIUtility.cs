using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

/**************************************************************************************************/
/*!
 *    \file    uGUIUtillity.cs
 *             uGUI 汎用関数
 *    \date    2014.12.26(Fri)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/


/**************************************************************************************************/
/*!
 *    \class   ToggleGroupExtension
 *             ToggleGroup 拡張
 *    \date    2014.12.26(Fri)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
public static class ToggleGroupExtension
{

  /**************************************************************************************************/
  /*!
   *    \fn      public static Toggle GetActive(this ToggleGroup aGroup)
   *             ToggleGroup から選択中の Toggle を取得
   *    \param   aGroup : ToggleGroup
   *    \return  選択中の Toggle
   *    \date    2014.12.26(Fri)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static Toggle GetActive(this ToggleGroup aGroup) {
    return aGroup.ActiveToggles().FirstOrDefault();
  }

  /**************************************************************************************************/
  /*!
   *    \fn      public static Toggle GetActiveText(this ToggleGroup aGroup)
   *             ToggleGroup から選択中の Toggle 下の Label.Text を取得
   *    \param   aGroup : ToggleGroup
   *    \return  Toggle の Text(Label)
   *    \date    2014.12.26(Fri)
   *    \author  Masayoshi.Matsuyama@Donuts
   */
  /**************************************************************************************************/
  public static Text GetActiveText(this ToggleGroup aGroup) {
    Toggle      active = GetActive(aGroup);
    if(active != null) {
      GameObject  label  = active.transform.Find("Label").gameObject;
      return label.GetComponent<Text>();
    }
    else {
      return null;
    }
  }
}

/********************************************** End of File *********************************************/
