using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000AC5 RID: 2757
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/RationTracker")]
public class RationTracker : WorldResourceAmountTracker<RationTracker>, ISaveLoadable
{
	// Token: 0x0600501E RID: 20510 RVA: 0x001D146B File Offset: 0x001CF66B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.itemTag = GameTags.Edible;
	}

	// Token: 0x0600501F RID: 20511 RVA: 0x001D1480 File Offset: 0x001CF680
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.caloriesConsumedByFood != null && this.caloriesConsumedByFood.Count > 0)
		{
			foreach (string key in this.caloriesConsumedByFood.Keys)
			{
				float num = this.caloriesConsumedByFood[key];
				float num2 = 0f;
				if (this.amountsConsumedByID.TryGetValue(key, out num2))
				{
					this.amountsConsumedByID[key] = num2 + num;
				}
				else
				{
					this.amountsConsumedByID.Add(key, num);
				}
			}
		}
		this.caloriesConsumedByFood = null;
	}

	// Token: 0x06005020 RID: 20512 RVA: 0x001D1534 File Offset: 0x001CF734
	protected override WorldResourceAmountTracker<RationTracker>.ItemData GetItemData(Pickupable item)
	{
		Edible component = item.GetComponent<Edible>();
		return new WorldResourceAmountTracker<RationTracker>.ItemData
		{
			ID = component.FoodID,
			amountValue = component.Calories,
			units = component.Units
		};
	}

	// Token: 0x06005021 RID: 20513 RVA: 0x001D1578 File Offset: 0x001CF778
	public float GetAmountConsumed()
	{
		float num = 0f;
		foreach (KeyValuePair<string, float> keyValuePair in this.amountsConsumedByID)
		{
			num += keyValuePair.Value;
		}
		return num;
	}

	// Token: 0x06005022 RID: 20514 RVA: 0x001D15D8 File Offset: 0x001CF7D8
	public float GetAmountConsumedForIDs(List<string> itemIDs)
	{
		float num = 0f;
		foreach (string key in itemIDs)
		{
			if (this.amountsConsumedByID.ContainsKey(key))
			{
				num += this.amountsConsumedByID[key];
			}
		}
		return num;
	}

	// Token: 0x06005023 RID: 20515 RVA: 0x001D1644 File Offset: 0x001CF844
	public float CountAmountForItemWithID(string ID, WorldInventory inventory, bool excludeUnreachable = true)
	{
		float num = 0f;
		ICollection<Pickupable> pickupables = inventory.GetPickupables(this.itemTag, false);
		if (pickupables != null)
		{
			foreach (Pickupable pickupable in pickupables)
			{
				if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
				{
					WorldResourceAmountTracker<RationTracker>.ItemData itemData = this.GetItemData(pickupable);
					if (itemData.ID == ID)
					{
						num += itemData.amountValue;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x04003583 RID: 13699
	[Serialize]
	public Dictionary<string, float> caloriesConsumedByFood = new Dictionary<string, float>();
}
