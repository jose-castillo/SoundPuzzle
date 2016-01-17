using UnityEngine;
using UnityEditor;
using System.Collections;

/**************************************************************************************************/
/*!
 *    \file    EditorSound.cs
 *             Unityのメニューから Donuts/Sound のメニュー管理
 *    \date    2014.12.25(Thu)
 *    \author  Masayoshi.Matsuyama@Donuts
 */
/**************************************************************************************************/

namespace Donuts
{
	namespace Sound
	{
    public class EditorSound : EditorWindow
    {

      /**************************************************************************************************/
      /*!
       *    \fn      static void OpenSoundCacheWindow()
       *             SoundCacheWindow を出す
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
	    [MenuItem("Donuts/Sound/Cache")]
      static void OpenSoundCacheWindow() {
		    EditorSoundCacheWindow winEditorSoundCacheWindow = (EditorSoundCacheWindow)EditorWindow.GetWindow (typeof(EditorSoundCacheWindow));
		    winEditorSoundCacheWindow.minSize = new Vector2(600.0f, 300.0f);
	    }

      /**************************************************************************************************/
      /*!
       *    \fn      static void OpenSoundStatus()
       *             SoundStatusを出す
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      [MenuItem("Donuts/Sound/Status")]
      static void OpenSoundStatus() {
		    EditorSoundStatus winEditorSoundStatus = (EditorSoundStatus)EditorWindow.GetWindow (typeof(EditorSoundStatus));
		    winEditorSoundStatus.minSize = new Vector2(300.0f, 150.0f);
	    }

      /**************************************************************************************************/
      /*!
       *    \fn      static void SetNormalSetting()
       *             Audioデータの一括設定を行う(オススメ設定)
       *    \remarks オススメ設定で処理します
       *               Format   : Compressed
       *               LoadType : StreamFromDisc
       *               3D Sound : Disable
       *             選択しているフォルダ以下を再帰的に処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      [MenuItem ("Donuts/Sound/AudioSetting/Normal Setting")]
      static void SetNormalSetting() {
        SelectedSetNormal();
      }

      /**************************************************************************************************/
      /*!
       *    \fn      static void ToggleCompression_Disable()
       *             Audioデータの一括設定を行う(圧縮設定)
       *    \remarks 圧縮設定を OFF
       *             選択しているフォルダ以下を再帰的に処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      [MenuItem ("Donuts/Sound/AudioSetting/Toggle audio compression/Disable")]
      static void ToggleCompression_Disable() {
        SelectedToggleCompressionSettings(AudioImporterFormat.Native);
      }

      /**************************************************************************************************/
      /*!
       *    \fn      static void ToggleCompression_Enable()
       *             Audioデータの一括設定を行う(圧縮設定)
       *    \remarks 圧縮設定を ON
       *             選択しているフォルダ以下を再帰的に処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      [MenuItem ("Donuts/Sound/AudioSetting/Toggle audio compression/Enable")]
      static void ToggleCompression_Enable() {
        SelectedToggleCompressionSettings(AudioImporterFormat.Compressed);
      }

      /**************************************************************************************************/
      /*!
       *    \fn      static void SetLoadTypeDecompressOnLoad()
       *             Audioデータの一括設定を行う(LoadType)
       *    \remarks LoadTypeを DecompressOnLoad
       *             選択しているフォルダ以下を再帰的に処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      [MenuItem ("Donuts/Sound/AudioSetting/Set load type/DecompressOnLoad")]
      static void SetLoadTypeDecompressOnLoad() {
		    SelectedSetLoadType(AudioClipLoadType.DecompressOnLoad);
      }

      /**************************************************************************************************/
      /*!
       *    \fn      static void SetLoadTypeDecompressOnLoad()
       *             Audioデータの一括設定を行う(LoadType)
       *    \remarks LoadTypeを CompressedInMemory
       *             選択しているフォルダ以下を再帰的に処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      [MenuItem ("Donuts/Sound/AudioSetting/Set load type/CompressedInMemory")]
      static void SetLoadTypeCompressedInMemory() {
		    SelectedSetLoadType(AudioClipLoadType.CompressedInMemory);
      }

      /**************************************************************************************************/
      /*!
       *    \fn      static void SetLoadTypeStreamFromDisc()
       *             Audioデータの一括設定を行う(LoadType)
       *    \remarks LoadTypeを StreamFromDisc
       *             選択しているフォルダ以下を再帰的に処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      [MenuItem ("Donuts/Sound/AudioSetting/Set load type/StreamFromDisc")]
      static void SetLoadTypeStreamFromDisc() {
		    SelectedSetLoadType(AudioClipLoadType.Streaming);
      }

      /**************************************************************************************************/
      /*!
       *    \fn      static void Toggle3DSound_Disable()
       *             Audioデータの一括設定を行う(3DSound)
       *    \remarks 3DSound を　OFF
       *             選択しているフォルダ以下を再帰的に処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      [MenuItem ("Donuts/Sound/AudioSetting/Toggle 3D sound/Disable")]
      static void Toggle3DSound_Disable() {
        SelectedToggle3DSoundSettings(false);
      }

      /**************************************************************************************************/
      /*!
       *    \fn      static void Toggle3DSound_Enable()
       *             Audioデータの一括設定を行う(3DSound)
       *    \remarks 3DSound を　ON
       *             選択しているフォルダ以下を再帰的に処理
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      [MenuItem ("Donuts/Sound/AudioSetting/Toggle 3D sound/Enable")]
      static void Toggle3DSound_Enable() {
        SelectedToggle3DSoundSettings(true);
      }

      /**************************************************************************************************/
      /*!
       *    \fn      static void SelectedSetNormal()
       *             Audioデータの一括設定を行う実際の処理(オススメ設定)
       *    \remarks オススメ設定で処理します
       *               Format   : Compressed
       *               LoadType : StreamFromDisc
       *               3D Sound : Disable
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static void SelectedSetNormal() {
        Object[] audioclips = GetSelectedAudioclips();
        Selection.objects = new Object[0];
        foreach (AudioClip audioclip in audioclips) {
          string path = AssetDatabase.GetAssetPath(audioclip);
          AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
          audioImporter.threeD        = false;
          AssetDatabase.ImportAsset(path);
        }
	    }

      /**************************************************************************************************/
      /*!
       *    \fn      static void SelectedToggleCompressionSettings(AudioImporterFormat newFormat)
       *             Audioデータの一括設定を行う実際の処理(圧縮設定)
       *    \param   newFormat : フォーマット
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static void SelectedToggleCompressionSettings(AudioImporterFormat newFormat) {
        Object[] audioclips = GetSelectedAudioclips();
        Selection.objects = new Object[0];
        foreach (AudioClip audioclip in audioclips) {
          string path = AssetDatabase.GetAssetPath(audioclip);
          AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
          AssetDatabase.ImportAsset(path);
        }
      }

      /**************************************************************************************************/
      /*!
       *    \fn      static void SelectedSetCompressionBitrate(int newCompressionBitrate)
       *             Audioデータの一括設定を行う実際の処理(ビットレート)
       *    \param   newCompressionBitrate : ビットレート （例：156000）
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
/*
      static void SelectedSetCompressionBitrate(int newCompressionBitrate) {
        Object[] audioclips = GetSelectedAudioclips();
        Selection.objects = new Object[0];
        foreach (AudioClip audioclip in audioclips) {
          string path = AssetDatabase.GetAssetPath(audioclip);
          AudioImporter audioImporter      = AssetImporter.GetAtPath(path) as AudioImporter;
          audioImporter.compressionBitrate = newCompressionBitrate;
          AssetDatabase.ImportAsset(path);
        }
      }
*/

      /**************************************************************************************************/
      /*!
       *    \fn      static void SelectedSetLoadType(AudioImporterLoadType loadtype)
       *             Audioデータの一括設定を行う実際の処理(LoadType)
       *    \param   loadType : ロードタイプ
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static void SelectedSetLoadType(AudioClipLoadType loadtype) {
        Object[] audioclips = GetSelectedAudioclips();
        Selection.objects = new Object[0];
        foreach (AudioClip audioclip in audioclips) {
          string path = AssetDatabase.GetAssetPath(audioclip);
          AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
          AssetDatabase.ImportAsset(path);
        }
      }

      /**************************************************************************************************/
      /*!
       *    \fn      static void SelectedToggle3DSoundSettings(bool enabled)
       *             Audioデータの一括設定を行う実際の処理(3DSound設定)
       *    \param   enabled : 3DSound にするか
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static void SelectedToggle3DSoundSettings(bool enabled) {
        Object[] audioclips = GetSelectedAudioclips();
        Selection.objects = new Object[0];
        foreach (AudioClip audioclip in audioclips) {
          string path = AssetDatabase.GetAssetPath(audioclip);
          AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
          audioImporter.threeD = enabled;
          AssetDatabase.ImportAsset(path);
        }
      }


      /**************************************************************************************************/
      /*!
       *    \fn      static void SelectedToggleForceToMonoSettings(bool enabled)
       *             Audioデータの一括設定を行う実際の処理(強制モノラル)
       *    \param   enabled : 強制モノラルにするか
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
/*
      static void SelectedToggleForceToMonoSettings(bool enabled) {
        Object[] audioclips = GetSelectedAudioclips();
        Selection.objects = new Object[0];
        foreach (AudioClip audioclip in audioclips) {
          string path = AssetDatabase.GetAssetPath(audioclip);
          AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
          audioImporter.forceToMono = enabled;
          AssetDatabase.ImportAsset(path);
        }
      }
*/

      /**************************************************************************************************/
      /*!
       *    \fn      static Object[] GetSelectedAudioclips()
       *             選択された所から再帰的にAudioClipを取得
       *    \return  取得した Object リスト
       *    \date    2014.12.25(Thu)
       *    \author  Masayoshi.Matsuyama@Donuts
       */
      /**************************************************************************************************/
      static Object[] GetSelectedAudioclips() {
        return Selection.GetFiltered(typeof(AudioClip), SelectionMode.DeepAssets);
      }
    }

	}	// Sound		
}	// Donuts

/********************************************** End of File *********************************************/
