using System;
using System.Collections.Generic;
using System.IO;
using KMod;
using TUNING;
using UnityEngine;

// Token: 0x02000352 RID: 850
public static class ModUtil
{
	// Token: 0x060011BA RID: 4538 RVA: 0x00068280 File Offset: 0x00066480
	public static void AddBuildingToPlanScreen(HashedString category, string building_id)
	{
		ModUtil.AddBuildingToPlanScreen(category, building_id, "uncategorized");
	}

	// Token: 0x060011BB RID: 4539 RVA: 0x0006828E File Offset: 0x0006648E
	public static void AddBuildingToPlanScreen(HashedString category, string building_id, string subcategoryID)
	{
		ModUtil.AddBuildingToPlanScreen(category, building_id, subcategoryID, null, ModUtil.BuildingOrdering.After);
	}

	// Token: 0x060011BC RID: 4540 RVA: 0x0006829C File Offset: 0x0006649C
	public static void AddBuildingToPlanScreen(HashedString category, string building_id, string subcategoryID, string relativeBuildingId, ModUtil.BuildingOrdering ordering = ModUtil.BuildingOrdering.After)
	{
		int num = BUILDINGS.PLANORDER.FindIndex((PlanScreen.PlanInfo x) => x.category == category);
		if (num < 0)
		{
			global::Debug.LogWarning(string.Format("Mod: Unable to add '{0}' as category '{1}' does not exist", building_id, category));
			return;
		}
		List<KeyValuePair<string, string>> buildingAndSubcategoryData = BUILDINGS.PLANORDER[num].buildingAndSubcategoryData;
		KeyValuePair<string, string> item = new KeyValuePair<string, string>(building_id, subcategoryID);
		if (relativeBuildingId == null)
		{
			buildingAndSubcategoryData.Add(item);
			return;
		}
		int num2 = buildingAndSubcategoryData.FindIndex((KeyValuePair<string, string> x) => x.Key == relativeBuildingId);
		if (num2 == -1)
		{
			buildingAndSubcategoryData.Add(item);
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Mod: Building '",
				relativeBuildingId,
				"' doesn't exist, inserting '",
				building_id,
				"' at the end of the list instead"
			}));
			return;
		}
		int index = (ordering == ModUtil.BuildingOrdering.After) ? (num2 + 1) : Mathf.Max(num2, 0);
		buildingAndSubcategoryData.Insert(index, item);
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x00068394 File Offset: 0x00066594
	[Obsolete("Use PlanScreen instead")]
	public static void AddBuildingToHotkeyBuildMenu(HashedString category, string building_id, global::Action hotkey)
	{
		BuildMenu.DisplayInfo info = BuildMenu.OrderedBuildings.GetInfo(category);
		if (info.category != category)
		{
			return;
		}
		(info.data as IList<BuildMenu.BuildingInfo>).Add(new BuildMenu.BuildingInfo(building_id, hotkey));
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x000683D4 File Offset: 0x000665D4
	public static KAnimFile AddKAnimMod(string name, KAnimFile.Mod anim_mod)
	{
		KAnimFile kanimFile = ScriptableObject.CreateInstance<KAnimFile>();
		kanimFile.mod = anim_mod;
		kanimFile.name = name;
		AnimCommandFile animCommandFile = new AnimCommandFile();
		KAnimGroupFile.GroupFile groupFile = new KAnimGroupFile.GroupFile();
		groupFile.groupID = animCommandFile.GetGroupName(kanimFile);
		groupFile.commandDirectory = "assets/" + name;
		animCommandFile.AddGroupFile(groupFile);
		if (KAnimGroupFile.GetGroupFile().AddAnimMod(groupFile, animCommandFile, kanimFile) == KAnimGroupFile.AddModResult.Added)
		{
			Assets.ModLoadedKAnims.Add(kanimFile);
		}
		return kanimFile;
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x00068444 File Offset: 0x00066644
	public static KAnimFile AddKAnim(string name, TextAsset anim_file, TextAsset build_file, IList<Texture2D> textures)
	{
		KAnimFile kanimFile = ScriptableObject.CreateInstance<KAnimFile>();
		kanimFile.Initialize(anim_file, build_file, textures);
		kanimFile.name = name;
		AnimCommandFile animCommandFile = new AnimCommandFile();
		KAnimGroupFile.GroupFile groupFile = new KAnimGroupFile.GroupFile();
		groupFile.groupID = animCommandFile.GetGroupName(kanimFile);
		groupFile.commandDirectory = "assets/" + name;
		animCommandFile.AddGroupFile(groupFile);
		KAnimGroupFile.GetGroupFile().AddAnimFile(groupFile, animCommandFile, kanimFile);
		Assets.ModLoadedKAnims.Add(kanimFile);
		return kanimFile;
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x000684B4 File Offset: 0x000666B4
	public static KAnimFile AddKAnim(string name, TextAsset anim_file, TextAsset build_file, Texture2D texture)
	{
		return ModUtil.AddKAnim(name, anim_file, build_file, new List<Texture2D>
		{
			texture
		});
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x000684D8 File Offset: 0x000666D8
	public static Substance CreateSubstance(string name, Element.State state, KAnimFile kanim, Material material, Color32 colour, Color32 ui_colour, Color32 conduit_colour)
	{
		return new Substance
		{
			name = name,
			nameTag = TagManager.Create(name),
			elementID = (SimHashes)Hash.SDBMLower(name),
			anim = kanim,
			colour = colour,
			uiColour = ui_colour,
			conduitColour = conduit_colour,
			material = material,
			renderedByWorld = ((state & Element.State.Solid) == Element.State.Solid)
		};
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x0006853B File Offset: 0x0006673B
	public static void RegisterForTranslation(Type locstring_tree_root)
	{
		Localization.RegisterForTranslation(locstring_tree_root);
		Localization.GenerateStringsTemplate(locstring_tree_root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
	}

	// Token: 0x02001237 RID: 4663
	public enum BuildingOrdering
	{
		// Token: 0x04006724 RID: 26404
		Before,
		// Token: 0x04006725 RID: 26405
		After
	}
}
