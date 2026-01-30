using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006F1 RID: 1777
[SerializationConfig(MemberSerialization.OptIn)]
public class BottleEmptier : StateMachineComponent<BottleEmptier.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06002BE4 RID: 11236 RVA: 0x000FFAB3 File Offset: 0x000FDCB3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.DefineManualPumpingAffectedBuildings();
		base.Subscribe<BottleEmptier>(493375141, BottleEmptier.OnRefreshUserMenuDelegate);
		base.Subscribe<BottleEmptier>(-905833192, BottleEmptier.OnCopySettingsDelegate);
	}

	// Token: 0x06002BE5 RID: 11237 RVA: 0x000FFAEE File Offset: 0x000FDCEE
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x06002BE6 RID: 11238 RVA: 0x000FFAF4 File Offset: 0x000FDCF4
	private void DefineManualPumpingAffectedBuildings()
	{
		if (BottleEmptier.manualPumpingAffectedBuildings.ContainsKey(this.isGasEmptier))
		{
			return;
		}
		List<string> list = new List<string>();
		Tag tag = this.isGasEmptier ? GameTags.GasSource : GameTags.LiquidSource;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (buildingDef.BuildingComplete.HasTag(tag))
			{
				list.Add(buildingDef.Name);
			}
		}
		BottleEmptier.manualPumpingAffectedBuildings.Add(this.isGasEmptier, list.ToArray());
	}

	// Token: 0x06002BE7 RID: 11239 RVA: 0x000FFBA0 File Offset: 0x000FDDA0
	private void OnChangeAllowManualPumpingStationFetching()
	{
		this.allowManualPumpingStationFetching = !this.allowManualPumpingStationFetching;
		base.smi.RefreshChore();
	}

	// Token: 0x06002BE8 RID: 11240 RVA: 0x000FFBBC File Offset: 0x000FDDBC
	private void OnRefreshUserMenu(object data)
	{
		string text = this.isGasEmptier ? UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.TOOLTIP : UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.TOOLTIP;
		string text2 = this.isGasEmptier ? UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.TOOLTIP : UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.TOOLTIP;
		if (BottleEmptier.manualPumpingAffectedBuildings.ContainsKey(this.isGasEmptier))
		{
			foreach (string arg in BottleEmptier.manualPumpingAffectedBuildings[this.isGasEmptier])
			{
				string str = string.Format(UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.ITEM, arg);
				text += str;
				text2 += str;
			}
		}
		if (this.isGasEmptier)
		{
			KIconButtonMenu.ButtonInfo button = this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text2, true) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 0.4f);
			return;
		}
		KIconButtonMenu.ButtonInfo button2 = this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text2, true) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button2, 0.4f);
	}

	// Token: 0x06002BE9 RID: 11241 RVA: 0x000FFD5C File Offset: 0x000FDF5C
	private void OnCopySettings(object data)
	{
		BottleEmptier component = ((GameObject)data).GetComponent<BottleEmptier>();
		this.allowManualPumpingStationFetching = component.allowManualPumpingStationFetching;
		base.smi.RefreshChore();
	}

	// Token: 0x04001A0E RID: 6670
	public float emptyRate = 10f;

	// Token: 0x04001A0F RID: 6671
	[Serialize]
	public bool allowManualPumpingStationFetching;

	// Token: 0x04001A10 RID: 6672
	[Serialize]
	public bool emit = true;

	// Token: 0x04001A11 RID: 6673
	public bool isGasEmptier;

	// Token: 0x04001A12 RID: 6674
	private static Dictionary<bool, string[]> manualPumpingAffectedBuildings = new Dictionary<bool, string[]>();

	// Token: 0x04001A13 RID: 6675
	private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>(delegate(BottleEmptier component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001A14 RID: 6676
	private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>(delegate(BottleEmptier component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020015B4 RID: 5556
	public class StatesInstance : GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.GameInstance
	{
		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06009436 RID: 37942 RVA: 0x00377EF1 File Offset: 0x003760F1
		// (set) Token: 0x06009437 RID: 37943 RVA: 0x00377EF9 File Offset: 0x003760F9
		public MeterController meter { get; private set; }

		// Token: 0x06009438 RID: 37944 RVA: 0x00377F04 File Offset: 0x00376104
		public StatesInstance(BottleEmptier smi) : base(smi)
		{
			TreeFilterable component = base.master.GetComponent<TreeFilterable>();
			component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_arrow",
				"meter_scale"
			});
			this.meter.meterController.GetComponent<KBatchedAnimTracker>().synchronizeEnabledState = false;
			this.meter.meterController.enabled = false;
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
			base.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		}

		// Token: 0x06009439 RID: 37945 RVA: 0x00377FD8 File Offset: 0x003761D8
		public void CreateChore()
		{
			HashSet<Tag> tags = base.GetComponent<TreeFilterable>().GetTags();
			Tag[] forbidden_tags;
			if (!base.master.allowManualPumpingStationFetching)
			{
				forbidden_tags = new Tag[]
				{
					GameTags.LiquidSource,
					GameTags.GasSource
				};
			}
			else
			{
				forbidden_tags = new Tag[0];
			}
			Storage component = base.GetComponent<Storage>();
			this.chore = new FetchChore(Db.Get().ChoreTypes.StorageFetch, component, component.Capacity(), tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags, null, true, null, null, null, Operational.State.Operational, 0);
		}

		// Token: 0x0600943A RID: 37946 RVA: 0x0037805D File Offset: 0x0037625D
		public void CancelChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Storage Changed");
				this.chore = null;
			}
		}

		// Token: 0x0600943B RID: 37947 RVA: 0x0037807E File Offset: 0x0037627E
		public void RefreshChore()
		{
			this.GoTo(base.sm.unoperational);
		}

		// Token: 0x0600943C RID: 37948 RVA: 0x00378091 File Offset: 0x00376291
		private void OnFilterChanged(HashSet<Tag> tags)
		{
			this.RefreshChore();
		}

		// Token: 0x0600943D RID: 37949 RVA: 0x0037809C File Offset: 0x0037629C
		private void OnStorageChange(object data)
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.RemainingCapacity() / this.storage.capacityKg));
			this.meter.meterController.enabled = (this.storage.ExactMassStored() > 0f);
		}

		// Token: 0x0600943E RID: 37950 RVA: 0x003780F2 File Offset: 0x003762F2
		private void OnOnlyFetchMarkedItemsSettingChanged(object data)
		{
			this.RefreshChore();
		}

		// Token: 0x0600943F RID: 37951 RVA: 0x003780FC File Offset: 0x003762FC
		public void StartMeter()
		{
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if (firstPrimaryElement == null)
			{
				return;
			}
			base.GetComponent<KBatchedAnimController>().SetSymbolTint(new KAnimHashedString("leak_ceiling"), firstPrimaryElement.Element.substance.colour);
			this.meter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
			this.meter.meterController.Play("empty", KAnim.PlayMode.Paused, 1f, 0f);
			Color32 colour = firstPrimaryElement.Element.substance.colour;
			colour.a = byte.MaxValue;
			this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
			this.meter.SetSymbolTint(new KAnimHashedString("water1"), colour);
			this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
			this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), colour);
			this.OnStorageChange(null);
		}

		// Token: 0x06009440 RID: 37952 RVA: 0x00378208 File Offset: 0x00376408
		private PrimaryElement GetFirstPrimaryElement()
		{
			for (int i = 0; i < this.storage.Count; i++)
			{
				GameObject gameObject = this.storage[i];
				if (!(gameObject == null))
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (!(component == null))
					{
						return component;
					}
				}
			}
			return null;
		}

		// Token: 0x06009441 RID: 37953 RVA: 0x00378254 File Offset: 0x00376454
		public void Emit(float dt)
		{
			if (!base.smi.master.emit)
			{
				return;
			}
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if (firstPrimaryElement == null)
			{
				return;
			}
			float num = Mathf.Min(firstPrimaryElement.Mass, base.master.emptyRate * dt);
			if (num <= 0f)
			{
				return;
			}
			Tag prefabTag = firstPrimaryElement.GetComponent<KPrefabID>().PrefabTag;
			float num2;
			SimUtil.DiseaseInfo diseaseInfo;
			float temperature;
			this.storage.ConsumeAndGetDisease(prefabTag, num, out num2, out diseaseInfo, out temperature);
			Vector3 position = base.transform.GetPosition();
			position.y += 1.8f;
			bool flag = base.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH;
			position.x += (flag ? -0.2f : 0.2f);
			int num3 = Grid.PosToCell(position) + (flag ? -1 : 1);
			if (Grid.Solid[num3])
			{
				num3 += (flag ? 1 : -1);
			}
			Element element = firstPrimaryElement.Element;
			ushort idx = element.idx;
			if (element.IsLiquid)
			{
				FallingWater.instance.AddParticle(num3, idx, num2, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, false, false);
				return;
			}
			SimMessages.ModifyCell(num3, idx, temperature, num2, diseaseInfo.idx, diseaseInfo.count, SimMessages.ReplaceType.None, false, -1);
		}

		// Token: 0x0400726D RID: 29293
		[MyCmpGet]
		public Storage storage;

		// Token: 0x0400726E RID: 29294
		private FetchChore chore;
	}

	// Token: 0x020015B5 RID: 5557
	public class States : GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier>
	{
		// Token: 0x06009442 RID: 37954 RVA: 0x00378390 File Offset: 0x00376590
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waitingfordelivery;
			this.statusItem = new StatusItem("BottleEmptier", "", "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItem.resolveStringCallback = delegate(string str, object data)
			{
				BottleEmptier bottleEmptier = (BottleEmptier)data;
				if (bottleEmptier == null)
				{
					return str;
				}
				if (bottleEmptier.allowManualPumpingStationFetching)
				{
					return bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.NAME;
				}
				return bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.NAME;
			};
			this.statusItem.resolveTooltipCallback = delegate(string str, object data)
			{
				BottleEmptier bottleEmptier = (BottleEmptier)data;
				if (bottleEmptier == null)
				{
					return str;
				}
				string result;
				if (bottleEmptier.allowManualPumpingStationFetching)
				{
					if (bottleEmptier.isGasEmptier)
					{
						result = BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.TOOLTIP;
					}
					else
					{
						result = BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.TOOLTIP;
					}
				}
				else if (bottleEmptier.isGasEmptier)
				{
					result = BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.TOOLTIP;
				}
				else
				{
					result = BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.TOOLTIP;
				}
				return result;
			};
			this.root.ToggleStatusItem(this.statusItem, (BottleEmptier.StatesInstance smi) => smi.master);
			this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
			this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.emptying, (BottleEmptier.StatesInstance smi) => smi.GetComponent<Storage>().ExactMassStored() > 0f).Enter("CreateChore", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.CreateChore();
			}).Exit("CancelChore", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.CancelChore();
			}).PlayAnim("on");
			this.emptying.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (BottleEmptier.StatesInstance smi) => smi.GetComponent<Storage>().ExactMassStored() == 0f).Enter("StartMeter", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.StartMeter();
			}).Update("Emit", delegate(BottleEmptier.StatesInstance smi, float dt)
			{
				smi.Emit(dt);
			}, UpdateRate.SIM_200ms, false).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		// Token: 0x04007270 RID: 29296
		private StatusItem statusItem;

		// Token: 0x04007271 RID: 29297
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State unoperational;

		// Token: 0x04007272 RID: 29298
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State waitingfordelivery;

		// Token: 0x04007273 RID: 29299
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State emptying;
	}
}
