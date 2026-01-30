using System;
using System.Collections.Generic;

// Token: 0x02000E50 RID: 3664
public interface IMissileSelectionInterface
{
	// Token: 0x0600742A RID: 29738
	bool AmmunitionIsAllowed(Tag tag);

	// Token: 0x0600742B RID: 29739
	bool IsAnyCosmicBlastShotAllowed();

	// Token: 0x0600742C RID: 29740
	void ChangeAmmunition(Tag tag, bool allowed);

	// Token: 0x0600742D RID: 29741
	void OnRowToggleClick();

	// Token: 0x0600742E RID: 29742
	List<Tag> GetValidAmmunitionTags();
}
