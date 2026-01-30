using System;
using KSerialization;

// Token: 0x02000A74 RID: 2676
public class NuclearWaste : GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>
{
	// Token: 0x06004DCB RID: 19915 RVA: 0x001C3D10 File Offset: 0x001C1F10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.PlayAnim((NuclearWaste.Instance smi) => smi.GetAnimToPlay(), KAnim.PlayMode.Once).Update(delegate(NuclearWaste.Instance smi, float dt)
		{
			smi.timeAlive += dt;
			string animToPlay = smi.GetAnimToPlay();
			if (smi.GetComponent<KBatchedAnimController>().GetCurrentAnim().name != animToPlay)
			{
				smi.Play(animToPlay, KAnim.PlayMode.Once);
			}
			if (smi.timeAlive >= 600f)
			{
				smi.GoTo(this.decayed);
			}
		}, UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.Absorb, delegate(NuclearWaste.Instance smi, object otherObject)
		{
			Pickupable pickupable = (Pickupable)otherObject;
			float timeAlive = pickupable.GetSMI<NuclearWaste.Instance>().timeAlive;
			float mass = pickupable.PrimaryElement.Mass;
			float mass2 = smi.master.GetComponent<PrimaryElement>().Mass;
			float timeAlive2 = ((mass2 - mass) * smi.timeAlive + mass * timeAlive) / mass2;
			smi.timeAlive = timeAlive2;
			string animToPlay = smi.GetAnimToPlay();
			if (smi.GetComponent<KBatchedAnimController>().GetCurrentAnim().name != animToPlay)
			{
				smi.Play(animToPlay, KAnim.PlayMode.Once);
			}
			if (smi.timeAlive >= 600f)
			{
				smi.GoTo(this.decayed);
			}
		});
		this.decayed.Enter(delegate(NuclearWaste.Instance smi)
		{
			smi.GetComponent<Dumpable>().Dump();
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x040033D5 RID: 13269
	private const float lifetime = 600f;

	// Token: 0x040033D6 RID: 13270
	public GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State idle;

	// Token: 0x040033D7 RID: 13271
	public GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State decayed;

	// Token: 0x02001B97 RID: 7063
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001B98 RID: 7064
	public new class Instance : GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.GameInstance
	{
		// Token: 0x0600AA77 RID: 43639 RVA: 0x003C4745 File Offset: 0x003C2945
		public Instance(IStateMachineTarget master, NuclearWaste.Def def) : base(master, def)
		{
		}

		// Token: 0x0600AA78 RID: 43640 RVA: 0x003C4750 File Offset: 0x003C2950
		public string GetAnimToPlay()
		{
			this.percentageRemaining = 1f - base.smi.timeAlive / 600f;
			if (this.percentageRemaining <= 0.33f)
			{
				return "idle1";
			}
			if (this.percentageRemaining <= 0.66f)
			{
				return "idle2";
			}
			return "idle3";
		}

		// Token: 0x0400854F RID: 34127
		[Serialize]
		public float timeAlive;

		// Token: 0x04008550 RID: 34128
		private float percentageRemaining;
	}
}
