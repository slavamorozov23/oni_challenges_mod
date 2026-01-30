using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000348 RID: 840
public class StarmapHexCellInventory : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06001160 RID: 4448 RVA: 0x00066ABB File Offset: 0x00064CBB
	public static void ClearStatics()
	{
		StarmapHexCellInventory.AllInventories.Clear();
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06001161 RID: 4449 RVA: 0x00066AC7 File Offset: 0x00064CC7
	public int ItemCount
	{
		get
		{
			if (this.Items != null)
			{
				return this.Items.Count;
			}
			return 0;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06001162 RID: 4450 RVA: 0x00066ADE File Offset: 0x00064CDE
	public float TotalMass
	{
		get
		{
			return this.ReadTotalMass();
		}
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x00066AE8 File Offset: 0x00064CE8
	public bool RegisterInventory(AxialI location)
	{
		StarmapHexCellInventory x = null;
		if (!StarmapHexCellInventory.AllInventories.TryGetValue(location, out x) || x == this)
		{
			StarmapHexCellInventory.AllInventories[location] = this;
			return true;
		}
		return false;
	}

	// Token: 0x06001164 RID: 4452 RVA: 0x00066B20 File Offset: 0x00064D20
	public void TransferAllItemsFromExternalInventory(StarmapHexCellInventory externalInventory)
	{
		bool flag = false;
		foreach (StarmapHexCellInventory.SerializedItem serializedItem in externalInventory.Items)
		{
			bool flag2 = this.TransferItemFromGroup(serializedItem.ID, serializedItem.Mass, serializedItem.StateMask) != null;
			flag = (flag || flag2);
		}
		if (flag)
		{
			base.gameObject.Trigger(-1697596308, null);
		}
		externalInventory.DeleteAll();
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x00066BA8 File Offset: 0x00064DA8
	private StarmapHexCellInventory.SerializedItem TransferItemFromGroup(Tag itemID, float mass, Element.State state)
	{
		return this.AddItem(itemID, mass, state, false);
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x00066BB4 File Offset: 0x00064DB4
	public StarmapHexCellInventory.SerializedItem AddItem(Element element, float mass)
	{
		return this.AddItem(element.id.CreateTag(), mass, element.state);
	}

	// Token: 0x06001167 RID: 4455 RVA: 0x00066BCE File Offset: 0x00064DCE
	public StarmapHexCellInventory.SerializedItem AddItem(Tag itemID, float mass, Element.State state)
	{
		return this.AddItem(itemID, mass, state, true);
	}

	// Token: 0x06001168 RID: 4456 RVA: 0x00066BDC File Offset: 0x00064DDC
	private StarmapHexCellInventory.SerializedItem AddItem(Tag itemID, float mass, Element.State state, bool triggerStorageChangeCb)
	{
		StarmapHexCellInventory.SerializedItem serializedItem = this.FindItem(itemID);
		if (serializedItem == null)
		{
			serializedItem = new StarmapHexCellInventory.SerializedItem(itemID, 0f, state);
			this.Items.Add(serializedItem);
		}
		serializedItem.Mass += mass;
		if (triggerStorageChangeCb)
		{
			base.gameObject.Trigger(-1697596308, null);
		}
		return serializedItem;
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x00066C31 File Offset: 0x00064E31
	public PrimaryElement ExtractAndSpawnItem(Tag ID)
	{
		return this.ExtractAndSpawnItemMass(ID, float.MaxValue);
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x00066C40 File Offset: 0x00064E40
	public PrimaryElement ExtractAndSpawnItemMass(Tag ID, float mass)
	{
		GameObject gameObject = null;
		PrimaryElement primaryElement = null;
		StarmapHexCellInventory.SerializedItem serializedItem = this.FindItem(ID);
		if (serializedItem != null)
		{
			float num = Mathf.Min(mass, serializedItem.Mass);
			Element element = ElementLoader.GetElement(ID);
			Vector3 position = base.transform.GetPosition();
			if (num <= 0f)
			{
				global::Debug.LogWarning("StarmapHexCellInventory.ExtractAndSpawn() found an invalid mass to extract from item ID(" + ID.ToString() + "). If the stored item had zero mass, it will be removed now");
				if (serializedItem.Mass <= 0f)
				{
					this.DeleteItem(serializedItem);
					return null;
				}
			}
			if (element != null)
			{
				if (element.IsGas)
				{
					gameObject = GasSourceManager.Instance.CreateChunk(element, num, element.defaultValues.temperature, byte.MaxValue, 0, position).gameObject;
				}
				else if (element.IsLiquid)
				{
					gameObject = LiquidSourceManager.Instance.CreateChunk(element, num, element.defaultValues.temperature, byte.MaxValue, 0, position).gameObject;
				}
				else if (element.IsSolid)
				{
					gameObject = element.substance.SpawnResource(position, num, element.defaultValues.temperature, byte.MaxValue, 0, true, false, true);
					gameObject.GetComponent<Pickupable>().prevent_absorb_until_stored = true;
					element.substance.ActivateSubstanceGameObject(gameObject, byte.MaxValue, 0);
				}
				primaryElement = gameObject.GetComponent<PrimaryElement>();
				primaryElement.KeepZeroMassObject = false;
			}
			else
			{
				GameObject prefab = Assets.GetPrefab(serializedItem.ID);
				if (!(prefab != null))
				{
					global::Debug.LogWarning("StarmapHexCellInventory.ExtractAndSpawn() found an invalid item ID(" + ID.ToString() + ") stored. Removing from list.");
					this.DeleteItem(serializedItem);
					return null;
				}
				gameObject = Util.KInstantiate(prefab, base.transform.gameObject, null);
				gameObject.transform.SetLocalPosition(position);
				primaryElement = gameObject.GetComponent<PrimaryElement>();
				primaryElement.Units = num;
				gameObject.SetActive(true);
			}
			if (primaryElement != null)
			{
				this.DeleteItemMass(serializedItem, num);
			}
		}
		return primaryElement;
	}

	// Token: 0x0600116B RID: 4459 RVA: 0x00066E14 File Offset: 0x00065014
	public float ExtractAndStoreItemMass(Tag ID, float mass, Storage storage)
	{
		StarmapHexCellInventory.SerializedItem serializedItem = this.FindItem(ID);
		if (serializedItem == null)
		{
			return 0f;
		}
		float num = Mathf.Min(mass, serializedItem.Mass);
		if (num <= 0f)
		{
			global::Debug.LogWarning("StarmapHexCellInventory.ExtractAndSpawn() found an invalid mass to extract from item ID(" + ID.ToString() + "). If the stored item had zero mass, it will be removed now");
			if (serializedItem.Mass <= 0f)
			{
				this.DeleteItem(serializedItem);
				return 0f;
			}
		}
		Element element = ElementLoader.GetElement(ID);
		if (element != null)
		{
			storage.AddElement(element.id, num, element.defaultValues.temperature, byte.MaxValue, 0, false, true);
			this.DeleteItemMass(serializedItem, num);
			return num;
		}
		GameObject prefab = Assets.GetPrefab(serializedItem.ID);
		if (prefab != null)
		{
			GameObject gameObject = Util.KInstantiate(prefab, base.transform.gameObject, null);
			gameObject.transform.SetLocalPosition(base.transform.GetPosition());
			gameObject.GetComponent<PrimaryElement>().Units = num;
			gameObject.SetActive(true);
			storage.Store(gameObject, true, false, true, false);
			this.DeleteItemMass(serializedItem, num);
			return num;
		}
		global::Debug.LogWarning("StarmapHexCellInventory.ExtractAndSpawn() found an invalid item ID(" + ID.ToString() + ") stored. Removing from list.");
		this.DeleteItem(serializedItem);
		return 0f;
	}

	// Token: 0x0600116C RID: 4460 RVA: 0x00066F51 File Offset: 0x00065151
	private void DeleteAll()
	{
		this.Items.Clear();
		base.gameObject.Trigger(-1697596308, null);
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x00066F6F File Offset: 0x0006516F
	private void DeleteItem(StarmapHexCellInventory.SerializedItem item)
	{
		this.DeleteItemMass(item, item.Mass);
	}

	// Token: 0x0600116E RID: 4462 RVA: 0x00066F7E File Offset: 0x0006517E
	private void DeleteItemMass(StarmapHexCellInventory.SerializedItem item, float massToDelete)
	{
		if (item != null)
		{
			item.Mass -= massToDelete;
			if (item.Mass <= 0f)
			{
				this.Items.Remove(item);
			}
			base.gameObject.Trigger(-1697596308, null);
		}
	}

	// Token: 0x0600116F RID: 4463 RVA: 0x00066FBC File Offset: 0x000651BC
	private void RefreshStatusItems(object data = null)
	{
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.ClusterMapHarvestableResource, this.Items);
	}

	// Token: 0x06001170 RID: 4464 RVA: 0x00066FF0 File Offset: 0x000651F0
	private StarmapHexCellInventory.SerializedItem FindItem(Tag id)
	{
		if (this.Items != null)
		{
			return this.Items.Find((StarmapHexCellInventory.SerializedItem i) => i.ID == id);
		}
		return null;
	}

	// Token: 0x06001171 RID: 4465 RVA: 0x0006702C File Offset: 0x0006522C
	private float ReadTotalMass()
	{
		if (this.Items == null || this.Items.Count == 0)
		{
			return 0f;
		}
		float num = 0f;
		foreach (StarmapHexCellInventory.SerializedItem serializedItem in this.Items)
		{
			num += serializedItem.Mass;
		}
		return num;
	}

	// Token: 0x06001172 RID: 4466 RVA: 0x000670A4 File Offset: 0x000652A4
	[OnDeserialized]
	internal void OnDeserializedMethod()
	{
		if (this.Items != null)
		{
			this.Items.RemoveAll((StarmapHexCellInventory.SerializedItem x) => Assets.TryGetPrefab(x.ID) == null);
			foreach (StarmapHexCellInventory.SerializedItem serializedItem in this.Items)
			{
				serializedItem.RecalculateState();
			}
		}
	}

	// Token: 0x06001173 RID: 4467 RVA: 0x00067128 File Offset: 0x00065328
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe(-1697596308, new Action<object>(this.RefreshStatusItems));
		this.RefreshStatusItems(null);
	}

	// Token: 0x04000B05 RID: 2821
	public static Dictionary<AxialI, StarmapHexCellInventory> AllInventories = new Dictionary<AxialI, StarmapHexCellInventory>();

	// Token: 0x04000B06 RID: 2822
	[Serialize]
	public List<StarmapHexCellInventory.SerializedItem> Items = new List<StarmapHexCellInventory.SerializedItem>();

	// Token: 0x02001233 RID: 4659
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SerializedItem
	{
		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x0600873D RID: 34621 RVA: 0x0034B209 File Offset: 0x00349409
		public Element.State StateMask
		{
			get
			{
				return this.stateMask;
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x0600873E RID: 34622 RVA: 0x0034B211 File Offset: 0x00349411
		public bool IsSolid
		{
			get
			{
				return (this.stateMask & Element.State.Solid) > Element.State.Vacuum;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x0600873F RID: 34623 RVA: 0x0034B21E File Offset: 0x0034941E
		public bool IsLiquid
		{
			get
			{
				return (this.stateMask & Element.State.Liquid) > Element.State.Vacuum;
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06008740 RID: 34624 RVA: 0x0034B22B File Offset: 0x0034942B
		public bool IsGas
		{
			get
			{
				return (this.stateMask & Element.State.Gas) > Element.State.Vacuum;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06008741 RID: 34625 RVA: 0x0034B238 File Offset: 0x00349438
		public bool IsEntity
		{
			get
			{
				return this.stateMask == Element.State.Vacuum;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06008742 RID: 34626 RVA: 0x0034B243 File Offset: 0x00349443
		public Element.State ItemMatterState
		{
			get
			{
				return this.stateMask;
			}
		}

		// Token: 0x06008743 RID: 34627 RVA: 0x0034B24B File Offset: 0x0034944B
		public SerializedItem(Tag id, float mass) : this(id, mass, Element.State.Vacuum)
		{
		}

		// Token: 0x06008744 RID: 34628 RVA: 0x0034B256 File Offset: 0x00349456
		public SerializedItem(Tag id, float mass, Element.State state)
		{
			this.ID = id;
			this.Mass = mass;
			this.stateMask = state;
		}

		// Token: 0x06008745 RID: 34629 RVA: 0x0034B274 File Offset: 0x00349474
		public void RecalculateState()
		{
			Element element = ElementLoader.GetElement(this.ID);
			if (element == null)
			{
				this.stateMask = Element.State.Vacuum;
				return;
			}
			this.stateMask = element.state;
		}

		// Token: 0x0400671B RID: 26395
		[Serialize]
		public Tag ID;

		// Token: 0x0400671C RID: 26396
		[Serialize]
		public float Mass;

		// Token: 0x0400671D RID: 26397
		private Element.State stateMask;
	}
}
