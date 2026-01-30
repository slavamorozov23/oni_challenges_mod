using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A1C RID: 2588
public class CritterEmoteMonitor : GameStateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>
{
	// Token: 0x06004BD4 RID: 19412 RVA: 0x001B88CE File Offset: 0x001B6ACE
	public static bool ShouldEmote(CritterEmoteMonitor.Instance smi)
	{
		return true;
	}

	// Token: 0x06004BD5 RID: 19413 RVA: 0x001B88D4 File Offset: 0x001B6AD4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		List<CritterEmotion> cooldownsToRemove = new List<CritterEmotion>();
		this.cooldown.ScheduleGoTo((CritterEmoteMonitor.Instance smi) => UnityEngine.Random.Range(37.5f, 75f), this.express).Enter(delegate(CritterEmoteMonitor.Instance smi)
		{
			NameDisplayScreen.Instance.SetThoughtBubbleDisplay(smi.gameObject, false, null, null, null);
		}).Update(delegate(CritterEmoteMonitor.Instance smi, float dt)
		{
			foreach (KeyValuePair<CritterEmotion, float> keyValuePair in smi.cooldowns)
			{
				if (Time.timeSinceLevelLoad > smi.cooldowns[keyValuePair.Key] + 30f)
				{
					cooldownsToRemove.Add(keyValuePair.Key);
				}
			}
			foreach (CritterEmotion key in cooldownsToRemove)
			{
				smi.cooldowns.Remove(key);
			}
			cooldownsToRemove.Clear();
		}, UpdateRate.SIM_200ms, false);
		this.express.ToggleBehaviour(GameTags.Creatures.Behaviours.CritterEmoteBehaviour, new StateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>.Transition.ConditionCallback(CritterEmoteMonitor.ShouldEmote), delegate(CritterEmoteMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
	}

	// Token: 0x04003243 RID: 12867
	public GameStateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>.State cooldown;

	// Token: 0x04003244 RID: 12868
	public GameStateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>.State express;

	// Token: 0x02001AC2 RID: 6850
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001AC3 RID: 6851
	public new class Instance : GameStateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>.GameInstance, IDevQuickAction
	{
		// Token: 0x0600A6FE RID: 42750 RVA: 0x003BB1A0 File Offset: 0x003B93A0
		public Instance(IStateMachineTarget master, CritterEmoteMonitor.Def def) : base(master, def)
		{
			this.emotePositive = Db.Get().Emotes.Critter.Positive;
			this.emoteNegative = Db.Get().Emotes.Critter.Negative;
		}

		// Token: 0x0600A6FF RID: 42751 RVA: 0x003BB20C File Offset: 0x003B940C
		public CritterEmotion GetCritterEmotion()
		{
			if (this.currentNegativeEmotions.Count > 0)
			{
				float num = float.PositiveInfinity;
				CritterEmotion result = null;
				foreach (CritterEmotion critterEmotion in this.currentNegativeEmotions)
				{
					if (!this.cooldowns.ContainsKey(critterEmotion))
					{
						return critterEmotion;
					}
					float num2 = this.cooldowns[critterEmotion];
					if (num2 < num)
					{
						num = num2;
						result = critterEmotion;
					}
				}
				return result;
			}
			if (this.currentPositiveEmotions.Count > 0)
			{
				float num3 = 0f;
				CritterEmotion result2 = null;
				foreach (CritterEmotion critterEmotion2 in this.currentPositiveEmotions)
				{
					if (!this.cooldowns.ContainsKey(critterEmotion2))
					{
						return critterEmotion2;
					}
					float num4 = this.cooldowns[critterEmotion2];
					if (num4 < num3)
					{
						num3 = num4;
						result2 = critterEmotion2;
					}
				}
				return result2;
			}
			return null;
		}

		// Token: 0x0600A700 RID: 42752 RVA: 0x003BB32C File Offset: 0x003B952C
		public void AddCritterEmotion(CritterEmotion emotion)
		{
			if (base.smi.GetSMI<BabyMonitor.Instance>() != null)
			{
				return;
			}
			if (!emotion.isPositiveEmotion)
			{
				if (this.currentNegativeEmotions.Contains(emotion))
				{
					return;
				}
				this.currentNegativeEmotions.Add(emotion);
			}
			else
			{
				if (this.currentPositiveEmotions.Contains(emotion))
				{
					return;
				}
				this.currentPositiveEmotions.Add(emotion);
			}
			if (base.smi.IsInsideState(base.sm.cooldown) && !this.cooldowns.ContainsKey(emotion))
			{
				base.smi.GoTo(base.sm.express);
			}
		}

		// Token: 0x0600A701 RID: 42753 RVA: 0x003BB3C4 File Offset: 0x003B95C4
		public void RemoveCritterEmotion(CritterEmotion emotion)
		{
			this.currentNegativeEmotions.RemoveAll((CritterEmotion e) => e.id == emotion.id);
			this.currentPositiveEmotions.RemoveAll((CritterEmotion e) => e.id == emotion.id);
		}

		// Token: 0x0600A702 RID: 42754 RVA: 0x003BB40E File Offset: 0x003B960E
		public List<DevQuickActionInstruction> GetDevInstructions()
		{
			return new List<DevQuickActionInstruction>
			{
				new DevQuickActionInstruction("Emote/Play", delegate()
				{
					base.smi.GoTo(base.smi.sm.express);
				})
			};
		}

		// Token: 0x040082A0 RID: 33440
		public Emote emotePositive;

		// Token: 0x040082A1 RID: 33441
		public Emote emoteNegative;

		// Token: 0x040082A2 RID: 33442
		public List<CritterEmotion> currentNegativeEmotions = new List<CritterEmotion>();

		// Token: 0x040082A3 RID: 33443
		public List<CritterEmotion> currentPositiveEmotions = new List<CritterEmotion>();

		// Token: 0x040082A4 RID: 33444
		public const float SPECIFIC_EMOTE_COOLDOWN = 30f;

		// Token: 0x040082A5 RID: 33445
		public Dictionary<CritterEmotion, float> cooldowns = new Dictionary<CritterEmotion, float>();
	}
}
