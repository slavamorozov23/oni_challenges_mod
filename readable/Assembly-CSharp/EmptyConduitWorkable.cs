using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000919 RID: 2329
[AddComponentMenu("KMonoBehaviour/Workable/EmptyConduitWorkable")]
public class EmptyConduitWorkable : Workable, IEmptyConduitWorkable
{
	// Token: 0x060040ED RID: 16621 RVA: 0x0016FB9C File Offset: 0x0016DD9C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		base.SetWorkTime(float.PositiveInfinity);
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		base.Subscribe<EmptyConduitWorkable>(2127324410, EmptyConduitWorkable.OnEmptyConduitCancelledDelegate);
		if (EmptyConduitWorkable.emptyLiquidConduitStatusItem == null)
		{
			EmptyConduitWorkable.emptyLiquidConduitStatusItem = new StatusItem("EmptyLiquidConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, 66, true, null);
			EmptyConduitWorkable.emptyGasConduitStatusItem = new StatusItem("EmptyGasConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.GasConduits.ID, 130, true, null);
		}
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x060040EE RID: 16622 RVA: 0x0016FC90 File Offset: 0x0016DE90
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elapsedTime != -1f)
		{
			this.MarkForEmptying();
		}
	}

	// Token: 0x060040EF RID: 16623 RVA: 0x0016FCAC File Offset: 0x0016DEAC
	public void MarkForEmptying()
	{
		if (this.chore == null && this.HasContents())
		{
			StatusItem statusItem = this.GetStatusItem();
			base.GetComponent<KSelectable>().ToggleStatusItem(statusItem, true, null);
			this.CreateWorkChore();
		}
	}

	// Token: 0x060040F0 RID: 16624 RVA: 0x0016FCE8 File Offset: 0x0016DEE8
	private bool HasContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.GetFlowManager().GetContents(cell).mass > 0f;
	}

	// Token: 0x060040F1 RID: 16625 RVA: 0x0016FD21 File Offset: 0x0016DF21
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

	// Token: 0x060040F2 RID: 16626 RVA: 0x0016FD58 File Offset: 0x0016DF58
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

	// Token: 0x060040F3 RID: 16627 RVA: 0x0016FDA4 File Offset: 0x0016DFA4
	protected override void OnCleanUp()
	{
		this.CancelEmptying();
		base.OnCleanUp();
	}

	// Token: 0x060040F4 RID: 16628 RVA: 0x0016FDB2 File Offset: 0x0016DFB2
	private ConduitFlow GetFlowManager()
	{
		if (this.conduit.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x060040F5 RID: 16629 RVA: 0x0016FDD7 File Offset: 0x0016DFD7
	private void OnEmptyConduitCancelled(object _)
	{
		this.CancelEmptying();
	}

	// Token: 0x060040F6 RID: 16630 RVA: 0x0016FDE0 File Offset: 0x0016DFE0
	private StatusItem GetStatusItem()
	{
		ConduitType type = this.conduit.type;
		StatusItem result;
		if (type != ConduitType.Gas)
		{
			if (type != ConduitType.Liquid)
			{
				throw new ArgumentException();
			}
			result = EmptyConduitWorkable.emptyLiquidConduitStatusItem;
		}
		else
		{
			result = EmptyConduitWorkable.emptyGasConduitStatusItem;
		}
		return result;
	}

	// Token: 0x060040F7 RID: 16631 RVA: 0x0016FE1C File Offset: 0x0016E01C
	private void CreateWorkChore()
	{
		base.GetComponent<Prioritizable>().AddRef();
		this.chore = new WorkChore<EmptyConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
		this.elapsedTime = 0f;
		this.emptiedPipe = false;
		this.shouldShowSkillPerkStatusItem = true;
		this.UpdateStatusItem(null);
	}

	// Token: 0x060040F8 RID: 16632 RVA: 0x0016FEAC File Offset: 0x0016E0AC
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
			if (this.GetFlowManager().GetContents(cell).mass > 0f)
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

	// Token: 0x060040F9 RID: 16633 RVA: 0x0016FF75 File Offset: 0x0016E175
	public override bool InstantlyFinish(WorkerBase worker)
	{
		worker.Work(4f);
		return true;
	}

	// Token: 0x060040FA RID: 16634 RVA: 0x0016FF84 File Offset: 0x0016E184
	public void EmptyContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		ConduitFlow.ConduitContents conduitContents = this.GetFlowManager().RemoveElement(cell, float.PositiveInfinity);
		this.elapsedTime = 0f;
		if (conduitContents.mass > 0f && conduitContents.element != SimHashes.Vacuum)
		{
			ConduitType type = this.conduit.type;
			IChunkManager instance;
			if (type != ConduitType.Gas)
			{
				if (type != ConduitType.Liquid)
				{
					throw new ArgumentException();
				}
				instance = LiquidSourceManager.Instance;
			}
			else
			{
				instance = GasSourceManager.Instance;
			}
			instance.CreateChunk(conduitContents.element, conduitContents.mass, conduitContents.temperature, conduitContents.diseaseIdx, conduitContents.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore)).Trigger(580035959, base.worker);
		}
	}

	// Token: 0x060040FB RID: 16635 RVA: 0x00170043 File Offset: 0x0016E243
	public override float GetPercentComplete()
	{
		return Mathf.Clamp01(this.elapsedTime / 4f);
	}

	// Token: 0x0400289F RID: 10399
	[MyCmpReq]
	private Conduit conduit;

	// Token: 0x040028A0 RID: 10400
	private static StatusItem emptyLiquidConduitStatusItem;

	// Token: 0x040028A1 RID: 10401
	private static StatusItem emptyGasConduitStatusItem;

	// Token: 0x040028A2 RID: 10402
	private Chore chore;

	// Token: 0x040028A3 RID: 10403
	private const float RECHECK_PIPE_INTERVAL = 2f;

	// Token: 0x040028A4 RID: 10404
	private const float TIME_TO_EMPTY_PIPE = 4f;

	// Token: 0x040028A5 RID: 10405
	private const float NO_EMPTY_SCHEDULED = -1f;

	// Token: 0x040028A6 RID: 10406
	[Serialize]
	private float elapsedTime = -1f;

	// Token: 0x040028A7 RID: 10407
	private bool emptiedPipe = true;

	// Token: 0x040028A8 RID: 10408
	private static readonly EventSystem.IntraObjectHandler<EmptyConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptyConduitWorkable>(delegate(EmptyConduitWorkable component, object data)
	{
		component.OnEmptyConduitCancelled(data);
	});
}
