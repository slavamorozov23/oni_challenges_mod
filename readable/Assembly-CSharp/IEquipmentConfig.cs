using System;
using UnityEngine;

// Token: 0x0200092B RID: 2347
public interface IEquipmentConfig
{
	// Token: 0x060041B2 RID: 16818
	EquipmentDef CreateEquipmentDef();

	// Token: 0x060041B3 RID: 16819
	void DoPostConfigure(GameObject go);

	// Token: 0x060041B4 RID: 16820 RVA: 0x001732B3 File Offset: 0x001714B3
	[Obsolete("Use IHasDlcRestrictions instead")]
	string[] GetDlcIds()
	{
		return null;
	}
}
