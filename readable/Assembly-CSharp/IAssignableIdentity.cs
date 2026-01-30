using System;
using System.Collections.Generic;

// Token: 0x020005E9 RID: 1513
public interface IAssignableIdentity
{
	// Token: 0x060022FC RID: 8956
	string GetProperName();

	// Token: 0x060022FD RID: 8957
	List<Ownables> GetOwners();

	// Token: 0x060022FE RID: 8958
	Ownables GetSoleOwner();

	// Token: 0x060022FF RID: 8959
	bool IsNull();

	// Token: 0x06002300 RID: 8960
	bool HasOwner(Assignables owner);

	// Token: 0x06002301 RID: 8961
	int NumOwners();
}
