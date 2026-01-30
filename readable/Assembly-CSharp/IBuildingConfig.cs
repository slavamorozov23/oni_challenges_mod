using System;
using UnityEngine;

// Token: 0x02000714 RID: 1812
public abstract class IBuildingConfig : IHasDlcRestrictions
{
	// Token: 0x06002D2B RID: 11563
	public abstract BuildingDef CreateBuildingDef();

	// Token: 0x06002D2C RID: 11564 RVA: 0x00106029 File Offset: 0x00104229
	public virtual void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x06002D2D RID: 11565
	public abstract void DoPostConfigureComplete(GameObject go);

	// Token: 0x06002D2E RID: 11566 RVA: 0x0010602B File Offset: 0x0010422B
	public virtual void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x0010602D File Offset: 0x0010422D
	public virtual void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06002D30 RID: 11568 RVA: 0x0010602F File Offset: 0x0010422F
	public virtual void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x06002D31 RID: 11569 RVA: 0x00106031 File Offset: 0x00104231
	[Obsolete("Implement GetRequiredDlcIds and/or GetForbiddenDlcIds instead")]
	public virtual string[] GetDlcIds()
	{
		return null;
	}

	// Token: 0x06002D32 RID: 11570 RVA: 0x00106034 File Offset: 0x00104234
	public virtual string[] GetRequiredDlcIds()
	{
		return null;
	}

	// Token: 0x06002D33 RID: 11571 RVA: 0x00106037 File Offset: 0x00104237
	public virtual string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06002D34 RID: 11572 RVA: 0x0010603A File Offset: 0x0010423A
	public virtual bool ForbidFromLoading()
	{
		return false;
	}
}
