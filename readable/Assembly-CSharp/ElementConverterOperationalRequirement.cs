using System;
using KSerialization;

// Token: 0x02000916 RID: 2326
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConverterOperationalRequirement : KMonoBehaviour
{
	// Token: 0x060040D8 RID: 16600 RVA: 0x0016F66F File Offset: 0x0016D86F
	private void onStorageChanged(object _)
	{
		this.operational.SetFlag(this.sufficientResources, this.converter.HasEnoughMassToStartConverting(false));
	}

	// Token: 0x060040D9 RID: 16601 RVA: 0x0016F68E File Offset: 0x0016D88E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.sufficientResources = new Operational.Flag("sufficientResources", this.operationalReq);
		base.Subscribe(-1697596308, new Action<object>(this.onStorageChanged));
		this.onStorageChanged(null);
	}

	// Token: 0x04002892 RID: 10386
	[MyCmpReq]
	private ElementConverter converter;

	// Token: 0x04002893 RID: 10387
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002894 RID: 10388
	private Operational.Flag.Type operationalReq;

	// Token: 0x04002895 RID: 10389
	private Operational.Flag sufficientResources;
}
