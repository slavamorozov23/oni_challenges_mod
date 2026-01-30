using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A3E RID: 2622
public class RecreationTimeMonitor : GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>
{
	// Token: 0x06004C88 RID: 19592 RVA: 0x001BCFAC File Offset: 0x001BB1AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.ScheduleBlocksTick, delegate(RecreationTimeMonitor.Instance smi)
		{
			smi.OnScheduleBlocksTick();
		}).Update(delegate(RecreationTimeMonitor.Instance smi, float dt)
		{
			smi.RefreshTimes();
		}, UpdateRate.SIM_200ms, false);
		this.bonusActive.ToggleEffect((RecreationTimeMonitor.Instance smi) => smi.moraleEffect).EventHandler(GameHashes.ScheduleBlocksTick, delegate(RecreationTimeMonitor.Instance smi)
		{
			smi.OnScheduleBlocksTick();
		}).Update(delegate(RecreationTimeMonitor.Instance smi, float dt)
		{
			smi.RefreshTimes();
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x040032E1 RID: 13025
	public const int MAX_BONUS = 5;

	// Token: 0x040032E2 RID: 13026
	public const float BONUS_DURATION_STANDARD = 600f;

	// Token: 0x040032E3 RID: 13027
	public const float BONUS_DURATION_BIONICS = 1800f;

	// Token: 0x040032E4 RID: 13028
	public GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.State idle;

	// Token: 0x040032E5 RID: 13029
	public GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.State bonusActive;

	// Token: 0x02001B20 RID: 6944
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001B21 RID: 6945
	public new class Instance : GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.GameInstance
	{
		// Token: 0x0600A884 RID: 43140 RVA: 0x003BF11C File Offset: 0x003BD31C
		public Instance(IStateMachineTarget master, RecreationTimeMonitor.Def def) : base(master, def)
		{
			this.bonus_duration = ((base.gameObject.PrefabID() == BionicMinionConfig.ID) ? 1800f : 600f);
			this.schedulable = master.GetComponent<Schedulable>();
			this.moraleModifier = new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, 0f, delegate()
			{
				if (Mathf.Clamp(this.moraleAddedTimes.Count - 1, 0, 5) == 5)
				{
					return DUPLICANTS.MODIFIERS.BREAK_BONUS.MAX_NAME;
				}
				return DUPLICANTS.MODIFIERS.BREAK_BONUS.NAME;
			}, false, false);
			this.moraleEffect.Add(this.moraleModifier);
			if ((SaveLoader.Instance.GameInfo.saveMajorVersion != 0 || SaveLoader.Instance.GameInfo.saveMinorVersion != 0) && SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 35))
			{
				this.RestoreFromSchedule();
			}
		}

		// Token: 0x0600A885 RID: 43141 RVA: 0x003BF233 File Offset: 0x003BD433
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshTimes();
		}

		// Token: 0x0600A886 RID: 43142 RVA: 0x003BF244 File Offset: 0x003BD444
		public void RefreshTimes()
		{
			for (int i = this.moraleAddedTimes.Count - 1; i >= 0; i--)
			{
				if (GameClock.Instance.GetTime() - this.moraleAddedTimes[i] > this.bonus_duration)
				{
					this.moraleAddedTimes.RemoveAt(i);
				}
			}
			int num = Math.Clamp(this.moraleAddedTimes.Count - 1, 0, 5);
			this.moraleModifier.SetValue((float)num);
			if (num > 0)
			{
				if (base.smi.GetCurrentState() != base.smi.sm.bonusActive)
				{
					base.smi.GoTo(base.smi.sm.bonusActive);
					return;
				}
			}
			else if (base.smi.GetCurrentState() != base.smi.sm.idle)
			{
				base.smi.GoTo(base.smi.sm.idle);
			}
		}

		// Token: 0x0600A887 RID: 43143 RVA: 0x003BF32C File Offset: 0x003BD52C
		public void OnScheduleBlocksTick()
		{
			if (ScheduleManager.Instance.GetSchedule(this.schedulable).GetPreviousScheduleBlock().GroupId == Db.Get().ScheduleGroups.Recreation.Id)
			{
				this.moraleAddedTimes.Add(GameClock.Instance.GetTime());
			}
		}

		// Token: 0x0600A888 RID: 43144 RVA: 0x003BF384 File Offset: 0x003BD584
		private void RestoreFromSchedule()
		{
			Effects component = base.GetComponent<Effects>();
			foreach (string effect_id in new string[]
			{
				"Break1",
				"Break2",
				"Break3",
				"Break4",
				"Break5"
			})
			{
				if (component.HasEffect(effect_id))
				{
					component.Remove(effect_id);
				}
			}
			Schedule schedule = ScheduleManager.Instance.GetSchedule(this.schedulable);
			List<ScheduleBlock> blocks = schedule.GetBlocks();
			int currentBlockIdx = schedule.GetCurrentBlockIdx();
			int num = 24;
			if (GameClock.Instance.GetTime() <= this.bonus_duration)
			{
				num = Math.Min(currentBlockIdx, Mathf.FloorToInt(GameClock.Instance.GetTime() / 25f));
			}
			for (int j = currentBlockIdx - num; j < currentBlockIdx; j++)
			{
				int k = j;
				global::Debug.Assert(blocks.Count > 0);
				while (k < 0)
				{
					k += blocks.Count;
				}
				if (blocks[k].GroupId == Db.Get().ScheduleGroups.Recreation.Id)
				{
					int num2;
					if (k > currentBlockIdx)
					{
						num2 = blocks.Count - k + currentBlockIdx - 1;
					}
					else
					{
						num2 = currentBlockIdx - k - 1;
					}
					float num3 = (float)num2 * 25f;
					float num4 = GameClock.Instance.GetTime() - num3;
					global::Debug.Assert(num4 > 0f);
					this.moraleAddedTimes.Add(num4);
				}
			}
		}

		// Token: 0x040083D2 RID: 33746
		[Serialize]
		public List<float> moraleAddedTimes = new List<float>();

		// Token: 0x040083D3 RID: 33747
		public Effect moraleEffect = new Effect("RecTimeEffect", "Rec Time Effect", "Rec Time Effect Description", 0f, false, false, false, null, -1f, 0f, null, "");

		// Token: 0x040083D4 RID: 33748
		private Schedulable schedulable;

		// Token: 0x040083D5 RID: 33749
		private AttributeModifier moraleModifier;

		// Token: 0x040083D6 RID: 33750
		private int shiftValue;

		// Token: 0x040083D7 RID: 33751
		private float bonus_duration;
	}
}
