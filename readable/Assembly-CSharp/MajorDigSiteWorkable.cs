using System;

// Token: 0x020002D6 RID: 726
public class MajorDigSiteWorkable : FossilExcavationWorkable
{
	// Token: 0x06000ECA RID: 3786 RVA: 0x00055E2A File Offset: 0x0005402A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x00055E3D File Offset: 0x0005403D
	protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MajorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x00055E58 File Offset: 0x00054058
	protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

	// Token: 0x0400099C RID: 2460
	private MajorFossilDigSite.Instance digsite;
}
