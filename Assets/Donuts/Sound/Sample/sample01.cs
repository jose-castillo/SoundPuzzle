using UnityEngine;
using System.Collections;

// サンプルシーン１
public class sample01 : MonoBehaviour {

	public void pushNextScene()
	{
		Application.LoadLevelAsync("sample02");
	}

}
