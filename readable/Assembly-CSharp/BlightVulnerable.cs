using System;

// Token: 0x02000889 RID: 2185
[SkipSaveFileSerialization]
public class BlightVulnerable : StateMachineComponent<BlightVulnerable.StatesInstance>
{
	// Token: 0x06003C24 RID: 15396 RVA: 0x00150C69 File Offset: 0x0014EE69
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003C25 RID: 15397 RVA: 0x00150C71 File Offset: 0x0014EE71
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003C26 RID: 15398 RVA: 0x00150C84 File Offset: 0x0014EE84
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x00150C8C File Offset: 0x0014EE8C
	public void MakeBlighted()
	{
		Debug.Log("Blighting plant", this);
		base.smi.sm.isBlighted.Set(true, base.smi, false);
	}

	// Token: 0x0400251A RID: 9498
	private SchedulerHandle handle;

	// Token: 0x0400251B RID: 9499
	public bool prefersDarkness;

	// Token: 0x02001857 RID: 6231
	public class StatesInstance : GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.GameInstance
	{
		// Token: 0x06009E9B RID: 40603 RVA: 0x003A3C99 File Offset: 0x003A1E99
		public StatesInstance(BlightVulnerable master) : base(master)
		{
		}
	}

	// Token: 0x02001858 RID: 6232
	public class States : GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable>
	{
		// Token: 0x06009E9C RID: 40604 RVA: 0x003A3CA4 File Offset: 0x003A1EA4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.comfortable;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.comfortable.ParamTransition<bool>(this.isBlighted, this.blighted, GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.IsTrue);
			this.blighted.TriggerOnEnter(GameHashes.BlightChanged, (BlightVulnerable.StatesInstance smi) => true).Enter(delegate(BlightVulnerable.StatesInstance smi)
			{
				smi.GetComponent<SeedProducer>().seedInfo.seedId = RotPileConfig.ID;
			}).ToggleTag(GameTags.Blighted).Exit(delegate(BlightVulnerable.StatesInstance smi)
			{
				GameplayEventManager.Instance.Trigger(-1425542080, smi.gameObject);
			});
		}

		// Token: 0x04007AA9 RID: 31401
		public StateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.BoolParameter isBlighted;

		// Token: 0x04007AAA RID: 31402
		public GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.State comfortable;

		// Token: 0x04007AAB RID: 31403
		public GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.State blighted;
	}
}
