using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000BEB RID: 3051
public class SubstanceTable : ScriptableObject, ISerializationCallbackReceiver
{
	// Token: 0x06005B8B RID: 23435 RVA: 0x002123B9 File Offset: 0x002105B9
	public List<Substance> GetList()
	{
		return this.list;
	}

	// Token: 0x06005B8C RID: 23436 RVA: 0x002123C4 File Offset: 0x002105C4
	public Substance GetSubstance(SimHashes substance)
	{
		int count = this.list.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.list[i].elementID == substance)
			{
				return this.list[i];
			}
		}
		return null;
	}

	// Token: 0x06005B8D RID: 23437 RVA: 0x0021240B File Offset: 0x0021060B
	public void OnBeforeSerialize()
	{
		this.BindAnimList();
	}

	// Token: 0x06005B8E RID: 23438 RVA: 0x00212413 File Offset: 0x00210613
	public void OnAfterDeserialize()
	{
		this.BindAnimList();
	}

	// Token: 0x06005B8F RID: 23439 RVA: 0x0021241C File Offset: 0x0021061C
	private void BindAnimList()
	{
		foreach (Substance substance in this.list)
		{
			if (substance.anim != null && (substance.anims == null || substance.anims.Length == 0))
			{
				substance.anims = new KAnimFile[1];
				substance.anims[0] = substance.anim;
			}
		}
	}

	// Token: 0x06005B90 RID: 23440 RVA: 0x002124A4 File Offset: 0x002106A4
	public void RemoveDuplicates()
	{
		this.list = this.list.Distinct(new SubstanceTable.SubstanceEqualityComparer()).ToList<Substance>();
	}

	// Token: 0x04003CFE RID: 15614
	[SerializeField]
	private List<Substance> list;

	// Token: 0x04003CFF RID: 15615
	public Material solidMaterial;

	// Token: 0x04003D00 RID: 15616
	public Material liquidMaterial;

	// Token: 0x02001D84 RID: 7556
	private class SubstanceEqualityComparer : IEqualityComparer<Substance>
	{
		// Token: 0x0600B14B RID: 45387 RVA: 0x003DCC69 File Offset: 0x003DAE69
		public bool Equals(Substance x, Substance y)
		{
			return x.elementID.Equals(y.elementID);
		}

		// Token: 0x0600B14C RID: 45388 RVA: 0x003DCC87 File Offset: 0x003DAE87
		public int GetHashCode(Substance obj)
		{
			return obj.elementID.GetHashCode();
		}
	}
}
