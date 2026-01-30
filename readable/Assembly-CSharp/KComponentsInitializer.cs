using System;

// Token: 0x020009D6 RID: 2518
public class KComponentsInitializer : KComponentSpawn
{
	// Token: 0x06004918 RID: 18712 RVA: 0x001A6E97 File Offset: 0x001A5097
	private void Awake()
	{
		KComponentSpawn.instance = this;
		this.comps = new GameComps();
	}
}
