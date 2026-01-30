using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000809 RID: 2057
public class StorageTile : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>
{
	// Token: 0x06003767 RID: 14183 RVA: 0x00137780 File Offset: 0x00135980
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.OnStorageChanged)).EventHandler(GameHashes.StorageTileTargetItemChanged, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals));
		this.idle.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.OnStorageChange, this.awaitingDelivery, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingDelivery)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.change, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingForSettingChange));
		this.change.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.NoLongerAwaitingForSettingChange)).DefaultState(this.change.awaitingSettingsChange);
		this.change.awaitingSettingsChange.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.StartWorkChore)).Exit(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.CancelWorkChore)).ToggleStatusItem(Db.Get().BuildingStatusItems.ChangeStorageTileTarget, null).WorkableCompleteTransition((StorageTile.Instance smi) => smi.GetWorkable(), this.change.complete);
		this.change.complete.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.ApplySettings)).Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.DropUndesiredItems)).EnterTransition(this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.HasAnyDesiredItemStored)).EnterTransition(this.awaitingDelivery, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingDelivery));
		this.awaitingDelivery.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.OnStorageChange, this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.HasAnyDesiredItemStored)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.change, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingForSettingChange));
	}

	// Token: 0x06003768 RID: 14184 RVA: 0x0013798B File Offset: 0x00135B8B
	public static void DropUndesiredItems(StorageTile.Instance smi)
	{
		smi.DropUndesiredItems();
	}

	// Token: 0x06003769 RID: 14185 RVA: 0x00137993 File Offset: 0x00135B93
	public static void ApplySettings(StorageTile.Instance smi)
	{
		smi.ApplySettings();
	}

	// Token: 0x0600376A RID: 14186 RVA: 0x0013799B File Offset: 0x00135B9B
	public static void StartWorkChore(StorageTile.Instance smi)
	{
		smi.StartChangeSettingChore();
	}

	// Token: 0x0600376B RID: 14187 RVA: 0x001379A3 File Offset: 0x00135BA3
	public static void CancelWorkChore(StorageTile.Instance smi)
	{
		smi.CanceChangeSettingChore();
	}

	// Token: 0x0600376C RID: 14188 RVA: 0x001379AB File Offset: 0x00135BAB
	public static void RefreshContentVisuals(StorageTile.Instance smi)
	{
		smi.UpdateContentSymbol();
	}

	// Token: 0x0600376D RID: 14189 RVA: 0x001379B3 File Offset: 0x00135BB3
	public static bool IsAwaitingForSettingChange(StorageTile.Instance smi)
	{
		return smi.IsPendingChange;
	}

	// Token: 0x0600376E RID: 14190 RVA: 0x001379BB File Offset: 0x00135BBB
	public static bool NoLongerAwaitingForSettingChange(StorageTile.Instance smi)
	{
		return !smi.IsPendingChange;
	}

	// Token: 0x0600376F RID: 14191 RVA: 0x001379C6 File Offset: 0x00135BC6
	public static bool HasAnyDesiredItemStored(StorageTile.Instance smi)
	{
		return smi.HasAnyDesiredContents;
	}

	// Token: 0x06003770 RID: 14192 RVA: 0x001379CE File Offset: 0x00135BCE
	public static void OnStorageChanged(StorageTile.Instance smi)
	{
		smi.PlayDoorAnimation();
		StorageTile.RefreshContentVisuals(smi);
	}

	// Token: 0x06003771 RID: 14193 RVA: 0x001379DC File Offset: 0x00135BDC
	public static bool IsAwaitingDelivery(StorageTile.Instance smi)
	{
		return !smi.IsPendingChange && !smi.HasAnyDesiredContents;
	}

	// Token: 0x040021BC RID: 8636
	public const string METER_TARGET = "meter_target";

	// Token: 0x040021BD RID: 8637
	public const string METER_ANIMATION = "meter";

	// Token: 0x040021BE RID: 8638
	public static HashedString DOOR_SYMBOL_NAME = new HashedString("storage_door");

	// Token: 0x040021BF RID: 8639
	public static HashedString ITEM_SYMBOL_TARGET = new HashedString("meter_target_object");

	// Token: 0x040021C0 RID: 8640
	public static HashedString ITEM_SYMBOL_NAME = new HashedString("object");

	// Token: 0x040021C1 RID: 8641
	public const string ITEM_SYMBOL_ANIMATION = "meter_object";

	// Token: 0x040021C2 RID: 8642
	public static HashedString ITEM_PREVIEW_SYMBOL_TARGET = new HashedString("meter_target_object_ui");

	// Token: 0x040021C3 RID: 8643
	public static HashedString ITEM_PREVIEW_SYMBOL_NAME = new HashedString("object_ui");

	// Token: 0x040021C4 RID: 8644
	public const string ITEM_PREVIEW_SYMBOL_ANIMATION = "meter_object_ui";

	// Token: 0x040021C5 RID: 8645
	public static HashedString ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME = new HashedString("placeholder");

	// Token: 0x040021C6 RID: 8646
	public const string DEFAULT_ANIMATION_NAME = "on";

	// Token: 0x040021C7 RID: 8647
	public const string STORAGE_CHANGE_ANIMATION_NAME = "door";

	// Token: 0x040021C8 RID: 8648
	public const string SYMBOL_ANIMATION_NAME_AWAITING_DELIVERY = "ui";

	// Token: 0x040021C9 RID: 8649
	public static Tag INVALID_TAG = GameTags.Void;

	// Token: 0x040021CA RID: 8650
	private StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.TagParameter TargetItemTag = new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.TagParameter(StorageTile.INVALID_TAG);

	// Token: 0x040021CB RID: 8651
	public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State idle;

	// Token: 0x040021CC RID: 8652
	public StorageTile.SettingsChangeStates change;

	// Token: 0x040021CD RID: 8653
	public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State awaitingDelivery;

	// Token: 0x02001792 RID: 6034
	public class SpecificItemTagSizeInstruction
	{
		// Token: 0x06009B8B RID: 39819 RVA: 0x0039593F File Offset: 0x00393B3F
		public SpecificItemTagSizeInstruction(Tag tag, float size)
		{
			this.tag = tag;
			this.sizeMultiplier = size;
		}

		// Token: 0x0400780A RID: 30730
		public Tag tag;

		// Token: 0x0400780B RID: 30731
		public float sizeMultiplier;
	}

	// Token: 0x02001793 RID: 6035
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009B8C RID: 39820 RVA: 0x00395958 File Offset: 0x00393B58
		public StorageTile.SpecificItemTagSizeInstruction GetSizeInstructionForObject(GameObject obj)
		{
			if (this.specialItemCases == null)
			{
				return null;
			}
			KPrefabID component = obj.GetComponent<KPrefabID>();
			foreach (StorageTile.SpecificItemTagSizeInstruction specificItemTagSizeInstruction in this.specialItemCases)
			{
				if (component.HasTag(specificItemTagSizeInstruction.tag))
				{
					return specificItemTagSizeInstruction;
				}
			}
			return null;
		}

		// Token: 0x0400780C RID: 30732
		public float MaxCapacity;

		// Token: 0x0400780D RID: 30733
		public StorageTile.SpecificItemTagSizeInstruction[] specialItemCases;
	}

	// Token: 0x02001794 RID: 6036
	public class SettingsChangeStates : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State
	{
		// Token: 0x0400780E RID: 30734
		public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State awaitingSettingsChange;

		// Token: 0x0400780F RID: 30735
		public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State complete;
	}

	// Token: 0x02001795 RID: 6037
	public new class Instance : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.GameInstance, IUserControlledCapacity
	{
		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06009B8F RID: 39823 RVA: 0x003959B0 File Offset: 0x00393BB0
		public Tag TargetTag
		{
			get
			{
				return base.smi.sm.TargetItemTag.Get(base.smi);
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06009B90 RID: 39824 RVA: 0x003959CD File Offset: 0x00393BCD
		public bool HasContents
		{
			get
			{
				return this.storage.MassStored() > 0f;
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06009B91 RID: 39825 RVA: 0x003959E1 File Offset: 0x00393BE1
		public bool HasAnyDesiredContents
		{
			get
			{
				if (!(this.TargetTag == StorageTile.INVALID_TAG))
				{
					return this.AmountOfDesiredContentStored > 0f;
				}
				return !this.HasContents;
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06009B92 RID: 39826 RVA: 0x00395A0C File Offset: 0x00393C0C
		public float AmountOfDesiredContentStored
		{
			get
			{
				if (!(this.TargetTag == StorageTile.INVALID_TAG))
				{
					return this.storage.GetMassAvailable(this.TargetTag);
				}
				return 0f;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06009B93 RID: 39827 RVA: 0x00395A37 File Offset: 0x00393C37
		public bool IsPendingChange
		{
			get
			{
				return this.GetTreeFilterableCurrentTag() != this.TargetTag;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06009B94 RID: 39828 RVA: 0x00395A4A File Offset: 0x00393C4A
		// (set) Token: 0x06009B95 RID: 39829 RVA: 0x00395A62 File Offset: 0x00393C62
		public float UserMaxCapacity
		{
			get
			{
				return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
			}
			set
			{
				this.userMaxCapacity = value;
				this.filteredStorage.FilterChanged();
				this.RefreshAmountMeter();
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06009B96 RID: 39830 RVA: 0x00395A7C File Offset: 0x00393C7C
		public float AmountStored
		{
			get
			{
				return this.storage.MassStored();
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06009B97 RID: 39831 RVA: 0x00395A89 File Offset: 0x00393C89
		public float MinCapacity
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06009B98 RID: 39832 RVA: 0x00395A90 File Offset: 0x00393C90
		public float MaxCapacity
		{
			get
			{
				return base.def.MaxCapacity;
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06009B99 RID: 39833 RVA: 0x00395A9D File Offset: 0x00393C9D
		public bool WholeValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06009B9A RID: 39834 RVA: 0x00395AA0 File Offset: 0x00393CA0
		public LocString CapacityUnits
		{
			get
			{
				return GameUtil.GetCurrentMassUnit(false);
			}
		}

		// Token: 0x06009B9B RID: 39835 RVA: 0x00395AA8 File Offset: 0x00393CA8
		private Tag GetTreeFilterableCurrentTag()
		{
			if (this.treeFilterable.GetTags() != null && this.treeFilterable.GetTags().Count != 0)
			{
				return this.treeFilterable.GetTags().GetRandom<Tag>();
			}
			return StorageTile.INVALID_TAG;
		}

		// Token: 0x06009B9C RID: 39836 RVA: 0x00395ADF File Offset: 0x00393CDF
		public StorageTileSwitchItemWorkable GetWorkable()
		{
			return base.smi.gameObject.GetComponent<StorageTileSwitchItemWorkable>();
		}

		// Token: 0x06009B9D RID: 39837 RVA: 0x00395AF4 File Offset: 0x00393CF4
		public Instance(IStateMachineTarget master, StorageTile.Def def) : base(master, def)
		{
			this.itemSymbol = this.CreateSymbolOverrideCapsule(StorageTile.ITEM_SYMBOL_TARGET, StorageTile.ITEM_SYMBOL_NAME, "meter_object");
			this.itemSymbol.usingNewSymbolOverrideSystem = true;
			this.itemSymbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(this.itemSymbol.gameObject);
			this.itemPreviewSymbol = this.CreateSymbolOverrideCapsule(StorageTile.ITEM_PREVIEW_SYMBOL_TARGET, StorageTile.ITEM_PREVIEW_SYMBOL_NAME, "meter_object_ui");
			this.defaultItemSymbolScale = this.itemSymbol.transform.localScale.x;
			this.defaultItemLocalPosition = this.itemSymbol.transform.localPosition;
			this.doorSymbol = this.CreateEmptyKAnimController(StorageTile.DOOR_SYMBOL_NAME.ToString());
			this.doorSymbol.initialAnim = "on";
			foreach (KAnim.Build.Symbol symbol in this.doorSymbol.AnimFiles[0].GetData().build.symbols)
			{
				this.doorSymbol.SetSymbolVisiblity(symbol.hash, symbol.hash == StorageTile.DOOR_SYMBOL_NAME);
			}
			this.doorSymbol.transform.SetParent(this.animController.transform, false);
			this.doorSymbol.transform.SetLocalPosition(-Vector3.forward * 0.05f);
			this.doorSymbol.onAnimComplete += this.OnDoorAnimationCompleted;
			this.doorSymbol.gameObject.SetActive(true);
			this.animController.SetSymbolVisiblity(StorageTile.DOOR_SYMBOL_NAME, false);
			this.doorAnimLink = new KAnimLink(this.animController, this.doorSymbol);
			this.amountMeter = new MeterController(this.animController, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
			this.filteredStorage = new FilteredStorage(this.storage, null, this, false, fetch_chore_type);
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			base.Subscribe(1606648047, new Action<object>(this.OnObjectReplaced));
		}

		// Token: 0x06009B9E RID: 39838 RVA: 0x00395D5D File Offset: 0x00393F5D
		public override void StartSM()
		{
			base.StartSM();
			this.filteredStorage.FilterChanged();
		}

		// Token: 0x06009B9F RID: 39839 RVA: 0x00395D70 File Offset: 0x00393F70
		public override void PostParamsInitialized()
		{
			if (this.TargetTag != StorageTile.INVALID_TAG && Assets.GetPrefab(this.TargetTag) == null)
			{
				this.SetTargetItem(StorageTile.INVALID_TAG);
				this.DropUndesiredItems();
			}
			base.PostParamsInitialized();
		}

		// Token: 0x06009BA0 RID: 39840 RVA: 0x00395DB0 File Offset: 0x00393FB0
		private void OnObjectReplaced(object data)
		{
			Constructable.ReplaceCallbackParameters value = ((Boxed<Constructable.ReplaceCallbackParameters>)data).value;
			List<GameObject> list = new List<GameObject>();
			Storage storage = this.storage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			if (value.Worker != null)
			{
				foreach (GameObject gameObject in list)
				{
					gameObject.GetComponent<Pickupable>().Trigger(580035959, value.Worker);
				}
			}
		}

		// Token: 0x06009BA1 RID: 39841 RVA: 0x00395E48 File Offset: 0x00394048
		private void OnDoorAnimationCompleted(HashedString animName)
		{
			if (animName == "door")
			{
				this.doorSymbol.Play("on", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06009BA2 RID: 39842 RVA: 0x00395E7C File Offset: 0x0039407C
		private KBatchedAnimController CreateEmptyKAnimController(string name)
		{
			GameObject gameObject = new GameObject(base.gameObject.name + "-" + name);
			gameObject.SetActive(false);
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("storagetile_kanim")
			};
			kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingFront;
			return kbatchedAnimController;
		}

		// Token: 0x06009BA3 RID: 39843 RVA: 0x00395ED8 File Offset: 0x003940D8
		private KBatchedAnimController CreateSymbolOverrideCapsule(HashedString symbolTarget, HashedString symbolName, string animationName)
		{
			KBatchedAnimController kbatchedAnimController = this.CreateEmptyKAnimController(symbolTarget.ToString());
			kbatchedAnimController.initialAnim = animationName;
			bool flag;
			Matrix4x4 symbolTransform = this.animController.GetSymbolTransform(symbolTarget, out flag);
			bool flag2;
			Matrix2x3 symbolLocalTransform = this.animController.GetSymbolLocalTransform(symbolTarget, out flag2);
			Vector3 position = symbolTransform.GetColumn(3);
			Vector3 localScale = Vector3.one * symbolLocalTransform.m00;
			kbatchedAnimController.transform.SetParent(base.transform, false);
			kbatchedAnimController.transform.SetPosition(position);
			Vector3 localPosition = kbatchedAnimController.transform.localPosition;
			localPosition.z = -0.0025f;
			kbatchedAnimController.transform.localPosition = localPosition;
			kbatchedAnimController.transform.localScale = localScale;
			kbatchedAnimController.gameObject.SetActive(false);
			this.animController.SetSymbolVisiblity(symbolTarget, false);
			return kbatchedAnimController;
		}

		// Token: 0x06009BA4 RID: 39844 RVA: 0x00395FB0 File Offset: 0x003941B0
		private void OnCopySettings(object sourceOBJ)
		{
			if (sourceOBJ != null)
			{
				StorageTile.Instance smi = ((GameObject)sourceOBJ).GetSMI<StorageTile.Instance>();
				if (smi != null)
				{
					this.SetTargetItem(smi.TargetTag);
					this.UserMaxCapacity = smi.UserMaxCapacity;
				}
			}
		}

		// Token: 0x06009BA5 RID: 39845 RVA: 0x00395FE8 File Offset: 0x003941E8
		public void RefreshAmountMeter()
		{
			float positionPercent = (this.UserMaxCapacity == 0f) ? 0f : Mathf.Clamp(this.AmountOfDesiredContentStored / this.UserMaxCapacity, 0f, 1f);
			this.amountMeter.SetPositionPercent(positionPercent);
		}

		// Token: 0x06009BA6 RID: 39846 RVA: 0x00396032 File Offset: 0x00394232
		public void PlayDoorAnimation()
		{
			this.doorSymbol.Play("door", KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06009BA7 RID: 39847 RVA: 0x00396054 File Offset: 0x00394254
		public void SetTargetItem(Tag tag)
		{
			base.sm.TargetItemTag.Set(tag, this, false);
			base.gameObject.Trigger(-2076953849, null);
		}

		// Token: 0x06009BA8 RID: 39848 RVA: 0x0039607C File Offset: 0x0039427C
		public void ApplySettings()
		{
			Tag treeFilterableCurrentTag = this.GetTreeFilterableCurrentTag();
			this.treeFilterable.RemoveTagFromFilter(treeFilterableCurrentTag);
		}

		// Token: 0x06009BA9 RID: 39849 RVA: 0x0039609C File Offset: 0x0039429C
		public void DropUndesiredItems()
		{
			Vector3 position = Grid.CellToPos(this.GetWorkable().LastCellWorkerUsed) + Vector3.right * Grid.CellSizeInMeters * 0.5f + Vector3.up * Grid.CellSizeInMeters * 0.5f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			if (this.TargetTag != StorageTile.INVALID_TAG)
			{
				this.treeFilterable.AddTagToFilter(this.TargetTag);
				GameObject[] array = this.storage.DropUnlessHasTag(this.TargetTag);
				if (array != null)
				{
					GameObject[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].transform.SetPosition(position);
					}
				}
			}
			else
			{
				this.storage.DropAll(position, false, false, default(Vector3), true, null);
			}
			this.storage.DropUnlessHasTag(this.TargetTag);
		}

		// Token: 0x06009BAA RID: 39850 RVA: 0x0039618C File Offset: 0x0039438C
		public void UpdateContentSymbol()
		{
			this.RefreshAmountMeter();
			bool flag = this.TargetTag == StorageTile.INVALID_TAG;
			if (flag && !this.HasContents)
			{
				this.itemSymbol.gameObject.SetActive(false);
				this.itemPreviewSymbol.gameObject.SetActive(false);
				this.animController.SetSymbolVisiblity(StorageTile.ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, false);
				return;
			}
			bool flag2 = !flag && (this.IsPendingChange || !this.HasAnyDesiredContents);
			string text = "";
			GameObject gameObject = (this.TargetTag == StorageTile.INVALID_TAG) ? Assets.GetPrefab(this.storage.items[0].PrefabID()) : Assets.GetPrefab(this.TargetTag);
			KAnimFile animFileFromPrefabWithTag = global::Def.GetAnimFileFromPrefabWithTag(gameObject, flag2 ? "ui" : "", out text);
			this.animController.SetSymbolVisiblity(StorageTile.ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, flag2);
			this.itemPreviewSymbol.gameObject.SetActive(flag2);
			this.itemSymbol.gameObject.SetActive(!flag2);
			if (flag2)
			{
				this.itemPreviewSymbol.SwapAnims(new KAnimFile[]
				{
					animFileFromPrefabWithTag
				});
				this.itemPreviewSymbol.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (gameObject.HasTag(GameTags.Egg))
			{
				string text2 = text;
				if (!string.IsNullOrEmpty(text2))
				{
					this.itemSymbolOverrideController.ApplySymbolOverridesByAffix(animFileFromPrefabWithTag, text2, null, 0);
				}
				text = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
			}
			else
			{
				this.itemSymbolOverrideController.RemoveAllSymbolOverrides(0);
				text = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
			}
			this.itemSymbol.SwapAnims(new KAnimFile[]
			{
				animFileFromPrefabWithTag
			});
			this.itemSymbol.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			StorageTile.SpecificItemTagSizeInstruction sizeInstructionForObject = base.def.GetSizeInstructionForObject(gameObject);
			this.itemSymbol.transform.localScale = Vector3.one * ((sizeInstructionForObject != null) ? sizeInstructionForObject.sizeMultiplier : this.defaultItemSymbolScale);
			KCollider2D component = gameObject.GetComponent<KCollider2D>();
			Vector3 localPosition = this.defaultItemLocalPosition;
			localPosition.y += ((component == null || component is KCircleCollider2D) ? 0f : (-component.offset.y * 0.5f));
			this.itemSymbol.transform.localPosition = localPosition;
		}

		// Token: 0x06009BAB RID: 39851 RVA: 0x003963F7 File Offset: 0x003945F7
		private void AbortChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Change settings Chore aborted");
				this.chore = null;
			}
		}

		// Token: 0x06009BAC RID: 39852 RVA: 0x00396418 File Offset: 0x00394618
		public void StartChangeSettingChore()
		{
			this.AbortChore();
			this.chore = new WorkChore<StorageTileSwitchItemWorkable>(Db.Get().ChoreTypes.Toggle, this.GetWorkable(), null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x06009BAD RID: 39853 RVA: 0x0039645C File Offset: 0x0039465C
		public void CanceChangeSettingChore()
		{
			this.AbortChore();
		}

		// Token: 0x04007810 RID: 30736
		[Serialize]
		private float userMaxCapacity = float.PositiveInfinity;

		// Token: 0x04007811 RID: 30737
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04007812 RID: 30738
		[MyCmpGet]
		private KBatchedAnimController animController;

		// Token: 0x04007813 RID: 30739
		[MyCmpGet]
		private TreeFilterable treeFilterable;

		// Token: 0x04007814 RID: 30740
		private FilteredStorage filteredStorage;

		// Token: 0x04007815 RID: 30741
		private Chore chore;

		// Token: 0x04007816 RID: 30742
		private MeterController amountMeter;

		// Token: 0x04007817 RID: 30743
		private KBatchedAnimController doorSymbol;

		// Token: 0x04007818 RID: 30744
		private KBatchedAnimController itemSymbol;

		// Token: 0x04007819 RID: 30745
		private SymbolOverrideController itemSymbolOverrideController;

		// Token: 0x0400781A RID: 30746
		private KBatchedAnimController itemPreviewSymbol;

		// Token: 0x0400781B RID: 30747
		private KAnimLink doorAnimLink;

		// Token: 0x0400781C RID: 30748
		private string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

		// Token: 0x0400781D RID: 30749
		private float defaultItemSymbolScale = -1f;

		// Token: 0x0400781E RID: 30750
		private Vector3 defaultItemLocalPosition = Vector3.zero;
	}
}
