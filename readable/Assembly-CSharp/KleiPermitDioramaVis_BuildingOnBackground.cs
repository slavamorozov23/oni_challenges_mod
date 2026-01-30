using System;
using Database;
using UnityEngine;

// Token: 0x02000D53 RID: 3411
public class KleiPermitDioramaVis_BuildingOnBackground : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06006999 RID: 27033 RVA: 0x0027FD00 File Offset: 0x0027DF00
	public void ConfigureSetup()
	{
		this.buildingKAnimPrefab.gameObject.SetActive(false);
		this.buildingKAnimArray = new KBatchedAnimController[9];
		for (int i = 0; i < this.buildingKAnimArray.Length; i++)
		{
			this.buildingKAnimArray[i] = (KBatchedAnimController)UnityEngine.Object.Instantiate(this.buildingKAnimPrefab, this.buildingKAnimPrefab.transform.parent, false);
		}
		Vector2 anchoredPosition = this.buildingKAnimPrefab.rectTransform().anchoredPosition;
		Vector2 a = 175f * Vector2.one;
		Vector2 a2 = anchoredPosition + a * new Vector2(-1f, 0f);
		int num = 0;
		for (int j = 0; j < 3; j++)
		{
			int k = 0;
			while (k < 3)
			{
				this.buildingKAnimArray[num].rectTransform().anchoredPosition = a2 + a * new Vector2((float)j, (float)k);
				this.buildingKAnimArray[num].gameObject.SetActive(true);
				k++;
				num++;
			}
		}
	}

	// Token: 0x0600699A RID: 27034 RVA: 0x0027FE04 File Offset: 0x0027E004
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600699B RID: 27035 RVA: 0x0027FE0C File Offset: 0x0027E00C
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		DebugUtil.DevAssert(buildingDef.WidthInCells == 1, "assert failed", null);
		DebugUtil.DevAssert(buildingDef.HeightInCells == 1, "assert failed", null);
		KBatchedAnimController[] array = this.buildingKAnimArray;
		for (int i = 0; i < array.Length; i++)
		{
			KleiPermitVisUtil.ConfigureToRenderBuilding(array[i], buildingPermit);
		}
	}

	// Token: 0x04004898 RID: 18584
	[SerializeField]
	private KBatchedAnimController buildingKAnimPrefab;

	// Token: 0x04004899 RID: 18585
	private KBatchedAnimController[] buildingKAnimArray;
}
