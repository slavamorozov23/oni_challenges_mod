using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D4C RID: 3404
public class KleiPermitDioramaVis : KMonoBehaviour
{
	// Token: 0x06006979 RID: 27001 RVA: 0x0027F1A7 File Offset: 0x0027D3A7
	protected override void OnPrefabInit()
	{
		this.Init();
	}

	// Token: 0x0600697A RID: 27002 RVA: 0x0027F1B0 File Offset: 0x0027D3B0
	private void Init()
	{
		if (this.initComplete)
		{
			return;
		}
		this.allVisList = ReflectionUtil.For<KleiPermitDioramaVis>(this).CollectValuesForFieldsThatInheritOrImplement<IKleiPermitDioramaVisTarget>(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		foreach (IKleiPermitDioramaVisTarget kleiPermitDioramaVisTarget in this.allVisList)
		{
			kleiPermitDioramaVisTarget.ConfigureSetup();
		}
		this.initComplete = true;
	}

	// Token: 0x0600697B RID: 27003 RVA: 0x0027F220 File Offset: 0x0027D420
	public void ConfigureWith(PermitResource permit)
	{
		if (!this.initComplete)
		{
			this.Init();
		}
		foreach (IKleiPermitDioramaVisTarget kleiPermitDioramaVisTarget in this.allVisList)
		{
			kleiPermitDioramaVisTarget.GetGameObject().SetActive(false);
		}
		KleiPermitVisUtil.ClearAnimation();
		IKleiPermitDioramaVisTarget permitVisTarget = this.GetPermitVisTarget(permit);
		permitVisTarget.GetGameObject().SetActive(true);
		permitVisTarget.ConfigureWith(permit);
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			this.dlcImage.gameObject.SetActive(true);
			this.dlcImage.sprite = Assets.GetSprite(DlcManager.GetDlcLargeLogo(dlcIdFrom));
			return;
		}
		this.dlcImage.gameObject.SetActive(false);
	}

	// Token: 0x0600697C RID: 27004 RVA: 0x0027F2EC File Offset: 0x0027D4EC
	private IKleiPermitDioramaVisTarget GetPermitVisTarget(PermitResource permit)
	{
		KleiPermitDioramaVis.lastRenderedPermit = permit;
		if (permit == null)
		{
			return this.fallbackVis.WithError(string.Format("Given invalid permit: {0}", permit));
		}
		if (permit.Category == PermitCategory.Equipment || permit.Category == PermitCategory.DupeTops || permit.Category == PermitCategory.DupeBottoms || permit.Category == PermitCategory.DupeGloves || permit.Category == PermitCategory.DupeShoes || permit.Category == PermitCategory.DupeHats || permit.Category == PermitCategory.DupeAccessories || permit.Category == PermitCategory.AtmoSuitHelmet || permit.Category == PermitCategory.AtmoSuitBody || permit.Category == PermitCategory.AtmoSuitGloves || permit.Category == PermitCategory.AtmoSuitBelt || permit.Category == PermitCategory.AtmoSuitShoes || permit.Category == PermitCategory.JetSuitHelmet || permit.Category == PermitCategory.JetSuitBody || permit.Category == PermitCategory.JetSuitGloves || permit.Category == PermitCategory.JetSuitShoes)
		{
			return this.equipmentVis;
		}
		if (permit.Category == PermitCategory.Building)
		{
			BuildLocationRule? buildLocationRule = KleiPermitVisUtil.GetBuildLocationRule(permit);
			BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef == null || !buildingDef.BuildingComplete.GetComponent<Bed>().IsNullOrDestroyed())
			{
				return this.buildingOnFloorVis;
			}
			BuildingFacadeResource buildingFacadeResource = permit as BuildingFacadeResource;
			if (buildingFacadeResource != null)
			{
				if (buildingFacadeResource.PrefabID.Contains("Wire") || buildingFacadeResource.PrefabID.Contains("Ribbon"))
				{
					return this.buildingWiresAndAutomationVis;
				}
				if (buildingFacadeResource.PrefabID.Contains("Logic"))
				{
					return this.buildingAutomationGatesVis;
				}
			}
			if (buildingDef.PrefabID == "RockCrusher" || buildingDef.PrefabID == "GasReservoir" || buildingDef.PrefabID == "ArcadeMachine" || buildingDef.PrefabID == "MicrobeMusher" || buildingDef.PrefabID == "FlushToilet" || buildingDef.PrefabID == "WashSink" || buildingDef.PrefabID == "Headquarters" || buildingDef.PrefabID == "GourmetCookingStation" || buildingDef.PrefabID == "ExobaseHeadquarters" || buildingDef.PrefabID == "SteamTurbine2" || buildingDef.PrefabID == "Generator" || buildingDef.PrefabID == "ResetSkillsStation" || buildingDef.PrefabID == "MetalRefinery" || buildingDef.PrefabID == "WaterPurifier")
			{
				return this.buildingOnFloorBigVis;
			}
			if (!buildingDef.BuildingComplete.GetComponent<RocketModule>().IsNullOrDestroyed() || !buildingDef.BuildingComplete.GetComponent<RocketEngine>().IsNullOrDestroyed())
			{
				return this.buildingRocketVis;
			}
			if (buildingDef.PrefabID == "PlanterBox" || buildingDef.PrefabID == "FlowerVase")
			{
				return this.buildingOnFloorBotanicalVis;
			}
			if (buildingDef.PrefabID == "ExteriorWall")
			{
				return this.wallpaperVis;
			}
			if (buildingDef.PrefabID == "FlowerVaseHanging" || buildingDef.PrefabID == "FlowerVaseHangingFancy")
			{
				return this.buildingHangingHookBotanicalVis;
			}
			if (buildLocationRule != null)
			{
				BuildLocationRule valueOrDefault = buildLocationRule.GetValueOrDefault();
				switch (valueOrDefault)
				{
				case BuildLocationRule.Anywhere:
				case BuildLocationRule.OnFloor:
					break;
				case BuildLocationRule.OnFloorOverSpace:
					goto IL_370;
				case BuildLocationRule.OnCeiling:
					return this.buildingOnCeilingVis.WithAlignment(Alignment.Top());
				case BuildLocationRule.OnWall:
					return this.buildingOnWallVis.WithAlignment(Alignment.Left());
				case BuildLocationRule.InCorner:
					return this.buildingInCeilingCornerVis.WithAlignment(Alignment.TopLeft());
				default:
					if (valueOrDefault != BuildLocationRule.OnFoundationRotatable)
					{
						goto IL_370;
					}
					break;
				}
				return this.buildingOnFloorVis;
			}
			IL_370:
			return this.fallbackVis.WithError(string.Format("No visualization available for building with BuildLocationRule of {0}", buildLocationRule));
		}
		else if (permit.Category == PermitCategory.Artwork)
		{
			BuildingDef buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef2.IsNullOrDestroyed())
			{
				return this.fallbackVis.WithError("Couldn't find building def for Artable " + permit.Id);
			}
			if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|24_0<Sculpture>(buildingDef2))
			{
				if (buildingDef2.PrefabID == "WoodSculpture")
				{
					return this.artablePaintingVis;
				}
				return this.artableSculptureVis;
			}
			else
			{
				if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|24_0<Painting>(buildingDef2))
				{
					return this.artablePaintingVis;
				}
				if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|24_0<MonumentPart>(buildingDef2))
				{
					return this.monumentPartVis;
				}
				return this.fallbackVis.WithError("No visualization available for Artable " + permit.Id);
			}
		}
		else
		{
			if (permit.Category != PermitCategory.JoyResponse)
			{
				return this.fallbackVis.WithError("No visualization has been defined for permit with id \"" + permit.Id + "\"");
			}
			if (permit is BalloonArtistFacadeResource)
			{
				return this.joyResponseBalloonVis;
			}
			return this.fallbackVis.WithError("No visualization available for JoyResponse " + permit.Id);
		}
	}

	// Token: 0x0600697D RID: 27005 RVA: 0x0027F77C File Offset: 0x0027D97C
	public static Sprite GetDioramaBackground(PermitCategory permitCategory)
	{
		switch (permitCategory)
		{
		case PermitCategory.DupeTops:
		case PermitCategory.DupeBottoms:
		case PermitCategory.DupeGloves:
		case PermitCategory.DupeShoes:
		case PermitCategory.DupeHats:
		case PermitCategory.DupeAccessories:
			return Assets.GetSprite("screen_bg_clothing");
		case PermitCategory.AtmoSuitHelmet:
		case PermitCategory.AtmoSuitBody:
		case PermitCategory.AtmoSuitGloves:
		case PermitCategory.AtmoSuitBelt:
		case PermitCategory.AtmoSuitShoes:
		case PermitCategory.JetSuitHelmet:
		case PermitCategory.JetSuitBody:
		case PermitCategory.JetSuitGloves:
		case PermitCategory.JetSuitShoes:
			return Assets.GetSprite("screen_bg_atmosuit");
		case PermitCategory.Building:
			return Assets.GetSprite("screen_bg_buildings");
		case PermitCategory.Artwork:
			return Assets.GetSprite("screen_bg_art");
		case PermitCategory.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		}
		return null;
	}

	// Token: 0x0600697E RID: 27006 RVA: 0x0027F838 File Offset: 0x0027DA38
	public static Sprite GetDioramaBackground(ClothingOutfitUtility.OutfitType outfitType)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return Assets.GetSprite("screen_bg_clothing");
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
		case ClothingOutfitUtility.OutfitType.JetSuit:
			return Assets.GetSprite("screen_bg_atmosuit");
		default:
			return null;
		}
	}

	// Token: 0x06006980 RID: 27008 RVA: 0x0027F896 File Offset: 0x0027DA96
	[CompilerGenerated]
	internal static bool <GetPermitVisTarget>g__Has|24_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	// Token: 0x04004875 RID: 18549
	[SerializeField]
	private Image dlcImage;

	// Token: 0x04004876 RID: 18550
	[SerializeField]
	private KleiPermitDioramaVis_Fallback fallbackVis;

	// Token: 0x04004877 RID: 18551
	[SerializeField]
	private KleiPermitDioramaVis_DupeEquipment equipmentVis;

	// Token: 0x04004878 RID: 18552
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorVis;

	// Token: 0x04004879 RID: 18553
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloorBig buildingOnFloorBigVis;

	// Token: 0x0400487A RID: 18554
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingOnWallVis;

	// Token: 0x0400487B RID: 18555
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingOnCeilingVis;

	// Token: 0x0400487C RID: 18556
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingInCeilingCornerVis;

	// Token: 0x0400487D RID: 18557
	[SerializeField]
	private KleiPermitDioramaVis_BuildingRocket buildingRocketVis;

	// Token: 0x0400487E RID: 18558
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorBotanicalVis;

	// Token: 0x0400487F RID: 18559
	[SerializeField]
	private KleiPermitDioramaVis_BuildingHangingHook buildingHangingHookBotanicalVis;

	// Token: 0x04004880 RID: 18560
	[SerializeField]
	private KleiPermitDioramaVis_WiresAndAutomation buildingWiresAndAutomationVis;

	// Token: 0x04004881 RID: 18561
	[SerializeField]
	private KleiPermitDioramaVis_AutomationGates buildingAutomationGatesVis;

	// Token: 0x04004882 RID: 18562
	[SerializeField]
	private KleiPermitDioramaVis_Wallpaper wallpaperVis;

	// Token: 0x04004883 RID: 18563
	[SerializeField]
	private KleiPermitDioramaVis_ArtablePainting artablePaintingVis;

	// Token: 0x04004884 RID: 18564
	[SerializeField]
	private KleiPermitDioramaVis_ArtableSculpture artableSculptureVis;

	// Token: 0x04004885 RID: 18565
	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon joyResponseBalloonVis;

	// Token: 0x04004886 RID: 18566
	[SerializeField]
	private KleiPermitDioramaVis_MonumentPart monumentPartVis;

	// Token: 0x04004887 RID: 18567
	private bool initComplete;

	// Token: 0x04004888 RID: 18568
	private IReadOnlyList<IKleiPermitDioramaVisTarget> allVisList;

	// Token: 0x04004889 RID: 18569
	public static PermitResource lastRenderedPermit;
}
