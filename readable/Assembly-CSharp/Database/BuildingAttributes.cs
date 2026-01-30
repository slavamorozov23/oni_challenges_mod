using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000F28 RID: 3880
	public class BuildingAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x06007C2B RID: 31787 RVA: 0x00304050 File Offset: 0x00302250
		public BuildingAttributes(ResourceSet parent) : base("BuildingAttributes", parent)
		{
			this.Decor = base.Add(new Klei.AI.Attribute("Decor", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.DecorRadius = base.Add(new Klei.AI.Attribute("DecorRadius", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.NoisePollution = base.Add(new Klei.AI.Attribute("NoisePollution", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.NoisePollutionRadius = base.Add(new Klei.AI.Attribute("NoisePollutionRadius", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Hygiene = base.Add(new Klei.AI.Attribute("Hygiene", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Comfort = base.Add(new Klei.AI.Attribute("Comfort", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.OverheatTemperature = base.Add(new Klei.AI.Attribute("OverheatTemperature", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.OverheatTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
			this.FatalTemperature = base.Add(new Klei.AI.Attribute("FatalTemperature", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.FatalTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
		}

		// Token: 0x040056F1 RID: 22257
		public Klei.AI.Attribute Decor;

		// Token: 0x040056F2 RID: 22258
		public Klei.AI.Attribute DecorRadius;

		// Token: 0x040056F3 RID: 22259
		public Klei.AI.Attribute NoisePollution;

		// Token: 0x040056F4 RID: 22260
		public Klei.AI.Attribute NoisePollutionRadius;

		// Token: 0x040056F5 RID: 22261
		public Klei.AI.Attribute Hygiene;

		// Token: 0x040056F6 RID: 22262
		public Klei.AI.Attribute Comfort;

		// Token: 0x040056F7 RID: 22263
		public Klei.AI.Attribute OverheatTemperature;

		// Token: 0x040056F8 RID: 22264
		public Klei.AI.Attribute FatalTemperature;
	}
}
