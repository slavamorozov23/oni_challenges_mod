using System;

// Token: 0x020002F8 RID: 760
public class MinorDigSiteWorkable : FossilExcavationWorkable
{
	// Token: 0x06000F7E RID: 3966 RVA: 0x0005AC36 File Offset: 0x00058E36
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x0005AC49 File Offset: 0x00058E49
	protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MinorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x0005AC64 File Offset: 0x00058E64
	protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

	// Token: 0x04000A21 RID: 2593
	private MinorFossilDigSite.Instance digsite;
}
