using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Graphic))]
public class MSLoader : BaseMeshEffect {

	public Image Image;
	public int Tag;

	private int m_CurrentTag;
	
	protected override void Awake ()
	{
		base.Awake ();
		Image = this.gameObject.GetComponent<Image>();
		m_CurrentTag = Tag;
	}

	public override void ModifyMesh (VertexHelper vh)
	{
		if (IsActive() == false)
		{
			return;
		}

		var vList = new List<UIVertex>();
		vh.GetUIVertexStream(vList);

		if(ModifyVertices(vList))
		{
			vh.Clear();
			vh.AddUIVertexTriangleStream(vList);
		}
		else
		{
			Debug.Log("No found image by tag");
		}

	}

	public bool ModifyVertices(List<UIVertex> verts)
    {

        if (IsActive() == false || verts == null || verts.Count == 0)
        {
			return false;
        }

		if (!Image)
        {
			return false;
        } 
    
		if(MSManager.Instance.SpritePackTable.ContainsKey(Tag))
		{
			var info = MSManager.Instance.SpritePackTable[Tag];
//			Image.sprite = info.SpriteObj;

//			for (int index = 0; index < 4; index ++)
//			{
			var vertex = verts[0];
			vertex.uv0.x = info.Info.LBS;
			vertex.uv0.y = info.Info.LBT;
			verts[0] = vertex;
			verts[5] = vertex;

			vertex = verts[1];
			vertex.uv0.x = info.Info.LBS;
			vertex.uv0.y = info.Info.RTT;
			verts[1] = vertex;

			vertex = verts[2];
			vertex.uv0.x = info.Info.RTS;
			vertex.uv0.y = info.Info.RTT;
			verts[2] = vertex;
			verts[3] = vertex;

			vertex = verts[4];
			vertex.uv0.x = info.Info.RTS;
			vertex.uv0.y = info.Info.LBT;
			verts[4] = vertex;



//			}

		}
		else
		{
			return false;
		}

		return true;

    
    }

	void Update()
	{
		if(Tag != m_CurrentTag)
		{
			m_CurrentTag = Tag;

			if(MSManager.Instance.SpritePackTable.ContainsKey(Tag))
			{
				var info = MSManager.Instance.SpritePackTable[Tag];
				Image.sprite = info.SpriteObj;
			}


			graphic.SetVerticesDirty();
		}
	}

}
