using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using KMod;
using TUNING;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x020006D9 RID: 1753
[AddComponentMenu("KMonoBehaviour/scripts/Assets")]
public class Assets : KMonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06002AF0 RID: 10992 RVA: 0x000FB298 File Offset: 0x000F9498
	protected override void OnPrefabInit()
	{
		Assets.instance = this;
		if (KPlayerPrefs.HasKey("TemperatureUnit"))
		{
			GameUtil.temperatureUnit = (GameUtil.TemperatureUnit)KPlayerPrefs.GetInt("TemperatureUnit");
		}
		if (KPlayerPrefs.HasKey("MassUnit"))
		{
			GameUtil.massUnit = (GameUtil.MassUnit)KPlayerPrefs.GetInt("MassUnit");
		}
		RecipeManager.DestroyInstance();
		RecipeManager.Get();
		Assets.AnimMaterial = this.AnimMaterialAsset;
		Assets.Prefabs = new List<KPrefabID>(from x in this.PrefabAssets
		where x != null
		select x);
		Assets.PrefabsByTag.Clear();
		Assets.PrefabsByAdditionalTags.Clear();
		Assets.CountableTags.Clear();
		Assets.Sprites = new Dictionary<HashedString, Sprite>();
		foreach (Sprite sprite in this.SpriteAssets)
		{
			if (!(sprite == null))
			{
				HashedString key = new HashedString(sprite.name);
				Assets.Sprites.Add(key, sprite);
			}
		}
		Assets.TintedSprites = (from x in this.TintedSpriteAssets
		where x != null && x.sprite != null
		select x).ToList<TintedSprite>();
		Assets.Materials = (from x in this.MaterialAssets
		where x != null
		select x).ToList<Material>();
		Assets.Textures = (from x in this.TextureAssets
		where x != null
		select x).ToList<Texture2D>();
		Assets.TextureAtlases = (from x in this.TextureAtlasAssets
		where x != null
		select x).ToList<TextureAtlas>();
		Assets.BlockTileDecorInfos = (from x in this.BlockTileDecorInfoAssets
		where x != null
		select x).ToList<BlockTileDecorInfo>();
		this.LoadAnims();
		Assets.UIPrefabs = this.UIPrefabAssets;
		Assets.DebugFont = this.DebugFontAsset;
		AsyncLoadManager<IGlobalAsyncLoader>.Run();
		GameAudioSheets.Get().Initialize();
		this.SubstanceListHookup();
		this.CreatePrefabs();
	}

	// Token: 0x06002AF1 RID: 10993 RVA: 0x000FB4F0 File Offset: 0x000F96F0
	private void CreatePrefabs()
	{
		Db.Get();
		Assets.BuildingDefs = new List<BuildingDef>();
		foreach (KPrefabID kprefabID in this.PrefabAssets)
		{
			if (!(kprefabID == null))
			{
				kprefabID.InitializeTags(true);
				Assets.AddPrefab(kprefabID);
			}
		}
		LegacyModMain.Load();
		Db.Get().PostProcess();
		ComplexRecipeManager.Get().PostProcess();
	}

	// Token: 0x06002AF2 RID: 10994 RVA: 0x000FB57C File Offset: 0x000F977C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Db.Get();
	}

	// Token: 0x06002AF3 RID: 10995 RVA: 0x000FB58C File Offset: 0x000F978C
	private static void TryAddCountableTag(KPrefabID prefab)
	{
		foreach (Tag tag in GameTags.DisplayAsUnits)
		{
			if (prefab.HasTag(tag))
			{
				Assets.AddCountableTag(prefab.PrefabTag);
				break;
			}
		}
	}

	// Token: 0x06002AF4 RID: 10996 RVA: 0x000FB5E8 File Offset: 0x000F97E8
	public static void AddCountableTag(Tag tag)
	{
		Assets.CountableTags.Add(tag);
	}

	// Token: 0x06002AF5 RID: 10997 RVA: 0x000FB5F6 File Offset: 0x000F97F6
	public static bool IsTagCountable(Tag tag)
	{
		return Assets.CountableTags.Contains(tag);
	}

	// Token: 0x06002AF6 RID: 10998 RVA: 0x000FB603 File Offset: 0x000F9803
	private static void TryAddSolidTransferArmConveyableTag(KPrefabID prefab)
	{
		if (prefab.HasAnyTags(STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE))
		{
			Assets.SolidTransferArmConeyableTags.Add(prefab.PrefabTag);
		}
	}

	// Token: 0x06002AF7 RID: 10999 RVA: 0x000FB623 File Offset: 0x000F9823
	public static bool IsTagSolidTransferArmConveyable(Tag tag)
	{
		return Assets.SolidTransferArmConeyableTags.Contains(tag);
	}

	// Token: 0x06002AF8 RID: 11000 RVA: 0x000FB630 File Offset: 0x000F9830
	private void LoadAnims()
	{
		KAnimBatchManager.DestroyInstance();
		KAnimGroupFile.DestroyInstance();
		KGlobalAnimParser.DestroyInstance();
		KAnimBatchManager.CreateInstance();
		KGlobalAnimParser.CreateInstance();
		KAnimGroupFile.LoadGroupResourceFile();
		if (BundledAssetsLoader.instance.Expansion1Assets != null)
		{
			this.AnimAssets.AddRange(BundledAssetsLoader.instance.Expansion1Assets.AnimAssets);
		}
		foreach (BundledAssets bundledAssets in BundledAssetsLoader.instance.DlcAssetsList)
		{
			this.AnimAssets.AddRange(bundledAssets.AnimAssets);
		}
		Assets.Anims = (from x in this.AnimAssets
		where x != null
		select x).ToList<KAnimFile>();
		Assets.Anims.AddRange(Assets.ModLoadedKAnims);
		Assets.AnimTable.Clear();
		foreach (KAnimFile kanimFile in Assets.Anims)
		{
			if (kanimFile != null)
			{
				HashedString key = kanimFile.name;
				Assets.AnimTable[key] = kanimFile;
			}
		}
		KAnimGroupFile.MapNamesToAnimFiles(Assets.AnimTable);
		Global.Instance.modManager.Load(Content.Animation);
		Assets.Anims.AddRange(Assets.ModLoadedKAnims);
		foreach (KAnimFile kanimFile2 in Assets.ModLoadedKAnims)
		{
			if (kanimFile2 != null)
			{
				HashedString key2 = kanimFile2.name;
				Assets.AnimTable[key2] = kanimFile2;
			}
		}
		global::Debug.Assert(Assets.AnimTable.Count > 0, "Anim Assets not yet loaded");
		KAnimGroupFile.LoadAll();
		foreach (KAnimFile kanimFile3 in Assets.Anims)
		{
			kanimFile3.FinalizeLoading();
		}
		KAnimBatchManager.Instance().CompleteInit();
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x000FB874 File Offset: 0x000F9A74
	private void SubstanceListHookup()
	{
		Dictionary<string, SubstanceTable> dictionary = new Dictionary<string, SubstanceTable>
		{
			{
				"",
				this.substanceTable
			}
		};
		if (BundledAssetsLoader.instance.Expansion1Assets != null)
		{
			dictionary["EXPANSION1_ID"] = BundledAssetsLoader.instance.Expansion1Assets.SubstanceTable;
		}
		Hashtable hashtable = new Hashtable();
		ElementsAudio.Instance.LoadData(AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<ElementAudioFileLoader>.Get().entries);
		ElementLoader.Load(ref hashtable, dictionary);
		List<Element> list = ElementLoader.elements.FindAll((Element e) => e.HasTag(GameTags.StartingMetalOre));
		GameTags.StartingMetalOres = new Tag[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			GameTags.StartingMetalOres[i] = list[i].tag;
		}
		GameTags.BasicMetalOres = GameTags.StartingMetalOres.Append(GameTags.BasicMetalOres);
		List<Element> list2 = ElementLoader.elements.FindAll((Element e) => e.HasTag(GameTags.StartingRefinedMetal));
		GameTags.StartingRefinedMetals = new Tag[list2.Count];
		for (int j = 0; j < list2.Count; j++)
		{
			GameTags.StartingRefinedMetals[j] = list2[j].tag;
		}
		GameTags.BasicRefinedMetals = GameTags.StartingRefinedMetals.Append(GameTags.BasicRefinedMetals);
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x000FB9DE File Offset: 0x000F9BDE
	public static string GetSimpleSoundEventName(EventReference event_ref)
	{
		return Assets.GetSimpleSoundEventName(KFMOD.GetEventReferencePath(event_ref));
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x000FB9EC File Offset: 0x000F9BEC
	public static string GetSimpleSoundEventName(string path)
	{
		string text = null;
		if (!Assets.simpleSoundEventNames.TryGetValue(path, out text))
		{
			int num = path.LastIndexOf('/');
			text = ((num != -1) ? path.Substring(num + 1) : path);
			Assets.simpleSoundEventNames[path] = text;
		}
		return text;
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x000FBA34 File Offset: 0x000F9C34
	private static BuildingDef GetDef(IList<BuildingDef> defs, string prefab_id)
	{
		int count = defs.Count;
		for (int i = 0; i < count; i++)
		{
			if (defs[i].PrefabID == prefab_id)
			{
				return defs[i];
			}
		}
		return null;
	}

	// Token: 0x06002AFD RID: 11005 RVA: 0x000FBA71 File Offset: 0x000F9C71
	public static BuildingDef GetBuildingDef(string prefab_id)
	{
		return Assets.GetDef(Assets.BuildingDefs, prefab_id);
	}

	// Token: 0x06002AFE RID: 11006 RVA: 0x000FBA80 File Offset: 0x000F9C80
	public static TintedSprite GetTintedSprite(string name)
	{
		TintedSprite result = null;
		if (Assets.TintedSprites != null)
		{
			for (int i = 0; i < Assets.TintedSprites.Count; i++)
			{
				if (Assets.TintedSprites[i].name == name)
				{
					result = Assets.TintedSprites[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06002AFF RID: 11007 RVA: 0x000FBAD4 File Offset: 0x000F9CD4
	public static Sprite GetSprite(HashedString name)
	{
		Sprite result = null;
		if (Assets.Sprites != null)
		{
			Assets.Sprites.TryGetValue(name, out result);
		}
		return result;
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x000FBAF9 File Offset: 0x000F9CF9
	public static VideoClip GetVideo(string name)
	{
		return Resources.Load<VideoClip>("video_webm/" + name);
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x000FBB0C File Offset: 0x000F9D0C
	public static Texture2D GetTexture(string name)
	{
		Texture2D result = null;
		if (Assets.Textures != null)
		{
			for (int i = 0; i < Assets.Textures.Count; i++)
			{
				if (Assets.Textures[i].name == name)
				{
					result = Assets.Textures[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x000FBB60 File Offset: 0x000F9D60
	public static ComicData GetComic(string id)
	{
		foreach (ComicData comicData in Assets.instance.comics)
		{
			if (comicData.name == id)
			{
				return comicData;
			}
		}
		return null;
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x000FBB9C File Offset: 0x000F9D9C
	public static void AddPrefab(KPrefabID prefab)
	{
		if (prefab == null)
		{
			return;
		}
		prefab.InitializeTags(true);
		prefab.UpdateSaveLoadTag();
		if (Assets.PrefabsByTag.ContainsKey(prefab.PrefabTag))
		{
			string str = "Tried loading prefab with duplicate tag, ignoring: ";
			Tag prefabTag = prefab.PrefabTag;
			global::Debug.LogWarning(str + prefabTag.ToString());
			return;
		}
		Assets.PrefabsByTag[prefab.PrefabTag] = prefab;
		foreach (Tag key in prefab.Tags)
		{
			if (!Assets.PrefabsByAdditionalTags.ContainsKey(key))
			{
				Assets.PrefabsByAdditionalTags[key] = new List<KPrefabID>();
			}
			Assets.PrefabsByAdditionalTags[key].Add(prefab);
		}
		Assets.Prefabs.Add(prefab);
		Assets.TryAddCountableTag(prefab);
		Assets.TryAddSolidTransferArmConveyableTag(prefab);
		if (Assets.OnAddPrefab != null)
		{
			Assets.OnAddPrefab(prefab);
		}
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x000FBCA0 File Offset: 0x000F9EA0
	public static void RegisterOnAddPrefab(Action<KPrefabID> on_add)
	{
		Assets.OnAddPrefab = (Action<KPrefabID>)Delegate.Combine(Assets.OnAddPrefab, on_add);
		foreach (KPrefabID obj in Assets.Prefabs)
		{
			on_add(obj);
		}
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x000FBD08 File Offset: 0x000F9F08
	public static void UnregisterOnAddPrefab(Action<KPrefabID> on_add)
	{
		Assets.OnAddPrefab = (Action<KPrefabID>)Delegate.Remove(Assets.OnAddPrefab, on_add);
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x000FBD1F File Offset: 0x000F9F1F
	public static void ClearOnAddPrefab()
	{
		Assets.OnAddPrefab = null;
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x000FBD28 File Offset: 0x000F9F28
	public static GameObject GetPrefab(Tag tag)
	{
		GameObject gameObject = Assets.TryGetPrefab(tag);
		if (gameObject == null)
		{
			string str = "Missing prefab: ";
			Tag tag2 = tag;
			global::Debug.LogWarning(str + tag2.ToString());
		}
		return gameObject;
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x000FBD64 File Offset: 0x000F9F64
	public static GameObject TryGetPrefab(Tag tag)
	{
		KPrefabID kprefabID = null;
		Assets.PrefabsByTag.TryGetValue(tag, out kprefabID);
		if (!(kprefabID != null))
		{
			return null;
		}
		return kprefabID.gameObject;
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x000FBD94 File Offset: 0x000F9F94
	public static List<GameObject> GetPrefabsWithTag(Tag tag)
	{
		List<GameObject> list = new List<GameObject>();
		if (Assets.PrefabsByAdditionalTags.ContainsKey(tag))
		{
			for (int i = 0; i < Assets.PrefabsByAdditionalTags[tag].Count; i++)
			{
				list.Add(Assets.PrefabsByAdditionalTags[tag][i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06002B0A RID: 11018 RVA: 0x000FBDEC File Offset: 0x000F9FEC
	public static List<GameObject> GetPrefabsWithComponent<Type>()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			if (Assets.Prefabs[i].GetComponent<Type>() != null)
			{
				list.Add(Assets.Prefabs[i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06002B0B RID: 11019 RVA: 0x000FBE44 File Offset: 0x000FA044
	public static List<Type> GetPrefabsWithComponentAsListOfComponents<Type>()
	{
		List<Type> list = new List<Type>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			Type component = Assets.Prefabs[i].GetComponent<Type>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x000FBE8D File Offset: 0x000FA08D
	public static GameObject GetPrefabWithComponent<Type>()
	{
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Type>();
		global::Debug.Assert(prefabsWithComponent.Count > 0, "There are no prefabs of type " + typeof(Type).Name);
		return prefabsWithComponent[0];
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x000FBEC4 File Offset: 0x000FA0C4
	public static List<Tag> GetPrefabTagsWithComponent<Type>()
	{
		List<Tag> list = new List<Tag>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			if (Assets.Prefabs[i].GetComponent<Type>() != null)
			{
				list.Add(Assets.Prefabs[i].PrefabID());
			}
		}
		return list;
	}

	// Token: 0x06002B0E RID: 11022 RVA: 0x000FBF1C File Offset: 0x000FA11C
	public static Assets GetInstanceEditorOnly()
	{
		Assets[] array = (Assets[])Resources.FindObjectsOfTypeAll(typeof(Assets));
		if (array != null)
		{
			int num = array.Length;
		}
		return array[0];
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x000FBF48 File Offset: 0x000FA148
	public static TextureAtlas GetTextureAtlas(string name)
	{
		foreach (TextureAtlas textureAtlas in Assets.TextureAtlases)
		{
			if (textureAtlas.name == name)
			{
				return textureAtlas;
			}
		}
		return null;
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x000FBFA8 File Offset: 0x000FA1A8
	public static Material GetMaterial(string name)
	{
		foreach (Material material in Assets.Materials)
		{
			if (material.name == name)
			{
				return material;
			}
		}
		return null;
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x000FC008 File Offset: 0x000FA208
	public static BlockTileDecorInfo GetBlockTileDecorInfo(string name)
	{
		foreach (BlockTileDecorInfo blockTileDecorInfo in Assets.BlockTileDecorInfos)
		{
			if (blockTileDecorInfo.name == name)
			{
				return blockTileDecorInfo;
			}
		}
		global::Debug.LogError("Could not find BlockTileDecorInfo named [" + name + "]");
		return null;
	}

	// Token: 0x06002B12 RID: 11026 RVA: 0x000FC080 File Offset: 0x000FA280
	public static KAnimFile GetAnim(HashedString name)
	{
		if (!name.IsValid)
		{
			global::Debug.LogWarning("Invalid hash name");
			return null;
		}
		KAnimFile kanimFile = null;
		Assets.AnimTable.TryGetValue(name, out kanimFile);
		if (kanimFile == null)
		{
			global::Debug.LogWarning("Missing Anim: [" + name.ToString() + "]. You may have to run Collect Anim on the Assets prefab");
		}
		return kanimFile;
	}

	// Token: 0x06002B13 RID: 11027 RVA: 0x000FC0DD File Offset: 0x000FA2DD
	public static bool TryGetAnim(HashedString name, out KAnimFile anim)
	{
		if (!name.IsValid)
		{
			global::Debug.LogWarning("Invalid hash name");
			anim = null;
			return false;
		}
		Assets.AnimTable.TryGetValue(name, out anim);
		return anim != null;
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x000FC10C File Offset: 0x000FA30C
	public void OnAfterDeserialize()
	{
		this.TintedSpriteAssets = (from x in this.TintedSpriteAssets
		where x != null && x.sprite != null
		select x).ToList<TintedSprite>();
		this.TintedSpriteAssets.Sort((TintedSprite a, TintedSprite b) => a.name.CompareTo(b.name));
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x000FC178 File Offset: 0x000FA378
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x000FC17C File Offset: 0x000FA37C
	public static void AddBuildingDef(BuildingDef def)
	{
		Assets.BuildingDefs = (from x in Assets.BuildingDefs
		where x.PrefabID != def.PrefabID
		select x).ToList<BuildingDef>();
		Assets.BuildingDefs.Add(def);
	}

	// Token: 0x04001999 RID: 6553
	public static List<KAnimFile> ModLoadedKAnims = new List<KAnimFile>();

	// Token: 0x0400199A RID: 6554
	private static Action<KPrefabID> OnAddPrefab;

	// Token: 0x0400199B RID: 6555
	public static List<BuildingDef> BuildingDefs;

	// Token: 0x0400199C RID: 6556
	public List<KPrefabID> PrefabAssets = new List<KPrefabID>();

	// Token: 0x0400199D RID: 6557
	public static List<KPrefabID> Prefabs = new List<KPrefabID>();

	// Token: 0x0400199E RID: 6558
	private static HashSet<Tag> CountableTags = new HashSet<Tag>();

	// Token: 0x0400199F RID: 6559
	private static HashSet<Tag> SolidTransferArmConeyableTags = new HashSet<Tag>();

	// Token: 0x040019A0 RID: 6560
	public List<Sprite> SpriteAssets;

	// Token: 0x040019A1 RID: 6561
	public static Dictionary<HashedString, Sprite> Sprites;

	// Token: 0x040019A2 RID: 6562
	public List<string> videoClipNames;

	// Token: 0x040019A3 RID: 6563
	private const string VIDEO_ASSET_PATH = "video_webm";

	// Token: 0x040019A4 RID: 6564
	public List<TintedSprite> TintedSpriteAssets;

	// Token: 0x040019A5 RID: 6565
	public static List<TintedSprite> TintedSprites;

	// Token: 0x040019A6 RID: 6566
	public List<Texture2D> TextureAssets;

	// Token: 0x040019A7 RID: 6567
	public static List<Texture2D> Textures;

	// Token: 0x040019A8 RID: 6568
	public static List<TextureAtlas> TextureAtlases;

	// Token: 0x040019A9 RID: 6569
	public List<TextureAtlas> TextureAtlasAssets;

	// Token: 0x040019AA RID: 6570
	public static List<Material> Materials;

	// Token: 0x040019AB RID: 6571
	public List<Material> MaterialAssets;

	// Token: 0x040019AC RID: 6572
	public static List<Shader> Shaders;

	// Token: 0x040019AD RID: 6573
	public List<Shader> ShaderAssets;

	// Token: 0x040019AE RID: 6574
	public static List<BlockTileDecorInfo> BlockTileDecorInfos;

	// Token: 0x040019AF RID: 6575
	public List<BlockTileDecorInfo> BlockTileDecorInfoAssets;

	// Token: 0x040019B0 RID: 6576
	public Material AnimMaterialAsset;

	// Token: 0x040019B1 RID: 6577
	public static Material AnimMaterial;

	// Token: 0x040019B2 RID: 6578
	public DiseaseVisualization DiseaseVisualization;

	// Token: 0x040019B3 RID: 6579
	public Sprite LegendColourBox;

	// Token: 0x040019B4 RID: 6580
	public Texture2D invalidAreaTex;

	// Token: 0x040019B5 RID: 6581
	public Assets.UIPrefabData UIPrefabAssets;

	// Token: 0x040019B6 RID: 6582
	public static Assets.UIPrefabData UIPrefabs;

	// Token: 0x040019B7 RID: 6583
	private static Dictionary<Tag, KPrefabID> PrefabsByTag = new Dictionary<Tag, KPrefabID>();

	// Token: 0x040019B8 RID: 6584
	private static Dictionary<Tag, List<KPrefabID>> PrefabsByAdditionalTags = new Dictionary<Tag, List<KPrefabID>>();

	// Token: 0x040019B9 RID: 6585
	public List<KAnimFile> AnimAssets;

	// Token: 0x040019BA RID: 6586
	public static List<KAnimFile> Anims;

	// Token: 0x040019BB RID: 6587
	private static Dictionary<HashedString, KAnimFile> AnimTable = new Dictionary<HashedString, KAnimFile>();

	// Token: 0x040019BC RID: 6588
	public Font DebugFontAsset;

	// Token: 0x040019BD RID: 6589
	public static Font DebugFont;

	// Token: 0x040019BE RID: 6590
	public SubstanceTable substanceTable;

	// Token: 0x040019BF RID: 6591
	[SerializeField]
	public TextAsset elementAudio;

	// Token: 0x040019C0 RID: 6592
	[SerializeField]
	public TextAsset personalitiesFile;

	// Token: 0x040019C1 RID: 6593
	public LogicModeUI logicModeUIData;

	// Token: 0x040019C2 RID: 6594
	public CommonPlacerConfig.CommonPlacerAssets commonPlacerAssets;

	// Token: 0x040019C3 RID: 6595
	public DigPlacerConfig.DigPlacerAssets digPlacerAssets;

	// Token: 0x040019C4 RID: 6596
	public MopPlacerConfig.MopPlacerAssets mopPlacerAssets;

	// Token: 0x040019C5 RID: 6597
	public MovePickupablePlacerConfig.MovePickupablePlacerAssets movePickupToPlacerAssets;

	// Token: 0x040019C6 RID: 6598
	public ComicData[] comics;

	// Token: 0x040019C7 RID: 6599
	public static Assets instance;

	// Token: 0x040019C8 RID: 6600
	private static Dictionary<string, string> simpleSoundEventNames = new Dictionary<string, string>();

	// Token: 0x02001594 RID: 5524
	[Serializable]
	public struct UIPrefabData
	{
		// Token: 0x04007219 RID: 29209
		public ProgressBar ProgressBar;

		// Token: 0x0400721A RID: 29210
		public HealthBar HealthBar;

		// Token: 0x0400721B RID: 29211
		public GameObject ResourceVisualizer;

		// Token: 0x0400721C RID: 29212
		public GameObject KAnimVisualizer;

		// Token: 0x0400721D RID: 29213
		public Image RegionCellBlocked;

		// Token: 0x0400721E RID: 29214
		public RectTransform PriorityOverlayIcon;

		// Token: 0x0400721F RID: 29215
		public RectTransform HarvestWhenReadyOverlayIcon;

		// Token: 0x04007220 RID: 29216
		public Assets.TableScreenAssets TableScreenWidgets;
	}

	// Token: 0x02001595 RID: 5525
	[Serializable]
	public struct TableScreenAssets
	{
		// Token: 0x04007221 RID: 29217
		public Material DefaultUIMaterial;

		// Token: 0x04007222 RID: 29218
		public Material DesaturatedUIMaterial;

		// Token: 0x04007223 RID: 29219
		public GameObject MinionPortrait;

		// Token: 0x04007224 RID: 29220
		public GameObject GenericPortrait;

		// Token: 0x04007225 RID: 29221
		public GameObject TogglePortrait;

		// Token: 0x04007226 RID: 29222
		public GameObject ButtonLabel;

		// Token: 0x04007227 RID: 29223
		public GameObject ButtonLabelWhite;

		// Token: 0x04007228 RID: 29224
		public GameObject Label;

		// Token: 0x04007229 RID: 29225
		public GameObject LabelHeader;

		// Token: 0x0400722A RID: 29226
		public GameObject Checkbox;

		// Token: 0x0400722B RID: 29227
		public GameObject BlankCell;

		// Token: 0x0400722C RID: 29228
		public GameObject SuperCheckbox_Horizontal;

		// Token: 0x0400722D RID: 29229
		public GameObject SuperCheckbox_Vertical;

		// Token: 0x0400722E RID: 29230
		public GameObject Spacer;

		// Token: 0x0400722F RID: 29231
		public GameObject NumericDropDown;

		// Token: 0x04007230 RID: 29232
		public GameObject DropDownHeader;

		// Token: 0x04007231 RID: 29233
		public GameObject PriorityGroupSelector;

		// Token: 0x04007232 RID: 29234
		public GameObject PriorityGroupSelectorHeader;

		// Token: 0x04007233 RID: 29235
		public GameObject PrioritizeRowWidget;

		// Token: 0x04007234 RID: 29236
		public GameObject PrioritizeRowHeaderWidget;
	}
}
