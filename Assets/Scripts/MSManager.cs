using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MSManager : MonoBehaviour {

	public Sprite Packet;
	public TextAsset PacketJson;
//
	public class SpritePack
	{
		public MergeSprite.SpriteInfo Info;
		public Sprite SpriteObj;

		public SpritePack(MergeSprite.SpriteInfo info, Sprite sprite)
		{
			Info = info;
			SpriteObj = sprite;
		}
	}

	private static MSManager m_Instance;
	public static MSManager Instance {
		get{return m_Instance;}
	}

	// tag, spritepack
	public Dictionary<int, SpritePack> SpritePackTable;


	void Awake(){
		if(!m_Instance)
		{
			m_Instance = this;
		}
	}


	// Use this for initialization
	void Start () {
		SpritePackTable = new Dictionary<int, SpritePack>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadSpritePack()
	{
//		string spritePackPath = Application.dataPath + "/Resources/SpritePack/";
//		DirectoryInfo dir = new DirectoryInfo(spritePackPath);
//		var files = dir.GetFiles("*.png");
//
//		foreach(var fileInfo in files)
//		{
//			var spriteTex = Resources.Load ("SpritePack/" + fileInfo.Name.) as Texture2D;
//			if(spriteTex != null)
//			{
//				var spriteJson = Resources.Load ("SpritePack/" + fileInfo.Name + ".json") as TextAsset;
//				var spriteInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<MergeSprite.SpriteInfo[]> (spriteJson.text);
//			}
//		}			

		if( Packet && PacketJson)
		{
			var spriteInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<MergeSprite.SpriteInfo[]> (PacketJson.text);
			foreach(var data in spriteInfo)
			{
				if(!SpritePackTable.ContainsKey(data.Tag))
				{
					SpritePackTable.Add(data.Tag, new SpritePack(data, Packet));
				}

			}



		}

	}



}
