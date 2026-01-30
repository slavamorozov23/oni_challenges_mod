using System;
using Database;
using UnityEngine;

// Token: 0x02000D5F RID: 3423
public static class KleiPermitVisUtil
{
	// Token: 0x060069DD RID: 27101 RVA: 0x00280964 File Offset: 0x0027EB64
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingFacadeResource buildingPermit)
	{
		KAnimFile anim = Assets.GetAnim(buildingPermit.AnimFile);
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			anim
		});
		buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(anim), KAnim.PlayMode.Loop, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x060069DE RID: 27102 RVA: 0x002809CC File Offset: 0x0027EBCC
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingDef buildingDef)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(buildingDef.AnimFiles);
		buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(buildingDef.AnimFiles[0]), KAnim.PlayMode.Loop, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x060069DF RID: 27103 RVA: 0x00280A24 File Offset: 0x0027EC24
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, ArtableStage artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			Assets.GetAnim(artablePermit.animFile)
		});
		buildingKAnim.Play(artablePermit.anim, KAnim.PlayMode.Once, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x060069E0 RID: 27104 RVA: 0x00280A8C File Offset: 0x0027EC8C
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, DbStickerBomb artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			artablePermit.animFile
		});
		HashedString defaultStickerAnimHash = KleiPermitVisUtil.GetDefaultStickerAnimHash(artablePermit.animFile);
		if (defaultStickerAnimHash != null)
		{
			buildingKAnim.Play(defaultStickerAnimHash, KAnim.PlayMode.Once, 1f, 0f);
		}
		else
		{
			global::Debug.Assert(false, "Couldn't find default sticker for sticker " + artablePermit.Id);
			buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(artablePermit.animFile), KAnim.PlayMode.Once, 1f, 0f);
		}
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x060069E1 RID: 27105 RVA: 0x00280B30 File Offset: 0x0027ED30
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, MonumentPartResource monumentPermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			monumentPermit.AnimFile
		});
		buildingKAnim.Play(monumentPermit.State, KAnim.PlayMode.Once, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x060069E2 RID: 27106 RVA: 0x00280B90 File Offset: 0x0027ED90
	public static void ConfigureBuildingPosition(RectTransform transform, PrefabDefinedUIPosition anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		anchorPosition.SetOn(transform);
		transform.anchoredPosition += new Vector2(176f * (float)buildingDef.WidthInCells * -(alignment.x - 0.5f), 176f * (float)buildingDef.HeightInCells * -alignment.y);
	}

	// Token: 0x060069E3 RID: 27107 RVA: 0x00280BEC File Offset: 0x0027EDEC
	public static void ConfigureBuildingPosition(RectTransform transform, Vector2 anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		transform.anchoredPosition = anchorPosition + new Vector2(176f * (float)buildingDef.WidthInCells * -(alignment.x - 0.5f), 176f * (float)buildingDef.HeightInCells * -alignment.y);
	}

	// Token: 0x060069E4 RID: 27108 RVA: 0x00280C3A File Offset: 0x0027EE3A
	public static void ClearAnimation()
	{
		if (!KleiPermitVisUtil.buildingAnimateIn.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(KleiPermitVisUtil.buildingAnimateIn.gameObject);
		}
	}

	// Token: 0x060069E5 RID: 27109 RVA: 0x00280C57 File Offset: 0x0027EE57
	public static void AnimateIn(KBatchedAnimController buildingKAnim, Updater extraUpdater = default(Updater), string place_anim = "place")
	{
		KleiPermitVisUtil.ClearAnimation();
		KleiPermitVisUtil.buildingAnimateIn = KleiPermitBuildingAnimateIn.MakeFor(buildingKAnim, extraUpdater, place_anim);
	}

	// Token: 0x060069E6 RID: 27110 RVA: 0x00280C6B File Offset: 0x0027EE6B
	public static HashedString GetFirstAnimHash(KAnimFile animFile)
	{
		return animFile.GetData().GetAnim(0).hash;
	}

	// Token: 0x060069E7 RID: 27111 RVA: 0x00280C80 File Offset: 0x0027EE80
	public static HashedString GetDefaultStickerAnimHash(KAnimFile stickerAnimFile)
	{
		KAnimFileData data = stickerAnimFile.GetData();
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim = data.GetAnim(i);
			if (anim.name.StartsWith("idle_sticker"))
			{
				return anim.hash;
			}
		}
		return null;
	}

	// Token: 0x060069E8 RID: 27112 RVA: 0x00280CCC File Offset: 0x0027EECC
	public static BuildLocationRule? GetBuildLocationRule(PermitResource permit)
	{
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		if (buildingDef == null)
		{
			return null;
		}
		return new BuildLocationRule?(buildingDef.BuildLocationRule);
	}

	// Token: 0x060069E9 RID: 27113 RVA: 0x00280D00 File Offset: 0x0027EF00
	public static BuildingDef GetBuildingDef(PermitResource permit)
	{
		BuildingFacadeResource buildingFacadeResource = permit as BuildingFacadeResource;
		if (buildingFacadeResource != null)
		{
			GameObject gameObject = Assets.TryGetPrefab(buildingFacadeResource.PrefabID);
			if (gameObject == null)
			{
				return null;
			}
			BuildingComplete component = gameObject.GetComponent<BuildingComplete>();
			if (component == null || !component)
			{
				return null;
			}
			return component.Def;
		}
		else
		{
			ArtableStage artableStage = permit as ArtableStage;
			if (artableStage != null)
			{
				BuildingComplete component2 = Assets.GetPrefab(artableStage.prefabId).GetComponent<BuildingComplete>();
				if (component2 == null || !component2)
				{
					return null;
				}
				return component2.Def;
			}
			else
			{
				if (!(permit is MonumentPartResource))
				{
					return null;
				}
				BuildingComplete component3 = Assets.GetPrefab("MonumentBottom").GetComponent<BuildingComplete>();
				if (component3 == null || !component3)
				{
					return null;
				}
				return component3.Def;
			}
		}
	}

	// Token: 0x040048C4 RID: 18628
	public const float TILE_SIZE_UI = 176f;

	// Token: 0x040048C5 RID: 18629
	public static KleiPermitBuildingAnimateIn buildingAnimateIn;
}
