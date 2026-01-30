using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008A3 RID: 2211
public interface IPlantConsumeEntities
{
	// Token: 0x06003CCF RID: 15567
	string GetConsumableEntitiesCategoryName();

	// Token: 0x06003CD0 RID: 15568
	string GetRequirementText();

	// Token: 0x06003CD1 RID: 15569
	List<KPrefabID> GetPrefabsOfPossiblePrey();

	// Token: 0x06003CD2 RID: 15570
	string[] GetFormattedPossiblePreyList();

	// Token: 0x06003CD3 RID: 15571
	bool IsEntityEdible(GameObject entity);

	// Token: 0x06003CD4 RID: 15572
	string GetConsumedEntityName();

	// Token: 0x06003CD5 RID: 15573
	bool AreEntitiesConsumptionRequirementsSatisfied();
}
