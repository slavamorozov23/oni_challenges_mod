using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D51 RID: 3409
public class KleiPermitDioramaVis_AutomationGates : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06006991 RID: 27025 RVA: 0x0027FB50 File Offset: 0x0027DD50
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06006992 RID: 27026 RVA: 0x0027FB58 File Offset: 0x0027DD58
	public void ConfigureSetup()
	{
	}

	// Token: 0x06006993 RID: 27027 RVA: 0x0027FB5C File Offset: 0x0027DD5C
	public void ConfigureWith(PermitResource permit)
	{
		this.itemSprite.gameObject.SetActive(false);
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		Dictionary<int, float> dictionary = new Dictionary<int, float>
		{
			{
				3,
				0.7f
			},
			{
				2,
				0.9f
			},
			{
				1,
				0.85f
			}
		};
		Dictionary<int, float> dictionary2 = new Dictionary<int, float>
		{
			{
				4,
				32f
			},
			{
				3,
				32f
			},
			{
				2,
				32f
			},
			{
				1,
				96f
			}
		};
		this.buildingKAnimPosition.SetOn(this.buildingKAnim);
		this.buildingKAnim.rectTransform().localScale = Vector3.one * dictionary[buildingDef.WidthInCells];
		this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, dictionary2[buildingDef.HeightInCells]);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater), "place");
	}

	// Token: 0x04004893 RID: 18579
	[SerializeField]
	private Image itemSprite;

	// Token: 0x04004894 RID: 18580
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04004895 RID: 18581
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
