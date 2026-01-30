using System;
using Database;

// Token: 0x02000C3F RID: 3135
public class EntityModifierSet : ModifierSet
{
	// Token: 0x06005ED2 RID: 24274 RVA: 0x0022AD60 File Offset: 0x00228F60
	public override void Initialize()
	{
		base.Initialize();
		this.DuplicantStatusItems = new DuplicantStatusItems(this.Root);
		this.ChoreGroups = new ChoreGroups(this.Root);
		base.LoadTraits();
	}

	// Token: 0x04003F42 RID: 16194
	public DuplicantStatusItems DuplicantStatusItems;

	// Token: 0x04003F43 RID: 16195
	public ChoreGroups ChoreGroups;
}
