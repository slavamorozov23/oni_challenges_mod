using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200060A RID: 1546
[AddComponentMenu("KMonoBehaviour/Workable/Moppable")]
public class Moppable : Workable, ISim1000ms, ISim200ms
{
	// Token: 0x0600240F RID: 9231 RVA: 0x000D09A4 File Offset: 0x000CEBA4
	private Moppable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06002410 RID: 9232 RVA: 0x000D0A00 File Offset: 0x000CEC00
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Mopping;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.childRenderer = base.GetComponentInChildren<MeshRenderer>();
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x000D0A84 File Offset: 0x000CEC84
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!this.IsThereLiquid())
		{
			base.gameObject.DeleteObject();
			return;
		}
		Grid.Objects[Grid.PosToCell(base.gameObject), 8] = base.gameObject;
		new WorkChore<Moppable>(Db.Get().ChoreTypes.Mop, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForMop, null);
		base.Subscribe<Moppable>(493375141, Moppable.OnRefreshUserMenuDelegate);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_mop_dirtywater_kanim")
		};
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Moppable.OnSpawn", base.gameObject, new Extents(Grid.PosToCell(this), new CellOffset[]
		{
			new CellOffset(0, 0)
		}), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		this.Refresh();
		base.Subscribe<Moppable>(-1432940121, Moppable.OnReachableChangedDelegate);
		new ReachabilityMonitor.Instance(this).StartSM();
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06002412 RID: 9234 RVA: 0x000D0BD0 File Offset: 0x000CEDD0
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("icon_cancel", UI.USERMENUACTIONS.CANCELMOP.NAME, new System.Action(this.OnCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELMOP.TOOLTIP, true), 1f);
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x000D0C2A File Offset: 0x000CEE2A
	private void OnCancel()
	{
		DetailsScreen.Instance.Show(false);
		base.gameObject.Trigger(2127324410, null);
	}

	// Token: 0x06002414 RID: 9236 RVA: 0x000D0C48 File Offset: 0x000CEE48
	protected override void OnStartWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Add(this, false);
		this.Refresh();
		this.MopTick(this.amountMoppedPerTick);
	}

	// Token: 0x06002415 RID: 9237 RVA: 0x000D0C68 File Offset: 0x000CEE68
	protected override void OnStopWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06002416 RID: 9238 RVA: 0x000D0C75 File Offset: 0x000CEE75
	protected override void OnCompleteWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06002417 RID: 9239 RVA: 0x000D0C82 File Offset: 0x000CEE82
	public override bool InstantlyFinish(WorkerBase worker)
	{
		this.MopTick(1000f);
		return true;
	}

	// Token: 0x06002418 RID: 9240 RVA: 0x000D0C90 File Offset: 0x000CEE90
	public void Sim1000ms(float dt)
	{
		if (this.amountMopped > 0f)
		{
			PopFX popFX = PopFXManager.Instance.SpawnFX((this.lastElementMopped == null) ? PopFXManager.Instance.sprite_Resource : Def.GetUISprite(this.lastElementMopped, "ui", false).first, null, GameUtil.GetFormattedMass(-this.amountMopped, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), base.transform, Vector3.zero, 1.5f, true, false, false);
			if (popFX != null && this.lastElementMopped != null && this.lastElementMopped.substance != null)
			{
				popFX.SetIconTint(this.lastElementMopped.substance.colour);
			}
			this.amountMopped = 0f;
			this.lastElementMopped = null;
		}
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x000D0D55 File Offset: 0x000CEF55
	public void Sim200ms(float dt)
	{
		if (base.worker != null)
		{
			this.Refresh();
			this.MopTick(this.amountMoppedPerTick);
		}
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x000D0D78 File Offset: 0x000CEF78
	private void OnCellMopped(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		if (this == null)
		{
			return;
		}
		if (mass_cb_info.mass > 0f)
		{
			this.amountMopped += mass_cb_info.mass;
			int cell = Grid.PosToCell(this);
			Element element = ElementLoader.elements[(int)mass_cb_info.elemIdx];
			SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(element, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
			substanceChunk.transform.SetPosition(substanceChunk.transform.GetPosition() + new Vector3((UnityEngine.Random.value - 0.5f) * 0.5f, 0f, 0f));
			this.lastElementMopped = element;
		}
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x000D0E3C File Offset: 0x000CF03C
	public static void MopCell(int cell, float amount, Action<Sim.MassConsumedCallback, object> cb)
	{
		if (Grid.Element[cell].IsLiquid)
		{
			int callbackIdx = -1;
			if (cb != null)
			{
				callbackIdx = Game.Instance.massConsumedCallbackManager.Add(cb, null, "Moppable").index;
			}
			SimMessages.ConsumeMass(cell, Grid.Element[cell].id, amount, 1, callbackIdx);
		}
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x000D0E90 File Offset: 0x000CF090
	private void MopTick(float mopAmount)
	{
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < this.offsets.Length; i++)
		{
			int num = Grid.OffsetCell(cell, this.offsets[i]);
			if (Grid.Element[num].IsLiquid)
			{
				Moppable.MopCell(num, mopAmount, new Action<Sim.MassConsumedCallback, object>(this.OnCellMopped));
			}
		}
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x000D0EEC File Offset: 0x000CF0EC
	private bool IsThereLiquid()
	{
		int cell = Grid.PosToCell(this);
		bool result = false;
		for (int i = 0; i < this.offsets.Length; i++)
		{
			int num = Grid.OffsetCell(cell, this.offsets[i]);
			if (Grid.Element[num].IsLiquid && Grid.Mass[num] <= MopTool.maxMopAmt)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x000D0F4C File Offset: 0x000CF14C
	private void Refresh()
	{
		if (!this.IsThereLiquid())
		{
			if (!this.destroyHandle.IsValid)
			{
				this.destroyHandle = GameScheduler.Instance.Schedule("DestroyMoppable", 1f, delegate(object moppable)
				{
					this.TryDestroy();
				}, this, null);
				return;
			}
		}
		else if (this.destroyHandle.IsValid)
		{
			this.destroyHandle.ClearScheduler();
		}
	}

	// Token: 0x0600241F RID: 9247 RVA: 0x000D0FAF File Offset: 0x000CF1AF
	private void OnLiquidChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x06002420 RID: 9248 RVA: 0x000D0FB7 File Offset: 0x000CF1B7
	private void TryDestroy()
	{
		if (this != null)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06002421 RID: 9249 RVA: 0x000D0FCD File Offset: 0x000CF1CD
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06002422 RID: 9250 RVA: 0x000D0FE8 File Offset: 0x000CF1E8
	private void OnReachableChanged(object data)
	{
		if (this.childRenderer != null)
		{
			Material material = this.childRenderer.material;
			bool value = ((Boxed<bool>)data).value;
			if (material.color == Game.Instance.uiColours.Dig.invalidLocation)
			{
				return;
			}
			KSelectable component = base.GetComponent<KSelectable>();
			if (value)
			{
				material.color = Game.Instance.uiColours.Dig.validLocation;
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.MopUnreachable, false);
				return;
			}
			component.AddStatusItem(Db.Get().BuildingStatusItems.MopUnreachable, this);
			GameScheduler.Instance.Schedule("Locomotion Tutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, true);
			}, null, null);
			material.color = Game.Instance.uiColours.Dig.unreachable;
		}
	}

	// Token: 0x04001503 RID: 5379
	[MyCmpReq]
	private KSelectable Selectable;

	// Token: 0x04001504 RID: 5380
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04001505 RID: 5381
	public float amountMoppedPerTick = 1000f;

	// Token: 0x04001506 RID: 5382
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001507 RID: 5383
	private SchedulerHandle destroyHandle;

	// Token: 0x04001508 RID: 5384
	private float amountMopped;

	// Token: 0x04001509 RID: 5385
	private Element lastElementMopped;

	// Token: 0x0400150A RID: 5386
	private MeshRenderer childRenderer;

	// Token: 0x0400150B RID: 5387
	private CellOffset[] offsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x0400150C RID: 5388
	private static readonly EventSystem.IntraObjectHandler<Moppable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Moppable>(delegate(Moppable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0400150D RID: 5389
	private static readonly EventSystem.IntraObjectHandler<Moppable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Moppable>(delegate(Moppable component, object data)
	{
		component.OnReachableChanged(data);
	});
}
