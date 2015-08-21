using UnityEngine;
using UnityEditor;
using System.Collections;

public class SpriteImportChecker : AssetPostprocessor 
{

	private void OnPreprocessTexture()
	{
		TextureImporter importer = assetImporter as TextureImporter;
		AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));

		var lowerCaseAssetPath = importer.assetPath.ToLower();

		if (lowerCaseAssetPath.IndexOf("sprites") != -1)
		{
			Debug.Log("Running processor on : " + lowerCaseAssetPath);
			importer.spritePixelsPerUnit = 1.0f;
		}
	}
}
