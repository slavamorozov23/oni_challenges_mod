using System;
using UnityEngine;

// Token: 0x02000B80 RID: 2944
public interface IHexCellCollector
{
	// Token: 0x060057C2 RID: 22466
	bool CheckIsCollecting();

	// Token: 0x060057C3 RID: 22467
	string GetProperName();

	// Token: 0x060057C4 RID: 22468
	Sprite GetUISprite();

	// Token: 0x060057C5 RID: 22469
	float GetCapacity();

	// Token: 0x060057C6 RID: 22470
	float GetMassStored();

	// Token: 0x060057C7 RID: 22471
	float TimeInState();

	// Token: 0x060057C8 RID: 22472
	string GetCapacityBarText();
}
