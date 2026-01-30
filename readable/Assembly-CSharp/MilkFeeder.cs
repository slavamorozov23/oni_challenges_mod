using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020007BC RID: 1980
public class MilkFeeder : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>
{
	// Token: 0x06003461 RID: 13409 RVA: 0x00128CCC File Offset: 0x00126ECC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.Enter(delegate(MilkFeeder.Instance smi)
		{
			smi.UpdateStorageMeter();
		}).EventHandler(GameHashes.OnStorageChange, delegate(MilkFeeder.Instance smi)
		{
			smi.UpdateStorageMeter();
		});
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (MilkFeeder.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).EventTransition(GameHashes.OperationalChanged, this.on.pst, (MilkFeeder.Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() != this.on.pre).EventTransition(GameHashes.OperationalChanged, this.off, (MilkFeeder.Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() == this.on.pre);
		this.on.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
		this.on.working.PlayAnim("on").DefaultState(this.on.working.empty);
		this.on.working.empty.PlayAnim("empty").EnterTransition(this.on.working.refilling, (MilkFeeder.Instance smi) => smi.HasEnoughMilkForOneFeeding()).EventHandler(GameHashes.OnStorageChange, delegate(MilkFeeder.Instance smi)
		{
			if (smi.HasEnoughMilkForOneFeeding())
			{
				smi.GoTo(this.on.working.refilling);
			}
		});
		this.on.working.refilling.PlayAnim("fill").OnAnimQueueComplete(this.on.working.full);
		this.on.working.full.PlayAnim("full").Enter(delegate(MilkFeeder.Instance smi)
		{
			this.isReadyToStartFeeding.Set(true, smi, false);
		}).Exit(delegate(MilkFeeder.Instance smi)
		{
			this.isReadyToStartFeeding.Set(false, smi, false);
		}).ParamTransition<DrinkMilkStates.Instance>(this.currentFeedingCritter, this.on.working.emptying, (MilkFeeder.Instance smi, DrinkMilkStates.Instance val) => val != null);
		this.on.working.emptying.EnterTransition(this.on.working.full, delegate(MilkFeeder.Instance smi)
		{
			DrinkMilkMonitor.Instance smi2 = this.currentFeedingCritter.Get(smi).GetSMI<DrinkMilkMonitor.Instance>();
			return smi2 != null && !smi2.def.consumesMilk;
		}).PlayAnim("emptying").OnAnimQueueComplete(this.on.working.empty).Exit(delegate(MilkFeeder.Instance smi)
		{
			smi.StopFeeding();
		});
		this.on.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x04001F99 RID: 8089
	private GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State off;

	// Token: 0x04001F9A RID: 8090
	private MilkFeeder.OnState on;

	// Token: 0x04001F9B RID: 8091
	public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.BoolParameter isReadyToStartFeeding;

	// Token: 0x04001F9C RID: 8092
	public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.ObjectParameter<DrinkMilkStates.Instance> currentFeedingCritter;

	// Token: 0x020016F1 RID: 5873
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009939 RID: 39225 RVA: 0x0038C1A4 File Offset: 0x0038A3A4
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			go.GetSMI<MilkFeeder.Instance>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(CREATURES.MODIFIERS.GOTMILK.NAME, "", Descriptor.DescriptorType.Effect);
			list.Add(item);
			Effect.AddModifierDescriptions(list, "HadMilk", true, "STRINGS.CREATURES.STATS.");
			return list;
		}
	}

	// Token: 0x020016F2 RID: 5874
	public class OnState : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
	{
		// Token: 0x0400763D RID: 30269
		public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pre;

		// Token: 0x0400763E RID: 30270
		public MilkFeeder.OnState.WorkingState working;

		// Token: 0x0400763F RID: 30271
		public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pst;

		// Token: 0x0200291C RID: 10524
		public class WorkingState : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
		{
			// Token: 0x0400B5B9 RID: 46521
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State empty;

			// Token: 0x0400B5BA RID: 46522
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State refilling;

			// Token: 0x0400B5BB RID: 46523
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State full;

			// Token: 0x0400B5BC RID: 46524
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State emptying;
		}
	}

	// Token: 0x020016F3 RID: 5875
	public new class Instance : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.GameInstance
	{
		// Token: 0x0600993C RID: 39228 RVA: 0x0038C204 File Offset: 0x0038A404
		public Instance(IStateMachineTarget master, MilkFeeder.Def def) : base(master, def)
		{
			this.milkStorage = base.GetComponent<Storage>();
			this.storageMeter = new MeterController(base.smi.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x0600993D RID: 39229 RVA: 0x0038C242 File Offset: 0x0038A442
		public override void StartSM()
		{
			base.StartSM();
			Components.MilkFeeders.Add(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x0600993E RID: 39230 RVA: 0x0038C260 File Offset: 0x0038A460
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Components.MilkFeeders.Remove(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x0600993F RID: 39231 RVA: 0x0038C27E File Offset: 0x0038A47E
		public void UpdateStorageMeter()
		{
			this.storageMeter.SetPositionPercent(1f - Mathf.Clamp01(this.milkStorage.RemainingCapacity() / this.milkStorage.capacityKg));
		}

		// Token: 0x06009940 RID: 39232 RVA: 0x0038C2AD File Offset: 0x0038A4AD
		public bool IsOperational()
		{
			return base.GetComponent<Operational>().IsOperational;
		}

		// Token: 0x06009941 RID: 39233 RVA: 0x0038C2BA File Offset: 0x0038A4BA
		public bool IsReserved()
		{
			return base.HasTag(GameTags.Creatures.ReservedByCreature);
		}

		// Token: 0x06009942 RID: 39234 RVA: 0x0038C2C8 File Offset: 0x0038A4C8
		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				global::Debug.Assert(!base.HasTag(GameTags.Creatures.ReservedByCreature));
				base.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
				return;
			}
			if (base.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a MilkFeeder that wasn't reserved", Array.Empty<object>());
		}

		// Token: 0x06009943 RID: 39235 RVA: 0x0038C335 File Offset: 0x0038A535
		public bool IsReadyToStartFeeding()
		{
			return this.IsOperational() && base.sm.isReadyToStartFeeding.Get(base.smi);
		}

		// Token: 0x06009944 RID: 39236 RVA: 0x0038C357 File Offset: 0x0038A557
		public void RequestToStartFeeding(DrinkMilkStates.Instance feedingCritter)
		{
			base.sm.currentFeedingCritter.Set(feedingCritter, base.smi, false);
		}

		// Token: 0x06009945 RID: 39237 RVA: 0x0038C374 File Offset: 0x0038A574
		public void StopFeeding()
		{
			DrinkMilkStates.Instance instance = base.sm.currentFeedingCritter.Get(base.smi);
			if (instance != null)
			{
				instance.RequestToStopFeeding();
			}
			base.sm.currentFeedingCritter.Set(null, base.smi, false);
		}

		// Token: 0x06009946 RID: 39238 RVA: 0x0038C3BA File Offset: 0x0038A5BA
		public bool HasEnoughMilkForOneFeeding()
		{
			return this.milkStorage.GetAmountAvailable(MilkFeederConfig.MILK_TAG) >= 5f;
		}

		// Token: 0x06009947 RID: 39239 RVA: 0x0038C3D6 File Offset: 0x0038A5D6
		public void ConsumeMilkForOneFeeding()
		{
			this.milkStorage.ConsumeIgnoringDisease(MilkFeederConfig.MILK_TAG, 5f);
		}

		// Token: 0x06009948 RID: 39240 RVA: 0x0038C3F0 File Offset: 0x0038A5F0
		public bool IsInCreaturePenRoom()
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			return roomOfGameObject != null && roomOfGameObject.roomType == Db.Get().RoomTypes.CreaturePen;
		}

		// Token: 0x04007640 RID: 30272
		public Storage milkStorage;

		// Token: 0x04007641 RID: 30273
		public MeterController storageMeter;
	}
}
