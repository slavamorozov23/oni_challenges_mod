using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A20 RID: 2592
public class DecorMonitor : GameStateMachine<DecorMonitor, DecorMonitor.Instance>
{
	// Token: 0x06004BE5 RID: 19429 RVA: 0x001B9180 File Offset: 0x001B7380
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleAttributeModifier("DecorSmoother", (DecorMonitor.Instance smi) => smi.GetDecorModifier(), (DecorMonitor.Instance smi) => true).Update("DecorSensing", delegate(DecorMonitor.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_200ms, false).EventHandler(GameHashes.NewDay, (DecorMonitor.Instance smi) => GameClock.Instance, delegate(DecorMonitor.Instance smi)
		{
			smi.OnNewDay();
		});
	}

	// Token: 0x04003252 RID: 12882
	public static float MAXIMUM_DECOR_VALUE = 120f;

	// Token: 0x02001AD3 RID: 6867
	public new class Instance : GameStateMachine<DecorMonitor, DecorMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A73A RID: 42810 RVA: 0x003BBC74 File Offset: 0x003B9E74
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.cycleTotalDecor = 2250f;
			this.amount = Db.Get().Amounts.Decor.Lookup(base.gameObject);
			this.modifier = new AttributeModifier(Db.Get().Amounts.Decor.deltaAttribute.Id, 1f, DUPLICANTS.NEEDS.DECOR.OBSERVED_DECOR, false, false, false);
		}

		// Token: 0x0600A73B RID: 42811 RVA: 0x003BBDA5 File Offset: 0x003B9FA5
		public AttributeModifier GetDecorModifier()
		{
			return this.modifier;
		}

		// Token: 0x0600A73C RID: 42812 RVA: 0x003BBDB0 File Offset: 0x003B9FB0
		public void Update(float dt)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!Grid.IsValidCell(cell))
			{
				return;
			}
			float decorAtCell = GameUtil.GetDecorAtCell(cell);
			this.cycleTotalDecor += decorAtCell * dt;
			float value = 0f;
			float num = 4.1666665f;
			if (Mathf.Abs(decorAtCell - this.amount.value) > 0.5f)
			{
				if (decorAtCell > this.amount.value)
				{
					value = 3f * num;
				}
				else if (decorAtCell < this.amount.value)
				{
					value = -num;
				}
			}
			else
			{
				this.amount.value = decorAtCell;
			}
			this.modifier.SetValue(value);
		}

		// Token: 0x0600A73D RID: 42813 RVA: 0x003BBE54 File Offset: 0x003BA054
		public void OnNewDay()
		{
			this.yesterdaysTotalDecor = this.cycleTotalDecor;
			this.cycleTotalDecor = 0f;
			float totalValue = base.gameObject.GetAttributes().Add(Db.Get().Attributes.DecorExpectation).GetTotalValue();
			float num = this.yesterdaysTotalDecor / 600f;
			num += totalValue;
			Effects component = base.gameObject.GetComponent<Effects>();
			foreach (KeyValuePair<float, string> keyValuePair in this.effectLookup)
			{
				if (num < keyValuePair.Key)
				{
					component.Add(keyValuePair.Value, true);
					break;
				}
			}
		}

		// Token: 0x0600A73E RID: 42814 RVA: 0x003BBF18 File Offset: 0x003BA118
		public float GetTodaysAverageDecor()
		{
			return this.cycleTotalDecor / (GameClock.Instance.GetCurrentCycleAsPercentage() * 600f);
		}

		// Token: 0x0600A73F RID: 42815 RVA: 0x003BBF31 File Offset: 0x003BA131
		public float GetYesterdaysAverageDecor()
		{
			return this.yesterdaysTotalDecor / 600f;
		}

		// Token: 0x040082D4 RID: 33492
		[Serialize]
		private float cycleTotalDecor;

		// Token: 0x040082D5 RID: 33493
		[Serialize]
		private float yesterdaysTotalDecor;

		// Token: 0x040082D6 RID: 33494
		private AmountInstance amount;

		// Token: 0x040082D7 RID: 33495
		private AttributeModifier modifier;

		// Token: 0x040082D8 RID: 33496
		private List<KeyValuePair<float, string>> effectLookup = new List<KeyValuePair<float, string>>
		{
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * -0.25f, "DecorMinus1"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0f, "Decor0"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.25f, "Decor1"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.5f, "Decor2"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.75f, "Decor3"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE, "Decor4"),
			new KeyValuePair<float, string>(float.MaxValue, "Decor5")
		};
	}
}
