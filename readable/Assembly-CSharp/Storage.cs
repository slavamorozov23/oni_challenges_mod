using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000643 RID: 1603
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Storage")]
public class Storage : Workable, ISaveLoadableDetails, IGameObjectEffectDescriptor, IStorage
{
	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06002689 RID: 9865 RVA: 0x000DD77C File Offset: 0x000DB97C
	public bool ShouldOnlyTransferFromLowerPriority
	{
		get
		{
			return this.onlyTransferFromLowerPriority || this.allowItemRemoval;
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x0600268A RID: 9866 RVA: 0x000DD78E File Offset: 0x000DB98E
	// (set) Token: 0x0600268B RID: 9867 RVA: 0x000DD796 File Offset: 0x000DB996
	public bool allowUIItemRemoval { get; set; }

	// Token: 0x170001BA RID: 442
	public GameObject this[int idx]
	{
		get
		{
			return this.items[idx];
		}
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x0600268D RID: 9869 RVA: 0x000DD7AD File Offset: 0x000DB9AD
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x0600268E RID: 9870 RVA: 0x000DD7BA File Offset: 0x000DB9BA
	// (set) Token: 0x0600268F RID: 9871 RVA: 0x000DD7C2 File Offset: 0x000DB9C2
	public bool ShouldSaveItems
	{
		get
		{
			return this.shouldSaveItems;
		}
		set
		{
			this.shouldSaveItems = value;
		}
	}

	// Token: 0x06002690 RID: 9872 RVA: 0x000DD7CB File Offset: 0x000DB9CB
	public bool ShouldShowInUI()
	{
		return this.showInUI;
	}

	// Token: 0x06002691 RID: 9873 RVA: 0x000DD7D3 File Offset: 0x000DB9D3
	public List<GameObject> GetItems()
	{
		return this.items;
	}

	// Token: 0x06002692 RID: 9874 RVA: 0x000DD7DB File Offset: 0x000DB9DB
	public void SetDefaultStoredItemModifiers(List<Storage.StoredItemModifier> modifiers)
	{
		this.defaultStoredItemModifers = modifiers;
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06002693 RID: 9875 RVA: 0x000DD7E4 File Offset: 0x000DB9E4
	public PrioritySetting masterPriority
	{
		get
		{
			if (this.prioritizable)
			{
				return this.prioritizable.GetMasterPriority();
			}
			return Chore.DefaultPrioritySetting;
		}
	}

	// Token: 0x06002694 RID: 9876 RVA: 0x000DD804 File Offset: 0x000DBA04
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		if (this.useGunForDelivery && worker.UsesMultiTool())
		{
			Workable.AnimInfo anim = base.GetAnim(worker);
			anim.smi = new MultitoolController.Instance(this, worker, "store", Assets.GetPrefab(EffectConfigs.OreAbsorbId));
			return anim;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06002695 RID: 9877 RVA: 0x000DD85C File Offset: 0x000DBA5C
	public override Vector3 GetTargetPoint()
	{
		Vector3 vector = base.GetTargetPoint();
		if (this.useGunForDelivery && this.gunTargetOffset != Vector2.zero)
		{
			if (this.rotatable != null)
			{
				vector += this.rotatable.GetRotatedOffset(this.gunTargetOffset);
			}
			else
			{
				vector += new Vector3(this.gunTargetOffset.x, this.gunTargetOffset.y, 0f);
			}
		}
		return vector;
	}

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06002696 RID: 9878 RVA: 0x000DD8E0 File Offset: 0x000DBAE0
	// (remove) Token: 0x06002697 RID: 9879 RVA: 0x000DD918 File Offset: 0x000DBB18
	public event System.Action OnStorageIncreased;

	// Token: 0x06002698 RID: 9880 RVA: 0x000DD950 File Offset: 0x000DBB50
	protected override void OnPrefabInit()
	{
		if (this.useWideOffsets)
		{
			base.SetOffsetTable(OffsetGroups.InvertedWideTable);
		}
		else
		{
			base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		}
		this.showProgressBar = false;
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		GameUtil.SubscribeToTags<Storage>(this, Storage.OnDeadTagAddedDelegate, true);
		base.Subscribe<Storage>(1502190696, Storage.OnQueueDestroyObjectDelegate);
		base.Subscribe<Storage>(-905833192, Storage.OnCopySettingsDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Storing;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.SetupStorageStatusItems();
	}

	// Token: 0x06002699 RID: 9881 RVA: 0x000DD9F8 File Offset: 0x000DBBF8
	private void SetupStorageStatusItems()
	{
		if (Storage.capacityStatusItem == null)
		{
			Storage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Storage.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				Storage storage = (Storage)data;
				float num = storage.MassStored();
				float num2 = storage.capacityKg;
				if (num > num2 - storage.storageFullMargin && num < num2)
				{
					num = num2;
				}
				else
				{
					num = Mathf.Floor(num);
				}
				string newValue = Util.FormatWholeNumber(num);
				IUserControlledCapacity component = storage.GetComponent<IUserControlledCapacity>();
				if (component != null)
				{
					num2 = Mathf.Min(component.UserMaxCapacity, num2);
				}
				string newValue2 = Util.FormatWholeNumber(num2);
				str = str.Replace("{Stored}", newValue);
				str = str.Replace("{Capacity}", newValue2);
				if (component != null)
				{
					str = str.Replace("{Units}", component.CapacityUnits);
				}
				else
				{
					str = str.Replace("{Units}", GameUtil.GetCurrentMassUnit(false));
				}
				return str;
			};
		}
		if (this.showCapacityStatusItem)
		{
			if (this.showCapacityAsMainStatus)
			{
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Storage.capacityStatusItem, this);
				return;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, Storage.capacityStatusItem, this);
		}
	}

	// Token: 0x0600269A RID: 9882 RVA: 0x000DDAB0 File Offset: 0x000DBCB0
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (!this.allowSettingOnlyFetchMarkedItems)
		{
			this.onlyFetchMarkedItems = false;
		}
		this.UpdateFetchCategory();
	}

	// Token: 0x0600269B RID: 9883 RVA: 0x000DDAC8 File Offset: 0x000DBCC8
	protected override void OnSpawn()
	{
		base.SetWorkTime(this.storageWorkTime);
		foreach (GameObject go in this.items)
		{
			this.ApplyStoredItemModifiers(go, true, true);
			if (this.sendOnStoreOnSpawn)
			{
				go.Trigger(856640610, this);
			}
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.SetSymbolVisiblity("sweep", this.onlyFetchMarkedItems);
		}
		Prioritizable component2 = base.GetComponent<Prioritizable>();
		if (component2 != null)
		{
			Prioritizable prioritizable = component2;
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
		}
		this.UpdateFetchCategory();
		if (this.showUnreachableStatus)
		{
			base.Subscribe<Storage>(-1432940121, Storage.OnReachableChangedDelegate);
			new ReachabilityMonitor.Instance(this).StartSM();
		}
	}

	// Token: 0x0600269C RID: 9884 RVA: 0x000DDBC0 File Offset: 0x000DBDC0
	public GameObject Store(GameObject go, bool hide_popups = false, bool block_events = false, bool do_disease_transfer = true, bool is_deserializing = false)
	{
		if (go == null)
		{
			return null;
		}
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		GameObject result = go;
		if (!hide_popups && PopFXManager.Instance != null)
		{
			LocString loc_string;
			Transform transform;
			if (this.fxPrefix == Storage.FXPrefix.Delivered)
			{
				loc_string = UI.DELIVERED;
				transform = base.transform;
			}
			else
			{
				loc_string = UI.PICKEDUP;
				transform = go.transform;
			}
			string text;
			if (!Assets.IsTagCountable(go.PrefabID()))
			{
				text = string.Format(loc_string, GameUtil.GetFormattedMass(component.Units, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), go.GetProperName());
			}
			else
			{
				text = string.Format(loc_string, (int)component.Units, go.GetProperName());
			}
			PopFXManager.Instance.SpawnFX(Def.GetUISprite(go, "ui", false).first, (this.fxPrefix == Storage.FXPrefix.Delivered) ? PopFXManager.Instance.sprite_Plus : PopFXManager.Instance.sprite_Negative, text, transform, this.storageFXOffset, 1.5f, true, false, false);
		}
		go.transform.parent = base.transform;
		Vector3 position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Move);
		position.z = go.transform.GetPosition().z;
		go.transform.SetPosition(position);
		if (!block_events && do_disease_transfer)
		{
			this.TransferDiseaseWithObject(go);
		}
		if (!is_deserializing)
		{
			Pickupable component2 = go.GetComponent<Pickupable>();
			if (component2 != null)
			{
				if (component2 != null && component2.prevent_absorb_until_stored)
				{
					component2.prevent_absorb_until_stored = false;
				}
				foreach (GameObject gameObject in this.items)
				{
					if (gameObject != null)
					{
						Pickupable component3 = gameObject.GetComponent<Pickupable>();
						if (component3 != null && component3.TryAbsorb(component2, hide_popups, true))
						{
							if (!block_events)
							{
								base.Trigger(-1697596308, go);
								Action<GameObject> onStorageChange = this.OnStorageChange;
								if (onStorageChange != null)
								{
									onStorageChange(go);
								}
								base.Trigger(-778359855, this);
								if (this.OnStorageIncreased != null)
								{
									this.OnStorageIncreased();
								}
							}
							this.ApplyStoredItemModifiers(go, true, false);
							result = gameObject;
							go = null;
							break;
						}
					}
				}
			}
		}
		if (go != null)
		{
			this.items.Add(go);
			if (!is_deserializing)
			{
				this.ApplyStoredItemModifiers(go, true, false);
			}
			if (!block_events)
			{
				go.Trigger(856640610, this);
				base.Trigger(-1697596308, go);
				Action<GameObject> onStorageChange2 = this.OnStorageChange;
				if (onStorageChange2 != null)
				{
					onStorageChange2(go);
				}
				base.Trigger(-778359855, this);
				if (this.OnStorageIncreased != null)
				{
					this.OnStorageIncreased();
				}
			}
		}
		return result;
	}

	// Token: 0x0600269D RID: 9885 RVA: 0x000DDE78 File Offset: 0x000DC078
	public PrimaryElement AddElement(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		Element element2 = ElementLoader.FindElementByHash(element);
		if (element2.IsGas)
		{
			return this.AddGasChunk(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		if (element2.IsLiquid)
		{
			return this.AddLiquid(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		if (element2.IsSolid)
		{
			return this.AddOre(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		return null;
	}

	// Token: 0x0600269E RID: 9886 RVA: 0x000DDEDC File Offset: 0x000DC0DC
	public PrimaryElement AddOre(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.Mass += mass;
			primaryElement.Temperature = finalTemperature;
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddOre");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			Element element2 = ElementLoader.FindElementByHash(element);
			GameObject gameObject = element2.substance.SpawnResource(base.transform.GetPosition(), mass, temperature, disease_idx, disease_count, true, false, true);
			gameObject.GetComponent<Pickupable>().prevent_absorb_until_stored = true;
			element2.substance.ActivateSubstanceGameObject(gameObject, disease_idx, disease_count);
			this.Store(gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x0600269F RID: 9887 RVA: 0x000DDFC0 File Offset: 0x000DC1C0
	public PrimaryElement AddLiquid(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.Mass += mass;
			primaryElement.Temperature = finalTemperature;
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddLiquid");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, base.transform.GetPosition());
			primaryElement = substanceChunk.GetComponent<PrimaryElement>();
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			this.Store(substanceChunk.gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x060026A0 RID: 9888 RVA: 0x000DE094 File Offset: 0x000DC294
	public PrimaryElement AddGasChunk(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float mass2 = primaryElement.Mass;
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, mass2, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.SetMassTemperature(mass2 + mass, finalTemperature);
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddGasChunk");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			SubstanceChunk substanceChunk = GasSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, base.transform.GetPosition());
			primaryElement = substanceChunk.GetComponent<PrimaryElement>();
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			this.Store(substanceChunk.gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x060026A1 RID: 9889 RVA: 0x000DE15C File Offset: 0x000DC35C
	public void Transfer(Storage target, bool block_events = false, bool hide_popups = false)
	{
		while (this.items.Count > 0)
		{
			this.Transfer(this.items[0], target, block_events, hide_popups);
		}
	}

	// Token: 0x060026A2 RID: 9890 RVA: 0x000DE184 File Offset: 0x000DC384
	public bool TransferMass(Storage dest_storage, Tag tag, float amount, bool flatten = false, bool block_events = false, bool hide_popups = false)
	{
		float num = amount;
		while (num > 0f && this.GetAmountAvailable(tag) > 0f)
		{
			num -= this.Transfer(dest_storage, tag, num, block_events, hide_popups);
		}
		if (flatten)
		{
			dest_storage.Flatten(tag);
		}
		return num <= 0f;
	}

	// Token: 0x060026A3 RID: 9891 RVA: 0x000DE1D4 File Offset: 0x000DC3D4
	public float Transfer(Storage dest_storage, Tag tag, float amount, bool block_events = false, bool hide_popups = false)
	{
		GameObject gameObject = this.FindFirst(tag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (amount < component.Units)
			{
				Pickupable component2 = gameObject.GetComponent<Pickupable>();
				Pickupable pickupable = component2.Take(amount);
				dest_storage.Store(pickupable.gameObject, hide_popups, block_events, true, false);
				if (!block_events)
				{
					base.Trigger(-1697596308, component2.gameObject);
					Action<GameObject> onStorageChange = this.OnStorageChange;
					if (onStorageChange != null)
					{
						onStorageChange(component2.gameObject);
					}
				}
			}
			else
			{
				this.Transfer(gameObject, dest_storage, block_events, hide_popups);
				amount = component.Units;
			}
			return amount;
		}
		return 0f;
	}

	// Token: 0x060026A4 RID: 9892 RVA: 0x000DE270 File Offset: 0x000DC470
	public bool Transfer(GameObject go, Storage target, bool block_events = false, bool hide_popups = false)
	{
		this.items.RemoveAll((GameObject it) => it == null);
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.items[i] == go)
			{
				this.items.RemoveAt(i);
				this.ApplyStoredItemModifiers(go, false, false);
				target.Store(go, hide_popups, block_events, true, false);
				if (!block_events)
				{
					base.Trigger(-1697596308, go);
					Action<GameObject> onStorageChange = this.OnStorageChange;
					if (onStorageChange != null)
					{
						onStorageChange(go);
					}
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x060026A5 RID: 9893 RVA: 0x000DE31C File Offset: 0x000DC51C
	public void TransferUnitMass(Storage dest_storage, Tag tag, float unitAmount, bool flatten = false, bool block_events = false, bool hide_popups = false)
	{
		float num = 0f;
		GameObject gameObject = this.FindFirst(tag);
		while (num < unitAmount && gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (unitAmount < component.Units)
			{
				Pickupable component2 = gameObject.GetComponent<Pickupable>();
				Pickupable pickupable = component2.TakeUnit(unitAmount);
				dest_storage.Store(pickupable.gameObject, hide_popups, block_events, true, false);
				if (block_events)
				{
					break;
				}
				base.Trigger(-1697596308, component2.gameObject);
				Action<GameObject> onStorageChange = this.OnStorageChange;
				if (onStorageChange == null)
				{
					return;
				}
				onStorageChange(component2.gameObject);
				return;
			}
			else
			{
				this.Transfer(gameObject, dest_storage, block_events, hide_popups);
				num += component.Units;
				gameObject = this.FindFirst(tag);
			}
		}
	}

	// Token: 0x060026A6 RID: 9894 RVA: 0x000DE3C8 File Offset: 0x000DC5C8
	public bool DropSome(Tag tag, float amount, bool ventGas = false, bool dumpLiquid = false, Vector3 offset = default(Vector3), bool doDiseaseTransfer = true, bool showInWorldNotification = false)
	{
		bool result = false;
		float num = amount;
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.Find(tag, pooledList);
		foreach (GameObject gameObject in pooledList)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			if (component)
			{
				Pickupable pickupable = component.Take(num);
				if (pickupable != null)
				{
					bool flag = false;
					if (ventGas || dumpLiquid)
					{
						Dumpable component2 = pickupable.GetComponent<Dumpable>();
						if (component2 != null)
						{
							if (ventGas && pickupable.GetComponent<PrimaryElement>().Element.IsGas)
							{
								component2.Dump(base.transform.GetPosition() + offset);
								flag = true;
								num -= pickupable.GetComponent<PrimaryElement>().Mass;
								base.Trigger(-1697596308, pickupable.gameObject);
								Action<GameObject> onStorageChange = this.OnStorageChange;
								if (onStorageChange != null)
								{
									onStorageChange(pickupable.gameObject);
								}
								result = true;
								if (showInWorldNotification)
								{
									PopFXManager.Instance.SpawnFX(Def.GetUISprite(component.gameObject, "ui", false).first, PopFXManager.Instance.sprite_Plus, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, true, false, false);
								}
							}
							if (dumpLiquid && pickupable.GetComponent<PrimaryElement>().Element.IsLiquid)
							{
								component2.Dump(base.transform.GetPosition() + offset);
								flag = true;
								num -= pickupable.GetComponent<PrimaryElement>().Mass;
								base.Trigger(-1697596308, pickupable.gameObject);
								Action<GameObject> onStorageChange2 = this.OnStorageChange;
								if (onStorageChange2 != null)
								{
									onStorageChange2(pickupable.gameObject);
								}
								result = true;
								if (showInWorldNotification)
								{
									PopFXManager.Instance.SpawnFX(Def.GetUISprite(component.gameObject, "ui", false).first, PopFXManager.Instance.sprite_Plus, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, true, false, false);
								}
							}
						}
					}
					if (!flag)
					{
						Vector3 position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore) + offset;
						pickupable.transform.SetPosition(position);
						KBatchedAnimController component3 = pickupable.GetComponent<KBatchedAnimController>();
						if (component3)
						{
							component3.SetSceneLayer(Grid.SceneLayer.Ore);
						}
						num -= pickupable.GetComponent<PrimaryElement>().Mass;
						this.MakeWorldActive(pickupable.gameObject);
						base.Trigger(-1697596308, pickupable.gameObject);
						Action<GameObject> onStorageChange3 = this.OnStorageChange;
						if (onStorageChange3 != null)
						{
							onStorageChange3(pickupable.gameObject);
						}
						result = true;
						if (showInWorldNotification)
						{
							PopFXManager.Instance.SpawnFX(Def.GetUISprite(component.gameObject, "ui", false).first, PopFXManager.Instance.sprite_Plus, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, true, false, false);
						}
					}
				}
			}
			if (num <= 0f)
			{
				break;
			}
		}
		pooledList.Recycle();
		return result;
	}

	// Token: 0x060026A7 RID: 9895 RVA: 0x000DE764 File Offset: 0x000DC964
	public void DropAll(Vector3 position, bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default(Vector3), bool do_disease_transfer = true, List<GameObject> collect_dropped_items = null)
	{
		while (this.items.Count > 0)
		{
			GameObject gameObject = this.items[0];
			if (do_disease_transfer)
			{
				this.TransferDiseaseWithObject(gameObject);
			}
			this.items.RemoveAt(0);
			if (gameObject != null)
			{
				bool flag = false;
				if (vent_gas || dump_liquid)
				{
					Dumpable component = gameObject.GetComponent<Dumpable>();
					if (component != null)
					{
						if (vent_gas && gameObject.GetComponent<PrimaryElement>().Element.IsGas)
						{
							component.Dump(position + offset);
							flag = true;
						}
						if (dump_liquid && gameObject.GetComponent<PrimaryElement>().Element.IsLiquid)
						{
							component.Dump(position + offset);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					gameObject.transform.SetPosition(position + offset);
					KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
					if (component2)
					{
						component2.SetSceneLayer(Grid.SceneLayer.Ore);
					}
					this.MakeWorldActive(gameObject);
					if (collect_dropped_items != null)
					{
						collect_dropped_items.Add(gameObject);
					}
				}
			}
		}
	}

	// Token: 0x060026A8 RID: 9896 RVA: 0x000DE859 File Offset: 0x000DCA59
	public void DropAll(bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default(Vector3), bool do_disease_transfer = true, List<GameObject> collect_dropped_items = null)
	{
		this.DropAll(Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore), vent_gas, dump_liquid, offset, do_disease_transfer, collect_dropped_items);
	}

	// Token: 0x060026A9 RID: 9897 RVA: 0x000DE878 File Offset: 0x000DCA78
	public void Drop(Tag t, List<GameObject> obj_list)
	{
		this.Find(t, obj_list);
		foreach (GameObject go in obj_list)
		{
			this.Drop(go, true);
		}
	}

	// Token: 0x060026AA RID: 9898 RVA: 0x000DE8D4 File Offset: 0x000DCAD4
	public void Drop(Tag t)
	{
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.Find(t, pooledList);
		foreach (GameObject go in pooledList)
		{
			this.Drop(go, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x060026AB RID: 9899 RVA: 0x000DE93C File Offset: 0x000DCB3C
	public void DropUnlessMatching(FetchChore chore)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				KPrefabID component = this.items[i].GetComponent<KPrefabID>();
				if (!(((chore.criteria == FetchChore.MatchCriteria.MatchID && chore.tags.Contains(component.PrefabTag)) || (chore.criteria == FetchChore.MatchCriteria.MatchTags && component.HasTag(chore.tagsFirst))) & (!chore.requiredTag.IsValid || component.HasTag(chore.requiredTag)) & !component.HasAnyTags(chore.forbiddenTags)))
				{
					GameObject gameObject = this.items[i];
					this.items.RemoveAt(i);
					i--;
					this.TransferDiseaseWithObject(gameObject);
					this.MakeWorldActive(gameObject);
				}
			}
		}
	}

	// Token: 0x060026AC RID: 9900 RVA: 0x000DEA20 File Offset: 0x000DCC20
	public GameObject[] DropUnlessHasTag(Tag tag)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null) && !this.items[i].GetComponent<KPrefabID>().HasTag(tag))
			{
				GameObject gameObject = this.items[i];
				this.items.RemoveAt(i);
				i--;
				this.TransferDiseaseWithObject(gameObject);
				this.MakeWorldActive(gameObject);
				Dumpable component = gameObject.GetComponent<Dumpable>();
				if (component != null)
				{
					component.Dump(base.transform.GetPosition());
				}
				list.Add(gameObject);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060026AD RID: 9901 RVA: 0x000DEAD8 File Offset: 0x000DCCD8
	public GameObject[] DropHasTags(Tag[] tag)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null) && this.items[i].GetComponent<KPrefabID>().HasAllTags(tag))
			{
				GameObject gameObject = this.items[i];
				this.items.RemoveAt(i);
				i--;
				this.TransferDiseaseWithObject(gameObject);
				this.MakeWorldActive(gameObject);
				Dumpable component = gameObject.GetComponent<Dumpable>();
				if (component != null)
				{
					component.Dump(base.transform.GetPosition());
				}
				list.Add(gameObject);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060026AE RID: 9902 RVA: 0x000DEB90 File Offset: 0x000DCD90
	public GameObject Drop(GameObject go, bool do_disease_transfer = true)
	{
		if (go == null)
		{
			return null;
		}
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(go != this.items[i]))
			{
				this.items[i] = this.items[count - 1];
				this.items.RemoveAt(count - 1);
				if (do_disease_transfer)
				{
					this.TransferDiseaseWithObject(go);
				}
				this.MakeWorldActive(go);
				break;
			}
		}
		return go;
	}

	// Token: 0x060026AF RID: 9903 RVA: 0x000DEC10 File Offset: 0x000DCE10
	public void RenotifyAll()
	{
		this.items.RemoveAll((GameObject it) => it == null);
		foreach (GameObject go in this.items)
		{
			go.Trigger(856640610, this);
		}
	}

	// Token: 0x060026B0 RID: 9904 RVA: 0x000DEC94 File Offset: 0x000DCE94
	private void TransferDiseaseWithObject(GameObject obj)
	{
		if (obj == null || !this.doDiseaseTransfer || this.primaryElement == null)
		{
			return;
		}
		PrimaryElement component = obj.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
		invalid.idx = component.DiseaseIdx;
		invalid.count = (int)((float)component.DiseaseCount * 0.05f);
		SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
		invalid2.idx = this.primaryElement.DiseaseIdx;
		invalid2.count = (int)((float)this.primaryElement.DiseaseCount * 0.05f);
		component.ModifyDiseaseCount(-invalid.count, "Storage.TransferDiseaseWithObject");
		this.primaryElement.ModifyDiseaseCount(-invalid2.count, "Storage.TransferDiseaseWithObject");
		if (invalid.count > 0)
		{
			this.primaryElement.AddDisease(invalid.idx, invalid.count, "Storage.TransferDiseaseWithObject");
		}
		if (invalid2.count > 0)
		{
			component.AddDisease(invalid2.idx, invalid2.count, "Storage.TransferDiseaseWithObject");
		}
	}

	// Token: 0x060026B1 RID: 9905 RVA: 0x000DED9C File Offset: 0x000DCF9C
	private void MakeWorldActive(GameObject go)
	{
		go.transform.parent = null;
		if (this.dropOffset != Vector2.zero)
		{
			go.transform.Translate(this.dropOffset);
		}
		go.Trigger(856640610, null);
		base.Trigger(-1697596308, go);
		Action<GameObject> onStorageChange = this.OnStorageChange;
		if (onStorageChange != null)
		{
			onStorageChange(go);
		}
		this.ApplyStoredItemModifiers(go, false, false);
		if (go != null)
		{
			PrimaryElement component = go.GetComponent<PrimaryElement>();
			if (component != null && component.KeepZeroMassObject)
			{
				component.KeepZeroMassObject = false;
				if (component.Mass <= 0f)
				{
					Util.KDestroyGameObject(go);
				}
			}
		}
	}

	// Token: 0x060026B2 RID: 9906 RVA: 0x000DEE4C File Offset: 0x000DD04C
	public List<GameObject> Find(Tag tag, List<GameObject> result)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				result.Add(gameObject);
			}
		}
		return result;
	}

	// Token: 0x060026B3 RID: 9907 RVA: 0x000DEE98 File Offset: 0x000DD098
	public GameObject FindFirst(Tag tag)
	{
		GameObject result = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				result = gameObject;
				break;
			}
		}
		return result;
	}

	// Token: 0x060026B4 RID: 9908 RVA: 0x000DEEE4 File Offset: 0x000DD0E4
	public PrimaryElement FindFirstWithMass(Tag tag, float mass = 0f)
	{
		PrimaryElement result = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.Mass > 0f && component.Mass >= mass)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060026B5 RID: 9909 RVA: 0x000DEF4C File Offset: 0x000DD14C
	private void Flatten(Tag tag_to_combine)
	{
		GameObject gameObject = this.FindFirst(tag_to_combine);
		if (gameObject == null)
		{
			return;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			GameObject gameObject2 = this.items[i];
			if (gameObject2.HasTag(tag_to_combine) && gameObject2 != gameObject)
			{
				PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
				component.Mass += component2.Mass;
				this.ConsumeIgnoringDisease(gameObject2);
			}
		}
	}

	// Token: 0x060026B6 RID: 9910 RVA: 0x000DEFCC File Offset: 0x000DD1CC
	public HashSet<Tag> GetAllIDsInStorage()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject go = this.items[i];
			hashSet.Add(go.PrefabID());
		}
		return hashSet;
	}

	// Token: 0x060026B7 RID: 9911 RVA: 0x000DF010 File Offset: 0x000DD210
	public GameObject Find(int ID)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (ID == gameObject.PrefabID().GetHashCode())
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x060026B8 RID: 9912 RVA: 0x000DF05A File Offset: 0x000DD25A
	public void ConsumeAllIgnoringDisease()
	{
		this.ConsumeAllIgnoringDisease(Tag.Invalid);
	}

	// Token: 0x060026B9 RID: 9913 RVA: 0x000DF068 File Offset: 0x000DD268
	public void ConsumeAllIgnoringDisease(Tag tag)
	{
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (!(tag != Tag.Invalid) || this.items[i].HasTag(tag))
			{
				this.ConsumeIgnoringDisease(this.items[i]);
			}
		}
	}

	// Token: 0x060026BA RID: 9914 RVA: 0x000DF0C0 File Offset: 0x000DD2C0
	public void ConsumeAndGetDisease(Tag tag, float amount, out float amount_consumed, out SimUtil.DiseaseInfo disease_info, out float aggregate_temperature)
	{
		SimHashes simHashes;
		this.ConsumeAndGetDisease(tag, amount, out amount_consumed, out disease_info, out aggregate_temperature, out simHashes);
	}

	// Token: 0x060026BB RID: 9915 RVA: 0x000DF0DC File Offset: 0x000DD2DC
	public void ConsumeAndGetDisease(Tag tag, float amount, out float amount_consumed, out SimUtil.DiseaseInfo disease_info, out float aggregate_temperature, out SimHashes mostRelevantItemElement)
	{
		DebugUtil.Assert(tag.IsValid);
		amount_consumed = 0f;
		disease_info = SimUtil.DiseaseInfo.Invalid;
		mostRelevantItemElement = SimHashes.Vacuum;
		aggregate_temperature = 0f;
		bool flag = false;
		float num = 0f;
		int num2 = 0;
		while (num2 < this.items.Count && amount > 0f)
		{
			GameObject gameObject = this.items[num2];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.Units > 0f)
				{
					flag = true;
					float num3 = Math.Min(component.Units, amount);
					global::Debug.Assert(num3 > 0f, "Delta amount was zero, which should be impossible.");
					aggregate_temperature = SimUtil.CalculateFinalTemperature(amount_consumed, aggregate_temperature, num3, component.Temperature);
					SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(component, num3 / component.Units);
					disease_info = SimUtil.CalculateFinalDiseaseInfo(disease_info, percentOfDisease);
					component.Units -= num3;
					component.ModifyDiseaseCount(-percentOfDisease.count, "Storage.ConsumeAndGetDisease");
					amount -= num3;
					amount_consumed += num3;
					if (num3 > num)
					{
						num = num3;
						mostRelevantItemElement = component.ElementID;
					}
				}
				if (component.Units <= 0f && !component.KeepZeroMassObject)
				{
					if (this.deleted_objects == null)
					{
						this.deleted_objects = new List<GameObject>();
					}
					this.deleted_objects.Add(gameObject);
				}
				base.Trigger(-1697596308, gameObject);
				Action<GameObject> onStorageChange = this.OnStorageChange;
				if (onStorageChange != null)
				{
					onStorageChange(gameObject);
				}
			}
			num2++;
		}
		if (!flag)
		{
			aggregate_temperature = base.GetComponent<PrimaryElement>().Temperature;
		}
		if (this.deleted_objects != null)
		{
			for (int i = 0; i < this.deleted_objects.Count; i++)
			{
				this.items.Remove(this.deleted_objects[i]);
				Util.KDestroyGameObject(this.deleted_objects[i]);
			}
			this.deleted_objects.Clear();
		}
	}

	// Token: 0x060026BC RID: 9916 RVA: 0x000DF2E8 File Offset: 0x000DD4E8
	public void ConsumeAndGetDisease(Recipe.Ingredient ingredient, out SimUtil.DiseaseInfo disease_info, out float temperature)
	{
		float num;
		this.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out num, out disease_info, out temperature);
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x000DF30C File Offset: 0x000DD50C
	public void ConsumeIgnoringDisease(Tag tag, float amount)
	{
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		this.ConsumeAndGetDisease(tag, amount, out num, out diseaseInfo, out num2);
	}

	// Token: 0x060026BE RID: 9918 RVA: 0x000DF328 File Offset: 0x000DD528
	public void ConsumeIgnoringDisease(GameObject item_go)
	{
		if (this.items.Contains(item_go))
		{
			PrimaryElement component = item_go.GetComponent<PrimaryElement>();
			if (component != null && component.KeepZeroMassObject)
			{
				component.Units = 0f;
				component.ModifyDiseaseCount(-component.DiseaseCount, "consume item");
				base.Trigger(-1697596308, item_go);
				Action<GameObject> onStorageChange = this.OnStorageChange;
				if (onStorageChange == null)
				{
					return;
				}
				onStorageChange(item_go);
				return;
			}
			else
			{
				this.items.Remove(item_go);
				base.Trigger(-1697596308, item_go);
				Action<GameObject> onStorageChange2 = this.OnStorageChange;
				if (onStorageChange2 != null)
				{
					onStorageChange2(item_go);
				}
				item_go.DeleteObject();
			}
		}
	}

	// Token: 0x060026BF RID: 9919 RVA: 0x000DF3CA File Offset: 0x000DD5CA
	public GameObject Drop(int ID)
	{
		return this.Drop(this.Find(ID), true);
	}

	// Token: 0x060026C0 RID: 9920 RVA: 0x000DF3DC File Offset: 0x000DD5DC
	private void OnDeath(object data)
	{
		List<GameObject> list = new List<GameObject>();
		bool vent_gas = true;
		bool dump_liquid = true;
		List<GameObject> collect_dropped_items = list;
		this.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
		if (this.onDestroyItemsDropped != null)
		{
			this.onDestroyItemsDropped(list);
		}
	}

	// Token: 0x060026C1 RID: 9921 RVA: 0x000DF418 File Offset: 0x000DD618
	public bool IsFull()
	{
		return this.RemainingCapacity() <= 0f;
	}

	// Token: 0x060026C2 RID: 9922 RVA: 0x000DF42A File Offset: 0x000DD62A
	public bool IsEmpty()
	{
		return this.items.Count == 0;
	}

	// Token: 0x060026C3 RID: 9923 RVA: 0x000DF43A File Offset: 0x000DD63A
	public float Capacity()
	{
		return this.capacityKg;
	}

	// Token: 0x060026C4 RID: 9924 RVA: 0x000DF442 File Offset: 0x000DD642
	public bool IsEndOfLife()
	{
		return this.endOfLife;
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x000DF44C File Offset: 0x000DD64C
	public float ExactMassStored()
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				PrimaryElement component = this.items[i].GetComponent<PrimaryElement>();
				if (component != null)
				{
					num += component.Units * component.MassPerUnit;
				}
			}
		}
		return num;
	}

	// Token: 0x060026C6 RID: 9926 RVA: 0x000DF4B5 File Offset: 0x000DD6B5
	public float MassStored()
	{
		return (float)Mathf.RoundToInt(this.ExactMassStored() * 1000f) / 1000f;
	}

	// Token: 0x060026C7 RID: 9927 RVA: 0x000DF4D0 File Offset: 0x000DD6D0
	public float UnitsStored()
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				PrimaryElement component = this.items[i].GetComponent<PrimaryElement>();
				if (component != null)
				{
					num += component.Units;
				}
			}
		}
		return (float)Mathf.RoundToInt(num * 1000f) / 1000f;
	}

	// Token: 0x060026C8 RID: 9928 RVA: 0x000DF544 File Offset: 0x000DD744
	public bool Has(Tag tag)
	{
		bool result = false;
		foreach (GameObject gameObject in this.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.HasTag(tag) && component.Mass > 0f)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060026C9 RID: 9929 RVA: 0x000DF5C0 File Offset: 0x000DD7C0
	public PrimaryElement AddToPrimaryElement(SimHashes element, float additional_mass, float temperature)
	{
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, additional_mass);
			primaryElement.Mass += additional_mass;
			primaryElement.Temperature = finalTemperature;
		}
		return primaryElement;
	}

	// Token: 0x060026CA RID: 9930 RVA: 0x000DF608 File Offset: 0x000DD808
	public PrimaryElement FindPrimaryElement(SimHashes element)
	{
		PrimaryElement result = null;
		foreach (GameObject gameObject in this.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.ElementID == element)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060026CB RID: 9931 RVA: 0x000DF674 File Offset: 0x000DD874
	public float RemainingCapacity()
	{
		return this.capacityKg - this.MassStored();
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x000DF683 File Offset: 0x000DD883
	public bool GetOnlyFetchMarkedItems()
	{
		return this.onlyFetchMarkedItems;
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x000DF68B File Offset: 0x000DD88B
	public void SetOnlyFetchMarkedItems(bool is_set)
	{
		if (is_set != this.onlyFetchMarkedItems)
		{
			this.onlyFetchMarkedItems = is_set;
			this.UpdateFetchCategory();
			base.Trigger(644822890, null);
			base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("sweep", is_set);
		}
	}

	// Token: 0x060026CE RID: 9934 RVA: 0x000DF6C5 File Offset: 0x000DD8C5
	private void UpdateFetchCategory()
	{
		if (this.fetchCategory == Storage.FetchCategory.Building)
		{
			return;
		}
		this.fetchCategory = (this.onlyFetchMarkedItems ? Storage.FetchCategory.StorageSweepOnly : Storage.FetchCategory.GeneralStorage);
	}

	// Token: 0x060026CF RID: 9935 RVA: 0x000DF6E2 File Offset: 0x000DD8E2
	protected override void OnCleanUp()
	{
		if (this.items.Count != 0)
		{
			global::Debug.LogWarning("Storage for [" + base.gameObject.name + "] is being destroyed but it still contains items!", base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x060026D0 RID: 9936 RVA: 0x000DF71C File Offset: 0x000DD91C
	private void OnQueueDestroyObject(object data)
	{
		this.endOfLife = true;
		List<GameObject> list = new List<GameObject>();
		bool vent_gas = true;
		bool dump_liquid = false;
		List<GameObject> collect_dropped_items = list;
		this.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
		if (this.onDestroyItemsDropped != null)
		{
			this.onDestroyItemsDropped(list);
		}
		this.OnCleanUp();
	}

	// Token: 0x060026D1 RID: 9937 RVA: 0x000DF765 File Offset: 0x000DD965
	public void Remove(GameObject go, bool do_disease_transfer = true)
	{
		this.items.Remove(go);
		if (do_disease_transfer)
		{
			this.TransferDiseaseWithObject(go);
		}
		base.Trigger(-1697596308, go);
		Action<GameObject> onStorageChange = this.OnStorageChange;
		if (onStorageChange != null)
		{
			onStorageChange(go);
		}
		this.ApplyStoredItemModifiers(go, false, false);
	}

	// Token: 0x060026D2 RID: 9938 RVA: 0x000DF7A8 File Offset: 0x000DD9A8
	public bool ForceStore(Tag tag, float amount)
	{
		global::Debug.Assert(amount < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT);
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				gameObject.GetComponent<PrimaryElement>().Mass += amount;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060026D3 RID: 9939 RVA: 0x000DF810 File Offset: 0x000DDA10
	public float GetAmountAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x060026D4 RID: 9940 RVA: 0x000DF868 File Offset: 0x000DDA68
	public float GetAmountAvailable(Tag tag, Tag[] forbiddenTags = null)
	{
		if (forbiddenTags == null)
		{
			return this.GetAmountAvailable(tag);
		}
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag) && !gameObject.HasAnyTags(forbiddenTags))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x060026D5 RID: 9941 RVA: 0x000DF8D4 File Offset: 0x000DDAD4
	public float GetUnitsAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x060026D6 RID: 9942 RVA: 0x000DF92C File Offset: 0x000DDB2C
	public float GetMassAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Mass;
			}
		}
		return num;
	}

	// Token: 0x060026D7 RID: 9943 RVA: 0x000DF984 File Offset: 0x000DDB84
	public float GetMassAvailable(SimHashes element)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.ElementID == element)
				{
					num += component.Mass;
				}
			}
		}
		return num;
	}

	// Token: 0x060026D8 RID: 9944 RVA: 0x000DF9E0 File Offset: 0x000DDBE0
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.showDescriptor)
		{
			descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.STORAGECAPACITY, GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STORAGECAPACITY, GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
		}
		return descriptors;
	}

	// Token: 0x060026D9 RID: 9945 RVA: 0x000DFA50 File Offset: 0x000DDC50
	public static void MakeItemTemperatureInsulated(GameObject go, bool is_stored, bool is_initializing)
	{
		SimTemperatureTransfer component = go.GetComponent<SimTemperatureTransfer>();
		if (component == null)
		{
			return;
		}
		component.enabled = !is_stored;
	}

	// Token: 0x060026DA RID: 9946 RVA: 0x000DFA78 File Offset: 0x000DDC78
	public static void MakeItemInvisible(GameObject go, bool is_stored, bool is_initializing)
	{
		if (is_initializing)
		{
			return;
		}
		bool flag = !is_stored;
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component != null && component.enabled != flag)
		{
			component.enabled = flag;
		}
		KSelectable component2 = go.GetComponent<KSelectable>();
		if (component2 != null && component2.enabled != flag)
		{
			component2.enabled = flag;
		}
	}

	// Token: 0x060026DB RID: 9947 RVA: 0x000DFACE File Offset: 0x000DDCCE
	public static void MakeItemSealed(GameObject go, bool is_stored, bool is_initializing)
	{
		if (go != null)
		{
			if (is_stored)
			{
				go.GetComponent<KPrefabID>().AddTag(GameTags.Sealed, false);
				return;
			}
			go.GetComponent<KPrefabID>().RemoveTag(GameTags.Sealed);
		}
	}

	// Token: 0x060026DC RID: 9948 RVA: 0x000DFAFE File Offset: 0x000DDCFE
	public static void MakeItemPreserved(GameObject go, bool is_stored, bool is_initializing)
	{
		if (go != null)
		{
			if (is_stored)
			{
				go.GetComponent<KPrefabID>().AddTag(GameTags.Preserved, false);
				return;
			}
			go.GetComponent<KPrefabID>().RemoveTag(GameTags.Preserved);
		}
	}

	// Token: 0x060026DD RID: 9949 RVA: 0x000DFB30 File Offset: 0x000DDD30
	private void ApplyStoredItemModifiers(GameObject go, bool is_stored, bool is_initializing)
	{
		List<Storage.StoredItemModifier> list = this.defaultStoredItemModifers;
		for (int i = 0; i < list.Count; i++)
		{
			Storage.StoredItemModifier storedItemModifier = list[i];
			for (int j = 0; j < Storage.StoredItemModifierHandlers.Count; j++)
			{
				Storage.StoredItemModifierInfo storedItemModifierInfo = Storage.StoredItemModifierHandlers[j];
				if (storedItemModifierInfo.modifier == storedItemModifier)
				{
					storedItemModifierInfo.toggleState(go, is_stored, is_initializing);
					break;
				}
			}
		}
	}

	// Token: 0x060026DE RID: 9950 RVA: 0x000DFB9C File Offset: 0x000DDD9C
	protected virtual void OnCopySettings(object data)
	{
		Storage component = ((GameObject)data).GetComponent<Storage>();
		if (component != null)
		{
			this.SetOnlyFetchMarkedItems(component.onlyFetchMarkedItems);
		}
	}

	// Token: 0x060026DF RID: 9951 RVA: 0x000DFBCC File Offset: 0x000DDDCC
	private void OnPriorityChanged(PrioritySetting priority)
	{
		foreach (GameObject go in this.items)
		{
			go.Trigger(-1626373771, this);
		}
	}

	// Token: 0x060026E0 RID: 9952 RVA: 0x000DFC24 File Offset: 0x000DDE24
	private void OnReachableChanged(object data)
	{
		bool value = ((Boxed<bool>)data).value;
		KSelectable component = base.GetComponent<KSelectable>();
		if (value)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, this);
	}

	// Token: 0x060026E1 RID: 9953 RVA: 0x000DFC74 File Offset: 0x000DDE74
	public void SetContentsDeleteOffGrid(bool delete_off_grid)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			Pickupable component = this.items[i].GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = delete_off_grid;
			}
			Storage component2 = this.items[i].GetComponent<Storage>();
			if (component2 != null)
			{
				component2.SetContentsDeleteOffGrid(delete_off_grid);
			}
		}
	}

	// Token: 0x060026E2 RID: 9954 RVA: 0x000DFCDC File Offset: 0x000DDEDC
	private bool ShouldSaveItem(GameObject go)
	{
		if (!this.shouldSaveItems)
		{
			return false;
		}
		bool result = false;
		if (go != null && go.GetComponent<SaveLoadRoot>() != null && go.GetComponent<PrimaryElement>().Mass > 0f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x060026E3 RID: 9955 RVA: 0x000DFD24 File Offset: 0x000DDF24
	public void Serialize(BinaryWriter writer)
	{
		int num = 0;
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ShouldSaveItem(this.items[i]))
			{
				num++;
			}
		}
		writer.Write(num);
		if (num == 0)
		{
			return;
		}
		if (this.items != null && this.items.Count > 0)
		{
			for (int j = 0; j < this.items.Count; j++)
			{
				GameObject gameObject = this.items[j];
				if (this.ShouldSaveItem(gameObject))
				{
					SaveLoadRoot component = gameObject.GetComponent<SaveLoadRoot>();
					if (component != null)
					{
						string name = gameObject.GetComponent<KPrefabID>().GetSaveLoadTag().Name;
						writer.WriteKleiString(name);
						component.Save(writer);
					}
					else
					{
						global::Debug.Log("Tried to save obj in storage but obj has no SaveLoadRoot", gameObject);
					}
				}
			}
		}
	}

	// Token: 0x060026E4 RID: 9956 RVA: 0x000DFE00 File Offset: 0x000DE000
	public void Deserialize(IReader reader)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		this.ClearItems();
		int num4 = reader.ReadInt32();
		this.items = new List<GameObject>(num4);
		for (int i = 0; i < num4; i++)
		{
			float realtimeSinceStartup2 = Time.realtimeSinceStartup;
			Tag tag = TagManager.Create(reader.ReadKleiString());
			SaveLoadRoot saveLoadRoot = SaveLoadRoot.Load(tag, reader);
			num += Time.realtimeSinceStartup - realtimeSinceStartup2;
			if (saveLoadRoot != null)
			{
				KBatchedAnimController component = saveLoadRoot.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					component.enabled = false;
				}
				saveLoadRoot.SetRegistered(false);
				float realtimeSinceStartup3 = Time.realtimeSinceStartup;
				GameObject gameObject = this.Store(saveLoadRoot.gameObject, true, true, false, true);
				num2 += Time.realtimeSinceStartup - realtimeSinceStartup3;
				if (gameObject != null)
				{
					Pickupable component2 = gameObject.GetComponent<Pickupable>();
					if (component2 != null)
					{
						float realtimeSinceStartup4 = Time.realtimeSinceStartup;
						component2.OnStore(this);
						num3 += Time.realtimeSinceStartup - realtimeSinceStartup4;
					}
					Storable component3 = gameObject.GetComponent<Storable>();
					if (component3 != null)
					{
						float realtimeSinceStartup5 = Time.realtimeSinceStartup;
						component3.OnStore(this);
						num3 += Time.realtimeSinceStartup - realtimeSinceStartup5;
					}
					if (this.dropOnLoad)
					{
						this.Drop(saveLoadRoot.gameObject, true);
					}
				}
			}
			else
			{
				global::Debug.LogWarning("Tried to deserialize " + tag.ToString() + " into storage but failed", base.gameObject);
			}
		}
	}

	// Token: 0x060026E5 RID: 9957 RVA: 0x000DFF7C File Offset: 0x000DE17C
	private void ClearItems()
	{
		foreach (GameObject go in this.items)
		{
			go.DeleteObject();
		}
		this.items.Clear();
	}

	// Token: 0x060026E6 RID: 9958 RVA: 0x000DFFD8 File Offset: 0x000DE1D8
	public void UpdateStoredItemCachedCells()
	{
		foreach (GameObject gameObject in this.items)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			if (component != null)
			{
				component.UpdateCachedCellFromStoragePosition();
			}
		}
	}

	// Token: 0x040016BF RID: 5823
	public bool allowItemRemoval;

	// Token: 0x040016C0 RID: 5824
	public bool ignoreSourcePriority;

	// Token: 0x040016C1 RID: 5825
	public bool onlyTransferFromLowerPriority;

	// Token: 0x040016C2 RID: 5826
	public float capacityKg = 20000f;

	// Token: 0x040016C3 RID: 5827
	public bool showDescriptor;

	// Token: 0x040016C5 RID: 5829
	public bool doDiseaseTransfer = true;

	// Token: 0x040016C6 RID: 5830
	public List<Tag> storageFilters;

	// Token: 0x040016C7 RID: 5831
	public bool useGunForDelivery = true;

	// Token: 0x040016C8 RID: 5832
	public bool sendOnStoreOnSpawn;

	// Token: 0x040016C9 RID: 5833
	public bool showInUI = true;

	// Token: 0x040016CA RID: 5834
	public bool storeDropsFromButcherables;

	// Token: 0x040016CB RID: 5835
	public bool allowClearable;

	// Token: 0x040016CC RID: 5836
	public bool showCapacityStatusItem;

	// Token: 0x040016CD RID: 5837
	public bool showCapacityAsMainStatus;

	// Token: 0x040016CE RID: 5838
	public bool showUnreachableStatus;

	// Token: 0x040016CF RID: 5839
	public bool showSideScreenTitleBar;

	// Token: 0x040016D0 RID: 5840
	public bool useWideOffsets;

	// Token: 0x040016D1 RID: 5841
	public Action<List<GameObject>> onDestroyItemsDropped;

	// Token: 0x040016D2 RID: 5842
	public Action<GameObject> OnStorageChange;

	// Token: 0x040016D3 RID: 5843
	public Vector2 dropOffset = Vector2.zero;

	// Token: 0x040016D4 RID: 5844
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x040016D5 RID: 5845
	public Vector2 gunTargetOffset;

	// Token: 0x040016D6 RID: 5846
	public Storage.FetchCategory fetchCategory;

	// Token: 0x040016D7 RID: 5847
	public int storageNetworkID = -1;

	// Token: 0x040016D8 RID: 5848
	public Tag storageID = GameTags.StoragesIds.DefaultStorage;

	// Token: 0x040016D9 RID: 5849
	public float storageFullMargin;

	// Token: 0x040016DA RID: 5850
	public Vector3 storageFXOffset = Vector3.zero;

	// Token: 0x040016DB RID: 5851
	private static readonly EventSystem.IntraObjectHandler<Storage> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x040016DC RID: 5852
	public Storage.FXPrefix fxPrefix;

	// Token: 0x040016DD RID: 5853
	public List<GameObject> items = new List<GameObject>();

	// Token: 0x040016DE RID: 5854
	[MyCmpGet]
	public Prioritizable prioritizable;

	// Token: 0x040016DF RID: 5855
	[MyCmpGet]
	public Automatable automatable;

	// Token: 0x040016E0 RID: 5856
	[MyCmpGet]
	protected PrimaryElement primaryElement;

	// Token: 0x040016E1 RID: 5857
	public bool dropOnLoad;

	// Token: 0x040016E2 RID: 5858
	protected float maxKGPerItem = float.MaxValue;

	// Token: 0x040016E3 RID: 5859
	private bool endOfLife;

	// Token: 0x040016E4 RID: 5860
	public bool allowSettingOnlyFetchMarkedItems = true;

	// Token: 0x040016E5 RID: 5861
	[Serialize]
	private bool onlyFetchMarkedItems;

	// Token: 0x040016E6 RID: 5862
	[Serialize]
	private bool shouldSaveItems = true;

	// Token: 0x040016E7 RID: 5863
	public float storageWorkTime = 1.5f;

	// Token: 0x040016E8 RID: 5864
	private static readonly List<Storage.StoredItemModifierInfo> StoredItemModifierHandlers = new List<Storage.StoredItemModifierInfo>
	{
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Hide, new Action<GameObject, bool, bool>(Storage.MakeItemInvisible)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Insulate, new Action<GameObject, bool, bool>(Storage.MakeItemTemperatureInsulated)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Seal, new Action<GameObject, bool, bool>(Storage.MakeItemSealed)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Preserve, new Action<GameObject, bool, bool>(Storage.MakeItemPreserved))
	};

	// Token: 0x040016E9 RID: 5865
	[SerializeField]
	private List<Storage.StoredItemModifier> defaultStoredItemModifers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide
	};

	// Token: 0x040016EA RID: 5866
	public static readonly List<Storage.StoredItemModifier> StandardSealedStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};

	// Token: 0x040016EB RID: 5867
	public static readonly List<Storage.StoredItemModifier> StandardFabricatorStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};

	// Token: 0x040016EC RID: 5868
	public static readonly List<Storage.StoredItemModifier> StandardInsulatedStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal,
		Storage.StoredItemModifier.Insulate
	};

	// Token: 0x040016EE RID: 5870
	private static StatusItem capacityStatusItem;

	// Token: 0x040016EF RID: 5871
	private static readonly EventSystem.IntraObjectHandler<Storage> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<Storage>(GameTags.Dead, delegate(Storage component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x040016F0 RID: 5872
	private static readonly EventSystem.IntraObjectHandler<Storage> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnQueueDestroyObject(data);
	});

	// Token: 0x040016F1 RID: 5873
	private static readonly EventSystem.IntraObjectHandler<Storage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040016F2 RID: 5874
	private List<GameObject> deleted_objects;

	// Token: 0x0200151F RID: 5407
	public enum StoredItemModifier
	{
		// Token: 0x040070CA RID: 28874
		Insulate,
		// Token: 0x040070CB RID: 28875
		Hide,
		// Token: 0x040070CC RID: 28876
		Seal,
		// Token: 0x040070CD RID: 28877
		Preserve
	}

	// Token: 0x02001520 RID: 5408
	public enum FetchCategory
	{
		// Token: 0x040070CF RID: 28879
		Building,
		// Token: 0x040070D0 RID: 28880
		GeneralStorage,
		// Token: 0x040070D1 RID: 28881
		StorageSweepOnly
	}

	// Token: 0x02001521 RID: 5409
	public enum FXPrefix
	{
		// Token: 0x040070D3 RID: 28883
		Delivered,
		// Token: 0x040070D4 RID: 28884
		PickedUp
	}

	// Token: 0x02001522 RID: 5410
	private struct StoredItemModifierInfo
	{
		// Token: 0x06009246 RID: 37446 RVA: 0x00373A9E File Offset: 0x00371C9E
		public StoredItemModifierInfo(Storage.StoredItemModifier modifier, Action<GameObject, bool, bool> toggle_state)
		{
			this.modifier = modifier;
			this.toggleState = toggle_state;
		}

		// Token: 0x040070D5 RID: 28885
		public Storage.StoredItemModifier modifier;

		// Token: 0x040070D6 RID: 28886
		public Action<GameObject, bool, bool> toggleState;
	}
}
