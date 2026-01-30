using System;
using System.Text;
using Klei.AI;
using STRINGS;

// Token: 0x02000C97 RID: 3223
public class DecorDisplayer : StandardAmountDisplayer
{
	// Token: 0x060062CC RID: 25292 RVA: 0x0024A565 File Offset: 0x00248765
	public DecorDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new DecorDisplayer.DecorAttributeFormatter();
	}

	// Token: 0x060062CD RID: 25293 RVA: 0x0024A57C File Offset: 0x0024877C
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		string format = LocText.ParseText(master.description);
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		stringBuilder.AppendFormat(format, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		int cell = Grid.PosToCell(instance.gameObject);
		if (Grid.IsValidCell(cell))
		{
			stringBuilder.Append(string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_CURRENT, GameUtil.GetDecorAtCell(cell)));
		}
		stringBuilder.Append("\n");
		DecorMonitor.Instance smi = instance.gameObject.GetSMI<DecorMonitor.Instance>();
		if (smi != null)
		{
			stringBuilder.AppendFormat(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_TODAY, this.formatter.GetFormattedValue(smi.GetTodaysAverageDecor(), GameUtil.TimeSlice.None));
			stringBuilder.AppendFormat(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_YESTERDAY, this.formatter.GetFormattedValue(smi.GetYesterdaysAverageDecor(), GameUtil.TimeSlice.None));
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x02001EC9 RID: 7881
	public class DecorAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x0600B4AC RID: 46252 RVA: 0x003EC13C File Offset: 0x003EA33C
		public DecorAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}
