using System;

// Token: 0x020002F9 RID: 761
public class ArtifactTier
{
	// Token: 0x06000F82 RID: 3970 RVA: 0x0005ACBD File Offset: 0x00058EBD
	public ArtifactTier(StringKey str_key, EffectorValues values, float payload_drop_chance)
	{
		this.decorValues = values;
		this.name_key = str_key;
		this.payloadDropChance = payload_drop_chance;
	}

	// Token: 0x04000A22 RID: 2594
	public EffectorValues decorValues;

	// Token: 0x04000A23 RID: 2595
	public StringKey name_key;

	// Token: 0x04000A24 RID: 2596
	public float payloadDropChance;
}
