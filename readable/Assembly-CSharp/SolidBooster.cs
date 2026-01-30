using System;
using UnityEngine;

// Token: 0x02000BA8 RID: 2984
public class SolidBooster : RocketEngine
{
	// Token: 0x06005933 RID: 22835 RVA: 0x00206147 File Offset: 0x00204347
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SolidBooster>(-887025858, SolidBooster.OnRocketLandedDelegate);
	}

	// Token: 0x06005934 RID: 22836 RVA: 0x00206160 File Offset: 0x00204360
	[ContextMenu("Fill Tank")]
	public void FillTank()
	{
		Element element = ElementLoader.GetElement(this.fuelTag);
		GameObject go = element.substance.SpawnResource(base.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element.defaultValues.temperature, byte.MaxValue, 0, false, false, false);
		this.fuelStorage.Store(go, false, false, true, false);
		element = ElementLoader.GetElement(GameTags.OxyRock);
		go = element.substance.SpawnResource(base.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element.defaultValues.temperature, byte.MaxValue, 0, false, false, false);
		this.fuelStorage.Store(go, false, false, true, false);
	}

	// Token: 0x06005935 RID: 22837 RVA: 0x00206228 File Offset: 0x00204428
	private void OnRocketLanded(object data)
	{
		if (this.fuelStorage != null && this.fuelStorage.items != null)
		{
			for (int i = this.fuelStorage.items.Count - 1; i >= 0; i--)
			{
				Util.KDestroyGameObject(this.fuelStorage.items[i]);
			}
			this.fuelStorage.items.Clear();
		}
	}

	// Token: 0x04003BE0 RID: 15328
	public Storage fuelStorage;

	// Token: 0x04003BE1 RID: 15329
	private static readonly EventSystem.IntraObjectHandler<SolidBooster> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SolidBooster>(delegate(SolidBooster component, object data)
	{
		component.OnRocketLanded(data);
	});
}
