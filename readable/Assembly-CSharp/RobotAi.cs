using System;
using UnityEngine;

// Token: 0x0200048F RID: 1167
public class RobotAi : GameStateMachine<RobotAi, RobotAi.Instance>
{
	// Token: 0x060018CE RID: 6350 RVA: 0x00089A94 File Offset: 0x00087C94
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleStateMachine((RobotAi.Instance smi) => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())).Enter(delegate(RobotAi.Instance smi)
		{
			if (smi.HasTag(GameTags.Dead))
			{
				smi.GoTo(this.dead);
				return;
			}
			smi.GoTo(this.alive);
		});
		this.alive.DefaultState(this.alive.normal).TagTransition(GameTags.Dead, this.dead, false).Toggle("Toggle Component Registration", delegate(RobotAi.Instance smi)
		{
			RobotAi.ToggleRegistration(smi, true);
		}, delegate(RobotAi.Instance smi)
		{
			RobotAi.ToggleRegistration(smi, false);
		});
		this.alive.normal.TagTransition(GameTags.Stored, this.alive.stored, false).Enter(delegate(RobotAi.Instance smi)
		{
			if (!smi.HasTag(GameTags.Robots.Models.FetchDrone))
			{
				smi.fallMonitor = new FallMonitor.Instance(smi.master, false, null);
				smi.fallMonitor.StartSM();
			}
		}).Exit(delegate(RobotAi.Instance smi)
		{
			if (smi.fallMonitor != null)
			{
				smi.fallMonitor.StopSM("StoredRobotAI");
			}
		});
		this.alive.stored.PlayAnim("in_storage").TagTransition(GameTags.Stored, this.alive.normal, true).ToggleBrain("stored").Enter(delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Navigator>().Pause("stored");
		}).Exit(delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Navigator>().Unpause("unstored");
		});
		this.dead.ToggleBrain("dead").ToggleComponentIfFound<Deconstructable>(false).ToggleStateMachine((RobotAi.Instance smi) => new FallWhenDeadMonitor.Instance(smi.master)).Enter("RefreshUserMenu", delegate(RobotAi.Instance smi)
		{
			smi.RefreshUserMenu();
		}).Enter("DropStorage", delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
		}).Enter("Delete", new StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback(RobotAi.DeleteOnDeath));
	}

	// Token: 0x060018CF RID: 6351 RVA: 0x00089CE0 File Offset: 0x00087EE0
	public static void DeleteOnDeath(RobotAi.Instance smi)
	{
		if (((RobotAi.Def)smi.def).DeleteOnDead)
		{
			smi.gameObject.DeleteObject();
		}
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x00089CFF File Offset: 0x00087EFF
	private static void ToggleRegistration(RobotAi.Instance smi, bool register)
	{
		if (register)
		{
			Components.LiveRobotsIdentities.Add(smi);
			return;
		}
		Components.LiveRobotsIdentities.Remove(smi);
	}

	// Token: 0x04000E57 RID: 3671
	public RobotAi.AliveStates alive;

	// Token: 0x04000E58 RID: 3672
	public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x020012AF RID: 4783
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006889 RID: 26761
		public bool DeleteOnDead;
	}

	// Token: 0x020012B0 RID: 4784
	public class AliveStates : GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400688A RID: 26762
		public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x0400688B RID: 26763
		public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State stored;
	}

	// Token: 0x020012B1 RID: 4785
	public new class Instance : GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600890D RID: 35085 RVA: 0x0034FCA0 File Offset: 0x0034DEA0
		public Instance(IStateMachineTarget master, RobotAi.Def def) : base(master, def)
		{
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			component.AddUrge(Db.Get().Urges.EmoteHighPriority);
			component.AddUrge(Db.Get().Urges.EmoteIdle);
			this.onBeginChoreHandlerID = base.Subscribe(-1988963660, new Action<object>(this.OnBeginChore));
		}

		// Token: 0x0600890E RID: 35086 RVA: 0x0034FD04 File Offset: 0x0034DF04
		private void OnBeginChore(object data)
		{
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				component.DropAll(false, false, default(Vector3), true, null);
			}
		}

		// Token: 0x0600890F RID: 35087 RVA: 0x0034FD34 File Offset: 0x0034DF34
		protected override void OnCleanUp()
		{
			base.Unsubscribe(ref this.onBeginChoreHandlerID);
			base.OnCleanUp();
		}

		// Token: 0x06008910 RID: 35088 RVA: 0x0034FD48 File Offset: 0x0034DF48
		public void RefreshUserMenu()
		{
			Game.Instance.userMenu.Refresh(base.master.gameObject);
		}

		// Token: 0x0400688C RID: 26764
		public FallMonitor.Instance fallMonitor;

		// Token: 0x0400688D RID: 26765
		private int onBeginChoreHandlerID;
	}
}
