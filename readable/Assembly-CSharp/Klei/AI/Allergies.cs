using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02001028 RID: 4136
	public class Allergies : Sickness
	{
		// Token: 0x06008074 RID: 32884 RVA: 0x0033884C File Offset: 0x00336A4C
		public Allergies() : base("Allergies", Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.00025f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation
		}, 60f, null)
		{
			float value = 0.025f;
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_allergies_kanim"
			}, Db.Get().Expressions.Pollen));
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, value, DUPLICANTS.DISEASES.ALLERGIES.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Sneezyness.Id, 10f, DUPLICANTS.DISEASES.ALLERGIES.NAME, false, false, true)
			}));
		}

		// Token: 0x04006149 RID: 24905
		public const string ID = "Allergies";

		// Token: 0x0400614A RID: 24906
		public const float STRESS_PER_CYCLE = 15f;
	}
}
