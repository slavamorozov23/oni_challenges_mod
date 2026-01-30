using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02001035 RID: 4149
	public class ZombieSickness : Sickness
	{
		// Token: 0x060080B7 RID: 32951 RVA: 0x0033B464 File Offset: 0x00339664
		public ZombieSickness() : base("ZombieSickness", Sickness.SicknessType.Pathogen, Sickness.Severity.Major, 0.00025f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation,
			Sickness.InfectionVector.Contact
		}, 10800f, "ZombieSicknessRecovery")
		{
			base.AddSicknessComponent(new CustomSickEffectSickness("spore_fx_kanim", "working_loop"));
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_spores_kanim",
				"anim_loco_spore_kanim"
			}, Db.Get().Expressions.SickSpores));
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier(Db.Get().Attributes.Athletics.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Strength.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Digging.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Construction.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Art.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Caring.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Learning.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Machinery.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Cooking.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Botanist.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Ranching.Id, -10f, DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME, false, false, true)
			}));
		}

		// Token: 0x04006182 RID: 24962
		public const string ID = "ZombieSickness";

		// Token: 0x04006183 RID: 24963
		public const string RECOVERY_ID = "ZombieSicknessRecovery";

		// Token: 0x04006184 RID: 24964
		public const int ATTRIBUTE_PENALTY = -10;
	}
}
