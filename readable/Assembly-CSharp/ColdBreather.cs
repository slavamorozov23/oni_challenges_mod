using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A9A RID: 2714
[SkipSaveFileSerialization]
public class ColdBreather : StateMachineComponent<ColdBreather.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06004EB8 RID: 20152 RVA: 0x001C9CA0 File Offset: 0x001C7EA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(ColdBreather.OnSimEmittedCallback), this, "ColdBreather");
		base.smi.StartSM();
	}

	// Token: 0x06004EB9 RID: 20153 RVA: 0x001C9CDA File Offset: 0x001C7EDA
	protected override void OnPrefabInit()
	{
		this.elementConsumer.EnableConsumption(false);
		base.Subscribe<ColdBreather>(1309017699, ColdBreather.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x06004EBA RID: 20154 RVA: 0x001C9D00 File Offset: 0x001C7F00
	private void OnReplanted(object data = null)
	{
		ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
		if (component == null)
		{
			return;
		}
		ElementConsumer component2 = base.GetComponent<ElementConsumer>();
		if (component.Replanted)
		{
			component2.consumptionRate = this.consumptionRate;
		}
		else
		{
			component2.consumptionRate = this.consumptionRate * 0.25f;
		}
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.emitRads = 480f;
			this.radiationEmitter.Refresh();
		}
	}

	// Token: 0x06004EBB RID: 20155 RVA: 0x001C9D78 File Offset: 0x001C7F78
	protected override void OnCleanUp()
	{
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "coldbreather");
		this.simEmitCBHandle.Clear();
		if (this.storage)
		{
			this.storage.DropAll(true, false, default(Vector3), true, null);
		}
		base.OnCleanUp();
	}

	// Token: 0x06004EBC RID: 20156 RVA: 0x001C9DD6 File Offset: 0x001C7FD6
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06004EBD RID: 20157 RVA: 0x001C9DEE File Offset: 0x001C7FEE
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.COLDBREATHER, UI.GAMEOBJECTEFFECTS.TOOLTIPS.COLDBREATHER, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x06004EBE RID: 20158 RVA: 0x001C9E16 File Offset: 0x001C8016
	private void SetEmitting(bool emitting)
	{
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.SetEmitting(emitting);
		}
	}

	// Token: 0x06004EBF RID: 20159 RVA: 0x001C9E34 File Offset: 0x001C8034
	private void Exhale()
	{
		if (this.lastEmitTag != Tag.Invalid)
		{
			return;
		}
		this.gases.Clear();
		this.storage.Find(GameTags.Gas, this.gases);
		if (this.nextGasEmitIndex >= this.gases.Count)
		{
			this.nextGasEmitIndex = 0;
		}
		while (this.nextGasEmitIndex < this.gases.Count)
		{
			int num = this.nextGasEmitIndex;
			this.nextGasEmitIndex = num + 1;
			int index = num;
			PrimaryElement component = this.gases[index].GetComponent<PrimaryElement>();
			if (component != null && component.Mass > 0f && this.simEmitCBHandle.IsValid())
			{
				float temperature = Mathf.Max(component.Element.lowTemp + 5f, component.Temperature + this.deltaEmitTemperature);
				int gameCell = Grid.PosToCell(base.transform.GetPosition() + this.emitOffsetCell);
				ushort idx = component.Element.idx;
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				SimMessages.EmitMass(gameCell, idx, component.Mass, temperature, component.DiseaseIdx, component.DiseaseCount, this.simEmitCBHandle.index);
				this.lastEmitTag = component.Element.tag;
				return;
			}
		}
	}

	// Token: 0x06004EC0 RID: 20160 RVA: 0x001C9F97 File Offset: 0x001C8197
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((ColdBreather)data).OnSimEmitted(info);
	}

	// Token: 0x06004EC1 RID: 20161 RVA: 0x001C9FA8 File Offset: 0x001C81A8
	private void OnSimEmitted(Sim.MassEmittedCallback info)
	{
		if (info.suceeded == 1 && this.storage && this.lastEmitTag.IsValid)
		{
			this.storage.ConsumeIgnoringDisease(this.lastEmitTag, info.mass);
		}
		this.lastEmitTag = Tag.Invalid;
	}

	// Token: 0x04003483 RID: 13443
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04003484 RID: 13444
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04003485 RID: 13445
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003486 RID: 13446
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04003487 RID: 13447
	[MyCmpGet]
	private RadiationEmitter radiationEmitter;

	// Token: 0x04003488 RID: 13448
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x04003489 RID: 13449
	private const float EXHALE_PERIOD = 1f;

	// Token: 0x0400348A RID: 13450
	public float consumptionRate;

	// Token: 0x0400348B RID: 13451
	public float deltaEmitTemperature = -5f;

	// Token: 0x0400348C RID: 13452
	public Vector3 emitOffsetCell = new Vector3(0f, 0f);

	// Token: 0x0400348D RID: 13453
	private List<GameObject> gases = new List<GameObject>();

	// Token: 0x0400348E RID: 13454
	private Tag lastEmitTag;

	// Token: 0x0400348F RID: 13455
	private int nextGasEmitIndex;

	// Token: 0x04003490 RID: 13456
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x04003491 RID: 13457
	private static readonly EventSystem.IntraObjectHandler<ColdBreather> OnReplantedDelegate = new EventSystem.IntraObjectHandler<ColdBreather>(delegate(ColdBreather component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x02001BB6 RID: 7094
	public class StatesInstance : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.GameInstance
	{
		// Token: 0x0600AACC RID: 43724 RVA: 0x003C56F5 File Offset: 0x003C38F5
		public StatesInstance(ColdBreather master) : base(master)
		{
		}
	}

	// Token: 0x02001BB7 RID: 7095
	public class States : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather>
	{
		// Token: 0x0600AACD RID: 43725 RVA: 0x003C5700 File Offset: 0x003C3900
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.grow;
			this.statusItemCooling = new StatusItem("cooling", CREATURES.STATUSITEMS.COOLING.NAME, CREATURES.STATUSITEMS.COOLING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(ColdBreather.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature).Update(delegate(ColdBreather.StatesInstance smi, float dt)
			{
				smi.master.Exhale();
			}, UpdateRate.SIM_200ms, false);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (ColdBreather.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop).ToggleMainStatusItem(this.statusItemCooling, null).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
				smi.master.SetEmitting(true);
			}).Exit(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
				smi.master.SetEmitting(false);
			});
			this.alive.wilting.PlayAnim("wilt1").EventTransition(GameHashes.WiltRecover, this.alive.mature, (ColdBreather.StatesInstance smi) => !smi.master.wiltCondition.IsWilting()).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.SetEmitting(false);
			});
		}

		// Token: 0x04008589 RID: 34185
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State grow;

		// Token: 0x0400858A RID: 34186
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State blocked_from_growing;

		// Token: 0x0400858B RID: 34187
		public ColdBreather.States.AliveStates alive;

		// Token: 0x0400858C RID: 34188
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State dead;

		// Token: 0x0400858D RID: 34189
		private StatusItem statusItemCooling;

		// Token: 0x020029F0 RID: 10736
		public class AliveStates : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.PlantAliveSubState
		{
			// Token: 0x0400B95F RID: 47455
			public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State mature;

			// Token: 0x0400B960 RID: 47456
			public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State wilting;
		}
	}
}
