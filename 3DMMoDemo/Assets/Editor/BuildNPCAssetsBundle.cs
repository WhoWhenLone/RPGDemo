using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class BuildNPCAssetsBundle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	[@MenuItem("AssetsBundle/Build AssetsBundle")]
	static void BuildAssetsBundles()
    {
		Debug.Log("创建AssetsBundle");
		string path = Application.streamingAssetsPath + "/AssetsBundle";
		BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
	}
}
