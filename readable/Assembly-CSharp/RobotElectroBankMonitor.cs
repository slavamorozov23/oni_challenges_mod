using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000B12 RID: 2834
public class RobotElectroBankMonitor : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>
{
	// Token: 0x0600529A RID: 21146 RVA: 0x001E0B3C File Offset: 0x001DED3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.powered;
		this.root.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			smi.ElectroBankStorageChange(null);
		}).TagTransition(GameTags.Dead, this.deceased, false).TagTransition(GameTags.Creatures.Die, this.deceased, false);
		this.powered.DefaultState(this.powered.highBattery).ParamTransition<bool>(this.hasElectrobank, this.powerdown, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.IsFalse).Update(delegate(RobotElectroBankMonitor.Instance smi, float dt)
		{
			RobotElectroBankMonitor.ConsumePower(smi, dt);
		}, UpdateRate.SIM_200ms, false);
		this.powered.highBattery.Transition(this.powered.lowBattery, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Not(new StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback(RobotElectroBankMonitor.ChargeDecent)), UpdateRate.SIM_200ms).Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			this.UpdateBatteryMeter(smi, RobotElectroBankMonitor.BATTER_FULL_SYMBOL);
		});
		this.powered.lowBattery.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
			this.UpdateBatteryMeter(smi, RobotElectroBankMonitor.BATTER_LOW_SYMBOL);
		}).Transition(this.powered.highBattery, new StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback(RobotElectroBankMonitor.ChargeDecent), UpdateRate.SIM_200ms).ToggleStatusItem((RobotElectroBankMonitor.Instance smi) => Db.Get().RobotStatusItems.LowBatteryNoCharge, null);
		this.powerdown.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
		}).ToggleBehaviour(GameTags.Robots.Behaviours.NoElectroBank, (RobotElectroBankMonitor.Instance smi) => true, delegate(RobotElectroBankMonitor.Instance smi)
		{
			smi.GoTo(this.powered);
		});
		this.deceased.DoNothing();
	}

	// Token: 0x0600529B RID: 21147 RVA: 0x001E0CFB File Offset: 0x001DEEFB
	private void UpdateBatteryMeter(RobotElectroBankMonitor.Instance smi, HashedString symbol)
	{
		smi.UpdateBatteryState(symbol);
	}

	// Token: 0x0600529C RID: 21148 RVA: 0x001E0D04 File Offset: 0x001DEF04
	public static bool ChargeDecent(RobotElectroBankMonitor.Instance smi)
	{
		float num = 0f;
		foreach (GameObject gameObject in smi.electroBankStorage.items)
		{
			if (!(gameObject == null))
			{
				num += gameObject.GetComponent<Electrobank>().Charge;
			}
		}
		return num >= smi.def.lowBatteryWarningPercent * 120000f;
	}

	// Token: 0x0600529D RID: 21149 RVA: 0x001E0D8C File Offset: 0x001DEF8C
	public static void ConsumePower(RobotElectroBankMonitor.Instance smi, float dt)
	{
		if (smi.electrobank == null)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
			return;
		}
		float joules = Mathf.Min(dt * Mathf.Abs(smi.bankAmount.GetDelta()), smi.electrobank.Charge);
		smi.electrobank.RemovePower(joules, true);
		if (smi.electrobank != null)
		{
			smi.bankAmount.value = smi.electrobank.Charge;
		}
	}

	// Token: 0x0600529E RID: 21150 RVA: 0x001E0E03 File Offset: 0x001DF003
	public static void RequestBattery(RobotElectroBankMonitor.Instance smi)
	{
		if (smi.fetchBatteryChore.IsPaused)
		{
			smi.fetchBatteryChore.Pause(smi.electrobank != null && RobotElectroBankMonitor.ChargeDecent(smi), "FlydoBattery");
		}
	}

	// Token: 0x040037CC RID: 14284
	public static readonly HashedString BATTER_SYMBOL = "meter_target";

	// Token: 0x040037CD RID: 14285
	public static readonly HashedString BATTER_FULL_SYMBOL = "battery_full";

	// Token: 0x040037CE RID: 14286
	public static readonly HashedString BATTER_LOW_SYMBOL = "battery_low";

	// Token: 0x040037CF RID: 14287
	public static readonly HashedString BATTER_DEAD_SYMBOL = "battery_dead";

	// Token: 0x040037D0 RID: 14288
	public RobotElectroBankMonitor.PoweredState powered;

	// Token: 0x040037D1 RID: 14289
	public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State deceased;

	// Token: 0x040037D2 RID: 14290
	public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State powerdown;

	// Token: 0x040037D3 RID: 14291
	public StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.BoolParameter hasElectrobank;

	// Token: 0x02001C51 RID: 7249
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040087AB RID: 34731
		public float lowBatteryWarningPercent;
	}

	// Token: 0x02001C52 RID: 7250
	public class PoweredState : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State
	{
		// Token: 0x040087AC RID: 34732
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State highBattery;

		// Token: 0x040087AD RID: 34733
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State lowBattery;
	}

	// Token: 0x02001C53 RID: 7251
	public new class Instance : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.GameInstance
	{
		// Token: 0x0600AD19 RID: 44313 RVA: 0x003CE7FC File Offset: 0x003CC9FC
		public Instance(IStateMachineTarget master, RobotElectroBankMonitor.Def def) : base(master, def)
		{
			this.fetchBatteryChore = base.GetComponent<ManualDeliveryKG>();
			foreach (Storage storage in master.gameObject.GetComponents<Storage>())
			{
				if (storage.storageID == GameTags.ChargedPortableBattery)
				{
					this.electroBankStorage = storage;
					break;
				}
			}
			foreach (GameObject gameObject in Assets.GetPrefabsWithTag(GameTags.ChargedPortableBattery))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				this.batteryTags.Add(component.PrefabTag);
			}
			this.bankAmount = Db.Get().Amounts.InternalElectroBank.Lookup(master.gameObject);
			this.electroBankStorage.Subscribe(-1697596308, new Action<object>(this.ElectroBankStorageChange));
			this.ElectroBankStorageChange(null);
			TreeFilterable component2 = base.GetComponent<TreeFilterable>();
			component2.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component2.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		}

		// Token: 0x0600AD1A RID: 44314 RVA: 0x003CE928 File Offset: 0x003CCB28
		public void ElectroBankStorageChange(object data = null)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component.storage != null && component.storage.storageID == GameTags.ChargedPortableBattery)
				{
					if (this.electroBankStorage.Count > 0 && this.electroBankStorage.items[0] != null)
					{
						this.electrobank = this.electroBankStorage.items[0].GetComponent<Electrobank>();
						this.bankAmount.value = this.electrobank.Charge;
					}
					else
					{
						this.electrobank = null;
					}
				}
				else if (this.electroBankStorage.Count <= 0)
				{
					this.electrobank = null;
					this.bankAmount.value = 0f;
					this.DropDischargedElectroBank(gameObject);
				}
				this.fetchBatteryChore.Pause(this.electrobank != null && RobotElectroBankMonitor.ChargeDecent(this), "Robot has sufficienct electrobank");
				base.sm.hasElectrobank.Set(this.electrobank != null, this, false);
				return;
			}
			if (this.electrobank == null)
			{
				if (this.electroBankStorage.Count > 0 && this.electroBankStorage.items[0] != null)
				{
					this.electrobank = this.electroBankStorage.items[0].GetComponent<Electrobank>();
					this.bankAmount.value = this.electrobank.Charge;
				}
				else
				{
					this.electrobank = null;
					this.bankAmount.value = 0f;
				}
				this.fetchBatteryChore.Pause(this.electrobank != null && RobotElectroBankMonitor.ChargeDecent(this), "Robot has sufficienct electrobank");
				base.sm.hasElectrobank.Set(this.electrobank != null, this, false);
			}
		}

		// Token: 0x0600AD1B RID: 44315 RVA: 0x003CEB14 File Offset: 0x003CCD14
		private void DropDischargedElectroBank(GameObject go)
		{
			Electrobank component = go.GetComponent<Electrobank>();
			if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
			{
				component.RemovePower(component.Charge, true);
			}
		}

		// Token: 0x0600AD1C RID: 44316 RVA: 0x003CEB54 File Offset: 0x003CCD54
		public void UpdateBatteryState(HashedString newState)
		{
			if (this.currentSymbolSwap.IsValid)
			{
				this.symbolOverrideController.RemoveSymbolOverride(this.currentSymbolSwap, 0);
			}
			KAnim.Build.Symbol symbol = this.animController.AnimFiles[0].GetData().build.GetSymbol(newState);
			this.symbolOverrideController.AddSymbolOverride(RobotElectroBankMonitor.BATTER_SYMBOL, symbol, 0);
			this.currentSymbolSwap = newState;
		}

		// Token: 0x0600AD1D RID: 44317 RVA: 0x003CEBC0 File Offset: 0x003CCDC0
		private void OnFilterChanged(HashSet<Tag> allowed_tags)
		{
			if (this.fetchBatteryChore != null)
			{
				List<Tag> list = new List<Tag>();
				foreach (Tag item in this.batteryTags)
				{
					if (!allowed_tags.Contains(item))
					{
						list.Add(item);
					}
				}
				this.fetchBatteryChore.ForbiddenTags = list.ToArray();
			}
		}

		// Token: 0x040087AE RID: 34734
		public Storage electroBankStorage;

		// Token: 0x040087AF RID: 34735
		public Electrobank electrobank;

		// Token: 0x040087B0 RID: 34736
		public ManualDeliveryKG fetchBatteryChore;

		// Token: 0x040087B1 RID: 34737
		public AmountInstance bankAmount;

		// Token: 0x040087B2 RID: 34738
		[MyCmpReq]
		private SymbolOverrideController symbolOverrideController;

		// Token: 0x040087B3 RID: 34739
		[MyCmpReq]
		private KBatchedAnimController animController;

		// Token: 0x040087B4 RID: 34740
		private HashedString currentSymbolSwap;

		// Token: 0x040087B5 RID: 34741
		private HashSet<Tag> batteryTags = new HashSet<Tag>();
	}
}
