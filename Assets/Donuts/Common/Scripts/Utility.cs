using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_DITOR

/**************************************************************************************************/
/*!
 *    \file    Utillity.cs
 *             汎用関数群
 *    \date    2014.12.22(Mon)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
namespace Donuts
{
  namespace Common
  {
    public class Utility
    {
      /**************************************************************************************************/
      /*!
       *    \fn      bool IsFloatEqual(float a, float b)
       *             float での一致判定
       *    \param   a : 比較する値
       *    \param   b : 比較される値
       *    \return  true で一致
       *    \date    2014.12.22(Mon)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static public bool IsFloatEqual(float a, float b) {
        if (a >= b-Mathf.Epsilon && a <= b + Mathf.Epsilon) {
          return true;
        } else {
          return false;
        }
      }

      /**************************************************************************************************/
      /*!
       *    \fn      IsSmartPhone()
       *             スマートフォンなのか？(Android or iOS)
       *    \return  true でスマホ
       *    \date    2014.12.22(Mon)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static public bool IsSmartPhone() {
        if ((Application.platform == RuntimePlatform.Android) ||
            (Application.platform == RuntimePlatform.IPhonePlayer)) {
          return true;
        }
        else {
          return false;
        }
      }

      /**************************************************************************************************/
      /*!
       *    \fn      Dictionary<string, string> GetDeviceInfoToDictionary()
       *             デバイス情報をDictionaryで返す
       *    \return  デバイス情報
       *    \date    2014.12.22(Mon)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static public Dictionary<string, string> GetDeviceInfoToDictionary() {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["device_model"]    = SystemInfo.deviceModel.ToString();
        data["device_os"]       = SystemInfo.operatingSystem.ToString();
        data["device_width"]    = Screen.width.ToString();
        data["device_height"]   = Screen.height.ToString();
#if UNITY_IPHONE
        if	(SystemInfo.operatingSystem.Contains("iPhone")) {
          data["device_gen"]    = UnityEngine.iOS.Device.generation.ToString();
        }
#endif  // UNITY_IPHONE
        return data;
      }  

      /**************************************************************************************************/
      /*!
       *    \fn      public static byte[] ConvertByte(object data)
       *             与えられた型を byte 列に変換して返す
       *    \param   data : 変換するデータ
       *    \return  バイト列（失敗時は null）
       *    \date    2015. 1.29(The)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      public static byte[] ConvertByte(object data) {
        // 型を調べてbyte[]に変換
        Type chk = data.GetType();

        // int -> byte[]
        if (chk.Equals (typeof (int))) {
          return BitConverter.GetBytes((int)data);
        }        
        // float -> byte[]
        else if (chk.Equals (typeof (float))) {
          return BitConverter.GetBytes((float)data);
        }
        // string -> byte[]
        else if (chk.Equals (typeof (string))) {
          return Encoding.UTF8.GetBytes(data.ToString());
        }
        // byte[]
        else if (chk.Equals (typeof (byte[]))) {
          return (byte[])data;
        }
        // それ以外
        else {
          return null;
        }
      }
    } // Utillity
  } // Common
} // Donuts

/********************************************** End of File *********************************************/
