using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200091A RID: 2330
[AddComponentMenu("KMonoBehaviour/Workable/EmptySolidConduitWorkable")]
public class EmptySolidConduitWorkable : Workable, IEmptyConduitWorkable
{
	// Token: 0x060040FE RID: 16638 RVA: 0x0017008C File Offset: 0x0016E28C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		base.SetWorkTime(float.PositiveInfinity);
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		base.Subscribe<EmptySolidConduitWorkable>(2127324410, EmptySolidConduitWorkable.OnEmptyConduitCancelledDelegate);
		if (EmptySolidConduitWorkable.emptySolidConduitStatusItem == null)
		{
			EmptySolidConduitWorkable.emptySolidConduitStatusItem = new StatusItem("EmptySolidConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.SolidConveyor.ID, 32770, true, null);
		}
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x060040FF RID: 16639 RVA: 0x0017014C File Offset: 0x0016E34C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elapsedTime != -1f)
		{
			this.MarkForEmptying();
		}
	}

	// Token: 0x06004100 RID: 16640 RVA: 0x00170168 File Offset: 0x0016E368
	public void MarkForEmptying()
	{
		if (this.chore == null && this.HasContents())
		{
			StatusItem statusItem = this.GetStatusItem();
			base.GetComponent<KSelectable>().ToggleStatusItem(statusItem, true, null);
			this.CreateWorkChore();
		}
	}

	// Token: 0x06004101 RID: 16641 RVA: 0x001701A4 File Offset: 0x0016E3A4
	private bool HasContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid();
	}

	// Token: 0x06004102 RID: 16642 RVA: 0x001701DB File Offset: 0x0016E3DB
	private void CancelEmptying()
	{
		this.CleanUpVisualization();
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel");
			this.chore = null;
			this.shouldShowSkillPerkStatusItem = false;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x06004103 RID: 16643 RVA: 0x00170210 File Offset: 0x0016E410
	private void CleanUpVisualization()
	{
		StatusItem statusItem = this.GetStatusItem();
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.ToggleStatusItem(statusItem, false, null);
		}
		this.elapsedTime = -1f;
		if (this.chore != null)
		{
			base.GetComponent<Prioritizable>().RemoveRef();
		}
	}

	// Token: 0x06004104 RID: 16644 RVA: 0x0017025C File Offset: 0x0016E45C
	protected override void OnCleanUp()
	{
		this.CancelEmptying();
		base.OnCleanUp();
	}

	// Token: 0x06004105 RID: 16645 RVA: 0x0017026A File Offset: 0x0016E46A
	private SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x06004106 RID: 16646 RVA: 0x00170276 File Offset: 0x0016E476
	private void OnEmptyConduitCancelled(object _)
	{
		this.CancelEmptying();
	}

	// Token: 0x06004107 RID: 16647 RVA: 0x0017027E File Offset: 0x0016E47E
	private StatusItem GetStatusItem()
	{
		return EmptySolidConduitWorkable.emptySolidConduitStatusItem;
	}

	// Token: 0x06004108 RID: 16648 RVA: 0x00170288 File Offset: 0x0016E488
	private void CreateWorkChore()
	{
		base.GetComponent<Prioritizable>().AddRef();
		this.chore = new WorkChore<EmptySolidConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
		this.elapsedTime = 0f;
		this.emptiedPipe = false;
		this.shouldShowSkillPerkStatusItem = true;
		this.UpdateStatusItem(null);
	}

	// Token: 0x06004109 RID: 16649 RVA: 0x00170318 File Offset: 0x0016E518
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.elapsedTime == -1f)
		{
			return true;
		}
		bool result = false;
		this.elapsedTime += dt;
		if (!this.emptiedPipe)
		{
			if (this.elapsedTime > 4f)
			{
				this.EmptyContents();
				this.emptiedPipe = true;
				this.elapsedTime = 0f;
			}
		}
		else if (this.elapsedTime > 2f)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			if (this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid())
			{
				this.elapsedTime = 0f;
				this.emptiedPipe = false;
			}
			else
			{
				this.CleanUpVisualization();
				this.chore = null;
				result = true;
				this.shouldShowSkillPerkStatusItem = false;
				this.UpdateStatusItem(null);
			}
		}
		return result;
	}

	// Token: 0x0600410A RID: 16650 RVA: 0x001703E1 File Offset: 0x0016E5E1
	public override bool InstantlyFinish(WorkerBase worker)
	{
		worker.Work(4f);
		return true;
	}

	// Token: 0x0600410B RID: 16651 RVA: 0x001703F0 File Offset: 0x0016E5F0
	public void EmptyContents()
	{
		int cell_idx = Grid.PosToCell(base.transform.GetPosition());
		this.GetFlowManager().RemovePickupable(cell_idx);
		this.elapsedTime = 0f;
	}

	// Token: 0x0600410C RID: 16652 RVA: 0x00170426 File Offset: 0x0016E626
	public override float GetPercentComplete()
	{
		return Mathf.Clamp01(this.elapsedTime / 4f);
	}

	// Token: 0x040028A9 RID: 10409
	[MyCmpReq]
	private SolidConduit conduit;

	// Token: 0x040028AA RID: 10410
	private static StatusItem emptySolidConduitStatusItem;

	// Token: 0x040028AB RID: 10411
	private Chore chore;

	// Token: 0x040028AC RID: 10412
	private const float RECHECK_PIPE_INTERVAL = 2f;

	// Token: 0x040028AD RID: 10413
	private const float TIME_TO_EMPTY_PIPE = 4f;

	// Token: 0x040028AE RID: 10414
	private const float NO_EMPTY_SCHEDULED = -1f;

	// Token: 0x040028AF RID: 10415
	[Serialize]
	private float elapsedTime = -1f;

	// Token: 0x040028B0 RID: 10416
	private bool emptiedPipe = true;

	// Token: 0x040028B1 RID: 10417
	private static readonly EventSystem.IntraObjectHandler<EmptySolidConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptySolidConduitWorkable>(delegate(EmptySolidConduitWorkable component, object data)
	{
		component.OnEmptyConduitCancelled(data);
	});
}
