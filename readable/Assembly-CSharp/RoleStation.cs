using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B19 RID: 2841
[AddComponentMenu("KMonoBehaviour/Workable/RoleStation")]
public class RoleStation : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x060052BC RID: 21180 RVA: 0x001E1DD8 File Offset: 0x001DFFD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = true;
		this.UpdateStatusItemDelegate = new Action<object>(this.UpdateSkillPointAvailableStatusItem);
	}

	// Token: 0x060052BD RID: 21181 RVA: 0x001E1DFC File Offset: 0x001DFFFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RoleStations.Add(this);
		this.smi = new RoleStation.RoleStationSM.Instance(this);
		this.smi.StartSM();
		base.SetWorkTime(7.53f);
		this.resetProgressOnStop = true;
		this.subscriptions.Add(Game.Instance.Subscribe(-1523247426, this.UpdateStatusItemDelegate));
		this.subscriptions.Add(Game.Instance.Subscribe(1505456302, this.UpdateStatusItemDelegate));
		this.UpdateSkillPointAvailableStatusItem(null);
	}

	// Token: 0x060052BE RID: 21182 RVA: 0x001E1E8C File Offset: 0x001E008C
	protected override void OnStopWork(WorkerBase worker)
	{
		Telepad.StatesInstance statesInstance = this.GetSMI<Telepad.StatesInstance>();
		statesInstance.sm.idlePortal.Trigger(statesInstance);
	}

	// Token: 0x060052BF RID: 21183 RVA: 0x001E1EB4 File Offset: 0x001E00B4
	private void UpdateSkillPointAvailableStatusItem(object data = null)
	{
		foreach (object obj in Components.MinionResumes)
		{
			MinionResume minionResume = (MinionResume)obj;
			if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
			{
				if (this.skillPointAvailableStatusItem == Guid.Empty)
				{
					this.skillPointAvailableStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, null);
				}
				return;
			}
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, false);
		this.skillPointAvailableStatusItem = Guid.Empty;
	}

	// Token: 0x060052C0 RID: 21184 RVA: 0x001E1F80 File Offset: 0x001E0180
	private Chore CreateWorkChore()
	{
		return new WorkChore<RoleStation>(Db.Get().ChoreTypes.LearnSkill, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
	}

	// Token: 0x060052C1 RID: 21185 RVA: 0x001E1FC1 File Offset: 0x001E01C1
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		worker.GetComponent<MinionResume>().SkillLearned();
	}

	// Token: 0x060052C2 RID: 21186 RVA: 0x001E1FD5 File Offset: 0x001E01D5
	private void OnSelectRolesClick()
	{
		DetailsScreen.Instance.Show(false);
		ManagementMenu.Instance.ToggleSkills();
	}

	// Token: 0x060052C3 RID: 21187 RVA: 0x001E1FEC File Offset: 0x001E01EC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int id in this.subscriptions)
		{
			Game.Instance.Unsubscribe(id);
		}
		Components.RoleStations.Remove(this);
	}

	// Token: 0x060052C4 RID: 21188 RVA: 0x001E2054 File Offset: 0x001E0254
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		return base.GetDescriptors(go);
	}

	// Token: 0x040037EC RID: 14316
	private Chore chore;

	// Token: 0x040037ED RID: 14317
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x040037EE RID: 14318
	[MyCmpAdd]
	private Operational operational;

	// Token: 0x040037EF RID: 14319
	private RoleStation.RoleStationSM.Instance smi;

	// Token: 0x040037F0 RID: 14320
	private Guid skillPointAvailableStatusItem;

	// Token: 0x040037F1 RID: 14321
	private Action<object> UpdateStatusItemDelegate;

	// Token: 0x040037F2 RID: 14322
	private List<int> subscriptions = new List<int>();

	// Token: 0x02001C63 RID: 7267
	public class RoleStationSM : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation>
	{
		// Token: 0x0600AD49 RID: 44361 RVA: 0x003CF154 File Offset: 0x003CD354
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (RoleStation.RoleStationSM.Instance smi) => smi.GetComponent<Operational>().IsOperational);
			this.operational.ToggleChore((RoleStation.RoleStationSM.Instance smi) => smi.master.CreateWorkChore(), this.unoperational);
		}

		// Token: 0x040087D6 RID: 34774
		public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State unoperational;

		// Token: 0x040087D7 RID: 34775
		public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State operational;

		// Token: 0x02002A18 RID: 10776
		public new class Instance : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.GameInstance
		{
			// Token: 0x0600D384 RID: 54148 RVA: 0x0043A9E3 File Offset: 0x00438BE3
			public Instance(RoleStation master) : base(master)
			{
			}
		}
	}
}
