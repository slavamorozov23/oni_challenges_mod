using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000832 RID: 2098
public class BundledAssetsLoader : KMonoBehaviour
{
	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06003934 RID: 14644 RVA: 0x0013F899 File Offset: 0x0013DA99
	// (set) Token: 0x06003935 RID: 14645 RVA: 0x0013F8A1 File Offset: 0x0013DAA1
	public BundledAssets Expansion1Assets { get; private set; }

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06003936 RID: 14646 RVA: 0x0013F8AA File Offset: 0x0013DAAA
	// (set) Token: 0x06003937 RID: 14647 RVA: 0x0013F8B2 File Offset: 0x0013DAB2
	public List<BundledAssets> DlcAssetsList { get; private set; }

	// Token: 0x06003938 RID: 14648 RVA: 0x0013F8BC File Offset: 0x0013DABC
	protected override void OnPrefabInit()
	{
		BundledAssetsLoader.instance = this;
		if (DlcManager.IsExpansion1Active())
		{
			global::Debug.Log("Loading Expansion1 assets from bundle");
			AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName("EXPANSION1_ID")));
			global::Debug.Assert(assetBundle != null, "Expansion1 is Active but its asset bundle failed to load");
			GameObject gameObject = assetBundle.LoadAsset<GameObject>("Expansion1Assets");
			global::Debug.Assert(gameObject != null, "Could not load the Expansion1Assets prefab");
			this.Expansion1Assets = Util.KInstantiate(gameObject, base.gameObject, null).GetComponent<BundledAssets>();
		}
		this.DlcAssetsList = new List<BundledAssets>(DlcManager.DLC_PACKS.Count);
		foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
		{
			if (DlcManager.IsContentSubscribed(keyValuePair.Key))
			{
				global::Debug.Log("Loading DLC " + keyValuePair.Key + " assets from bundle");
				AssetBundle assetBundle2 = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName(keyValuePair.Key)));
				global::Debug.Assert(assetBundle2 != null, "DLC " + keyValuePair.Key + " is Active but its asset bundle failed to load");
				GameObject gameObject2 = assetBundle2.LoadAsset<GameObject>(keyValuePair.Value.directory + "Assets");
				global::Debug.Assert(gameObject2 != null, "Could not load the " + keyValuePair.Key + " prefab");
				this.DlcAssetsList.Add(Util.KInstantiate(gameObject2, base.gameObject, null).GetComponent<BundledAssets>());
			}
		}
	}

	// Token: 0x040022E9 RID: 8937
	public static BundledAssetsLoader instance;
}
