using System;
using System.Collections.Generic;
using Database;

// Token: 0x02000588 RID: 1416
public abstract class BlueprintProvider : IHasDlcRestrictions
{
	// Token: 0x06001FAD RID: 8109 RVA: 0x000AB4A4 File Offset: 0x000A96A4
	protected void AddBuilding(string prefabConfigId, PermitRarity rarity, string permitId, string animFile)
	{
		this.blueprintCollection.buildingFacades.Add(new BuildingFacadeInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, prefabConfigId, animFile, null, this.requiredDlcIds, this.forbiddenDlcIds));
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x000AB518 File Offset: 0x000A9718
	protected void AddBuildingWithInteract(string prefabConfigId, PermitRarity rarity, string permitId, string animFile, Dictionary<string, string> interact_anim)
	{
		this.blueprintCollection.buildingFacades.Add(new BuildingFacadeInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, prefabConfigId, animFile, interact_anim, this.requiredDlcIds, this.forbiddenDlcIds));
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x000AB58C File Offset: 0x000A978C
	protected void AddClothing(BlueprintProvider.ClothingType clothingType, PermitRarity rarity, string permitId, string animFile)
	{
		this.blueprintCollection.clothingItems.Add(new ClothingItemInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), (PermitCategory)clothingType, rarity, animFile, this.requiredDlcIds, this.forbiddenDlcIds));
	}

	// Token: 0x06001FB0 RID: 8112 RVA: 0x000AB600 File Offset: 0x000A9800
	protected BlueprintProvider.ArtableInfoAuthoringHelper AddArtable(BlueprintProvider.ArtableType artableType, PermitRarity rarity, string permitId, string animFile)
	{
		string text;
		switch (artableType)
		{
		case BlueprintProvider.ArtableType.Painting:
			text = "Canvas";
			break;
		case BlueprintProvider.ArtableType.PaintingTall:
			text = "CanvasTall";
			break;
		case BlueprintProvider.ArtableType.PaintingWide:
			text = "CanvasWide";
			break;
		case BlueprintProvider.ArtableType.Sculpture:
			text = "Sculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureSmall:
			text = "SmallSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureIce:
			text = "IceSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureMetal:
			text = "MetalSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureMarble:
			text = "MarbleSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureWood:
			text = "WoodSculpture";
			break;
		case BlueprintProvider.ArtableType.FossilSculpture:
			text = "FossilSculpture";
			break;
		case BlueprintProvider.ArtableType.CeilingFossilSculpture:
			text = "CeilingFossilSculpture";
			break;
		default:
			text = null;
			break;
		}
		bool flag = true;
		if (text == null)
		{
			DebugUtil.DevAssert(false, "Failed to get buildingConfigId from " + artableType.ToString(), null);
			flag = false;
		}
		BlueprintProvider.ArtableInfoAuthoringHelper result;
		if (flag)
		{
			KAnimFile kanimFile;
			ArtableInfo artableInfo = new ArtableInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, animFile, (!Assets.TryGetAnim(animFile, out kanimFile)) ? null : kanimFile.GetData().GetAnim(0).name, 0, false, "error", text, "", this.requiredDlcIds, this.forbiddenDlcIds);
			result = new BlueprintProvider.ArtableInfoAuthoringHelper(artableType, artableInfo);
			result.Quality(BlueprintProvider.ArtableQuality.LookingGreat);
			this.blueprintCollection.artables.Add(artableInfo);
		}
		else
		{
			result = default(BlueprintProvider.ArtableInfoAuthoringHelper);
		}
		return result;
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x000AB77C File Offset: 0x000A997C
	protected void AddJoyResponse(BlueprintProvider.JoyResponseType joyResponseType, PermitRarity rarity, string permitId, string animFile)
	{
		if (joyResponseType == BlueprintProvider.JoyResponseType.BallonSet)
		{
			this.blueprintCollection.balloonArtistFacades.Add(new BalloonArtistFacadeInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, animFile, BalloonArtistFacadeType.ThreeSet, this.requiredDlcIds, this.forbiddenDlcIds));
			return;
		}
		throw new NotImplementedException("Missing case for " + joyResponseType.ToString());
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x000AB810 File Offset: 0x000A9A10
	protected void AddOutfit(BlueprintProvider.OutfitType outfitType, string outfitId, string[] permitIdList)
	{
		this.blueprintCollection.outfits.Add(new ClothingOutfitResource(outfitId, permitIdList, Strings.Get("STRINGS.BLUEPRINTS." + outfitId.ToUpper() + ".NAME"), (ClothingOutfitUtility.OutfitType)outfitType, this.requiredDlcIds, this.forbiddenDlcIds));
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x000AB860 File Offset: 0x000A9A60
	protected void AddMonumentPart(BlueprintProvider.MonumentPart part, PermitRarity rarity, string permitId, string animFile)
	{
		string symbolName = "";
		switch (part)
		{
		case BlueprintProvider.MonumentPart.Bottom:
			symbolName = "base";
			break;
		case BlueprintProvider.MonumentPart.Middle:
			symbolName = "mid";
			break;
		case BlueprintProvider.MonumentPart.Top:
			symbolName = "top";
			break;
		}
		this.blueprintCollection.monumentParts.Add(new MonumentPartInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, animFile, permitId.Replace("permit_", ""), symbolName, (MonumentPartResource.Part)part, this.requiredDlcIds, this.forbiddenDlcIds));
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x000AB912 File Offset: 0x000A9B12
	public virtual string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000AB91A File Offset: 0x000A9B1A
	public virtual string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x06001FB6 RID: 8118
	public abstract void SetupBlueprints();

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000AB922 File Offset: 0x000A9B22
	public void Internal_PreSetupBlueprints()
	{
		this.requiredDlcIds = this.GetRequiredDlcIds();
		this.forbiddenDlcIds = this.GetForbiddenDlcIds();
	}

	// Token: 0x0400128B RID: 4747
	public BlueprintCollection blueprintCollection;

	// Token: 0x0400128C RID: 4748
	private string[] requiredDlcIds;

	// Token: 0x0400128D RID: 4749
	private string[] forbiddenDlcIds;

	// Token: 0x0200140A RID: 5130
	public enum ArtableType
	{
		// Token: 0x04006D48 RID: 27976
		Painting,
		// Token: 0x04006D49 RID: 27977
		PaintingTall,
		// Token: 0x04006D4A RID: 27978
		PaintingWide,
		// Token: 0x04006D4B RID: 27979
		Sculpture,
		// Token: 0x04006D4C RID: 27980
		SculptureSmall,
		// Token: 0x04006D4D RID: 27981
		SculptureIce,
		// Token: 0x04006D4E RID: 27982
		SculptureMetal,
		// Token: 0x04006D4F RID: 27983
		SculptureMarble,
		// Token: 0x04006D50 RID: 27984
		SculptureWood,
		// Token: 0x04006D51 RID: 27985
		FossilSculpture,
		// Token: 0x04006D52 RID: 27986
		CeilingFossilSculpture
	}

	// Token: 0x0200140B RID: 5131
	public enum ArtableQuality
	{
		// Token: 0x04006D54 RID: 27988
		LookingGreat,
		// Token: 0x04006D55 RID: 27989
		LookingOkay,
		// Token: 0x04006D56 RID: 27990
		LookingUgly
	}

	// Token: 0x0200140C RID: 5132
	public enum ClothingType
	{
		// Token: 0x04006D58 RID: 27992
		DupeTops = 1,
		// Token: 0x04006D59 RID: 27993
		DupeBottoms,
		// Token: 0x04006D5A RID: 27994
		DupeGloves,
		// Token: 0x04006D5B RID: 27995
		DupeShoes,
		// Token: 0x04006D5C RID: 27996
		DupeHats,
		// Token: 0x04006D5D RID: 27997
		DupeAccessories,
		// Token: 0x04006D5E RID: 27998
		AtmoSuitHelmet,
		// Token: 0x04006D5F RID: 27999
		AtmoSuitBody,
		// Token: 0x04006D60 RID: 28000
		AtmoSuitGloves,
		// Token: 0x04006D61 RID: 28001
		AtmoSuitBelt,
		// Token: 0x04006D62 RID: 28002
		AtmoSuitShoes,
		// Token: 0x04006D63 RID: 28003
		JetSuitHelmet = 18,
		// Token: 0x04006D64 RID: 28004
		JetSuitBody,
		// Token: 0x04006D65 RID: 28005
		JetSuitGloves,
		// Token: 0x04006D66 RID: 28006
		JetSuitShoes
	}

	// Token: 0x0200140D RID: 5133
	public enum OutfitType
	{
		// Token: 0x04006D68 RID: 28008
		Clothing,
		// Token: 0x04006D69 RID: 28009
		AtmoSuit = 2,
		// Token: 0x04006D6A RID: 28010
		JetSuit
	}

	// Token: 0x0200140E RID: 5134
	public enum JoyResponseType
	{
		// Token: 0x04006D6C RID: 28012
		BallonSet
	}

	// Token: 0x0200140F RID: 5135
	public enum MonumentPart
	{
		// Token: 0x04006D6E RID: 28014
		Bottom,
		// Token: 0x04006D6F RID: 28015
		Top = 2,
		// Token: 0x04006D70 RID: 28016
		Middle = 1
	}

	// Token: 0x02001410 RID: 5136
	protected readonly ref struct ArtableInfoAuthoringHelper
	{
		// Token: 0x06008E76 RID: 36470 RVA: 0x0036918E File Offset: 0x0036738E
		public ArtableInfoAuthoringHelper(BlueprintProvider.ArtableType artableType, ArtableInfo artableInfo)
		{
			this.artableType = artableType;
			this.artableInfo = artableInfo;
		}

		// Token: 0x06008E77 RID: 36471 RVA: 0x003691A0 File Offset: 0x003673A0
		public void Quality(BlueprintProvider.ArtableQuality artableQuality)
		{
			if (this.artableInfo == null)
			{
				return;
			}
			int num;
			int num2;
			int num3;
			if (this.artableType == BlueprintProvider.ArtableType.SculptureWood)
			{
				num = 4;
				num2 = 8;
				num3 = 12;
			}
			else
			{
				num = 5;
				num2 = 10;
				num3 = 15;
			}
			int decor_value;
			bool cheer_on_complete;
			string status_id;
			switch (artableQuality)
			{
			case BlueprintProvider.ArtableQuality.LookingGreat:
				decor_value = num3;
				cheer_on_complete = true;
				status_id = "LookingGreat";
				break;
			case BlueprintProvider.ArtableQuality.LookingOkay:
				decor_value = num2;
				cheer_on_complete = false;
				status_id = "LookingOkay";
				break;
			case BlueprintProvider.ArtableQuality.LookingUgly:
				decor_value = num;
				cheer_on_complete = false;
				status_id = "LookingUgly";
				break;
			default:
				throw new ArgumentException();
			}
			this.artableInfo.decor_value = decor_value;
			this.artableInfo.cheer_on_complete = cheer_on_complete;
			this.artableInfo.status_id = status_id;
		}

		// Token: 0x04006D71 RID: 28017
		private readonly BlueprintProvider.ArtableType artableType;

		// Token: 0x04006D72 RID: 28018
		private readonly ArtableInfo artableInfo;
	}
}
