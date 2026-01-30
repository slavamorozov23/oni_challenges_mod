using System;
using KSerialization;

// Token: 0x0200093E RID: 2366
[SerializationConfig(MemberSerialization.OptIn)]
public class CellEventInstance : EventInstanceBase, ISaveLoadable
{
	// Token: 0x0600421A RID: 16922 RVA: 0x00174BFE File Offset: 0x00172DFE
	public CellEventInstance(int cell, int data, int data2, CellEvent ev) : base(ev)
	{
		this.cell = cell;
		this.data = data;
		this.data2 = data2;
	}

	// Token: 0x04002942 RID: 10562
	[Serialize]
	public int cell;

	// Token: 0x04002943 RID: 10563
	[Serialize]
	public int data;

	// Token: 0x04002944 RID: 10564
	[Serialize]
	public int data2;
}
