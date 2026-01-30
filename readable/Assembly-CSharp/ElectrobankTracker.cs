using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200090F RID: 2319
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ElectrobankTracker")]
public class ElectrobankTracker : WorldResourceAmountTracker<ElectrobankTracker>, ISaveLoadable
{
	// Token: 0x06004083 RID: 16515 RVA: 0x0016D65B File Offset: 0x0016B85B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ignoredTags = new Tag[GameTags.BionicIncompatibleBatteries.Count];
		GameTags.BionicIncompatibleBatteries.CopyTo(this.ignoredTags, 0);
		this.itemTag = GameTags.ChargedPortableBattery;
	}

	// Token: 0x06004084 RID: 16516 RVA: 0x0016D694 File Offset: 0x0016B894
	protected override WorldResourceAmountTracker<ElectrobankTracker>.ItemData GetItemData(Pickupable item)
	{
		Electrobank component = item.GetComponent<Electrobank>();
		return new WorldResourceAmountTracker<ElectrobankTracker>.ItemData
		{
			ID = component.ID,
			amountValue = component.Charge * item.PrimaryElement.Units,
			units = item.PrimaryElement.Units
		};
	}
}
