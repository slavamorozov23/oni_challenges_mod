using System;
using Klei.AI;

// Token: 0x02000BDC RID: 3036
[SkipSaveFileSerialization]
public class StarryEyed : StateMachineComponent<StarryEyed.StatesInstance>
{
	// Token: 0x06005AED RID: 23277 RVA: 0x0020F181 File Offset: 0x0020D381
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x04003C93 RID: 15507
	private const string STARRY_EYED_EFFECT_NAME = "StarryEyed";

	// Token: 0x02001D6D RID: 7533
	public class StatesInstance : GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.GameInstance
	{
		// Token: 0x0600B12B RID: 45355 RVA: 0x003DC766 File Offset: 0x003DA966
		public StatesInstance(StarryEyed master) : base(master)
		{
		}

		// Token: 0x0600B12C RID: 45356 RVA: 0x003DC770 File Offset: 0x003DA970
		public bool IsInSpace()
		{
			WorldContainer myWorld = this.GetMyWorld();
			if (!myWorld)
			{
				return false;
			}
			int parentWorldId = myWorld.ParentWorldId;
			int id = myWorld.id;
			return myWorld.GetComponent<Clustercraft>() && parentWorldId == id;
		}
	}

	// Token: 0x02001D6E RID: 7534
	public class States : GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed>
	{
		// Token: 0x0600B12D RID: 45357 RVA: 0x003DC7B0 File Offset: 0x003DA9B0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Enter(delegate(StarryEyed.StatesInstance smi)
			{
				if (smi.IsInSpace())
				{
					smi.GoTo(this.inSpace);
				}
			});
			this.idle.EventTransition(GameHashes.MinionMigration, (StarryEyed.StatesInstance smi) => Game.Instance, this.inSpace, (StarryEyed.StatesInstance smi) => smi.IsInSpace()).Enter(delegate(StarryEyed.StatesInstance smi)
			{
				Effects component = smi.master.gameObject.GetComponent<Effects>();
				if (component != null && component.HasEffect("StarryEyed"))
				{
					component.Remove("StarryEyed");
				}
			});
			this.inSpace.EventTransition(GameHashes.MinionMigration, (StarryEyed.StatesInstance smi) => Game.Instance, this.idle, (StarryEyed.StatesInstance smi) => !smi.IsInSpace()).ToggleEffect("StarryEyed");
		}

		// Token: 0x04008B3B RID: 35643
		public GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State idle;

		// Token: 0x04008B3C RID: 35644
		public GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State inSpace;
	}
}
