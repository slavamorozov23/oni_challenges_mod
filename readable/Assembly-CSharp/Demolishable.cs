using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005D1 RID: 1489
[RequireComponent(typeof(Prioritizable))]
public class Demolishable : Workable
{
	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06002237 RID: 8759 RVA: 0x000C6E05 File Offset: 0x000C5005
	public bool HasBeenDestroyed
	{
		get
		{
			return this.destroyed;
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06002238 RID: 8760 RVA: 0x000C6E10 File Offset: 0x000C5010
	private CellOffset[] placementOffsets
	{
		get
		{
			Building component = base.GetComponent<Building>();
			if (component != null)
			{
				return component.Def.PlacementOffsets;
			}
			OccupyArea component2 = base.GetComponent<OccupyArea>();
			if (component2 != null)
			{
				return component2.OccupiedCellsOffsets;
			}
			global::Debug.Assert(false, "Ack! We put a Demolishable on something that's neither a Building nor OccupyArea!", this);
			return null;
		}
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x000C6E60 File Offset: 0x000C5060
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDemolish.Id;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Deconstructing;
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.multitoolContext = "demolish";
		this.multitoolHitEffectTag = EffectConfigs.DemolishSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		Building component = base.GetComponent<Building>();
		if (component != null && component.Def.IsTilePiece)
		{
			base.SetWorkTime(component.Def.ConstructionTime * 0.5f);
			return;
		}
		base.SetWorkTime(30f);
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x000C6F70 File Offset: 0x000C5170
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Demolishable>(493375141, Demolishable.OnRefreshUserMenuDelegate);
		base.Subscribe<Demolishable>(-111137758, Demolishable.OnRefreshUserMenuDelegate);
		base.Subscribe<Demolishable>(2127324410, Demolishable.OnCancelDelegate);
		base.Subscribe<Demolishable>(-790448070, Demolishable.OnDeconstructDelegate);
		CellOffset[][] table = OffsetGroups.InvertedStandardTable;
		CellOffset[] filter = null;
		Building component = base.GetComponent<Building>();
		if (component != null && component.Def.IsTilePiece)
		{
			table = OffsetGroups.InvertedStandardTableWithCorners;
			filter = component.Def.ConstructionOffsetFilter;
		}
		CellOffset[][] offsetTable = OffsetGroups.BuildReachabilityTable(this.placementOffsets, table, filter);
		base.SetOffsetTable(offsetTable);
		if (this.isMarkedForDemolition)
		{
			this.QueueDemolition();
		}
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x000C7021 File Offset: 0x000C5221
	protected override void OnStartWork(WorkerBase worker)
	{
		this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("DeconstructBar");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDemolition, false);
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x000C7059 File Offset: 0x000C5259
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.TriggerDestroy();
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x000C7061 File Offset: 0x000C5261
	private void TriggerDestroy()
	{
		if (this == null || this.destroyed)
		{
			return;
		}
		this.destroyed = true;
		this.isMarkedForDemolition = false;
		base.gameObject.DeleteObject();
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x000C7090 File Offset: 0x000C5290
	private void QueueDemolition()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
			return;
		}
		if (this.chore == null)
		{
			Prioritizable.AddRef(base.gameObject);
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDemolish.Id;
			this.chore = new WorkChore<Demolishable>(Db.Get().ChoreTypes.Demolish, this, null, true, null, null, null, true, null, false, false, Assets.GetAnim("anim_interacts_clothingfactory_kanim"), true, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingDemolition, this);
			this.isMarkedForDemolition = true;
			base.Trigger(2108245096, "Demolish");
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x000C7150 File Offset: 0x000C5350
	private void OnRefreshUserMenu(object data)
	{
		if (!this.allowDemolition)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore == null) ? new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.DEMOLISH.NAME, new System.Action(this.OnDemolish), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DEMOLISH.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.DEMOLISH.NAME_OFF, new System.Action(this.OnDemolish), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DEMOLISH.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 0f);
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x000C71F4 File Offset: 0x000C53F4
	public void CancelDemolition()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancelled demolition");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDemolition, false);
			base.ShowProgressBar(false);
			this.isMarkedForDemolition = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x000C725C File Offset: 0x000C545C
	private void OnCancel(object _)
	{
		this.CancelDemolition();
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x000C7264 File Offset: 0x000C5464
	private void OnDemolish(object data)
	{
		if (this.allowDemolition || DebugHandler.InstantBuildMode)
		{
			this.QueueDemolition();
		}
	}

	// Token: 0x06002243 RID: 8771 RVA: 0x000C727B File Offset: 0x000C547B
	private void OnDemolish()
	{
		if (this.chore == null)
		{
			this.QueueDemolition();
			return;
		}
		this.CancelDemolition();
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x000C7292 File Offset: 0x000C5492
	protected override void UpdateStatusItem(object data = null)
	{
		this.shouldShowSkillPerkStatusItem = this.isMarkedForDemolition;
		base.UpdateStatusItem(data);
	}

	// Token: 0x040013FB RID: 5115
	public Chore chore;

	// Token: 0x040013FC RID: 5116
	public bool allowDemolition = true;

	// Token: 0x040013FD RID: 5117
	[Serialize]
	private bool isMarkedForDemolition;

	// Token: 0x040013FE RID: 5118
	private bool destroyed;

	// Token: 0x040013FF RID: 5119
	private static readonly EventSystem.IntraObjectHandler<Demolishable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Demolishable>(delegate(Demolishable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001400 RID: 5120
	private static readonly EventSystem.IntraObjectHandler<Demolishable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Demolishable>(delegate(Demolishable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04001401 RID: 5121
	private static readonly EventSystem.IntraObjectHandler<Demolishable> OnDeconstructDelegate = new EventSystem.IntraObjectHandler<Demolishable>(delegate(Demolishable component, object data)
	{
		component.OnDemolish(data);
	});
}
