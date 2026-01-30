using System;
using STRINGS;

// Token: 0x0200088E RID: 2190
public class CreatureDiseaseCleaner : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>
{
	// Token: 0x06003C4E RID: 15438 RVA: 0x00151514 File Offset: 0x0014F714
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cleaning;
		GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.CLEANING.NAME;
		string tooltip = CREATURES.STATUSITEMS.CLEANING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.cleaning.DefaultState(this.cleaning.clean_pre).ScheduleGoTo((CreatureDiseaseCleaner.Instance smi) => smi.def.cleanDuration, this.cleaning.clean_pst);
		this.cleaning.clean_pre.PlayAnim("clean_water_pre").OnAnimQueueComplete(this.cleaning.clean);
		this.cleaning.clean.Enter(delegate(CreatureDiseaseCleaner.Instance smi)
		{
			smi.EnableDiseaseEmitter(true);
		}).QueueAnim("clean_water_loop", true, null).Transition(this.cleaning.clean_pst, (CreatureDiseaseCleaner.Instance smi) => !smi.GetSMI<CleaningMonitor.Instance>().CanCleanElementState(), UpdateRate.SIM_1000ms).Exit(delegate(CreatureDiseaseCleaner.Instance smi)
		{
			smi.EnableDiseaseEmitter(false);
		});
		this.cleaning.clean_pst.PlayAnim("clean_water_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Cleaning, false);
	}

	// Token: 0x0400252C RID: 9516
	public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State behaviourcomplete;

	// Token: 0x0400252D RID: 9517
	public CreatureDiseaseCleaner.CleaningStates cleaning;

	// Token: 0x0400252E RID: 9518
	public StateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.Signal cellChangedSignal;

	// Token: 0x0200185E RID: 6238
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009EAC RID: 40620 RVA: 0x003A3E59 File Offset: 0x003A2059
		public Def(float duration)
		{
			this.cleanDuration = duration;
		}

		// Token: 0x04007AB3 RID: 31411
		public float cleanDuration;
	}

	// Token: 0x0200185F RID: 6239
	public class CleaningStates : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State
	{
		// Token: 0x04007AB4 RID: 31412
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean_pre;

		// Token: 0x04007AB5 RID: 31413
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean;

		// Token: 0x04007AB6 RID: 31414
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean_pst;
	}

	// Token: 0x02001860 RID: 6240
	public new class Instance : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.GameInstance
	{
		// Token: 0x06009EAE RID: 40622 RVA: 0x003A3E70 File Offset: 0x003A2070
		public Instance(Chore<CreatureDiseaseCleaner.Instance> chore, CreatureDiseaseCleaner.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Cleaning);
		}

		// Token: 0x06009EAF RID: 40623 RVA: 0x003A3E94 File Offset: 0x003A2094
		public void EnableDiseaseEmitter(bool enable = true)
		{
			DiseaseEmitter component = base.GetComponent<DiseaseEmitter>();
			if (component != null)
			{
				component.SetEnable(enable);
			}
		}
	}
}
