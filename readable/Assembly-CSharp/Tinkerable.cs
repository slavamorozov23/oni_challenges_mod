using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000BF8 RID: 3064
[AddComponentMenu("KMonoBehaviour/Workable/Tinkerable")]
public class Tinkerable : Workable
{
	// Token: 0x06005BFC RID: 23548 RVA: 0x002148C4 File Offset: 0x00212AC4
	public static Tinkerable MakePowerTinkerable(GameObject prefab)
	{
		RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
		roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
		Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
		tinkerable.tinkerMaterialTag = PowerControlStationConfig.TINKER_TOOLS;
		tinkerable.tinkerMaterialAmount = 1f;
		tinkerable.tinkerMass = 5f;
		tinkerable.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
		tinkerable.onCompleteSFX = "Generator_Microchip_installed";
		tinkerable.boostSymbolNames = new string[]
		{
			"booster",
			"blue_light_bloom"
		};
		tinkerable.SetWorkTime(30f);
		tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		tinkerable.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		tinkerable.choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;
		tinkerable.choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;
		tinkerable.addedEffect = "PowerTinker";
		tinkerable.effectAttributeId = Db.Get().Attributes.Machinery.Id;
		tinkerable.effectMultiplier = 0.025f;
		tinkerable.multitoolContext = "powertinker";
		tinkerable.multitoolHitEffectTag = "fx_powertinker_splash";
		tinkerable.shouldShowSkillPerkStatusItem = false;
		prefab.AddOrGet<Storage>();
		prefab.AddOrGet<Effects>();
		prefab.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable);
		};
		return tinkerable;
	}

	// Token: 0x06005BFD RID: 23549 RVA: 0x00214A58 File Offset: 0x00212C58
	public static Tinkerable MakeFarmTinkerable(GameObject prefab)
	{
		RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
		roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
		Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
		tinkerable.tinkerMaterialTag = FarmStationConfig.TINKER_TOOLS;
		tinkerable.tinkerMaterialAmount = 1f;
		tinkerable.tinkerMass = 5f;
		tinkerable.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		tinkerable.addedEffect = "FarmTinker";
		tinkerable.effectAttributeId = Db.Get().Attributes.Botanist.Id;
		tinkerable.effectMultiplier = 0.1f;
		tinkerable.SetWorkTime(15f);
		tinkerable.attributeConverter = Db.Get().AttributeConverters.PlantTendSpeed;
		tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		tinkerable.choreTypeTinker = Db.Get().ChoreTypes.CropTend.IdHash;
		tinkerable.choreTypeFetch = Db.Get().ChoreTypes.FarmFetch.IdHash;
		tinkerable.multitoolContext = "tend";
		tinkerable.multitoolHitEffectTag = "fx_tend_splash";
		tinkerable.shouldShowSkillPerkStatusItem = false;
		prefab.AddOrGet<Storage>();
		prefab.AddOrGet<Effects>();
		prefab.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable);
		};
		return tinkerable;
	}

	// Token: 0x06005BFE RID: 23550 RVA: 0x00214BD0 File Offset: 0x00212DD0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		base.Subscribe<Tinkerable>(-1157678353, Tinkerable.OnEffectRemovedDelegate);
		base.Subscribe<Tinkerable>(-1697596308, Tinkerable.OnStorageChangeDelegate);
		base.Subscribe<Tinkerable>(144050788, Tinkerable.OnUpdateRoomDelegate);
		base.Subscribe<Tinkerable>(-592767678, Tinkerable.OnOperationalChangedDelegate);
	}

	// Token: 0x06005BFF RID: 23551 RVA: 0x00214C7D File Offset: 0x00212E7D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.prioritizableAdded = true;
		base.Subscribe<Tinkerable>(493375141, Tinkerable.OnRefreshUserMenuDelegate);
		this.UpdateVisual();
	}

	// Token: 0x06005C00 RID: 23552 RVA: 0x00214CAE File Offset: 0x00212EAE
	protected override void OnCleanUp()
	{
		this.UpdateMaterialReservation(false);
		if (this.updateHandle.IsValid)
		{
			this.updateHandle.ClearScheduler();
		}
		if (this.prioritizableAdded)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x06005C01 RID: 23553 RVA: 0x00214CE8 File Offset: 0x00212EE8
	private void OnOperationalChanged(object _)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06005C02 RID: 23554 RVA: 0x00214CF0 File Offset: 0x00212EF0
	private void OnEffectRemoved(object _)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06005C03 RID: 23555 RVA: 0x00214CF8 File Offset: 0x00212EF8
	private void OnUpdateRoom(object _)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06005C04 RID: 23556 RVA: 0x00214D00 File Offset: 0x00212F00
	private void OnStorageChange(object data)
	{
		if (((GameObject)data).IsPrefabID(this.tinkerMaterialTag))
		{
			this.QueueUpdateChore();
		}
	}

	// Token: 0x06005C05 RID: 23557 RVA: 0x00214D1C File Offset: 0x00212F1C
	private void QueueUpdateChore()
	{
		if (this.updateHandle.IsValid)
		{
			this.updateHandle.ClearScheduler();
		}
		this.updateHandle = GameScheduler.Instance.Schedule("UpdateTinkerChore", 1.2f, new Action<object>(this.UpdateChoreCallback), null, null);
	}

	// Token: 0x06005C06 RID: 23558 RVA: 0x00214D69 File Offset: 0x00212F69
	private void UpdateChoreCallback(object obj)
	{
		this.UpdateChore();
	}

	// Token: 0x06005C07 RID: 23559 RVA: 0x00214D74 File Offset: 0x00212F74
	private void UpdateChore()
	{
		Operational component = base.GetComponent<Operational>();
		bool flag = component == null || component.IsFunctional;
		bool flag2 = this.HasEffect();
		bool flag3 = this.HasCorrectRoom();
		bool flag4 = !flag2 && flag && flag3 && this.userMenuAllowed;
		bool flag5 = flag2 || !flag3 || !this.userMenuAllowed;
		if (this.chore == null && flag4)
		{
			this.UpdateMaterialReservation(true);
			if (this.HasMaterial())
			{
				this.chore = new WorkChore<Tinkerable>(Db.Get().ChoreTypes.GetByHash(this.choreTypeTinker), this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				if (component != null)
				{
					this.chore.AddPrecondition(ChorePreconditions.instance.IsFunctional, component);
				}
			}
			else
			{
				this.chore = new FetchChore(Db.Get().ChoreTypes.GetByHash(this.choreTypeFetch), this.storage, this.tinkerMaterialAmount * this.tinkerMass, new HashSet<Tag>
				{
					this.tinkerMaterialTag
				}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, new Action<Chore>(this.OnFetchComplete), null, null, Operational.State.Functional, 0);
			}
			this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
			if (!string.IsNullOrEmpty(base.GetComponent<RoomTracker>().requiredRoomType))
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.IsInMyRoom, Grid.PosToCell(base.transform.GetPosition()));
				return;
			}
		}
		else if (this.chore != null && flag5)
		{
			this.UpdateMaterialReservation(false);
			this.chore.Cancel("No longer needed");
			this.chore = null;
		}
	}

	// Token: 0x06005C08 RID: 23560 RVA: 0x00214F23 File Offset: 0x00213123
	private bool HasCorrectRoom()
	{
		return this.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x06005C09 RID: 23561 RVA: 0x00214F30 File Offset: 0x00213130
	private bool RoomHasTinkerstation()
	{
		if (!this.roomTracker.IsInCorrectRoom())
		{
			return false;
		}
		if (this.roomTracker.room == null)
		{
			return false;
		}
		foreach (KPrefabID kprefabID in this.roomTracker.room.buildings)
		{
			if (!(kprefabID == null))
			{
				TinkerStation component = kprefabID.GetComponent<TinkerStation>();
				if (component != null && component.outputPrefab == this.tinkerMaterialTag)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005C0A RID: 23562 RVA: 0x00214FD8 File Offset: 0x002131D8
	private void UpdateMaterialReservation(bool shouldReserve)
	{
		if (shouldReserve && !this.hasReservedMaterial)
		{
			MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, this.tinkerMaterialAmount, base.gameObject.GetMyWorldId());
			this.hasReservedMaterial = shouldReserve;
			return;
		}
		if (!shouldReserve && this.hasReservedMaterial)
		{
			MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, -this.tinkerMaterialAmount, base.gameObject.GetMyWorldId());
			this.hasReservedMaterial = shouldReserve;
		}
	}

	// Token: 0x06005C0B RID: 23563 RVA: 0x00215043 File Offset: 0x00213243
	private void OnFetchComplete(Chore data)
	{
		this.UpdateMaterialReservation(false);
		this.chore = null;
		this.UpdateChore();
	}

	// Token: 0x06005C0C RID: 23564 RVA: 0x0021505C File Offset: 0x0021325C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.storage.ConsumeIgnoringDisease(this.tinkerMaterialTag, this.tinkerMaterialAmount);
		float totalValue = worker.GetAttributes().Get(Db.Get().Attributes.Get(this.effectAttributeId)).GetTotalValue();
		this.effects.Add(this.addedEffect, true).timeRemaining *= 1f + totalValue * this.effectMultiplier;
		this.UpdateVisual();
		this.UpdateMaterialReservation(false);
		this.chore = null;
		this.UpdateChore();
		string sound = GlobalAssets.GetSound(this.onCompleteSFX, false);
		if (sound != null)
		{
			SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, base.transform.position, 1f, false));
		}
	}

	// Token: 0x06005C0D RID: 23565 RVA: 0x00215120 File Offset: 0x00213320
	private void UpdateVisual()
	{
		if (this.boostSymbolNames == null)
		{
			return;
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		bool is_visible = this.effects.HasEffect(this.addedEffect);
		foreach (string str in this.boostSymbolNames)
		{
			component.SetSymbolVisiblity(str, is_visible);
		}
	}

	// Token: 0x06005C0E RID: 23566 RVA: 0x00215177 File Offset: 0x00213377
	private bool HasMaterial()
	{
		return this.storage.GetAmountAvailable(this.tinkerMaterialTag) >= this.tinkerMaterialAmount;
	}

	// Token: 0x06005C0F RID: 23567 RVA: 0x00215195 File Offset: 0x00213395
	private bool HasEffect()
	{
		return this.effects.HasEffect(this.addedEffect);
	}

	// Token: 0x06005C10 RID: 23568 RVA: 0x002151A8 File Offset: 0x002133A8
	private void OnRefreshUserMenu(object data)
	{
		if (this.roomTracker.IsInCorrectRoom())
		{
			string name = Db.Get().effects.Get(this.addedEffect).Name;
			string properName = this.GetProperName();
			KIconButtonMenu.ButtonInfo button = this.userMenuAllowed ? new KIconButtonMenu.ButtonInfo("action_switch_toggle", UI.USERMENUACTIONS.TINKER.DISALLOW, new System.Action(this.OnClickToggleTinker), global::Action.NumActions, null, null, null, string.Format(UI.USERMENUACTIONS.TINKER.TOOLTIP_DISALLOW, name, properName), true) : new KIconButtonMenu.ButtonInfo("action_switch_toggle", UI.USERMENUACTIONS.TINKER.ALLOW, new System.Action(this.OnClickToggleTinker), global::Action.NumActions, null, null, null, string.Format(UI.USERMENUACTIONS.TINKER.TOOLTIP_ALLOW, name, properName), true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x06005C11 RID: 23569 RVA: 0x00215281 File Offset: 0x00213481
	private void OnClickToggleTinker()
	{
		this.userMenuAllowed = !this.userMenuAllowed;
		this.UpdateChore();
	}

	// Token: 0x04003D4B RID: 15691
	private Chore chore;

	// Token: 0x04003D4C RID: 15692
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003D4D RID: 15693
	[MyCmpGet]
	private Effects effects;

	// Token: 0x04003D4E RID: 15694
	[MyCmpGet]
	private RoomTracker roomTracker;

	// Token: 0x04003D4F RID: 15695
	public Tag tinkerMaterialTag;

	// Token: 0x04003D50 RID: 15696
	public float tinkerMaterialAmount;

	// Token: 0x04003D51 RID: 15697
	public float tinkerMass;

	// Token: 0x04003D52 RID: 15698
	public string addedEffect;

	// Token: 0x04003D53 RID: 15699
	public string effectAttributeId;

	// Token: 0x04003D54 RID: 15700
	public float effectMultiplier;

	// Token: 0x04003D55 RID: 15701
	public string[] boostSymbolNames;

	// Token: 0x04003D56 RID: 15702
	public string onCompleteSFX;

	// Token: 0x04003D57 RID: 15703
	public HashedString choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;

	// Token: 0x04003D58 RID: 15704
	public HashedString choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;

	// Token: 0x04003D59 RID: 15705
	[Serialize]
	private bool userMenuAllowed = true;

	// Token: 0x04003D5A RID: 15706
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnEffectRemovedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnEffectRemoved(data);
	});

	// Token: 0x04003D5B RID: 15707
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04003D5C RID: 15708
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x04003D5D RID: 15709
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04003D5E RID: 15710
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04003D5F RID: 15711
	private bool prioritizableAdded;

	// Token: 0x04003D60 RID: 15712
	private SchedulerHandle updateHandle;

	// Token: 0x04003D61 RID: 15713
	private bool hasReservedMaterial;
}
