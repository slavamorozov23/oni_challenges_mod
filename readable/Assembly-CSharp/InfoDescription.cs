using System;
using UnityEngine;

// Token: 0x02000D2C RID: 3372
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InfoDescription")]
public class InfoDescription : KMonoBehaviour
{
	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x06006828 RID: 26664 RVA: 0x00274E64 File Offset: 0x00273064
	// (set) Token: 0x06006827 RID: 26663 RVA: 0x00274E3D File Offset: 0x0027303D
	public string DescriptionLocString
	{
		get
		{
			return this.descriptionLocString;
		}
		set
		{
			this.descriptionLocString = value;
			if (this.descriptionLocString != null)
			{
				this.description = Strings.Get(this.descriptionLocString);
			}
		}
	}

	// Token: 0x06006829 RID: 26665 RVA: 0x00274E6C File Offset: 0x0027306C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (!string.IsNullOrEmpty(this.nameLocString))
		{
			this.displayName = Strings.Get(this.nameLocString);
		}
		if (!string.IsNullOrEmpty(this.descriptionLocString))
		{
			this.description = Strings.Get(this.descriptionLocString);
		}
	}

	// Token: 0x04004788 RID: 18312
	public string nameLocString = "";

	// Token: 0x04004789 RID: 18313
	private string descriptionLocString = "";

	// Token: 0x0400478A RID: 18314
	public string description;

	// Token: 0x0400478B RID: 18315
	public string effect = "";

	// Token: 0x0400478C RID: 18316
	public string displayName;
}
