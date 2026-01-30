using System;
using UnityEngine;

// Token: 0x02000A8A RID: 2698
public class OxygenMask : KMonoBehaviour, ISim200ms
{
	// Token: 0x06004E61 RID: 20065 RVA: 0x001C82D1 File Offset: 0x001C64D1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OxygenMask>(608245985, OxygenMask.OnSuitTankDeltaDelegate);
	}

	// Token: 0x06004E62 RID: 20066 RVA: 0x001C82EC File Offset: 0x001C64EC
	private void CheckOxygenLevels(object data)
	{
		if (this.suitTank.IsEmpty())
		{
			Equippable component = base.GetComponent<Equippable>();
			if (component.assignee != null)
			{
				Ownables soleOwner = component.assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					soleOwner.GetComponent<Equipment>().Unequip(component);
				}
			}
		}
	}

	// Token: 0x06004E63 RID: 20067 RVA: 0x001C8338 File Offset: 0x001C6538
	public void Sim200ms(float dt)
	{
		if (base.GetComponent<Equippable>().assignee == null)
		{
			float num = this.leakRate * dt;
			float massAvailable = this.storage.GetMassAvailable(this.suitTank.elementTag);
			num = Mathf.Min(num, massAvailable);
			this.storage.DropSome(this.suitTank.elementTag, num, true, true, default(Vector3), true, false);
		}
		if (this.suitTank.IsEmpty())
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x0400343E RID: 13374
	private static readonly EventSystem.IntraObjectHandler<OxygenMask> OnSuitTankDeltaDelegate = new EventSystem.IntraObjectHandler<OxygenMask>(delegate(OxygenMask component, object data)
	{
		component.CheckOxygenLevels(data);
	});

	// Token: 0x0400343F RID: 13375
	[MyCmpGet]
	private SuitTank suitTank;

	// Token: 0x04003440 RID: 13376
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003441 RID: 13377
	private float leakRate = 0.1f;
}
