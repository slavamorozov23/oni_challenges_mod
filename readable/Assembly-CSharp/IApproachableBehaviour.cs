using System;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public interface IApproachableBehaviour
{
	// Token: 0x060003C3 RID: 963
	bool IsValidTarget();

	// Token: 0x060003C4 RID: 964
	GameObject GetTarget();

	// Token: 0x060003C5 RID: 965
	StatusItem GetApproachStatusItem();

	// Token: 0x060003C6 RID: 966
	StatusItem GetBehaviourStatusItem();

	// Token: 0x060003C7 RID: 967 RVA: 0x000202CE File Offset: 0x0001E4CE
	CellOffset[] GetApproachOffsets()
	{
		return OffsetGroups.Use;
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x000202D5 File Offset: 0x0001E4D5
	void OnArrive()
	{
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x000202D7 File Offset: 0x0001E4D7
	void OnSuccess()
	{
	}

	// Token: 0x060003CA RID: 970 RVA: 0x000202D9 File Offset: 0x0001E4D9
	void OnFailure()
	{
	}
}
