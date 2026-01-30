using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000AB3 RID: 2739
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PreventFOWRevealTracker")]
public class PreventFOWRevealTracker : KMonoBehaviour
{
	// Token: 0x06004F7D RID: 20349 RVA: 0x001CD8E8 File Offset: 0x001CBAE8
	[OnSerializing]
	private void OnSerialize()
	{
		this.preventFOWRevealCells.Clear();
		for (int i = 0; i < Grid.VisMasks.Length; i++)
		{
			if (Grid.PreventFogOfWarReveal[i])
			{
				this.preventFOWRevealCells.Add(i);
			}
		}
	}

	// Token: 0x06004F7E RID: 20350 RVA: 0x001CD92C File Offset: 0x001CBB2C
	[OnDeserialized]
	private void OnDeserialized()
	{
		foreach (int i in this.preventFOWRevealCells)
		{
			Grid.PreventFogOfWarReveal[i] = true;
		}
	}

	// Token: 0x04003522 RID: 13602
	[Serialize]
	public List<int> preventFOWRevealCells;
}
