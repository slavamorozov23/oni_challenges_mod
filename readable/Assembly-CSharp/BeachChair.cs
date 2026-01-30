using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020006E6 RID: 1766
public class BeachChair : StateMachineComponent<BeachChair.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06002B92 RID: 11154 RVA: 0x000FE3DD File Offset: 0x000FC5DD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06002B93 RID: 11155 RVA: 0x000FE3F0 File Offset: 0x000FC5F0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06002B94 RID: 11156 RVA: 0x000FE3F8 File Offset: 0x000FC5F8
	public static void AddModifierDescriptions(List<Descriptor> descs, string effect_id, bool high_lux)
	{
		Klei.AI.Modifier modifier = Db.Get().effects.Get(effect_id);
		LocString locString = high_lux ? BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_HIGH : BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_LOW;
		LocString locString2 = high_lux ? BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_HIGH_TOOLTIP : BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_LOW_TOOLTIP;
		foreach (AttributeModifier attributeModifier in modifier.SelfModifiers)
		{
			Descriptor item = new Descriptor(locString.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()).Replace("{lux}", GameUtil.GetFormattedLux(BeachChairConfig.TAN_LUX)), locString2.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()).Replace("{lux}", GameUtil.GetFormattedLux(BeachChairConfig.TAN_LUX)), Descriptor.DescriptorType.Effect, false);
			item.IncreaseIndent();
			descs.Add(item);
		}
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x000FE538 File Offset: 0x000FC738
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		BeachChair.AddModifierDescriptions(list, this.specificEffectLit, true);
		BeachChair.AddModifierDescriptions(list, this.specificEffectUnlit, false);
		return list;
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x000FE585 File Offset: 0x000FC785
	public void SetLit(bool v)
	{
		base.smi.sm.lit.Set(v, base.smi, false);
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x000FE5A5 File Offset: 0x000FC7A5
	public void SetWorker(WorkerBase worker)
	{
		base.smi.sm.worker.Set(worker, base.smi);
	}

	// Token: 0x040019F5 RID: 6645
	public string specificEffectUnlit;

	// Token: 0x040019F6 RID: 6646
	public string specificEffectLit;

	// Token: 0x040019F7 RID: 6647
	public string trackingEffect;

	// Token: 0x040019F8 RID: 6648
	public const float LIT_RATIO_FOR_POSITIVE_EFFECT = 0.75f;

	// Token: 0x0200159E RID: 5534
	public class States : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair>
	{
		// Token: 0x060093E6 RID: 37862 RVA: 0x00376EA8 File Offset: 0x003750A8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.MissingRequirements, null);
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<BeachChair.StatesInstance, Chore>(this.CreateChore), this.inoperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.working_pre);
			this.ready.working_pre.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Target(this.worker).PlayAnim("working_pre").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_lit);
					return;
				}
				smi.GoTo(this.ready.working_unlit);
			});
			this.ready.working_unlit.DefaultState(this.ready.working_unlit.working).Enter(delegate(BeachChair.StatesInstance smi)
			{
				BeachChairWorkable component = smi.master.GetComponent<BeachChairWorkable>();
				component.workingPstComplete = (component.workingPstFailed = this.UNLIT_PST_ANIMS);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.TanningLightInsufficient, null).WorkableStopTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.post).Target(this.worker).PlayAnim("working_unlit_pre");
			this.ready.working_unlit.working.ParamTransition<bool>(this.lit, this.ready.working_unlit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsTrue).Target(this.worker).QueueAnim("working_unlit_loop", true, null);
			this.ready.working_unlit.post.Target(this.worker).PlayAnim("working_unlit_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_lit);
					return;
				}
				smi.GoTo(this.ready.working_unlit.working);
			});
			this.ready.working_lit.DefaultState(this.ready.working_lit.working).Enter(delegate(BeachChair.StatesInstance smi)
			{
				BeachChairWorkable component = smi.master.GetComponent<BeachChairWorkable>();
				component.workingPstComplete = (component.workingPstFailed = this.LIT_PST_ANIMS);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.TanningLightSufficient, null).WorkableStopTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.post).Target(this.worker).PlayAnim("working_lit_pre");
			this.ready.working_lit.working.ParamTransition<bool>(this.lit, this.ready.working_lit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsFalse).Target(this.worker).QueueAnim("working_lit_loop", true, null).ScheduleGoTo((BeachChair.StatesInstance smi) => UnityEngine.Random.Range(5f, 15f), this.ready.working_lit.silly);
			this.ready.working_lit.silly.ParamTransition<bool>(this.lit, this.ready.working_lit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsFalse).Target(this.worker).PlayAnim((BeachChair.StatesInstance smi) => this.SILLY_ANIMS[UnityEngine.Random.Range(0, this.SILLY_ANIMS.Length)], KAnim.PlayMode.Once).OnAnimQueueComplete(this.ready.working_lit.working);
			this.ready.working_lit.post.Target(this.worker).PlayAnim("working_lit_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (!this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_unlit);
					return;
				}
				smi.GoTo(this.ready.working_lit.working);
			});
			this.ready.post.PlayAnim("working_pst").Exit(delegate(BeachChair.StatesInstance smi)
			{
				this.worker.Set(null, smi);
			}).OnAnimQueueComplete(this.ready);
		}

		// Token: 0x060093E7 RID: 37863 RVA: 0x003772C4 File Offset: 0x003754C4
		private Chore CreateChore(BeachChair.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<BeachChairWorkable>();
			WorkChore<BeachChairWorkable> workChore = new WorkChore<BeachChairWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x04007248 RID: 29256
		public StateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.BoolParameter lit;

		// Token: 0x04007249 RID: 29257
		public StateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.TargetParameter worker;

		// Token: 0x0400724A RID: 29258
		private GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State inoperational;

		// Token: 0x0400724B RID: 29259
		private BeachChair.States.ReadyStates ready;

		// Token: 0x0400724C RID: 29260
		private HashedString[] UNLIT_PST_ANIMS = new HashedString[]
		{
			"working_unlit_pst",
			"working_pst"
		};

		// Token: 0x0400724D RID: 29261
		private HashedString[] LIT_PST_ANIMS = new HashedString[]
		{
			"working_lit_pst",
			"working_pst"
		};

		// Token: 0x0400724E RID: 29262
		private string[] SILLY_ANIMS = new string[]
		{
			"working_lit_loop1",
			"working_lit_loop2",
			"working_lit_loop3"
		};

		// Token: 0x020028BC RID: 10428
		public class LitWorkingStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x0400B364 RID: 45924
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working;

			// Token: 0x0400B365 RID: 45925
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State silly;

			// Token: 0x0400B366 RID: 45926
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}

		// Token: 0x020028BD RID: 10429
		public class WorkingStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x0400B367 RID: 45927
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working;

			// Token: 0x0400B368 RID: 45928
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}

		// Token: 0x020028BE RID: 10430
		public class ReadyStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x0400B369 RID: 45929
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State idle;

			// Token: 0x0400B36A RID: 45930
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working_pre;

			// Token: 0x0400B36B RID: 45931
			public BeachChair.States.WorkingStates working_unlit;

			// Token: 0x0400B36C RID: 45932
			public BeachChair.States.LitWorkingStates working_lit;

			// Token: 0x0400B36D RID: 45933
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}
	}

	// Token: 0x0200159F RID: 5535
	public class StatesInstance : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.GameInstance
	{
		// Token: 0x060093F0 RID: 37872 RVA: 0x003774DA File Offset: 0x003756DA
		public StatesInstance(BeachChair smi) : base(smi)
		{
		}
	}
}
