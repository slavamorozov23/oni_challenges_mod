using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000AA1 RID: 2721
public class Oxyfern : StateMachineComponent<Oxyfern.StatesInstance>
{
	// Token: 0x06004EE5 RID: 20197 RVA: 0x001CA75B File Offset: 0x001C895B
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06004EE6 RID: 20198 RVA: 0x001CA773 File Offset: 0x001C8973
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004EE7 RID: 20199 RVA: 0x001CA786 File Offset: 0x001C8986
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (Tutorial.Instance.oxygenGenerators.Contains(base.gameObject))
		{
			Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		}
	}

	// Token: 0x06004EE8 RID: 20200 RVA: 0x001CA7BB File Offset: 0x001C89BB
	protected override void OnPrefabInit()
	{
		base.Subscribe<Oxyfern>(1309017699, Oxyfern.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x06004EE9 RID: 20201 RVA: 0x001CA7D4 File Offset: 0x001C89D4
	private void OnReplanted(object data = null)
	{
		this.SetConsumptionRate();
		if (this.receptacleMonitor.Replanted)
		{
			Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
		}
	}

	// Token: 0x06004EEA RID: 20202 RVA: 0x001CA7FE File Offset: 0x001C89FE
	public void SetConsumptionRate()
	{
		if (this.receptacleMonitor.Replanted)
		{
			this.elementConsumer.consumptionRate = 0.00062500004f;
			return;
		}
		this.elementConsumer.consumptionRate = 0.00015625001f;
	}

	// Token: 0x040034BF RID: 13503
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x040034C0 RID: 13504
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x040034C1 RID: 13505
	[MyCmpReq]
	private ElementConverter elementConverter;

	// Token: 0x040034C2 RID: 13506
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x040034C3 RID: 13507
	private static readonly EventSystem.IntraObjectHandler<Oxyfern> OnReplantedDelegate = new EventSystem.IntraObjectHandler<Oxyfern>(delegate(Oxyfern component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x02001BC5 RID: 7109
	public class StatesInstance : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.GameInstance
	{
		// Token: 0x0600AAFF RID: 43775 RVA: 0x003C6F8B File Offset: 0x003C518B
		public StatesInstance(Oxyfern master) : base(master)
		{
		}
	}

	// Token: 0x02001BC6 RID: 7110
	public class States : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern>
	{
		// Token: 0x0600AB00 RID: 43776 RVA: 0x003C6F94 File Offset: 0x003C5194
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.grow;
			GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(Oxyfern.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(Oxyfern.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_pst", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (Oxyfern.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle_full", KAnim.PlayMode.Loop).Enter(delegate(Oxyfern.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit(delegate(Oxyfern.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			});
			this.alive.wilting.PlayAnim("wilt3").EventTransition(GameHashes.WiltRecover, this.alive.mature, (Oxyfern.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x040085A9 RID: 34217
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State grow;

		// Token: 0x040085AA RID: 34218
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State blocked_from_growing;

		// Token: 0x040085AB RID: 34219
		public Oxyfern.States.AliveStates alive;

		// Token: 0x040085AC RID: 34220
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State dead;

		// Token: 0x02002A01 RID: 10753
		public class AliveStates : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.PlantAliveSubState
		{
			// Token: 0x0400B9B6 RID: 47542
			public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State mature;

			// Token: 0x0400B9B7 RID: 47543
			public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State wilting;
		}
	}
}
