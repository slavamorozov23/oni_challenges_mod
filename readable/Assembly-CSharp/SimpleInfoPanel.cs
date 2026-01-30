using System;
using UnityEngine;

// Token: 0x02000D00 RID: 3328
public class SimpleInfoPanel
{
	// Token: 0x060066D8 RID: 26328 RVA: 0x0026B8C7 File Offset: 0x00269AC7
	public SimpleInfoPanel(SimpleInfoScreen simpleInfoRoot)
	{
		this.simpleInfoRoot = simpleInfoRoot;
	}

	// Token: 0x060066D9 RID: 26329 RVA: 0x0026B8D6 File Offset: 0x00269AD6
	public virtual void Refresh(CollapsibleDetailContentPanel panel, GameObject selectedTarget)
	{
	}

	// Token: 0x0400465A RID: 18010
	protected SimpleInfoScreen simpleInfoRoot;
}
