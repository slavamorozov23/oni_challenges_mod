using System;

// Token: 0x0200061B RID: 1563
public class Painting : Artable
{
	// Token: 0x060024E2 RID: 9442 RVA: 0x000D3F10 File Offset: 0x000D2110
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.multitoolContext = "paint";
		this.multitoolHitEffectTag = "fx_paint_splash";
	}

	// Token: 0x060024E3 RID: 9443 RVA: 0x000D3F43 File Offset: 0x000D2143
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Paintings.Add(this);
	}

	// Token: 0x060024E4 RID: 9444 RVA: 0x000D3F56 File Offset: 0x000D2156
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Paintings.Remove(this);
	}

	// Token: 0x060024E5 RID: 9445 RVA: 0x000D3F69 File Offset: 0x000D2169
	public override void SetStage(string stage_id, bool skip_effect)
	{
		base.SetStage(stage_id, skip_effect);
		if (Db.GetArtableStages().Get(stage_id) == null)
		{
			Debug.LogError("Missing stage: " + stage_id);
		}
	}
}
