using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x0200102B RID: 4139
	public class FoodSickness : Sickness
	{
		// Token: 0x0600808A RID: 32906 RVA: 0x00339B98 File Offset: 0x00337D98
		public FoodSickness() : base("FoodSickness", Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.005f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Digestion
		}, 1020f, "FoodSicknessRecovery")
		{
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier("BladderDelta", 0.33333334f, DUPLICANTS.DISEASES.FOODSICKNESS.NAME, false, false, true),
				new AttributeModifier("ToiletEfficiency", -0.2f, DUPLICANTS.DISEASES.FOODSICKNESS.NAME, false, false, true),
				new AttributeModifier("StaminaDelta", -0.05f, DUPLICANTS.DISEASES.FOODSICKNESS.NAME, false, false, true)
			}));
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_sick_kanim"
			}, Db.Get().Expressions.Sick));
			base.AddSicknessComponent(new PeriodicEmoteSickness(Db.Get().Emotes.Minion.Sick, 10f));
		}

		// Token: 0x04006160 RID: 24928
		public const string ID = "FoodSickness";

		// Token: 0x04006161 RID: 24929
		public const string RECOVERY_ID = "FoodSicknessRecovery";

		// Token: 0x04006162 RID: 24930
		private const float VOMIT_FREQUENCY = 200f;
	}
}
