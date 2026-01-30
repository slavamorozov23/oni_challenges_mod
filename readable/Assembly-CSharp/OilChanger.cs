using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007CE RID: 1998
public class OilChanger : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>
{
	// Token: 0x060034F8 RID: 13560 RVA: 0x0012C0E0 File Offset: 0x0012A2E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.inoperational;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.UpdateStorageMeter));
		this.inoperational.PlayAnim("off").Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.LED_Off)).Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.UpdateStorageMeter)).TagTransition(GameTags.Operational, this.operational, false);
		this.operational.PlayAnim("on").Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.UpdateStorageMeter)).TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.operational.oilNeeded);
		this.operational.oilNeeded.Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.LED_Off)).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, null).EventTransition(GameHashes.OnStorageChange, this.operational.ready, new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.Transition.ConditionCallback(OilChanger.HasEnoughLubricant));
		this.operational.ready.Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.LED_On)).ToggleChore(new Func<OilChanger.Instance, Chore>(OilChanger.CreateChore), this.operational.oilNeeded);
	}

	// Token: 0x060034F9 RID: 13561 RVA: 0x0012C22C File Offset: 0x0012A42C
	public static bool HasEnoughLubricant(OilChanger.Instance smi)
	{
		return smi.OilAmount >= smi.def.MIN_LUBRICANT_MASS_TO_WORK;
	}

	// Token: 0x060034FA RID: 13562 RVA: 0x0012C244 File Offset: 0x0012A444
	private static bool IsOperational(OilChanger.Instance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x060034FB RID: 13563 RVA: 0x0012C24C File Offset: 0x0012A44C
	public static void UpdateStorageMeter(OilChanger.Instance smi)
	{
		smi.UpdateStorageMeter();
	}

	// Token: 0x060034FC RID: 13564 RVA: 0x0012C254 File Offset: 0x0012A454
	public static void LED_On(OilChanger.Instance smi)
	{
		smi.SetLEDState(true);
	}

	// Token: 0x060034FD RID: 13565 RVA: 0x0012C25D File Offset: 0x0012A45D
	public static void LED_Off(OilChanger.Instance smi)
	{
		smi.SetLEDState(false);
	}

	// Token: 0x060034FE RID: 13566 RVA: 0x0012C268 File Offset: 0x0012A468
	private static WorkChore<OilChangerWorkableUse> CreateChore(OilChanger.Instance smi)
	{
		return new WorkChore<OilChangerWorkableUse>(Db.Get().ChoreTypes.OilChange, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
	}

	// Token: 0x04002009 RID: 8201
	public const string STORAGE_METER_TARGET_NAME = "meter_target";

	// Token: 0x0400200A RID: 8202
	public const string STORAGE_METER_ANIM_NAME = "meter";

	// Token: 0x0400200B RID: 8203
	public const string LED_METER_TARGET_NAME = "light_target";

	// Token: 0x0400200C RID: 8204
	public const string LED_METER_ANIM_ON_NAME = "light_on";

	// Token: 0x0400200D RID: 8205
	public const string LED_METER_ANIM_OFF_NAME = "light_off";

	// Token: 0x0400200E RID: 8206
	public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State inoperational;

	// Token: 0x0400200F RID: 8207
	public OilChanger.OperationalStates operational;

	// Token: 0x0200171B RID: 5915
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040076E9 RID: 30441
		public float MIN_LUBRICANT_MASS_TO_WORK = 200f;
	}

	// Token: 0x0200171C RID: 5916
	public class OperationalStates : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State
	{
		// Token: 0x040076EA RID: 30442
		public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State oilNeeded;

		// Token: 0x040076EB RID: 30443
		public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State ready;
	}

	// Token: 0x0200171D RID: 5917
	public new class Instance : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.GameInstance, IFetchList
	{
		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06009A03 RID: 39427 RVA: 0x0038F653 File Offset: 0x0038D853
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06009A04 RID: 39428 RVA: 0x0038F660 File Offset: 0x0038D860
		public float OilAmount
		{
			get
			{
				return this.storage.GetMassAvailable(GameTags.LubricatingOil);
			}
		}

		// Token: 0x06009A05 RID: 39429 RVA: 0x0038F674 File Offset: 0x0038D874
		public Instance(IStateMachineTarget master, OilChanger.Def def)
		{
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			Tag lubricatingOil = GameTags.LubricatingOil;
			dictionary[lubricatingOil] = 0f;
			this.remainingLubricationMass = dictionary;
			base..ctor(master, def);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			this.storage = base.GetComponent<Storage>();
			this.operational = base.GetComponent<Operational>();
			this.oilStorageMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			this.readyLightMeter = new MeterController(component, "light_target", "light_off", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
		}

		// Token: 0x06009A06 RID: 39430 RVA: 0x0038F704 File Offset: 0x0038D904
		public void SetLEDState(bool isOn)
		{
			string s = isOn ? "light_on" : "light_off";
			this.readyLightMeter.meterController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06009A07 RID: 39431 RVA: 0x0038F744 File Offset: 0x0038D944
		public void UpdateStorageMeter()
		{
			float positionPercent = this.OilAmount / this.storage.capacityKg;
			this.oilStorageMeter.SetPositionPercent(positionPercent);
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06009A08 RID: 39432 RVA: 0x0038F770 File Offset: 0x0038D970
		public Storage Destination
		{
			get
			{
				return this.storage;
			}
		}

		// Token: 0x06009A09 RID: 39433 RVA: 0x0038F778 File Offset: 0x0038D978
		public float GetMinimumAmount(Tag tag)
		{
			return base.def.MIN_LUBRICANT_MASS_TO_WORK;
		}

		// Token: 0x06009A0A RID: 39434 RVA: 0x0038F785 File Offset: 0x0038D985
		public Dictionary<Tag, float> GetRemaining()
		{
			this.remainingLubricationMass[GameTags.LubricatingOil] = Mathf.Clamp(base.def.MIN_LUBRICANT_MASS_TO_WORK - this.OilAmount, 0f, base.def.MIN_LUBRICANT_MASS_TO_WORK);
			return this.remainingLubricationMass;
		}

		// Token: 0x06009A0B RID: 39435 RVA: 0x0038F7C4 File Offset: 0x0038D9C4
		public Dictionary<Tag, float> GetRemainingMinimum()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040076EC RID: 30444
		private Storage storage;

		// Token: 0x040076ED RID: 30445
		private Operational operational;

		// Token: 0x040076EE RID: 30446
		private MeterController oilStorageMeter;

		// Token: 0x040076EF RID: 30447
		private MeterController readyLightMeter;

		// Token: 0x040076F0 RID: 30448
		private Dictionary<Tag, float> remainingLubricationMass;
	}
}
