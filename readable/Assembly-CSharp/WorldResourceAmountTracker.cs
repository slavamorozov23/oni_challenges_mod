using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000C29 RID: 3113
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class WorldResourceAmountTracker<T> : KMonoBehaviour where T : KMonoBehaviour
{
	// Token: 0x06005E1B RID: 24091 RVA: 0x00220E62 File Offset: 0x0021F062
	public static void DestroyInstance()
	{
		WorldResourceAmountTracker<T>.instance = default(T);
	}

	// Token: 0x06005E1C RID: 24092 RVA: 0x00220E6F File Offset: 0x0021F06F
	public static T Get()
	{
		return WorldResourceAmountTracker<T>.instance;
	}

	// Token: 0x06005E1D RID: 24093 RVA: 0x00220E78 File Offset: 0x0021F078
	protected override void OnPrefabInit()
	{
		Debug.Assert(WorldResourceAmountTracker<T>.instance == null, "Error, WorldResourceAmountTracker of type T has already been initialize and another instance is attempting to initialize. this isn't allowed because T is meant to be a singleton, ensure only one instance exist. existing instance GameObject: " + ((WorldResourceAmountTracker<T>.instance == null) ? "" : WorldResourceAmountTracker<T>.instance.gameObject.name) + ". Error triggered by instance of T in GameObject: " + base.gameObject.name);
		WorldResourceAmountTracker<T>.instance = (this as T);
		this.itemTag = GameTags.Edible;
	}

	// Token: 0x06005E1E RID: 24094 RVA: 0x00220EFC File Offset: 0x0021F0FC
	protected override void OnSpawn()
	{
		base.Subscribe(631075836, new Action<object>(this.OnNewDay));
	}

	// Token: 0x06005E1F RID: 24095 RVA: 0x00220F16 File Offset: 0x0021F116
	private void OnNewDay(object data)
	{
		this.previousFrame = this.currentFrame;
		this.currentFrame = default(WorldResourceAmountTracker<T>.Frame);
	}

	// Token: 0x06005E20 RID: 24096
	protected abstract WorldResourceAmountTracker<T>.ItemData GetItemData(Pickupable item);

	// Token: 0x06005E21 RID: 24097 RVA: 0x00220F30 File Offset: 0x0021F130
	public float CountAmount(Dictionary<string, float> unitCountByID, WorldInventory inventory, bool excludeUnreachable = true)
	{
		float num;
		return this.CountAmount(unitCountByID, out num, inventory, excludeUnreachable);
	}

	// Token: 0x06005E22 RID: 24098 RVA: 0x00220F48 File Offset: 0x0021F148
	public float CountAmount(Dictionary<string, float> unitCountByID, out float totalUnitsFound, WorldInventory inventory, bool excludeUnreachable)
	{
		float num = 0f;
		totalUnitsFound = 0f;
		ICollection<Pickupable> pickupables = inventory.GetPickupables(this.itemTag, false);
		if (pickupables != null)
		{
			foreach (Pickupable pickupable in pickupables)
			{
				if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
				{
					if (this.ignoredTags != null)
					{
						bool flag = false;
						foreach (Tag tag in this.ignoredTags)
						{
							if (pickupable.KPrefabID.HasTag(tag))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							continue;
						}
					}
					WorldResourceAmountTracker<T>.ItemData itemData = this.GetItemData(pickupable);
					num += itemData.amountValue;
					if (unitCountByID != null)
					{
						if (!unitCountByID.ContainsKey(itemData.ID))
						{
							unitCountByID[itemData.ID] = 0f;
						}
						string id = itemData.ID;
						unitCountByID[id] += itemData.units;
					}
					totalUnitsFound += itemData.units;
				}
			}
		}
		return num;
	}

	// Token: 0x06005E23 RID: 24099 RVA: 0x00221078 File Offset: 0x0021F278
	public void RegisterAmountProduced(float val)
	{
		this.currentFrame.amountProduced = this.currentFrame.amountProduced + val;
	}

	// Token: 0x06005E24 RID: 24100 RVA: 0x0022108C File Offset: 0x0021F28C
	public void RegisterAmountConsumed(string ID, float valueConsumed)
	{
		this.currentFrame.amountConsumed = this.currentFrame.amountConsumed + valueConsumed;
		if (!this.amountsConsumedByID.ContainsKey(ID))
		{
			this.amountsConsumedByID.Add(ID, valueConsumed);
			return;
		}
		Dictionary<string, float> dictionary = this.amountsConsumedByID;
		dictionary[ID] += valueConsumed;
	}

	// Token: 0x04003E8C RID: 16012
	private static T instance;

	// Token: 0x04003E8D RID: 16013
	[Serialize]
	public WorldResourceAmountTracker<T>.Frame currentFrame;

	// Token: 0x04003E8E RID: 16014
	[Serialize]
	public WorldResourceAmountTracker<T>.Frame previousFrame;

	// Token: 0x04003E8F RID: 16015
	[Serialize]
	public Dictionary<string, float> amountsConsumedByID = new Dictionary<string, float>();

	// Token: 0x04003E90 RID: 16016
	protected Tag itemTag;

	// Token: 0x04003E91 RID: 16017
	protected Tag[] ignoredTags;

	// Token: 0x02001DC7 RID: 7623
	protected struct ItemData
	{
		// Token: 0x04008C3B RID: 35899
		public string ID;

		// Token: 0x04008C3C RID: 35900
		public float amountValue;

		// Token: 0x04008C3D RID: 35901
		public float units;
	}

	// Token: 0x02001DC8 RID: 7624
	public struct Frame
	{
		// Token: 0x04008C3E RID: 35902
		public float amountProduced;

		// Token: 0x04008C3F RID: 35903
		public float amountConsumed;
	}
}
