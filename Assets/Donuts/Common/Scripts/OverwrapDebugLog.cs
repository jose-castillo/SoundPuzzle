#if RELEASE_BUILD
#define DEBUG_LOG_OVERWRAP
#endif

#if DEBUG_LOG_OVERWRAP
using UnityEngine;

/**************************************************************************************************/
/*!
 *    \file    OverwrapDebugLog.cs
 *             リリースビルド時のデバッグログ抑制
 *    \date    2014.12.22(Mon)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/
public static class Debug
{
  static public void Break(){
    if(IsEnable ()) UnityEngine.Debug.Break ();
  }
  static bool IsEnable(){ return UnityEngine.Debug.isDebugBuild; }

  static public void Log( object message ){
  }

  static public void Log( object message, Object context ) {
  }

  static public void LogError( object message ){
  }
  
  static public void LogError( object message, Object context ) {
  }
  
  static public void LogWarning( object message ){
  }
  
  static public void LogWarning( object message, Object context ) {
  }

  static public void DrawLine(Vector3 start, Vector3 end, Color color, float duration=0f, bool depthTest=true)
  {
    if(IsEnable ()) UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
  }
}
#endif  // DEBUG_LOG_OVERWRAP


/********************************************** End of File *********************************************/
