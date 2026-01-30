using System;

// Token: 0x02000762 RID: 1890
public class FossilMineSM : ComplexFabricatorSM
{
	// Token: 0x06002FD1 RID: 12241 RVA: 0x0011439E File Offset: 0x0011259E
	protected override void OnSpawn()
	{
	}

	// Token: 0x06002FD2 RID: 12242 RVA: 0x001143A0 File Offset: 0x001125A0
	public void Activate()
	{
		base.smi.StartSM();
	}

	// Token: 0x06002FD3 RID: 12243 RVA: 0x001143AD File Offset: 0x001125AD
	public void Deactivate()
	{
		base.smi.StopSM("FossilMine.Deactivated");
	}
}
