using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000BAE RID: 2990
[SerializationConfig(MemberSerialization.OptIn)]
public class TouristModule : StateMachineComponent<TouristModule.StatesInstance>
{
	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06005999 RID: 22937 RVA: 0x00208861 File Offset: 0x00206A61
	public bool IsSuspended
	{
		get
		{
			return this.isSuspended;
		}
	}

	// Token: 0x0600599A RID: 22938 RVA: 0x00208869 File Offset: 0x00206A69
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600599B RID: 22939 RVA: 0x00208871 File Offset: 0x00206A71
	public void SetSuspended(bool state)
	{
		this.isSuspended = state;
	}

	// Token: 0x0600599C RID: 22940 RVA: 0x0020887C File Offset: 0x00206A7C
	public void ReleaseAstronaut(object data, bool applyBuff = false)
	{
		if (this.releasingAstronaut)
		{
			return;
		}
		this.releasingAstronaut = true;
		MinionStorage component = base.GetComponent<MinionStorage>();
		List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
		for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
		{
			MinionStorage.Info info = storedMinionInfo[i];
			GameObject gameObject = component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(base.smi.master.transform.GetPosition())));
			if (Grid.FakeFloor[Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1)])
			{
				gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
				if (applyBuff)
				{
					gameObject.GetComponent<Effects>().Add(Db.Get().effects.Get("SpaceTourist"), true);
					JoyBehaviourMonitor.Instance smi = gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
					if (smi != null)
					{
						smi.GoToOverjoyed();
					}
				}
			}
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x0600599D RID: 22941 RVA: 0x0020896C File Offset: 0x00206B6C
	public void OnSuspend(object data)
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null)
		{
			component.capacityKg = component.MassStored();
			component.allowItemRemoval = false;
		}
		if (base.GetComponent<ManualDeliveryKG>() != null)
		{
			UnityEngine.Object.Destroy(base.GetComponent<ManualDeliveryKG>());
		}
		this.SetSuspended(true);
	}

	// Token: 0x0600599E RID: 22942 RVA: 0x002089BC File Offset: 0x00206BBC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.storage = base.GetComponent<Storage>();
		this.assignable = base.GetComponent<Assignable>();
		base.smi.StartSM();
		int cell = Grid.PosToCell(base.gameObject);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("TouristModule.gantryChanged", base.gameObject, cell, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnGantryChanged));
		this.OnGantryChanged(null);
		base.Subscribe<TouristModule>(-1277991738, TouristModule.OnSuspendDelegate);
		base.Subscribe<TouristModule>(684616645, TouristModule.OnAssigneeChangedDelegate);
	}

	// Token: 0x0600599F RID: 22943 RVA: 0x00208A5C File Offset: 0x00206C5C
	private void OnGantryChanged(object data)
	{
		if (base.gameObject != null)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.HasGantry, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.MissingGantry, false);
			int i = Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1);
			if (Grid.FakeFloor[i])
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.HasGantry, null);
				return;
			}
			component.AddStatusItem(Db.Get().BuildingStatusItems.MissingGantry, null);
		}
	}

	// Token: 0x060059A0 RID: 22944 RVA: 0x00208B08 File Offset: 0x00206D08
	private Chore CreateWorkChore()
	{
		WorkChore<CommandModuleWorkable> workChore = new WorkChore<CommandModuleWorkable>(Db.Get().ChoreTypes.Astronaut, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, this.assignable);
		return workChore;
	}

	// Token: 0x060059A1 RID: 22945 RVA: 0x00208B60 File Offset: 0x00206D60
	private void OnAssigneeChanged(object data)
	{
		if (data == null && base.gameObject.HasTag(GameTags.RocketOnGround) && base.GetComponent<MinionStorage>().GetStoredMinionInfo().Count > 0)
		{
			this.ReleaseAstronaut(null, false);
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
	}

	// Token: 0x060059A2 RID: 22946 RVA: 0x00208BB8 File Offset: 0x00206DB8
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry.Clear();
		this.ReleaseAstronaut(null, false);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.smi.StopSM("cleanup");
	}

	// Token: 0x04003C05 RID: 15365
	public Storage storage;

	// Token: 0x04003C06 RID: 15366
	[Serialize]
	private bool isSuspended;

	// Token: 0x04003C07 RID: 15367
	private bool releasingAstronaut;

	// Token: 0x04003C08 RID: 15368
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

	// Token: 0x04003C09 RID: 15369
	public Assignable assignable;

	// Token: 0x04003C0A RID: 15370
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003C0B RID: 15371
	private static readonly EventSystem.IntraObjectHandler<TouristModule> OnSuspendDelegate = new EventSystem.IntraObjectHandler<TouristModule>(delegate(TouristModule component, object data)
	{
		component.OnSuspend(data);
	});

	// Token: 0x04003C0C RID: 15372
	private static readonly EventSystem.IntraObjectHandler<TouristModule> OnAssigneeChangedDelegate = new EventSystem.IntraObjectHandler<TouristModule>(delegate(TouristModule component, object data)
	{
		component.OnAssigneeChanged(data);
	});

	// Token: 0x02001D41 RID: 7489
	public class StatesInstance : GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.GameInstance
	{
		// Token: 0x0600B0A7 RID: 45223 RVA: 0x003DB5C4 File Offset: 0x003D97C4
		public StatesInstance(TouristModule smi) : base(smi)
		{
			smi.gameObject.Subscribe(-887025858, delegate(object data)
			{
				smi.SetSuspended(false);
				smi.ReleaseAstronaut(null, true);
				smi.assignable.Unassign();
			});
		}
	}

	// Token: 0x02001D42 RID: 7490
	public class States : GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule>
	{
		// Token: 0x0600B0A8 RID: 45224 RVA: 0x003DB60C File Offset: 0x003D980C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).GoTo(this.awaitingTourist);
			this.awaitingTourist.PlayAnim("grounded", KAnim.PlayMode.Loop).ToggleChore((TouristModule.StatesInstance smi) => smi.master.CreateWorkChore(), this.hasTourist);
			this.hasTourist.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.RocketLanded, this.idle, null).EventTransition(GameHashes.AssigneeChanged, this.idle, null);
		}

		// Token: 0x04008ACA RID: 35530
		public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State idle;

		// Token: 0x04008ACB RID: 35531
		public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State awaitingTourist;

		// Token: 0x04008ACC RID: 35532
		public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State hasTourist;
	}
}
