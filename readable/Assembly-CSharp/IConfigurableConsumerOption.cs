using System;
using UnityEngine;

// Token: 0x02000E30 RID: 3632
public interface IConfigurableConsumerOption
{
	// Token: 0x0600734B RID: 29515
	Tag GetID();

	// Token: 0x0600734C RID: 29516
	string GetName();

	// Token: 0x0600734D RID: 29517
	string GetDetailedDescription();

	// Token: 0x0600734E RID: 29518
	string GetDescription();

	// Token: 0x0600734F RID: 29519
	Sprite GetIcon();

	// Token: 0x06007350 RID: 29520
	IConfigurableConsumerIngredient[] GetIngredients();
}
