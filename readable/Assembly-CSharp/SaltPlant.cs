using System;

// Token: 0x02000AA9 RID: 2729
public class SaltPlant : StateMachineComponent<SaltPlant.StatesInstance>
{
	// Token: 0x06004F18 RID: 20248 RVA: 0x001CB365 File Offset: 0x001C9565
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SaltPlant>(-724860998, SaltPlant.OnWiltDelegate);
		base.Subscribe<SaltPlant>(712767498, SaltPlant.OnWiltRecoverDelegate);
	}

	// Token: 0x06004F19 RID: 20249 RVA: 0x001CB38F File Offset: 0x001C958F
	private void OnWilt(object data = null)
	{
		base.gameObject.GetComponent<ElementConsumer>().EnableConsumption(false);
	}

	// Token: 0x06004F1A RID: 20250 RVA: 0x001CB3A2 File Offset: 0x001C95A2
	private void OnWiltRecover(object data = null)
	{
		base.gameObject.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x040034D9 RID: 13529
	private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltDelegate = new EventSystem.IntraObjectHandler<SaltPlant>(delegate(SaltPlant component, object data)
	{
		component.OnWilt(data);
	});

	// Token: 0x040034DA RID: 13530
	private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltRecoverDelegate = new EventSystem.IntraObjectHandler<SaltPlant>(delegate(SaltPlant component, object data)
	{
		component.OnWiltRecover(data);
	});

	// Token: 0x02001BD9 RID: 7129
	public class StatesInstance : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.GameInstance
	{
		// Token: 0x0600AB65 RID: 43877 RVA: 0x003C8770 File Offset: 0x003C6970
		public StatesInstance(SaltPlant master) : base(master)
		{
		}
	}

	// Token: 0x02001BDA RID: 7130
	public class States : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant>
	{
		// Token: 0x0600AB66 RID: 43878 RVA: 0x003C8779 File Offset: 0x003C6979
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.alive;
			this.alive.DoNothing();
		}

		// Token: 0x040085D7 RID: 34263
		public GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.State alive;
	}
}
