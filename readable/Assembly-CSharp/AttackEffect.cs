using System;

// Token: 0x02000862 RID: 2146
[Serializable]
public class AttackEffect
{
	// Token: 0x06003AE2 RID: 15074 RVA: 0x001489C3 File Offset: 0x00146BC3
	public AttackEffect(string ID, float probability)
	{
		this.effectID = ID;
		this.effectProbability = probability;
	}

	// Token: 0x040023D2 RID: 9170
	public string effectID;

	// Token: 0x040023D3 RID: 9171
	public float effectProbability;
}
