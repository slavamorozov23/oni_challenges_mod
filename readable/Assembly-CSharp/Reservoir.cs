using System;
using UnityEngine;

// Token: 0x02000B08 RID: 2824
[AddComponentMenu("KMonoBehaviour/scripts/Reservoir")]
public class Reservoir : KMonoBehaviour
{
	// Token: 0x06005240 RID: 21056 RVA: 0x001DD7CC File Offset: 0x001DB9CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_fill",
			"meter_OL"
		});
		base.Subscribe<Reservoir>(-1697596308, Reservoir.OnStorageChangeDelegate);
		this.OnStorageChange(null);
	}

	// Token: 0x06005241 RID: 21057 RVA: 0x001DD82B File Offset: 0x001DBA2B
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
	}

	// Token: 0x0400379D RID: 14237
	private MeterController meter;

	// Token: 0x0400379E RID: 14238
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400379F RID: 14239
	private static readonly EventSystem.IntraObjectHandler<Reservoir> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Reservoir>(delegate(Reservoir component, object data)
	{
		component.OnStorageChange(data);
	});
}
