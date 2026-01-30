using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000F33 RID: 3891
	public class CritterAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x06007C56 RID: 31830 RVA: 0x0030E8C4 File Offset: 0x0030CAC4
		public CritterAttributes(ResourceSet parent) : base("CritterAttributes", parent)
		{
			this.Happiness = base.Add(new Klei.AI.Attribute("Happiness", Strings.Get("STRINGS.CREATURES.STATS.HAPPINESS.NAME"), "", Strings.Get("STRINGS.CREATURES.STATS.HAPPINESS.TOOLTIP"), 0f, Klei.AI.Attribute.Display.General, false, "ui_icon_happiness", null, null));
			this.Happiness.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Metabolism = base.Add(new Klei.AI.Attribute("Metabolism", false, Klei.AI.Attribute.Display.Details, false, 100f, "ui_icon_metabolism", null, null, null));
			this.Metabolism.SetFormatter(new ToPercentAttributeFormatter(100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x04005944 RID: 22852
		public Klei.AI.Attribute Happiness;

		// Token: 0x04005945 RID: 22853
		public Klei.AI.Attribute Metabolism;
	}
}
