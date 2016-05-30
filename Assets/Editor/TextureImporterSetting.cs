using UnityEngine;
using System.Collections;
using UnityEditor;

public class TextureImporterSetting : AssetPostprocessor {


	void OnPreprocessTexture(){
	
		var importer = assetImporter as TextureImporter;
		importer.isReadable = true;


		Debug.Log (importer.name);
	}




}
