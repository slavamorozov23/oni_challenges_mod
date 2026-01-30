using System;
using STRINGS;

// Token: 0x020007B5 RID: 1973
public class MassiveHeatSink : StateMachineComponent<MassiveHeatSink.StatesInstance>
{
	// Token: 0x06003429 RID: 13353 RVA: 0x00127ECC File Offset: 0x001260CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001F7D RID: 8061
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001F7E RID: 8062
	[MyCmpReq]
	private ElementConverter elementConverter;

	// Token: 0x020016E6 RID: 5862
	public class States : GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink>
	{
		// Token: 0x060098F8 RID: 39160 RVA: 0x0038AB30 File Offset: 0x00388D30
		private string AwaitingFuelResolveString(string str, object obj)
		{
			ElementConverter elementConverter = ((MassiveHeatSink.StatesInstance)obj).master.elementConverter;
			string arg = elementConverter.consumedElements[0].Tag.ProperName();
			string formattedMass = GameUtil.GetFormattedMass(elementConverter.consumedElements[0].MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
			str = string.Format(str, arg, formattedMass);
			return str;
		}

		// Token: 0x060098F9 RID: 39161 RVA: 0x0038AB90 File Offset: 0x00388D90
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.idle, (MassiveHeatSink.StatesInstance smi) => smi.master.operational.IsOperational);
			GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State state = this.idle.EventTransition(GameHashes.OperationalChanged, this.disabled, (MassiveHeatSink.StatesInstance smi) => !smi.master.operational.IsOperational);
			string name = BUILDING.STATUSITEMS.AWAITINGFUEL.NAME;
			string tooltip = BUILDING.STATUSITEMS.AWAITINGFUEL.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Exclamation;
			NotificationType notification_type = NotificationType.BadMinor;
			bool allow_multiples = false;
			Func<string, MassiveHeatSink.StatesInstance, string> resolve_string_callback = new Func<string, MassiveHeatSink.StatesInstance, string>(this.AwaitingFuelResolveString);
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, resolve_string_callback, null, null).EventTransition(GameHashes.OnStorageChange, this.active, (MassiveHeatSink.StatesInstance smi) => smi.master.elementConverter.HasEnoughMassToStartConverting(false));
			this.active.EventTransition(GameHashes.OperationalChanged, this.disabled, (MassiveHeatSink.StatesInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.OnStorageChange, this.idle, (MassiveHeatSink.StatesInstance smi) => !smi.master.elementConverter.HasEnoughMassToStartConverting(false)).Enter(delegate(MassiveHeatSink.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(MassiveHeatSink.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
		}

		// Token: 0x0400760C RID: 30220
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State disabled;

		// Token: 0x0400760D RID: 30221
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State idle;

		// Token: 0x0400760E RID: 30222
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State active;
	}

	// Token: 0x020016E7 RID: 5863
	public class StatesInstance : GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.GameInstance
	{
		// Token: 0x060098FB RID: 39163 RVA: 0x0038AD34 File Offset: 0x00388F34
		public StatesInstance(MassiveHeatSink master) : base(master)
		{
		}
	}
}
