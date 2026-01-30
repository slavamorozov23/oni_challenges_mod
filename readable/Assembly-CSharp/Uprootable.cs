using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000C02 RID: 3074
[AddComponentMenu("KMonoBehaviour/Workable/Uprootable")]
public class Uprootable : Workable, IDigActionEntity
{
	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x06005C5D RID: 23645 RVA: 0x00217805 File Offset: 0x00215A05
	public bool IsMarkedForUproot
	{
		get
		{
			return this.isMarkedForUproot;
		}
	}

	// Token: 0x06005C5E RID: 23646 RVA: 0x0021780D File Offset: 0x00215A0D
	public bool CanUproot()
	{
		return this.canBeUprooted && !this.uprootComplete;
	}

	// Token: 0x06005C5F RID: 23647 RVA: 0x00217822 File Offset: 0x00215A22
	public static bool CanUproot(GameObject plant, out Uprootable uprootable)
	{
		if (plant == null)
		{
			uprootable = null;
			return false;
		}
		uprootable = plant.GetComponent<Uprootable>();
		return uprootable != null && uprootable.CanUproot();
	}

	// Token: 0x06005C60 RID: 23648 RVA: 0x00217850 File Offset: 0x00215A50
	public static bool CanUproot(GameObject plant)
	{
		Uprootable uprootable;
		return Uprootable.CanUproot(plant, out uprootable);
	}

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x06005C61 RID: 23649 RVA: 0x00217865 File Offset: 0x00215A65
	public Storage GetPlanterStorage
	{
		get
		{
			return this.planterStorage;
		}
	}

	// Token: 0x06005C62 RID: 23650 RVA: 0x00217870 File Offset: 0x00215A70
	protected Uprootable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.buttonLabel = UI.USERMENUACTIONS.UPROOT.NAME;
		this.buttonTooltip = UI.USERMENUACTIONS.UPROOT.TOOLTIP;
		this.cancelButtonLabel = UI.USERMENUACTIONS.CANCELUPROOT.NAME;
		this.cancelButtonTooltip = UI.USERMENUACTIONS.CANCELUPROOT.TOOLTIP;
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
	}

	// Token: 0x06005C63 RID: 23651 RVA: 0x00217910 File Offset: 0x00215B10
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
		base.Subscribe<Uprootable>(1309017699, Uprootable.OnPlanterStorageDelegate);
	}

	// Token: 0x06005C64 RID: 23652 RVA: 0x002179C4 File Offset: 0x00215BC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Uprootable>(2127324410, Uprootable.ForceCancelUprootDelegate);
		base.SetWorkTime(12.5f);
		base.Subscribe<Uprootable>(2127324410, Uprootable.OnCancelDelegate);
		base.Subscribe<Uprootable>(493375141, Uprootable.OnRefreshUserMenuDelegate);
		this.faceTargetWhenWorking = true;
		Components.Uprootables.Add(this);
		this.area = base.GetComponent<OccupyArea>();
		Prioritizable.AddRef(base.gameObject);
		base.gameObject.AddTag(GameTags.Plant);
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		this.partitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.gameObject.GetComponent<KPrefabID>(), extents, GameScenePartitioner.Instance.plants, null);
		GameScenePartitioner.Instance.TriggerEvent(extents, GameScenePartitioner.Instance.plantsChangedLayer, this);
		if (this.isMarkedForUproot)
		{
			this.MarkForUproot(true);
		}
	}

	// Token: 0x06005C65 RID: 23653 RVA: 0x00217AC8 File Offset: 0x00215CC8
	private void OnPlanterStorage(object data)
	{
		this.planterStorage = (Storage)data;
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.showIcon = (this.planterStorage == null);
		}
	}

	// Token: 0x06005C66 RID: 23654 RVA: 0x00217B03 File Offset: 0x00215D03
	public bool IsInPlanterBox()
	{
		return this.planterStorage != null;
	}

	// Token: 0x06005C67 RID: 23655 RVA: 0x00217B14 File Offset: 0x00215D14
	public void Uproot()
	{
		this.isMarkedForUproot = false;
		this.chore = null;
		this.uprootComplete = true;
		base.Trigger(-216549700, this);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06005C68 RID: 23656 RVA: 0x00217B8F File Offset: 0x00215D8F
	public void SetCanBeUprooted(bool state)
	{
		this.canBeUprooted = state;
		if (this.canBeUprooted)
		{
			this.SetUprootedComplete(false);
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06005C69 RID: 23657 RVA: 0x00217BBC File Offset: 0x00215DBC
	public void SetUprootedComplete(bool state)
	{
		this.uprootComplete = state;
	}

	// Token: 0x06005C6A RID: 23658 RVA: 0x00217BC8 File Offset: 0x00215DC8
	public void MarkForUproot(bool instantOnDebug = true)
	{
		if (!this.canBeUprooted)
		{
			return;
		}
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
			this.Uproot();
		}
		else if (this.chore == null)
		{
			ChoreType chore_type = this.choreTypeIdHash.IsValid ? Db.Get().ChoreTypes.GetByHash(this.choreTypeIdHash) : Db.Get().ChoreTypes.Uproot;
			this.chore = new WorkChore<Uprootable>(chore_type, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			base.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, this);
		}
		this.isMarkedForUproot = true;
	}

	// Token: 0x06005C6B RID: 23659 RVA: 0x00217C63 File Offset: 0x00215E63
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.Uproot();
	}

	// Token: 0x06005C6C RID: 23660 RVA: 0x00217C6C File Offset: 0x00215E6C
	private void OnCancel(object _)
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel uproot");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		}
		this.isMarkedForUproot = false;
		this.choreTypeIdHash = HashedString.Invalid;
		Game.Instance.userMenu.Refresh(base.gameObject);
		base.Trigger(1198393204, null);
	}

	// Token: 0x06005C6D RID: 23661 RVA: 0x00217CE7 File Offset: 0x00215EE7
	public bool HasChore()
	{
		return this.chore != null;
	}

	// Token: 0x06005C6E RID: 23662 RVA: 0x00217CF4 File Offset: 0x00215EF4
	private void OnClickUproot()
	{
		this.MarkForUproot(true);
	}

	// Token: 0x06005C6F RID: 23663 RVA: 0x00217CFD File Offset: 0x00215EFD
	protected void OnClickCancelUproot()
	{
		this.OnCancel(null);
	}

	// Token: 0x06005C70 RID: 23664 RVA: 0x00217D06 File Offset: 0x00215F06
	public virtual void ForceCancelUproot(object _ = null)
	{
		this.OnCancel(null);
	}

	// Token: 0x06005C71 RID: 23665 RVA: 0x00217D10 File Offset: 0x00215F10
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showUserMenuButtons)
		{
			return;
		}
		if (this.uprootComplete)
		{
			if (this.deselectOnUproot)
			{
				KSelectable component = base.GetComponent<KSelectable>();
				if (component != null && SelectTool.Instance.selected == component)
				{
					SelectTool.Instance.Select(null, false);
				}
			}
			return;
		}
		if (!this.canBeUprooted)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_uproot", this.cancelButtonLabel, new System.Action(this.OnClickCancelUproot), global::Action.NumActions, null, null, null, this.cancelButtonTooltip, true) : new KIconButtonMenu.ButtonInfo("action_uproot", this.buttonLabel, new System.Action(this.OnClickUproot), global::Action.NumActions, null, null, null, this.buttonTooltip, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06005C72 RID: 23666 RVA: 0x00217DEC File Offset: 0x00215FEC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.TriggerEvent(extents, GameScenePartitioner.Instance.plantsChangedLayer, this);
		Components.Uprootables.Remove(this);
	}

	// Token: 0x06005C73 RID: 23667 RVA: 0x00217E52 File Offset: 0x00216052
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
	}

	// Token: 0x06005C74 RID: 23668 RVA: 0x00217E77 File Offset: 0x00216077
	public void Dig()
	{
		this.Uproot();
	}

	// Token: 0x06005C75 RID: 23669 RVA: 0x00217E7F File Offset: 0x0021607F
	public void MarkForDig(bool instantOnDebug = true)
	{
		this.MarkForUproot(instantOnDebug);
	}

	// Token: 0x04003D88 RID: 15752
	[Serialize]
	protected bool isMarkedForUproot;

	// Token: 0x04003D89 RID: 15753
	protected bool uprootComplete;

	// Token: 0x04003D8A RID: 15754
	[MyCmpReq]
	private Prioritizable prioritizable;

	// Token: 0x04003D8B RID: 15755
	[SerializeField]
	public HashedString choreTypeIdHash;

	// Token: 0x04003D8C RID: 15756
	[Serialize]
	protected bool canBeUprooted = true;

	// Token: 0x04003D8D RID: 15757
	public bool deselectOnUproot = true;

	// Token: 0x04003D8E RID: 15758
	protected Chore chore;

	// Token: 0x04003D8F RID: 15759
	private string buttonLabel;

	// Token: 0x04003D90 RID: 15760
	private string buttonTooltip;

	// Token: 0x04003D91 RID: 15761
	private string cancelButtonLabel;

	// Token: 0x04003D92 RID: 15762
	private string cancelButtonTooltip;

	// Token: 0x04003D93 RID: 15763
	private StatusItem pendingStatusItem;

	// Token: 0x04003D94 RID: 15764
	public OccupyArea area;

	// Token: 0x04003D95 RID: 15765
	private Storage planterStorage;

	// Token: 0x04003D96 RID: 15766
	public bool showUserMenuButtons = true;

	// Token: 0x04003D97 RID: 15767
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003D98 RID: 15768
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnPlanterStorageDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnPlanterStorage(data);
	});

	// Token: 0x04003D99 RID: 15769
	private static readonly EventSystem.IntraObjectHandler<Uprootable> ForceCancelUprootDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.ForceCancelUproot(data);
	});

	// Token: 0x04003D9A RID: 15770
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04003D9B RID: 15771
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
