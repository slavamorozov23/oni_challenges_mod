using System;
using KSerialization;

// Token: 0x020007DC RID: 2012
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantAirConditioner : AirConditioner
{
	// Token: 0x06003569 RID: 13673 RVA: 0x0012DDC0 File Offset: 0x0012BFC0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<PlantAirConditioner>(-1396791468, PlantAirConditioner.OnFertilizedDelegate);
		base.Subscribe<PlantAirConditioner>(-1073674739, PlantAirConditioner.OnUnfertilizedDelegate);
	}

	// Token: 0x0600356A RID: 13674 RVA: 0x0012DDEA File Offset: 0x0012BFEA
	private void OnFertilized(object data)
	{
		this.operational.SetFlag(PlantAirConditioner.fertilizedFlag, true);
	}

	// Token: 0x0600356B RID: 13675 RVA: 0x0012DDFD File Offset: 0x0012BFFD
	private void OnUnfertilized(object data)
	{
		this.operational.SetFlag(PlantAirConditioner.fertilizedFlag, false);
	}

	// Token: 0x0400205B RID: 8283
	private static readonly Operational.Flag fertilizedFlag = new Operational.Flag("fertilized", Operational.Flag.Type.Requirement);

	// Token: 0x0400205C RID: 8284
	private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnFertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>(delegate(PlantAirConditioner component, object data)
	{
		component.OnFertilized(data);
	});

	// Token: 0x0400205D RID: 8285
	private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnUnfertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>(delegate(PlantAirConditioner component, object data)
	{
		component.OnUnfertilized(data);
	});
}
