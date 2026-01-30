using System;

// Token: 0x020009F3 RID: 2547
[SkipSaveFileSerialization]
public class Loner : StateMachineComponent<Loner.StatesInstance>
{
	// Token: 0x06004A5C RID: 19036 RVA: 0x001AEF54 File Offset: 0x001AD154
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001A55 RID: 6741
	public class StatesInstance : GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.GameInstance
	{
		// Token: 0x0600A53E RID: 42302 RVA: 0x003B6154 File Offset: 0x003B4354
		public StatesInstance(Loner master) : base(master)
		{
		}

		// Token: 0x0600A53F RID: 42303 RVA: 0x003B6160 File Offset: 0x003B4360
		public bool IsAlone()
		{
			WorldContainer myWorld = this.GetMyWorld();
			if (!myWorld)
			{
				return false;
			}
			int parentWorldId = myWorld.ParentWorldId;
			int id = myWorld.id;
			MinionIdentity component = base.GetComponent<MinionIdentity>();
			foreach (object obj in Components.LiveMinionIdentities)
			{
				MinionIdentity minionIdentity = (MinionIdentity)obj;
				if (component != minionIdentity)
				{
					int myWorldId = minionIdentity.GetMyWorldId();
					if (id == myWorldId || parentWorldId == myWorldId)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	// Token: 0x02001A56 RID: 6742
	public class States : GameStateMachine<Loner.States, Loner.StatesInstance, Loner>
	{
		// Token: 0x0600A540 RID: 42304 RVA: 0x003B6208 File Offset: 0x003B4408
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Enter(delegate(Loner.StatesInstance smi)
			{
				if (smi.IsAlone())
				{
					smi.GoTo(this.alone);
				}
			});
			this.idle.EventTransition(GameHashes.MinionMigration, (Loner.StatesInstance smi) => Game.Instance, this.alone, (Loner.StatesInstance smi) => smi.IsAlone()).EventTransition(GameHashes.MinionDelta, (Loner.StatesInstance smi) => Game.Instance, this.alone, (Loner.StatesInstance smi) => smi.IsAlone());
			this.alone.EventTransition(GameHashes.MinionMigration, (Loner.StatesInstance smi) => Game.Instance, this.idle, (Loner.StatesInstance smi) => !smi.IsAlone()).EventTransition(GameHashes.MinionDelta, (Loner.StatesInstance smi) => Game.Instance, this.idle, (Loner.StatesInstance smi) => !smi.IsAlone()).ToggleEffect("Loner");
		}

		// Token: 0x04008166 RID: 33126
		public GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State idle;

		// Token: 0x04008167 RID: 33127
		public GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State alone;
	}
}
