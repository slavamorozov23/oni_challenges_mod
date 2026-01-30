using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000F4E RID: 3918
	public class PlantAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x06007CB1 RID: 31921 RVA: 0x00315474 File Offset: 0x00313674
		public PlantAttributes(ResourceSet parent) : base("PlantAttributes", parent)
		{
			this.WiltTempRangeMod = base.Add(new Klei.AI.Attribute("WiltTempRangeMod", false, Klei.AI.Attribute.Display.Normal, false, 1f, null, null, null, null));
			this.WiltTempRangeMod.SetFormatter(new PercentAttributeFormatter());
			this.YieldAmount = base.Add(new Klei.AI.Attribute("YieldAmount", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.YieldAmount.SetFormatter(new PercentAttributeFormatter());
			this.HarvestTime = base.Add(new Klei.AI.Attribute("HarvestTime", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.HarvestTime.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Time, GameUtil.TimeSlice.None));
			this.DecorBonus = base.Add(new Klei.AI.Attribute("DecorBonus", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.DecorBonus.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.MinLightLux = base.Add(new Klei.AI.Attribute("MinLightLux", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MinLightLux.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Lux, GameUtil.TimeSlice.None));
			this.FertilizerUsageMod = base.Add(new Klei.AI.Attribute("FertilizerUsageMod", false, Klei.AI.Attribute.Display.Normal, false, 1f, null, null, null, null));
			this.FertilizerUsageMod.SetFormatter(new PercentAttributeFormatter());
			this.MinRadiationThreshold = base.Add(new Klei.AI.Attribute("MinRadiationThreshold", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MinRadiationThreshold.SetFormatter(new RadsPerCycleAttributeFormatter());
			this.MaxRadiationThreshold = base.Add(new Klei.AI.Attribute("MaxRadiationThreshold", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MaxRadiationThreshold.SetFormatter(new RadsPerCycleAttributeFormatter());
		}

		// Token: 0x04005B18 RID: 23320
		public Klei.AI.Attribute WiltTempRangeMod;

		// Token: 0x04005B19 RID: 23321
		public Klei.AI.Attribute YieldAmount;

		// Token: 0x04005B1A RID: 23322
		public Klei.AI.Attribute HarvestTime;

		// Token: 0x04005B1B RID: 23323
		public Klei.AI.Attribute DecorBonus;

		// Token: 0x04005B1C RID: 23324
		public Klei.AI.Attribute MinLightLux;

		// Token: 0x04005B1D RID: 23325
		public Klei.AI.Attribute FertilizerUsageMod;

		// Token: 0x04005B1E RID: 23326
		public Klei.AI.Attribute MinRadiationThreshold;

		// Token: 0x04005B1F RID: 23327
		public Klei.AI.Attribute MaxRadiationThreshold;
	}
}
