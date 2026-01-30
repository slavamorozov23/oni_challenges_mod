using System;

// Token: 0x0200050B RID: 1291
public class SafetyChecker
{
	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06001BDC RID: 7132 RVA: 0x00099FDB File Offset: 0x000981DB
	// (set) Token: 0x06001BDD RID: 7133 RVA: 0x00099FE3 File Offset: 0x000981E3
	public SafetyChecker.Condition[] conditions { get; private set; }

	// Token: 0x06001BDE RID: 7134 RVA: 0x00099FEC File Offset: 0x000981EC
	public SafetyChecker(SafetyChecker.Condition[] conditions)
	{
		this.conditions = conditions;
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x00099FFC File Offset: 0x000981FC
	public int GetSafetyConditions(int cell, int cost, SafetyChecker.Context context, out bool all_conditions_met)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.conditions.Length; i++)
		{
			SafetyChecker.Condition condition = this.conditions[i];
			if (condition.callback(cell, cost, context))
			{
				num |= condition.mask;
				num2++;
			}
		}
		all_conditions_met = (num2 == this.conditions.Length);
		return num;
	}

	// Token: 0x0200139B RID: 5019
	public struct Condition
	{
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06008C82 RID: 35970 RVA: 0x00361F64 File Offset: 0x00360164
		// (set) Token: 0x06008C83 RID: 35971 RVA: 0x00361F6C File Offset: 0x0036016C
		public SafetyChecker.Condition.Callback callback { readonly get; private set; }

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06008C84 RID: 35972 RVA: 0x00361F75 File Offset: 0x00360175
		// (set) Token: 0x06008C85 RID: 35973 RVA: 0x00361F7D File Offset: 0x0036017D
		public int mask { readonly get; private set; }

		// Token: 0x06008C86 RID: 35974 RVA: 0x00361F86 File Offset: 0x00360186
		public Condition(string id, int condition_mask, SafetyChecker.Condition.Callback condition_callback)
		{
			this = default(SafetyChecker.Condition);
			this.callback = condition_callback;
			this.mask = condition_mask;
		}

		// Token: 0x02002804 RID: 10244
		// (Invoke) Token: 0x0600CAE6 RID: 51942
		public delegate bool Callback(int cell, int cost, SafetyChecker.Context context);
	}

	// Token: 0x0200139C RID: 5020
	public struct Context
	{
		// Token: 0x06008C87 RID: 35975 RVA: 0x00361FA0 File Offset: 0x003601A0
		public Context(KMonoBehaviour cmp)
		{
			this.cell = Grid.PosToCell(cmp);
			this.navigator = cmp.GetComponent<Navigator>();
			this.oxygenBreather = cmp.GetComponent<OxygenBreather>();
			this.minionBrain = cmp.GetComponent<MinionBrain>();
			this.temperatureTransferer = cmp.GetComponent<SimTemperatureTransfer>();
			this.primaryElement = cmp.GetComponent<PrimaryElement>();
			this.worldID = this.navigator.GetMyWorldId();
		}

		// Token: 0x04006BE5 RID: 27621
		public Navigator navigator;

		// Token: 0x04006BE6 RID: 27622
		public OxygenBreather oxygenBreather;

		// Token: 0x04006BE7 RID: 27623
		public SimTemperatureTransfer temperatureTransferer;

		// Token: 0x04006BE8 RID: 27624
		public PrimaryElement primaryElement;

		// Token: 0x04006BE9 RID: 27625
		public MinionBrain minionBrain;

		// Token: 0x04006BEA RID: 27626
		public int worldID;

		// Token: 0x04006BEB RID: 27627
		public int cell;
	}
}
