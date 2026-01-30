using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000839 RID: 2105
[AddComponentMenu("KMonoBehaviour/Workable/Carvable")]
public class Carvable : Workable, IDigActionEntity
{
	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06003965 RID: 14693 RVA: 0x001406DF File Offset: 0x0013E8DF
	public bool IsMarkedForCarve
	{
		get
		{
			return this.isMarkedForCarve;
		}
	}

	// Token: 0x06003966 RID: 14694 RVA: 0x001406E8 File Offset: 0x0013E8E8
	protected Carvable()
	{
		this.buttonLabel = UI.USERMENUACTIONS.CARVE.NAME;
		this.buttonTooltip = UI.USERMENUACTIONS.CARVE.TOOLTIP;
		this.cancelButtonLabel = UI.USERMENUACTIONS.CANCELCARVE.NAME;
		this.cancelButtonTooltip = UI.USERMENUACTIONS.CANCELCARVE.TOOLTIP;
	}

	// Token: 0x06003967 RID: 14695 RVA: 0x00140744 File Offset: 0x0013E944
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.pendingStatusItem = new StatusItem("PendingCarve", "MISC", "status_item_pending_carve", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.workerStatusItem = new StatusItem("Carving", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.workerStatusItem.resolveStringCallback = delegate(string str, object data)
		{
			Workable workable = (Workable)data;
			if (workable != null && workable.GetComponent<KSelectable>() != null)
			{
				str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
			}
			return str;
		};
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sculpture_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x001407F8 File Offset: 0x0013E9F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(10f);
		base.Subscribe<Carvable>(2127324410, Carvable.OnCancelDelegate);
		base.Subscribe<Carvable>(493375141, Carvable.OnRefreshUserMenuDelegate);
		this.faceTargetWhenWorking = true;
		Prioritizable.AddRef(base.gameObject);
		OccupyArea component = base.gameObject.GetComponent<OccupyArea>();
		int cell = Grid.PosToCell(this);
		foreach (CellOffset offset in component.OccupiedCellsOffsets)
		{
			Grid.ObjectLayers[5][Grid.OffsetCell(cell, offset)] = base.gameObject;
		}
		if (this.isMarkedForCarve)
		{
			this.MarkForCarve(true);
		}
	}

	// Token: 0x06003969 RID: 14697 RVA: 0x001408A0 File Offset: 0x0013EAA0
	public void Carve()
	{
		this.isMarkedForCarve = false;
		this.chore = null;
		base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
		this.ProducePickupable(this.dropItemPrefabId);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600396A RID: 14698 RVA: 0x00140910 File Offset: 0x0013EB10
	public void MarkForCarve(bool instantOnDebug = true)
	{
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
			this.Carve();
			return;
		}
		if (this.chore == null)
		{
			this.isMarkedForCarve = true;
			this.chore = new WorkChore<Carvable>(Db.Get().ChoreTypes.Dig, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
			base.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, this);
		}
	}

	// Token: 0x0600396B RID: 14699 RVA: 0x00140991 File Offset: 0x0013EB91
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.Carve();
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x0014099C File Offset: 0x0013EB9C
	private void OnCancel(object _)
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel uproot");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
		}
		this.isMarkedForCarve = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0600396D RID: 14701 RVA: 0x001409F7 File Offset: 0x0013EBF7
	private void OnClickCarve()
	{
		this.MarkForCarve(true);
	}

	// Token: 0x0600396E RID: 14702 RVA: 0x00140A00 File Offset: 0x0013EC00
	protected void OnClickCancelCarve()
	{
		this.OnCancel(null);
	}

	// Token: 0x0600396F RID: 14703 RVA: 0x00140A0C File Offset: 0x0013EC0C
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showUserMenuButtons)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_carve", this.cancelButtonLabel, new System.Action(this.OnClickCancelCarve), global::Action.NumActions, null, null, null, this.cancelButtonTooltip, true) : new KIconButtonMenu.ButtonInfo("action_carve", this.buttonLabel, new System.Action(this.OnClickCarve), global::Action.NumActions, null, null, null, this.buttonTooltip, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06003970 RID: 14704 RVA: 0x00140AA0 File Offset: 0x0013ECA0
	protected override void OnCleanUp()
	{
		OccupyArea component = base.gameObject.GetComponent<OccupyArea>();
		int cell = Grid.PosToCell(this);
		foreach (CellOffset offset in component.OccupiedCellsOffsets)
		{
			if (Grid.ObjectLayers[5][Grid.OffsetCell(cell, offset)] == base.gameObject)
			{
				Grid.ObjectLayers[5][Grid.OffsetCell(cell, offset)] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06003971 RID: 14705 RVA: 0x00140B15 File Offset: 0x0013ED15
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
	}

	// Token: 0x06003972 RID: 14706 RVA: 0x00140B34 File Offset: 0x0013ED34
	private GameObject ProducePickupable(string pickupablePrefabId)
	{
		if (pickupablePrefabId != null)
		{
			Vector3 position = base.gameObject.transform.GetPosition() + new Vector3(0f, 0.5f, 0f);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(pickupablePrefabId)), position, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
			gameObject.GetComponent<PrimaryElement>().Temperature = component.Temperature;
			gameObject.SetActive(true);
			string properName = gameObject.GetProperName();
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, properName, gameObject.transform, 1.5f, false);
			return gameObject;
		}
		return null;
	}

	// Token: 0x06003973 RID: 14707 RVA: 0x00140BD7 File Offset: 0x0013EDD7
	public void Dig()
	{
		this.Carve();
	}

	// Token: 0x06003974 RID: 14708 RVA: 0x00140BDF File Offset: 0x0013EDDF
	public void MarkForDig(bool instantOnDebug = true)
	{
		this.MarkForCarve(instantOnDebug);
	}

	// Token: 0x04002312 RID: 8978
	[Serialize]
	protected bool isMarkedForCarve;

	// Token: 0x04002313 RID: 8979
	protected Chore chore;

	// Token: 0x04002314 RID: 8980
	private string buttonLabel;

	// Token: 0x04002315 RID: 8981
	private string buttonTooltip;

	// Token: 0x04002316 RID: 8982
	private string cancelButtonLabel;

	// Token: 0x04002317 RID: 8983
	private string cancelButtonTooltip;

	// Token: 0x04002318 RID: 8984
	private StatusItem pendingStatusItem;

	// Token: 0x04002319 RID: 8985
	public bool showUserMenuButtons = true;

	// Token: 0x0400231A RID: 8986
	public string dropItemPrefabId;

	// Token: 0x0400231B RID: 8987
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400231C RID: 8988
	private static readonly EventSystem.IntraObjectHandler<Carvable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Carvable>(delegate(Carvable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x0400231D RID: 8989
	private static readonly EventSystem.IntraObjectHandler<Carvable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Carvable>(delegate(Carvable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
