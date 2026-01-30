using System;

// Token: 0x020005FD RID: 1533
public class LongRangeSculpture : Sculpture
{
	// Token: 0x060023A0 RID: 9120 RVA: 0x000CDDC4 File Offset: 0x000CBFC4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = null;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.multitoolContext = "dig";
		this.multitoolHitEffectTag = "fx_dig_splash";
	}
}
