using System;
using System.Collections;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005D3 RID: 1491
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Diggable")]
public class Diggable : Workable
{
	// Token: 0x17000157 RID: 343
	// (get) Token: 0x0600224B RID: 8779 RVA: 0x000C737B File Offset: 0x000C557B
	public bool Reachable
	{
		get
		{
			return this.isReachable;
		}
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x000C7384 File Offset: 0x000C5584
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Digging;
		this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.DigRequiresSkillPerk;
		this.faceTargetWhenWorking = true;
		base.Subscribe<Diggable>(-1432940121, Diggable.OnReachableChangedDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.DiggingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.multitoolContext = "dig";
		this.multitoolHitEffectTag = "fx_dig_splash";
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x000C7457 File Offset: 0x000C5657
	private Diggable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
	}

	// Token: 0x0600224E RID: 8782 RVA: 0x000C7474 File Offset: 0x000C5674
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cached_cell = Grid.PosToCell(this);
		this.originalDigElement = Grid.Element[this.cached_cell];
		if (this.originalDigElement.hardness == 255)
		{
			this.OnCancel();
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForDig, null);
		this.UpdateColor(this.isReachable);
		Grid.Objects[this.cached_cell, 7] = base.gameObject;
		ChoreType chore_type = Db.Get().ChoreTypes.Dig;
		if (this.choreTypeIdHash.IsValid)
		{
			chore_type = Db.Get().ChoreTypes.GetByHash(this.choreTypeIdHash);
		}
		this.chore = new WorkChore<Diggable>(chore_type, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Diggable.OnSpawn", base.gameObject, Grid.PosToCell(this), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.OnSolidChanged(null);
		new ReachabilityMonitor.Instance(this).StartSM();
		base.Subscribe<Diggable>(493375141, Diggable.OnRefreshUserMenuDelegate);
		this.handle = Game.Instance.Subscribe(-1523247426, Workable.UpdateStatusItemDispatcher, this);
		Components.Diggables.Add(this);
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x000C75E7 File Offset: 0x000C57E7
	public override int GetCell()
	{
		return this.cached_cell;
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x000C75F0 File Offset: 0x000C57F0
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		if (this.overrideAnims != null && this.overrideAnims.Length != 0)
		{
			result.overrideAnims = this.overrideAnims;
		}
		if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
		{
			result.smi = new MultitoolController.Instance(this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
		}
		return result;
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x000C7660 File Offset: 0x000C5860
	private static bool IsCellBuildable(int cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null && gameObject.GetComponent<Constructable>() != null)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06002252 RID: 8786 RVA: 0x000C7696 File Offset: 0x000C5896
	private IEnumerator PeriodicUnstableFallingRecheck()
	{
		yield return SequenceUtil.WaitForSeconds(2f);
		this.OnSolidChanged(null);
		yield break;
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x000C76A8 File Offset: 0x000C58A8
	private void OnSolidChanged(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		GameScenePartitioner.Instance.Free(ref this.unstableEntry);
		int num = -1;
		this.UpdateColor(this.isReachable);
		if (Grid.Element[this.cached_cell].hardness == 255)
		{
			this.UpdateColor(false);
			this.requiredSkillPerk = null;
			this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigUnobtanium);
		}
		else if (Grid.Element[this.cached_cell].hardness >= 251)
		{
			bool flag = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigRadioactiveMaterials);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigRadioactiveMaterials.Id;
			this.materialDisplay.sharedMaterial = this.materials[3];
		}
		else if (Grid.Element[this.cached_cell].hardness >= 200)
		{
			bool flag2 = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigSuperDuperHard);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigSuperDuperHard.Id;
			this.materialDisplay.sharedMaterial = this.materials[3];
		}
		else if (Grid.Element[this.cached_cell].hardness >= 150)
		{
			bool flag3 = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag3 = true;
						break;
					}
				}
			}
			if (!flag3)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigNearlyImpenetrable);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigNearlyImpenetrable.Id;
			this.materialDisplay.sharedMaterial = this.materials[2];
		}
		else if (Grid.Element[this.cached_cell].hardness >= 50)
		{
			bool flag4 = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag4 = true;
						break;
					}
				}
			}
			if (!flag4)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigVeryFirm);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigVeryFirm.Id;
			this.materialDisplay.sharedMaterial = this.materials[1];
		}
		else
		{
			this.requiredSkillPerk = null;
			this.chore.GetPreconditions().Remove(this.chore.GetPreconditions().Find((Chore.PreconditionInstance o) => o.condition.id == ChorePreconditions.instance.HasSkillPerk.id));
		}
		this.UpdateStatusItem(null);
		bool flag5 = false;
		if (!Grid.Solid[this.cached_cell])
		{
			num = Diggable.GetUnstableCellAbove(this.cached_cell);
			if (num == -1)
			{
				flag5 = true;
			}
			else
			{
				base.StartCoroutine("PeriodicUnstableFallingRecheck");
			}
		}
		else if (Grid.Foundation[this.cached_cell])
		{
			flag5 = true;
		}
		if (!flag5)
		{
			if (num != -1)
			{
				Extents extents = default(Extents);
				Grid.CellToXY(this.cached_cell, out extents.x, out extents.y);
				extents.width = 1;
				extents.height = (num - this.cached_cell + Grid.WidthInCells - 1) / Grid.WidthInCells + 1;
				this.unstableEntry = GameScenePartitioner.Instance.Add("Diggable.OnSolidChanged", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
			}
			return;
		}
		this.isDigComplete = true;
		if (this.chore == null || !this.chore.InProgress())
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		base.GetComponentInChildren<MeshRenderer>().enabled = false;
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000C7BF4 File Offset: 0x000C5DF4
	public Element GetTargetElement()
	{
		return Grid.Element[this.cached_cell];
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x000C7C02 File Offset: 0x000C5E02
	public override string GetConversationTopic()
	{
		return this.originalDigElement.tag.Name;
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x000C7C14 File Offset: 0x000C5E14
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		Diggable.DoDigTick(this.cached_cell, dt);
		return this.isDigComplete;
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x000C7C28 File Offset: 0x000C5E28
	protected override void OnStopWork(WorkerBase worker)
	{
		if (this.isDigComplete)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x000C7C40 File Offset: 0x000C5E40
	public override bool InstantlyFinish(WorkerBase worker)
	{
		if (Grid.Element[this.cached_cell].hardness == 255)
		{
			return false;
		}
		float approximateDigTime = Diggable.GetApproximateDigTime(this.cached_cell);
		worker.Work(approximateDigTime);
		return true;
	}

	// Token: 0x06002259 RID: 8793 RVA: 0x000C7C7C File Offset: 0x000C5E7C
	public static void DoDigTick(int cell, float dt)
	{
		Diggable.DoDigTick(cell, dt, WorldDamage.DamageType.Absolute);
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x000C7C88 File Offset: 0x000C5E88
	public static void DoDigTick(int cell, float dt, WorldDamage.DamageType damageType)
	{
		float approximateDigTime = Diggable.GetApproximateDigTime(cell);
		float amount = dt / approximateDigTime;
		WorldDamage.Instance.ApplyDamage(cell, amount, -1, damageType, null, null);
	}

	// Token: 0x0600225B RID: 8795 RVA: 0x000C7CB4 File Offset: 0x000C5EB4
	public static float GetApproximateDigTime(int cell)
	{
		float num = (float)Grid.Element[cell].hardness;
		if (num == 255f)
		{
			return float.MaxValue;
		}
		Element element = ElementLoader.FindElementByHash(SimHashes.Ice);
		float num2 = num / (float)element.hardness;
		float num3 = Mathf.Min(Grid.Mass[cell], 400f) / 400f;
		float num4 = 4f * num3;
		return num4 + num2 * num4;
	}

	// Token: 0x0600225C RID: 8796 RVA: 0x000C7D20 File Offset: 0x000C5F20
	public static Diggable GetDiggable(int cell)
	{
		GameObject gameObject = Grid.Objects[cell, 7];
		if (gameObject != null)
		{
			return gameObject.GetComponent<Diggable>();
		}
		return null;
	}

	// Token: 0x0600225D RID: 8797 RVA: 0x000C7D4B File Offset: 0x000C5F4B
	public static bool IsDiggable(int cell)
	{
		if (Grid.Solid[cell])
		{
			return !Grid.Foundation[cell];
		}
		return Diggable.GetUnstableCellAbove(cell) != Grid.InvalidCell;
	}

	// Token: 0x0600225E RID: 8798 RVA: 0x000C7D7C File Offset: 0x000C5F7C
	private static int GetUnstableCellAbove(int cell)
	{
		Vector2I cellXY = Grid.CellToXY(cell);
		List<int> cellsContainingFallingAbove = World.Instance.GetComponent<UnstableGroundManager>().GetCellsContainingFallingAbove(cellXY);
		if (cellsContainingFallingAbove.Contains(cell))
		{
			return cell;
		}
		byte b = Grid.WorldIdx[cell];
		int num = Grid.CellAbove(cell);
		while (Grid.IsValidCell(num) && Grid.WorldIdx[num] == b)
		{
			if (Grid.Foundation[num])
			{
				return Grid.InvalidCell;
			}
			if (Grid.Solid[num])
			{
				if (Grid.Element[num].IsUnstable)
				{
					return num;
				}
				return Grid.InvalidCell;
			}
			else
			{
				if (cellsContainingFallingAbove.Contains(num))
				{
					return num;
				}
				num = Grid.CellAbove(num);
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x000C7E1C File Offset: 0x000C601C
	public static bool RequiresTool(Element e)
	{
		return false;
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x000C7E1F File Offset: 0x000C601F
	public static bool Undiggable(Element e)
	{
		return e.id == SimHashes.Unobtanium;
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x000C7E30 File Offset: 0x000C6030
	private void OnReachableChanged(object data)
	{
		if (this.childRenderer == null)
		{
			this.childRenderer = base.GetComponentInChildren<MeshRenderer>();
		}
		Material material = this.childRenderer.material;
		this.isReachable = ((Boxed<bool>)data).value;
		if (material.color == Game.Instance.uiColours.Dig.invalidLocation)
		{
			return;
		}
		this.UpdateColor(this.isReachable);
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.isReachable)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.DigUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.DigUnreachable, this);
		GameScheduler.Instance.Schedule("Locomotion Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, true);
		}, null, null);
	}

	// Token: 0x06002262 RID: 8802 RVA: 0x000C7F14 File Offset: 0x000C6114
	private void UpdateColor(bool reachable)
	{
		if (this.childRenderer != null)
		{
			Material material = this.childRenderer.material;
			if (Diggable.RequiresTool(Grid.Element[Grid.PosToCell(base.gameObject)]) || Diggable.Undiggable(Grid.Element[Grid.PosToCell(base.gameObject)]))
			{
				material.color = Game.Instance.uiColours.Dig.invalidLocation;
				return;
			}
			if (Grid.Element[Grid.PosToCell(base.gameObject)].hardness >= 50)
			{
				if (reachable)
				{
					material.color = Game.Instance.uiColours.Dig.validLocation;
				}
				else
				{
					material.color = Game.Instance.uiColours.Dig.unreachable;
				}
				this.multitoolContext = Diggable.lasersForHardness[1].first;
				this.multitoolHitEffectTag = Diggable.lasersForHardness[1].second;
				return;
			}
			if (reachable)
			{
				material.color = Game.Instance.uiColours.Dig.validLocation;
			}
			else
			{
				material.color = Game.Instance.uiColours.Dig.unreachable;
			}
			this.multitoolContext = Diggable.lasersForHardness[0].first;
			this.multitoolHitEffectTag = Diggable.lasersForHardness[0].second;
		}
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x000C8078 File Offset: 0x000C6278
	public override float GetPercentComplete()
	{
		return Grid.Damage[Grid.PosToCell(this)];
	}

	// Token: 0x06002264 RID: 8804 RVA: 0x000C8088 File Offset: 0x000C6288
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.unstableEntry);
		Game.Instance.Unsubscribe(ref this.handle);
		int cell = Grid.PosToCell(this);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.digDestroyedLayer, null);
		Components.Diggables.Remove(this);
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x000C80F3 File Offset: 0x000C62F3
	private void OnCancel()
	{
		if (DetailsScreen.Instance != null)
		{
			DetailsScreen.Instance.Show(false);
		}
		base.gameObject.Trigger(2127324410, null);
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x000C8120 File Offset: 0x000C6320
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("icon_cancel", UI.USERMENUACTIONS.CANCELDIG.NAME, new System.Action(this.OnCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELDIG.TOOLTIP, true), 1f);
	}

	// Token: 0x04001403 RID: 5123
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001404 RID: 5124
	private HandleVector<int>.Handle unstableEntry;

	// Token: 0x04001405 RID: 5125
	private MeshRenderer childRenderer;

	// Token: 0x04001406 RID: 5126
	private bool isReachable;

	// Token: 0x04001407 RID: 5127
	private int cached_cell = -1;

	// Token: 0x04001408 RID: 5128
	private Element originalDigElement;

	// Token: 0x04001409 RID: 5129
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x0400140A RID: 5130
	[SerializeField]
	public HashedString choreTypeIdHash;

	// Token: 0x0400140B RID: 5131
	[SerializeField]
	public Material[] materials;

	// Token: 0x0400140C RID: 5132
	[SerializeField]
	public MeshRenderer materialDisplay;

	// Token: 0x0400140D RID: 5133
	private bool isDigComplete;

	// Token: 0x0400140E RID: 5134
	private static List<global::Tuple<string, Tag>> lasersForHardness = new List<global::Tuple<string, Tag>>
	{
		new global::Tuple<string, Tag>("dig", "fx_dig_splash"),
		new global::Tuple<string, Tag>("specialistdig", "fx_dig_splash")
	};

	// Token: 0x0400140F RID: 5135
	private int handle;

	// Token: 0x04001410 RID: 5136
	private static readonly EventSystem.IntraObjectHandler<Diggable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Diggable>(delegate(Diggable component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x04001411 RID: 5137
	private static readonly EventSystem.IntraObjectHandler<Diggable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Diggable>(delegate(Diggable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001412 RID: 5138
	public Chore chore;
}
