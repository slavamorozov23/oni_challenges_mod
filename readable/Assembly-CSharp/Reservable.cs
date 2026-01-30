using System;
using UnityEngine;

// Token: 0x02000B07 RID: 2823
[AddComponentMenu("KMonoBehaviour/scripts/Reservable")]
public class Reservable : KMonoBehaviour
{
	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x0600523A RID: 21050 RVA: 0x001DD76D File Offset: 0x001DB96D
	public GameObject ReservedBy
	{
		get
		{
			return this.reservedBy;
		}
	}

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x0600523B RID: 21051 RVA: 0x001DD775 File Offset: 0x001DB975
	public bool IsReserved
	{
		get
		{
			return this.reservedBy != null;
		}
	}

	// Token: 0x0600523C RID: 21052 RVA: 0x001DD783 File Offset: 0x001DB983
	public bool Reserve(GameObject reserver)
	{
		if (this.reservedBy == null)
		{
			this.reservedBy = reserver;
			return true;
		}
		return false;
	}

	// Token: 0x0600523D RID: 21053 RVA: 0x001DD79D File Offset: 0x001DB99D
	public void ClearReservation()
	{
		this.reservedBy = null;
	}

	// Token: 0x0600523E RID: 21054 RVA: 0x001DD7A6 File Offset: 0x001DB9A6
	public bool IsReservableBy(GameObject reserver)
	{
		return this.reservedBy == null || this.reservedBy == reserver;
	}

	// Token: 0x0400379C RID: 14236
	private GameObject reservedBy;
}
