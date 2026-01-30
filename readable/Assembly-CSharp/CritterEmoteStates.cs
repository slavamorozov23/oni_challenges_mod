using System;
using Database;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class CritterEmoteStates : GameStateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>
{
	// Token: 0x0600043A RID: 1082 RVA: 0x0002327C File Offset: 0x0002147C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(delegate(CritterEmoteStates.Instance smi)
		{
			smi.emotion = smi.GetSMI<CritterEmoteMonitor.Instance>().GetCritterEmotion();
			if (smi.emotion != null)
			{
				smi.GoTo(this.playing);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.playing.ToggleAnims((CritterEmoteStates.Instance smi) => smi.emoteBuildFile).PlayAnims(delegate(CritterEmoteStates.Instance smi)
		{
			if (!smi.emotion.isPositiveEmotion)
			{
				return new HashedString[]
				{
					"react_neg"
				};
			}
			return new HashedString[]
			{
				"react_pos"
			};
		}, KAnim.PlayMode.Once).ScheduleGoTo(10f, this.behaviourcomplete).OnAnimQueueComplete(this.behaviourcomplete).Enter(delegate(CritterEmoteStates.Instance smi)
		{
			CritterEmoteMonitor.Instance smi2 = smi.GetSMI<CritterEmoteMonitor.Instance>();
			smi.emotion = smi2.GetCritterEmotion();
			if (!smi2.cooldowns.ContainsKey(smi.emotion))
			{
				smi2.cooldowns.Add(smi.emotion, Time.timeSinceLevelLoad);
			}
			else
			{
				smi2.cooldowns[smi.emotion] = Time.timeSinceLevelLoad;
			}
			if (smi.emotion.sprite != null)
			{
				NameDisplayScreen.Instance.SetThoughtBubbleDisplay(smi.gameObject, true, "", Assets.GetSprite("bubble_alert"), smi.emotion.sprite);
				smi.hasSetThoughtBubble = true;
			}
		}).Exit(delegate(CritterEmoteStates.Instance smi)
		{
			if (smi.hasSetThoughtBubble)
			{
				NameDisplayScreen.Instance.SetThoughtBubbleDisplay(smi.gameObject, false, null, null, null);
				smi.hasSetThoughtBubble = false;
			}
		});
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Behaviours.CritterEmoteBehaviour, false);
	}

	// Token: 0x04000324 RID: 804
	public GameStateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>.State playing;

	// Token: 0x04000325 RID: 805
	public GameStateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>.State behaviourcomplete;

	// Token: 0x0200110E RID: 4366
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600839E RID: 33694 RVA: 0x00343A82 File Offset: 0x00341C82
		public Def(KAnimFile emoteBuildFile)
		{
			this.emoteBuildFile = emoteBuildFile;
		}

		// Token: 0x040063E9 RID: 25577
		public KAnimFile emoteBuildFile;
	}

	// Token: 0x0200110F RID: 4367
	public new class Instance : GameStateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>.GameInstance
	{
		// Token: 0x0600839F RID: 33695 RVA: 0x00343A91 File Offset: 0x00341C91
		public Instance(Chore<CritterEmoteStates.Instance> chore, CritterEmoteStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.CritterEmoteBehaviour);
			this.emoteBuildFile = def.emoteBuildFile;
		}

		// Token: 0x040063EA RID: 25578
		public KAnimFile emoteBuildFile;

		// Token: 0x040063EB RID: 25579
		public CritterEmotion emotion;

		// Token: 0x040063EC RID: 25580
		public bool hasSetThoughtBubble;
	}
}
