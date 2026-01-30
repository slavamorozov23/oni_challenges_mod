using System;

namespace Klei.AI
{
	// Token: 0x0200104C RID: 4172
	public class FertilityModifier : Resource
	{
		// Token: 0x06008133 RID: 33075 RVA: 0x0033E48C File Offset: 0x0033C68C
		public FertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction) : base(id, name)
		{
			this.Description = description;
			this.TargetTag = targetTag;
			this.TooltipCB = tooltipCB;
			this.ApplyFunction = applyFunction;
		}

		// Token: 0x06008134 RID: 33076 RVA: 0x0033E4B5 File Offset: 0x0033C6B5
		public string GetTooltip()
		{
			if (this.TooltipCB != null)
			{
				return this.TooltipCB(this.Description);
			}
			return this.Description;
		}

		// Token: 0x040061E8 RID: 25064
		public string Description;

		// Token: 0x040061E9 RID: 25065
		public Tag TargetTag;

		// Token: 0x040061EA RID: 25066
		public Func<string, string> TooltipCB;

		// Token: 0x040061EB RID: 25067
		public FertilityModifier.FertilityModFn ApplyFunction;

		// Token: 0x0200274F RID: 10063
		// (Invoke) Token: 0x0600C899 RID: 51353
		public delegate void FertilityModFn(FertilityMonitor.Instance inst, Tag eggTag);
	}
}
