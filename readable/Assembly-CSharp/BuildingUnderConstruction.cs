using System;
using UnityEngine;

// Token: 0x02000702 RID: 1794
public class BuildingUnderConstruction : Building
{
	// Token: 0x06002C75 RID: 11381 RVA: 0x00102B38 File Offset: 0x00100D38
	protected override void OnPrefabInit()
	{
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(this.Def.SceneLayer);
		base.transform.SetPosition(position);
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Construction"));
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Rotatable component2 = base.GetComponent<Rotatable>();
		if (component != null && component2 == null)
		{
			component.Offset = this.Def.GetVisualizerOffset();
		}
		KBoxCollider2D component3 = base.GetComponent<KBoxCollider2D>();
		if (component3 != null)
		{
			Vector3 visualizerOffset = this.Def.GetVisualizerOffset();
			component3.offset += new Vector2(visualizerOffset.x, visualizerOffset.y);
		}
		base.OnPrefabInit();
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x00102C04 File Offset: 0x00100E04
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.Def.IsTilePiece)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.Def.RunOnArea(cell, base.Orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
			});
		}
		base.RegisterBlockTileRenderer();
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x00102C59 File Offset: 0x00100E59
	protected override void OnCleanUp()
	{
		base.UnregisterBlockTileRenderer();
		base.OnCleanUp();
	}

	// Token: 0x04001A5B RID: 6747
	[MyCmpAdd]
	private KSelectable selectable;

	// Token: 0x04001A5C RID: 6748
	[MyCmpAdd]
	private SaveLoadRoot saveLoadRoot;

	// Token: 0x04001A5D RID: 6749
	[MyCmpAdd]
	private KPrefabID kPrefabID;
}
