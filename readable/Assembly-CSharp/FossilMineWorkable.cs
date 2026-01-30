using System;

// Token: 0x02000220 RID: 544
public class FossilMineWorkable : ComplexFabricatorWorkable
{
	// Token: 0x06000B06 RID: 2822 RVA: 0x00042907 File Offset: 0x00040B07
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.shouldShowSkillPerkStatusItem = false;
	}
}
