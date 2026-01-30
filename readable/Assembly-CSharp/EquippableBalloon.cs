using System;
using Database;
using KSerialization;
using TUNING;

// Token: 0x02000933 RID: 2355
public class EquippableBalloon : StateMachineComponent<EquippableBalloon.StatesInstance>
{
	// Token: 0x060041D5 RID: 16853 RVA: 0x00173D0F File Offset: 0x00171F0F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
	}

	// Token: 0x060041D6 RID: 16854 RVA: 0x00173D32 File Offset: 0x00171F32
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.ApplyBalloonOverrideToBalloonFx();
	}

	// Token: 0x060041D7 RID: 16855 RVA: 0x00173D4B File Offset: 0x00171F4B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060041D8 RID: 16856 RVA: 0x00173D53 File Offset: 0x00171F53
	public void SetBalloonOverride(BalloonOverrideSymbol balloonOverride)
	{
		base.smi.facadeAnim = balloonOverride.animFileID;
		base.smi.symbolID = balloonOverride.animFileSymbolID;
		this.ApplyBalloonOverrideToBalloonFx();
	}

	// Token: 0x060041D9 RID: 16857 RVA: 0x00173D80 File Offset: 0x00171F80
	public void ApplyBalloonOverrideToBalloonFx()
	{
		Equippable component = base.GetComponent<Equippable>();
		if (!component.IsNullOrDestroyed() && !component.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = component.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			BalloonFX.Instance smi = ((KMonoBehaviour)soleOwner.GetComponent<MinionAssignablesProxy>().target).GetSMI<BalloonFX.Instance>();
			if (!smi.IsNullOrDestroyed())
			{
				new BalloonOverrideSymbol(base.smi.facadeAnim, base.smi.symbolID).ApplyTo(smi);
			}
		}
	}

	// Token: 0x0200192C RID: 6444
	public class StatesInstance : GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.GameInstance
	{
		// Token: 0x0600A18F RID: 41359 RVA: 0x003AC0B9 File Offset: 0x003AA2B9
		public StatesInstance(EquippableBalloon master) : base(master)
		{
		}

		// Token: 0x04007D1C RID: 32028
		[Serialize]
		public float transitionTime;

		// Token: 0x04007D1D RID: 32029
		[Serialize]
		public string facadeAnim;

		// Token: 0x04007D1E RID: 32030
		[Serialize]
		public string symbolID;
	}

	// Token: 0x0200192D RID: 6445
	public class States : GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon>
	{
		// Token: 0x0600A190 RID: 41360 RVA: 0x003AC0C4 File Offset: 0x003AA2C4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Transition(this.destroy, (EquippableBalloon.StatesInstance smi) => GameClock.Instance.GetTime() >= smi.transitionTime, UpdateRate.SIM_200ms);
			this.destroy.Enter(delegate(EquippableBalloon.StatesInstance smi)
			{
				smi.master.GetComponent<Equippable>().Unassign();
			});
		}

		// Token: 0x04007D1F RID: 32031
		public GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.State destroy;
	}
}
