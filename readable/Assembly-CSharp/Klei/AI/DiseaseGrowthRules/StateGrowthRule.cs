using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x0200105E RID: 4190
	public class StateGrowthRule : GrowthRule
	{
		// Token: 0x060081D0 RID: 33232 RVA: 0x00340E9E File Offset: 0x0033F09E
		public StateGrowthRule(Element.State state)
		{
			this.state = state;
		}

		// Token: 0x060081D1 RID: 33233 RVA: 0x00340EAD File Offset: 0x0033F0AD
		public override bool Test(Element e)
		{
			return e.IsState(this.state);
		}

		// Token: 0x060081D2 RID: 33234 RVA: 0x00340EBB File Offset: 0x0033F0BB
		public override string Name()
		{
			return Element.GetStateString(this.state);
		}

		// Token: 0x0400623C RID: 25148
		public Element.State state;
	}
}
