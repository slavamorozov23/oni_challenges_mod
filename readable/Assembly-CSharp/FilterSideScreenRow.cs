using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E3C RID: 3644
[AddComponentMenu("KMonoBehaviour/scripts/FilterSideScreenRow")]
public class FilterSideScreenRow : SingleItemSelectionRow
{
	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x06007395 RID: 29589 RVA: 0x002C22C4 File Offset: 0x002C04C4
	public override string InvalidTagTitle
	{
		get
		{
			return UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
		}
	}

	// Token: 0x06007396 RID: 29590 RVA: 0x002C22D0 File Offset: 0x002C04D0
	protected override void SetIcon(Sprite sprite, Color color)
	{
		if (this.icon != null)
		{
			this.icon.gameObject.SetActive(false);
		}
	}
}
