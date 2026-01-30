using System;

// Token: 0x02000495 RID: 1173
public class BeOfflineChore : Chore<BeOfflineChore.StatesInstance>
{
	// Token: 0x060018E6 RID: 6374 RVA: 0x0008A3D0 File Offset: 0x000885D0
	public static string GetPowerDownAnimPre(BeOfflineChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType == NavType.Ladder || currentNavType == NavType.Pole)
		{
			return "ladder_power_down";
		}
		return "power_down";
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x0008A404 File Offset: 0x00088604
	public static string GetPowerDownAnimLoop(BeOfflineChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType == NavType.Ladder || currentNavType == NavType.Pole)
		{
			return "ladder_power_down_idle";
		}
		return "power_down_idle";
	}

	// Token: 0x060018E8 RID: 6376 RVA: 0x0008A438 File Offset: 0x00088638
	public BeOfflineChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BeOffline, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BeOfflineChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.NotInTube, null);
	}

	// Token: 0x04000E62 RID: 3682
	public const string EFFECT_NAME = "BionicOffline";

	// Token: 0x020012BE RID: 4798
	public class StatesInstance : GameStateMachine<BeOfflineChore.States, BeOfflineChore.StatesInstance, BeOfflineChore, object>.GameInstance
	{
		// Token: 0x0600894A RID: 35146 RVA: 0x003515F1 File Offset: 0x0034F7F1
		public StatesInstance(BeOfflineChore master) : base(master)
		{
		}
	}

	// Token: 0x020012BF RID: 4799
	public class States : GameStateMachine<BeOfflineChore.States, BeOfflineChore.StatesInstance, BeOfflineChore>
	{
		// Token: 0x0600894B RID: 35147 RVA: 0x003515FC File Offset: 0x0034F7FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAnims("anim_bionic_kanim", 0f).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicOfflineIncapacitated, (BeOfflineChore.StatesInstance smi) => smi.master.gameObject.GetSMI<BionicBatteryMonitor.Instance>()).ToggleEffect("BionicOffline").PlayAnim(new Func<BeOfflineChore.StatesInstance, string>(BeOfflineChore.GetPowerDownAnimPre), KAnim.PlayMode.Once).QueueAnim(new Func<BeOfflineChore.StatesInstance, string>(BeOfflineChore.GetPowerDownAnimLoop), true, null);
		}
	}
}
