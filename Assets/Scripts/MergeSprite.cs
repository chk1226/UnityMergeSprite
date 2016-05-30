using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class MergeSprite : MonoBehaviour {
	
	public uint WidthHeight = 1024;
	public uint RectPerSprite = 128;
	public uint Space = 2;

	public List<Sprite> SpriteList;

	private List<SpriteInfo> m_SpriteInfolist;
	public class SpriteInfo
	{
		public int Tag;
		public float LBS;	// left-bottm s
		public float LBT;	// left-bottm t
		public float RTS;	// right-top s
		public float RTT;	// right-top t
	}

//
//	// Use this for initialization
	void Start () {
		m_SpriteInfolist = new List<SpriteInfo> ();	
	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
//

	public void ApplySprite()
	{

//		if (Img) {
//			var texture = CreateTexture2d ();
//			Sprite sprite = Sprite.Create(texture, new Rect(0, 0, WidthHeight, WidthHeight), new Vector2(0.5f, 0.5f));
//
//			Img.sprite = sprite;		
//		
//		}


	}

	public void BuildSprite()
	{

		var texture = CreateTexture2dAndInfo ();
		Sprite sprite = Sprite.Create(texture, new Rect(0, 0, WidthHeight, WidthHeight), new Vector2(0.5f, 0.5f));


		var span = DateTime.Now.Subtract (new DateTime(1970, 1, 1, 0 ,0 ,0));
		var path = "/Resources/MergeSprites/" + span.TotalSeconds.ToString ("0");
		SaveSpriteAndInfo (sprite, path);

		UnityEngine.Object.Destroy (texture);
		UnityEngine.Object.Destroy (sprite);

		UnityEditor.AssetDatabase.Refresh ();
		path = "Assets" + path + ".png";
//		UnityEditor.AssetDatabase.ImportAsset (path);
		var importer = UnityEditor.AssetImporter.GetAtPath (path) as UnityEditor.TextureImporter;
		if (importer) {
		
			importer.textureFormat = UnityEditor.TextureImporterFormat.AutomaticCompressed;
			importer.textureType = UnityEditor.TextureImporterType.Sprite;
//			importer.compressionQuality = 50;
			importer.mipmapEnabled = false;

			UnityEditor.AssetDatabase.WriteImportSettingsIfDirty (path);
			UnityEditor.AssetDatabase.ImportAsset (path);

		} else {
			Debug.LogWarning ("importer texture is error" );
		}
	}


	private Texture2D CreateTexture2dAndInfo()
	{
		var path = "Sprites/{0}";

		var texture = new Texture2D ((int)WidthHeight, (int)WidthHeight, TextureFormat.ARGB32, false);
		float spriteOffsetST = (float)RectPerSprite / WidthHeight;
		m_SpriteInfolist.Clear ();

		int spriteIndex = 0;
		for(int h = 0; h < WidthHeight; h++)
		{
			if( h + (int)(RectPerSprite + Space) > WidthHeight)
			{
				break;
			}

			for (int w = 0; w < WidthHeight; w++) 
			{
				if(spriteIndex >= SpriteList.Count)
				{
					goto MS_FINISH;
				}

				if( w + (int)(RectPerSprite + Space) > WidthHeight)
				{
					break;
				}

				var spriteName = SpriteList [spriteIndex].name;
				var assetPath = UnityEditor.AssetDatabase.GetAssetPath (SpriteList [spriteIndex]);

				var importer = UnityEditor.AssetImporter.GetAtPath (assetPath) as UnityEditor.TextureImporter;
				if (importer) {
					UnityEditor.AssetDatabase.ImportAsset (assetPath);
					var t2d = Resources.Load (string.Format (path, spriteName)) as Texture2D;
					texture.SetPixels (w, h, (int)RectPerSprite, (int)RectPerSprite, t2d.GetPixels ());
				}

				SpriteInfo spriteInfo = new SpriteInfo ();
				spriteInfo.Tag = spriteIndex;
				spriteInfo.LBS = (float)w/WidthHeight;
				spriteInfo.LBT = (float)h/WidthHeight;
				spriteInfo.RTS = spriteInfo.LBS + spriteOffsetST;
				spriteInfo.RTT = spriteInfo.LBT + spriteOffsetST;

				m_SpriteInfolist.Add (spriteInfo);

				w += (int)(RectPerSprite + Space);
				spriteIndex++;
			}
			h += (int)(RectPerSprite + Space);

		}

MS_FINISH:

		texture.Apply ();
				
		return texture;
	}


	private void SaveSpriteAndInfo(Sprite sprite, string path)
	{
		var bytes = sprite.texture.EncodeToPNG ();
		File.WriteAllBytes (Application.dataPath + path + ".png", bytes);

		string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject (m_SpriteInfolist);
		File.WriteAllText (Application.dataPath + path + ".json", jsonData);


	}


}
