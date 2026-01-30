using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000B77 RID: 2935
[SerializationConfig(MemberSerialization.OptIn)]
public class CommandModule : StateMachineComponent<CommandModule.StatesInstance>
{
	// Token: 0x06005742 RID: 22338 RVA: 0x001FBCEC File Offset: 0x001F9EEC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rocketStats = new RocketStats(this);
		this.conditions = base.GetComponent<CommandConditions>();
	}

	// Token: 0x06005743 RID: 22339 RVA: 0x001FBD0C File Offset: 0x001F9F0C
	public void ReleaseAstronaut(bool fill_bladder)
	{
		if (this.releasingAstronaut || this.robotPilotControlled)
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
			if (!(gameObject == null))
			{
				if (Grid.FakeFloor[Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1)])
				{
					gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
				}
				if (fill_bladder)
				{
					AmountInstance amountInstance = Db.Get().Amounts.Bladder.Lookup(gameObject);
					if (amountInstance != null)
					{
						amountInstance.value = amountInstance.GetMax();
					}
				}
			}
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x06005744 RID: 22340 RVA: 0x001FBE00 File Offset: 0x001FA000
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.storage = base.GetComponent<Storage>();
		if (!this.robotPilotControlled)
		{
			this.assignable = base.GetComponent<Assignable>();
			this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAssignTo));
			int cell = Grid.PosToCell(base.gameObject);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("CommandModule.gantryChanged", base.gameObject, cell, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnGantryChanged));
			this.OnGantryChanged(null);
		}
		base.smi.StartSM();
	}

	// Token: 0x06005745 RID: 22341 RVA: 0x001FBE9C File Offset: 0x001FA09C
	private bool CanAssignTo(MinionAssignablesProxy worker)
	{
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			return minionIdentity.GetComponent<MinionResume>().HasPerk(Db.Get().SkillPerks.CanUseRockets);
		}
		StoredMinionIdentity storedMinionIdentity = worker.target as StoredMinionIdentity;
		if (storedMinionIdentity != null)
		{
			if (storedMinionIdentity.model == BionicMinionConfig.MODEL)
			{
				MinionStorageDataHolder component = storedMinionIdentity.GetComponent<MinionStorageDataHolder>();
				if (component != null)
				{
					MinionStorageDataHolder.DataPack dataPack = component.GetDataPack<BionicUpgradesMonitor.Instance>();
					if (dataPack != null)
					{
						MinionStorageDataHolder.DataPackData dataPackData = dataPack.PeekData();
						if (dataPackData != null && dataPackData.Tags != null)
						{
							Tag[] tags = dataPackData.Tags;
							for (int i = 0; i < tags.Length; i++)
							{
								if (tags[i] == "Booster_PilotVanilla1")
								{
									return true;
								}
							}
						}
					}
				}
			}
			return storedMinionIdentity.HasPerk(Db.Get().SkillPerks.CanUseRockets);
		}
		return false;
	}

	// Token: 0x06005746 RID: 22342 RVA: 0x001FBF78 File Offset: 0x001FA178
	private static bool HasValidGantry(GameObject go)
	{
		int num = Grid.OffsetCell(Grid.PosToCell(go), 0, -1);
		return Grid.IsValidCell(num) && Grid.FakeFloor[num];
	}

	// Token: 0x06005747 RID: 22343 RVA: 0x001FBFA8 File Offset: 0x001FA1A8
	private void OnGantryChanged(object data)
	{
		if (base.gameObject != null)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.HasGantry, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.MissingGantry, false);
			if (CommandModule.HasValidGantry(base.smi.master.gameObject))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.HasGantry, null);
			}
			else
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.MissingGantry, null);
			}
			base.smi.sm.gantryChanged.Trigger(base.smi);
		}
	}

	// Token: 0x06005748 RID: 22344 RVA: 0x001FC060 File Offset: 0x001FA260
	private Chore CreateWorkChore()
	{
		WorkChore<CommandModuleWorkable> workChore = new WorkChore<CommandModuleWorkable>(Db.Get().ChoreTypes.Astronaut, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanUseRockets);
		workChore.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, this.assignable);
		return workChore;
	}

	// Token: 0x06005749 RID: 22345 RVA: 0x001FC0D6 File Offset: 0x001FA2D6
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry.Clear();
		this.ReleaseAstronaut(false);
		base.smi.StopSM("cleanup");
	}

	// Token: 0x04003AB4 RID: 15028
	public Storage storage;

	// Token: 0x04003AB5 RID: 15029
	public RocketStats rocketStats;

	// Token: 0x04003AB6 RID: 15030
	public CommandConditions conditions;

	// Token: 0x04003AB7 RID: 15031
	private bool releasingAstronaut;

	// Token: 0x04003AB8 RID: 15032
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

	// Token: 0x04003AB9 RID: 15033
	public Assignable assignable;

	// Token: 0x04003ABA RID: 15034
	public bool robotPilotControlled;

	// Token: 0x04003ABB RID: 15035
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x02001CF7 RID: 7415
	public class StatesInstance : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.GameInstance
	{
		// Token: 0x0600AF5E RID: 44894 RVA: 0x003D5D6E File Offset: 0x003D3F6E
		public StatesInstance(CommandModule master) : base(master)
		{
		}

		// Token: 0x0600AF5F RID: 44895 RVA: 0x003D5D78 File Offset: 0x003D3F78
		public void SetSuspended(bool suspended)
		{
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				component.allowItemRemoval = !suspended;
			}
			ManualDeliveryKG component2 = base.GetComponent<ManualDeliveryKG>();
			if (component2 != null)
			{
				component2.Pause(suspended, "Rocket is suspended");
			}
		}

		// Token: 0x0600AF60 RID: 44896 RVA: 0x003D5DBC File Offset: 0x003D3FBC
		public bool CheckStoredMinionIsAssignee()
		{
			if (base.smi.master.robotPilotControlled)
			{
				return true;
			}
			foreach (MinionStorage.Info info in base.GetComponent<MinionStorage>().GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					KPrefabID kprefabID = info.serializedMinion.Get();
					if (!(kprefabID == null))
					{
						StoredMinionIdentity component = kprefabID.GetComponent<StoredMinionIdentity>();
						if (base.GetComponent<Assignable>().assignee == component.assignableProxy.Get())
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	// Token: 0x02001CF8 RID: 7416
	public class States : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule>
	{
		// Token: 0x0600AF61 RID: 44897 RVA: 0x003D5E68 File Offset: 0x003D4068
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grounded;
			this.grounded.PlayAnim("grounded", KAnim.PlayMode.Loop).DefaultState(this.grounded.awaitingAstronaut).TagTransition(GameTags.RocketNotOnGround, this.spaceborne, false);
			this.grounded.refreshChore.GoTo(this.grounded.awaitingAstronaut);
			this.grounded.awaitingAstronaut.Enter(delegate(CommandModule.StatesInstance smi)
			{
				if (smi.CheckStoredMinionIsAssignee())
				{
					smi.GoTo(this.grounded.hasAstronaut);
				}
				Game.Instance.userMenu.Refresh(smi.gameObject);
			}).EventHandler(GameHashes.AssigneeChanged, delegate(CommandModule.StatesInstance smi)
			{
				if (smi.CheckStoredMinionIsAssignee())
				{
					smi.GoTo(this.grounded.hasAstronaut);
				}
				else
				{
					smi.GoTo(this.grounded.refreshChore);
				}
				Game.Instance.userMenu.Refresh(smi.gameObject);
			}).ToggleChore((CommandModule.StatesInstance smi) => smi.master.CreateWorkChore(), this.grounded.hasAstronaut);
			this.grounded.hasAstronaut.EventHandler(GameHashes.AssigneeChanged, delegate(CommandModule.StatesInstance smi)
			{
				if (!smi.CheckStoredMinionIsAssignee())
				{
					smi.GoTo(this.grounded.waitingToRelease);
				}
			});
			this.grounded.waitingToRelease.ToggleStatusItem(Db.Get().BuildingStatusItems.DisembarkingDuplicant, null).OnSignal(this.gantryChanged, this.grounded.awaitingAstronaut, delegate(CommandModule.StatesInstance smi, StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.SignalParameter param)
			{
				if (CommandModule.HasValidGantry(smi.gameObject))
				{
					smi.master.ReleaseAstronaut(this.accumulatedPee.Get(smi));
					this.accumulatedPee.Set(false, smi, false);
					Game.Instance.userMenu.Refresh(smi.gameObject);
					return true;
				}
				return false;
			});
			this.spaceborne.DefaultState(this.spaceborne.launch);
			this.spaceborne.launch.Enter(delegate(CommandModule.StatesInstance smi)
			{
				smi.SetSuspended(true);
			}).GoTo(this.spaceborne.idle);
			this.spaceborne.idle.TagTransition(GameTags.RocketNotOnGround, this.spaceborne.land, true);
			this.spaceborne.land.Enter(delegate(CommandModule.StatesInstance smi)
			{
				smi.SetSuspended(false);
				Game.Instance.userMenu.Refresh(smi.gameObject);
				this.accumulatedPee.Set(true, smi, false);
			}).GoTo(this.grounded.waitingToRelease);
		}

		// Token: 0x040089DA RID: 35290
		public StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.Signal gantryChanged;

		// Token: 0x040089DB RID: 35291
		public StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.BoolParameter accumulatedPee;

		// Token: 0x040089DC RID: 35292
		public CommandModule.States.GroundedStates grounded;

		// Token: 0x040089DD RID: 35293
		public CommandModule.States.SpaceborneStates spaceborne;

		// Token: 0x02002A2C RID: 10796
		public class GroundedStates : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State
		{
			// Token: 0x0400BA5F RID: 47711
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State refreshChore;

			// Token: 0x0400BA60 RID: 47712
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State awaitingAstronaut;

			// Token: 0x0400BA61 RID: 47713
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State hasAstronaut;

			// Token: 0x0400BA62 RID: 47714
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State waitingToRelease;
		}

		// Token: 0x02002A2D RID: 10797
		public class SpaceborneStates : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State
		{
			// Token: 0x0400BA63 RID: 47715
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State launch;

			// Token: 0x0400BA64 RID: 47716
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State idle;

			// Token: 0x0400BA65 RID: 47717
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State land;
		}
	}
}
