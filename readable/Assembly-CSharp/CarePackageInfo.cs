using System;
using UnityEngine;

// Token: 0x02000999 RID: 2457
public class CarePackageInfo : ITelepadDeliverable
{
	// Token: 0x060046A9 RID: 18089 RVA: 0x00198F11 File Offset: 0x00197111
	public CarePackageInfo(string ID, float amount, Func<bool> requirement)
	{
		this.id = ID;
		this.quantity = amount;
		this.requirement = requirement;
	}

	// Token: 0x060046AA RID: 18090 RVA: 0x00198F2E File Offset: 0x0019712E
	public CarePackageInfo(string ID, float amount, Func<bool> requirement, string facadeID)
	{
		this.id = ID;
		this.quantity = amount;
		this.requirement = requirement;
		this.facadeID = facadeID;
	}

	// Token: 0x060046AB RID: 18091 RVA: 0x00198F54 File Offset: 0x00197154
	public GameObject Deliver(Vector3 location)
	{
		location += Vector3.right / 2f;
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(CarePackageConfig.ID), location);
		gameObject.SetActive(true);
		gameObject.GetComponent<CarePackage>().SetInfo(this);
		return gameObject;
	}

	// Token: 0x04002F8D RID: 12173
	public readonly string id;

	// Token: 0x04002F8E RID: 12174
	public readonly float quantity;

	// Token: 0x04002F8F RID: 12175
	public readonly Func<bool> requirement;

	// Token: 0x04002F90 RID: 12176
	public readonly string facadeID;
}
