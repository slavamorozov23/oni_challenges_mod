using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EA4 RID: 3748
[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanetVisualizer")]
public class StarmapPlanetVisualizer : KMonoBehaviour
{
	// Token: 0x04005350 RID: 21328
	public Image image;

	// Token: 0x04005351 RID: 21329
	public LocText label;

	// Token: 0x04005352 RID: 21330
	public MultiToggle button;

	// Token: 0x04005353 RID: 21331
	public RectTransform selection;

	// Token: 0x04005354 RID: 21332
	public GameObject analysisSelection;

	// Token: 0x04005355 RID: 21333
	public Image unknownBG;

	// Token: 0x04005356 RID: 21334
	public GameObject rocketIconContainer;
}
