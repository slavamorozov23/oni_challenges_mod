using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020006E1 RID: 1761
[AddComponentMenu("KMonoBehaviour/scripts/AtmoSuit")]
public class AtmoSuit : KMonoBehaviour
{
	// Token: 0x06002B79 RID: 11129 RVA: 0x000FDAD3 File Offset: 0x000FBCD3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<AtmoSuit>(-1697596308, AtmoSuit.OnStorageChangedDelegate);
	}

	// Token: 0x06002B7A RID: 11130 RVA: 0x000FDAEC File Offset: 0x000FBCEC
	private void RefreshStatusEffects(object data)
	{
		if (this == null)
		{
			return;
		}
		Equippable component = base.GetComponent<Equippable>();
		Storage component2 = base.GetComponent<Storage>();
		bool flag = component2.Has(GameTags.AnyWater) || component2.Has(SimHashes.LiquidGunk.CreateTag());
		if (component.assignee != null && flag)
		{
			Ownables soleOwner = component.assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					AssignableSlotInstance slot = ((KMonoBehaviour)component.assignee).GetComponent<Equipment>().GetSlot(component.slot);
					Effects component3 = targetGameObject.GetComponent<Effects>();
					if (component3 != null && !component3.HasEffect("SoiledSuit") && !slot.IsUnassigning())
					{
						component3.Add("SoiledSuit", true);
					}
				}
			}
		}
	}

	// Token: 0x040019EB RID: 6635
	private static readonly EventSystem.IntraObjectHandler<AtmoSuit> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<AtmoSuit>(delegate(AtmoSuit component, object data)
	{
		component.RefreshStatusEffects(data);
	});
}
