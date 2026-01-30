using System;

namespace TUNING
{
	// Token: 0x02000FD8 RID: 4056
	public class MEDICINE
	{
		// Token: 0x04005E9B RID: 24219
		public const float DEFAULT_MASS = 1f;

		// Token: 0x04005E9C RID: 24220
		public const float RECUPERATION_DISEASE_MULTIPLIER = 1.1f;

		// Token: 0x04005E9D RID: 24221
		public const float RECUPERATION_DOCTORED_DISEASE_MULTIPLIER = 1.2f;

		// Token: 0x04005E9E RID: 24222
		public const float WORK_TIME = 10f;

		// Token: 0x04005E9F RID: 24223
		public static readonly MedicineInfo BASICBOOSTER = new MedicineInfo("BasicBooster", "Medicine_BasicBooster", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04005EA0 RID: 24224
		public static readonly MedicineInfo INTERMEDIATEBOOSTER = new MedicineInfo("IntermediateBooster", "Medicine_IntermediateBooster", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04005EA1 RID: 24225
		public static readonly MedicineInfo BASICCURE = new MedicineInfo("BasicCure", null, MedicineInfo.MedicineType.CureSpecific, null, new string[]
		{
			"FoodSickness"
		});

		// Token: 0x04005EA2 RID: 24226
		public static readonly MedicineInfo ANTIHISTAMINE = new MedicineInfo("Antihistamine", "HistamineSuppression", MedicineInfo.MedicineType.CureSpecific, null, new string[]
		{
			"Allergies"
		}, new string[]
		{
			"DupeMosquitoBite"
		});

		// Token: 0x04005EA3 RID: 24227
		public static readonly MedicineInfo INTERMEDIATECURE = new MedicineInfo("IntermediateCure", null, MedicineInfo.MedicineType.CureSpecific, "DoctorStation", new string[]
		{
			"SlimeSickness"
		});

		// Token: 0x04005EA4 RID: 24228
		public static readonly MedicineInfo ADVANCEDCURE = new MedicineInfo("AdvancedCure", null, MedicineInfo.MedicineType.CureSpecific, "AdvancedDoctorStation", new string[]
		{
			"ZombieSickness"
		});

		// Token: 0x04005EA5 RID: 24229
		public static readonly MedicineInfo BASICRADPILL = new MedicineInfo("BasicRadPill", "Medicine_BasicRadPill", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04005EA6 RID: 24230
		public static readonly MedicineInfo INTERMEDIATERADPILL = new MedicineInfo("IntermediateRadPill", "Medicine_IntermediateRadPill", MedicineInfo.MedicineType.Booster, "AdvancedDoctorStation", null);
	}
}
