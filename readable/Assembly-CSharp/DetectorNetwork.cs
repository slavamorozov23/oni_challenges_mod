using System;
using STRINGS;

// Token: 0x0200073E RID: 1854
public class DetectorNetwork : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>
{
	// Token: 0x06002EB3 RID: 11955 RVA: 0x0010DB0C File Offset: 0x0010BD0C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (DetectorNetwork.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.operational.InitializeStates(this).EventTransition(GameHashes.OperationalChanged, this.inoperational, (DetectorNetwork.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}

	// Token: 0x04001BAF RID: 7087
	public StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.FloatParameter networkQuality;

	// Token: 0x04001BB0 RID: 7088
	public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State inoperational;

	// Token: 0x04001BB1 RID: 7089
	public DetectorNetwork.NetworkStates operational;

	// Token: 0x02001613 RID: 5651
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001614 RID: 5652
	public class NetworkStates : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State
	{
		// Token: 0x060095EF RID: 38383 RVA: 0x0037DEE4 File Offset: 0x0037C0E4
		public DetectorNetwork.NetworkStates InitializeStates(DetectorNetwork parent)
		{
			base.DefaultState(this.poor);
			GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State state = this.poor;
			string name = BUILDING.STATUSITEMS.NETWORKQUALITY.NAME;
			string tooltip = BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Exclamation;
			NotificationType notification_type = NotificationType.BadMinor;
			bool allow_multiples = false;
			Func<string, DetectorNetwork.Instance, string> resolve_string_callback = new Func<string, DetectorNetwork.Instance, string>(this.StringCallback);
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, resolve_string_callback, null, null).ParamTransition<float>(parent.networkQuality, this.good, (DetectorNetwork.Instance smi, float p) => (double)p >= 0.8);
			GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State state2 = this.good;
			string name2 = BUILDING.STATUSITEMS.NETWORKQUALITY.NAME;
			string tooltip2 = BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			resolve_string_callback = new Func<string, DetectorNetwork.Instance, string>(this.StringCallback);
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, resolve_string_callback, null, null).ParamTransition<float>(parent.networkQuality, this.poor, (DetectorNetwork.Instance smi, float p) => (double)p < 0.8);
			return this;
		}

		// Token: 0x060095F0 RID: 38384 RVA: 0x0037DFEC File Offset: 0x0037C1EC
		private string StringCallback(string str, DetectorNetwork.Instance smi)
		{
			MathUtil.MinMax detectTimeRangeForWorld = Game.Instance.spaceScannerNetworkManager.GetDetectTimeRangeForWorld(smi.GetMyWorldId());
			float num = Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(smi.GetMyWorldId());
			num = num.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(0f, 0.5f));
			return str.Replace("{TotalQuality}", GameUtil.GetFormattedPercent(smi.GetNetworkQuality01() * 100f, GameUtil.TimeSlice.None)).Replace("{WorstTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.min, "F0")).Replace("{BestTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.max, "F0")).Replace("{Coverage}", GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x040073B2 RID: 29618
		public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State poor;

		// Token: 0x040073B3 RID: 29619
		public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State good;
	}

	// Token: 0x02001615 RID: 5653
	public new class Instance : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.GameInstance
	{
		// Token: 0x060095F2 RID: 38386 RVA: 0x0037E0BC File Offset: 0x0037C2BC
		public Instance(IStateMachineTarget master, DetectorNetwork.Def def) : base(master, def)
		{
		}

		// Token: 0x060095F3 RID: 38387 RVA: 0x0037E0C6 File Offset: 0x0037C2C6
		public override void StartSM()
		{
			this.worldId = base.master.gameObject.GetMyWorldId();
			Components.DetectorNetworks.Add(this.worldId, this);
			base.StartSM();
		}

		// Token: 0x060095F4 RID: 38388 RVA: 0x0037E0F5 File Offset: 0x0037C2F5
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Components.DetectorNetworks.Remove(this.worldId, this);
		}

		// Token: 0x060095F5 RID: 38389 RVA: 0x0037E10F File Offset: 0x0037C30F
		public void Internal_SetNetworkQuality(float quality01)
		{
			base.sm.networkQuality.Set(quality01, base.smi, false);
		}

		// Token: 0x060095F6 RID: 38390 RVA: 0x0037E12A File Offset: 0x0037C32A
		public float GetNetworkQuality01()
		{
			return base.sm.networkQuality.Get(base.smi);
		}

		// Token: 0x040073B4 RID: 29620
		[NonSerialized]
		private int worldId;
	}
}
