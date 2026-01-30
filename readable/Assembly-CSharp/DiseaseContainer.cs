using System;
using UnityEngine;

// Token: 0x020008F8 RID: 2296
public struct DiseaseContainer
{
	// Token: 0x06003FB9 RID: 16313 RVA: 0x00166B18 File Offset: 0x00164D18
	public DiseaseContainer(GameObject go, ushort elemIdx)
	{
		this.elemIdx = elemIdx;
		this.isContainer = (go.GetComponent<IUserControlledCapacity>() != null && go.GetComponent<Storage>() != null);
		Conduit component = go.GetComponent<Conduit>();
		if (component != null)
		{
			this.conduitType = component.type;
		}
		else
		{
			this.conduitType = ConduitType.None;
		}
		this.controller = go.GetComponent<KBatchedAnimController>();
		this.overpopulationCount = 1;
		this.instanceGrowthRate = 1f;
		this.accumulatedError = 0f;
		this.visualDiseaseProvider = null;
		this.autoDisinfectable = go.GetComponent<AutoDisinfectable>();
		if (this.autoDisinfectable != null)
		{
			AutoDisinfectableManager.Instance.AddAutoDisinfectable(this.autoDisinfectable);
		}
	}

	// Token: 0x06003FBA RID: 16314 RVA: 0x00166BC8 File Offset: 0x00164DC8
	public void Clear()
	{
		this.controller = null;
	}

	// Token: 0x04002774 RID: 10100
	public AutoDisinfectable autoDisinfectable;

	// Token: 0x04002775 RID: 10101
	public ushort elemIdx;

	// Token: 0x04002776 RID: 10102
	public bool isContainer;

	// Token: 0x04002777 RID: 10103
	public ConduitType conduitType;

	// Token: 0x04002778 RID: 10104
	public KBatchedAnimController controller;

	// Token: 0x04002779 RID: 10105
	public GameObject visualDiseaseProvider;

	// Token: 0x0400277A RID: 10106
	public int overpopulationCount;

	// Token: 0x0400277B RID: 10107
	public float instanceGrowthRate;

	// Token: 0x0400277C RID: 10108
	public float accumulatedError;
}
