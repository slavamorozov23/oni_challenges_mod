using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x02000716 RID: 1814
[SerializationConfig(MemberSerialization.OptIn)]
public class BuildingFacade : KMonoBehaviour
{
	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06002D41 RID: 11585 RVA: 0x001065FE File Offset: 0x001047FE
	public string CurrentFacade
	{
		get
		{
			return this.currentFacade;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06002D42 RID: 11586 RVA: 0x00106606 File Offset: 0x00104806
	public bool IsOriginal
	{
		get
		{
			return this.currentFacade.IsNullOrWhiteSpace();
		}
	}

	// Token: 0x06002D43 RID: 11587 RVA: 0x00106613 File Offset: 0x00104813
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x06002D44 RID: 11588 RVA: 0x00106615 File Offset: 0x00104815
	protected override void OnSpawn()
	{
		if (!this.IsOriginal)
		{
			this.ApplyBuildingFacade(Db.GetBuildingFacades().TryGet(this.currentFacade), false);
		}
	}

	// Token: 0x06002D45 RID: 11589 RVA: 0x00106636 File Offset: 0x00104836
	public void ApplyDefaultFacade(bool shouldTryAnimate = false)
	{
		this.currentFacade = "DEFAULT_FACADE";
		this.ClearFacade(shouldTryAnimate);
	}

	// Token: 0x06002D46 RID: 11590 RVA: 0x0010664C File Offset: 0x0010484C
	public void ApplyBuildingFacade(BuildingFacadeResource facade, bool shouldTryAnimate = false)
	{
		if (facade == null)
		{
			this.ClearFacade(false);
			return;
		}
		this.currentFacade = facade.Id;
		KAnimFile[] array = new KAnimFile[]
		{
			Assets.GetAnim(facade.AnimFile)
		};
		this.ChangeBuilding(array, facade.Name, facade.Description, facade.InteractFile, shouldTryAnimate);
	}

	// Token: 0x06002D47 RID: 11591 RVA: 0x001066A4 File Offset: 0x001048A4
	private void ClearFacade(bool shouldTryAnimate = false)
	{
		Building component = base.GetComponent<Building>();
		this.ChangeBuilding(component.Def.AnimFiles, component.Def.Name, component.Def.Desc, null, shouldTryAnimate);
	}

	// Token: 0x06002D48 RID: 11592 RVA: 0x001066E4 File Offset: 0x001048E4
	private void ChangeBuilding(KAnimFile[] animFiles, string displayName, string desc, Dictionary<string, string> interactAnimsNames = null, bool shouldTryAnimate = false)
	{
		this.interactAnims.Clear();
		if (interactAnimsNames != null && interactAnimsNames.Count > 0)
		{
			this.interactAnims = new Dictionary<string, KAnimFile[]>();
			foreach (KeyValuePair<string, string> keyValuePair in interactAnimsNames)
			{
				this.interactAnims.Add(keyValuePair.Key, new KAnimFile[]
				{
					Assets.GetAnim(keyValuePair.Value)
				});
			}
		}
		Building[] components = base.GetComponents<Building>();
		foreach (Building building in components)
		{
			KBatchedAnimController component = building.GetComponent<KBatchedAnimController>();
			HashedString batchGroupID = component.batchGroupID;
			component.SwapAnims(animFiles);
			foreach (KBatchedAnimController kbatchedAnimController in building.GetComponentsInChildren<KBatchedAnimController>(true))
			{
				if (kbatchedAnimController.batchGroupID == batchGroupID)
				{
					kbatchedAnimController.SwapAnims(animFiles);
				}
			}
			if (!this.animateIn.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(this.animateIn);
				this.animateIn = null;
			}
			if (shouldTryAnimate)
			{
				this.animateIn = BuildingFacadeAnimateIn.MakeFor(component);
				string parameter = "Unlocked";
				float parameterValue = 1f;
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound(KleiInventoryScreen.GetFacadeItemSoundName(Db.Get().Permits.TryGet(this.currentFacade)) + "_Click", false), parameter, parameterValue);
			}
		}
		base.GetComponent<KSelectable>().SetName(displayName);
		if (base.GetComponent<AnimTileable>() != null && components.Length != 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(components[0].GetExtents(), GameScenePartitioner.Instance.objectLayers[1], null);
		}
	}

	// Token: 0x06002D49 RID: 11593 RVA: 0x001068A4 File Offset: 0x00104AA4
	public string GetNextFacade()
	{
		BuildingDef def = base.GetComponent<Building>().Def;
		int num = def.AvailableFacades.FindIndex((string s) => s == this.currentFacade) + 1;
		if (num >= def.AvailableFacades.Count)
		{
			num = 0;
		}
		return def.AvailableFacades[num];
	}

	// Token: 0x04001AE9 RID: 6889
	[Serialize]
	private string currentFacade;

	// Token: 0x04001AEA RID: 6890
	public KAnimFile[] animFiles;

	// Token: 0x04001AEB RID: 6891
	public Dictionary<string, KAnimFile[]> interactAnims = new Dictionary<string, KAnimFile[]>();

	// Token: 0x04001AEC RID: 6892
	private BuildingFacadeAnimateIn animateIn;
}
