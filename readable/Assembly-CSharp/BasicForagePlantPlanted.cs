using System;
using UnityEngine;

// Token: 0x02000A97 RID: 2711
public class BasicForagePlantPlanted : StateMachineComponent<BasicForagePlantPlanted.StatesInstance>
{
	// Token: 0x06004EAA RID: 20138 RVA: 0x001C9B1B File Offset: 0x001C7D1B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004EAB RID: 20139 RVA: 0x001C9B2E File Offset: 0x001C7D2E
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x0400347A RID: 13434
	[MyCmpReq]
	private Harvestable harvestable;

	// Token: 0x0400347B RID: 13435
	[MyCmpReq]
	private SeedProducer seedProducer;

	// Token: 0x0400347C RID: 13436
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x02001BB1 RID: 7089
	public class StatesInstance : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.GameInstance
	{
		// Token: 0x0600AABF RID: 43711 RVA: 0x003C524D File Offset: 0x003C344D
		public StatesInstance(BasicForagePlantPlanted smi) : base(smi)
		{
		}
	}

	// Token: 0x02001BB2 RID: 7090
	public class States : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted>
	{
		// Token: 0x0600AAC0 RID: 43712 RVA: 0x003C5258 File Offset: 0x003C3458
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.seed_grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.seed_grow.PlayAnim("idle", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive.idle, null);
			this.alive.InitializeStates(this.masterTarget, this.dead);
			this.alive.idle.PlayAnim("idle").EventTransition(GameHashes.Harvest, this.alive.harvest, null).Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				smi.master.harvestable.SetCanBeHarvested(true);
			});
			this.alive.harvest.Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				smi.master.seedProducer.DropSeed(null);
			}).GoTo(this.dead);
			this.dead.Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.animController.StopAndClear();
				UnityEngine.Object.Destroy(smi.master.animController);
				smi.master.DestroySelf(null);
			});
		}

		// Token: 0x04008581 RID: 34177
		public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State seed_grow;

		// Token: 0x04008582 RID: 34178
		public BasicForagePlantPlanted.States.AliveStates alive;

		// Token: 0x04008583 RID: 34179
		public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State dead;

		// Token: 0x020029EC RID: 10732
		public class AliveStates : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.PlantAliveSubState
		{
			// Token: 0x0400B94D RID: 47437
			public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State idle;

			// Token: 0x0400B94E RID: 47438
			public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State harvest;
		}
	}
}
