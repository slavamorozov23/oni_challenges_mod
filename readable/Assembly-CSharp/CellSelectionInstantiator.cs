using System;
using UnityEngine;

// Token: 0x0200083A RID: 2106
public class CellSelectionInstantiator : MonoBehaviour
{
	// Token: 0x06003976 RID: 14710 RVA: 0x00140C20 File Offset: 0x0013EE20
	private void Awake()
	{
		GameObject gameObject = Util.KInstantiate(this.CellSelectionPrefab, null, "WorldSelectionCollider");
		GameObject gameObject2 = Util.KInstantiate(this.CellSelectionPrefab, null, "WorldSelectionCollider");
		CellSelectionObject component = gameObject.GetComponent<CellSelectionObject>();
		CellSelectionObject component2 = gameObject2.GetComponent<CellSelectionObject>();
		component.alternateSelectionObject = component2;
		component2.alternateSelectionObject = component;
	}

	// Token: 0x0400231E RID: 8990
	public GameObject CellSelectionPrefab;
}
