using System;

// Token: 0x02000215 RID: 533
public class FOODDEHYDRATORTUNING
{
	// Token: 0x04000790 RID: 1936
	public const float INTERNAL_WORK_TIME = 250f;

	// Token: 0x04000791 RID: 1937
	public const float DUPE_WORK_TIME = 50f;

	// Token: 0x04000792 RID: 1938
	public const float GAS_CONSUMPTION_PER_SECOND = 0.020000001f;

	// Token: 0x04000793 RID: 1939
	public const float REQUIRED_FUEL_AMOUNT = 5.0000005f;

	// Token: 0x04000794 RID: 1940
	public const float CO2_EMIT_RATE = 0.0050000004f;

	// Token: 0x04000795 RID: 1941
	public const float CO2_EMIT_TEMPERATURE = 348.15f;

	// Token: 0x04000796 RID: 1942
	public const float PLASTIC_KG = 12f;

	// Token: 0x04000797 RID: 1943
	public const float WATER_OUTPUT_KG = 6f;

	// Token: 0x04000798 RID: 1944
	public const float FOOD_PACKETS = 6f;

	// Token: 0x04000799 RID: 1945
	public const float KCAL_PER_PACKET = 1000f;

	// Token: 0x0400079A RID: 1946
	public const float FOOD_KCAL = 6000000f;

	// Token: 0x0400079B RID: 1947
	public static Tag FUEL_TAG = SimHashes.Methane.CreateTag();
}
