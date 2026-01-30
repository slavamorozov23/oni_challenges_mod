using System;
using Rendering;
using UnityEngine;

// Token: 0x02000C38 RID: 3128
public class BlockTileDecorInfo : ScriptableObject
{
	// Token: 0x06005E93 RID: 24211 RVA: 0x0022A310 File Offset: 0x00228510
	public void PostProcess()
	{
		if (this.decor != null && this.atlas != null && this.atlas.items != null)
		{
			for (int i = 0; i < this.decor.Length; i++)
			{
				if (this.decor[i].variants != null && this.decor[i].variants.Length != 0)
				{
					for (int j = 0; j < this.decor[i].variants.Length; j++)
					{
						bool flag = false;
						foreach (TextureAtlas.Item item in this.atlas.items)
						{
							string text = item.name;
							int num = text.IndexOf("/");
							if (num != -1)
							{
								text = text.Substring(num + 1);
							}
							if (this.decor[i].variants[j].name == text)
							{
								this.decor[i].variants[j].atlasItem = item;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							DebugUtil.LogErrorArgs(new object[]
							{
								base.name,
								"/",
								this.decor[i].name,
								"could not find ",
								this.decor[i].variants[j].name,
								"in",
								this.atlas.name
							});
						}
					}
				}
			}
		}
	}

	// Token: 0x04003EDF RID: 16095
	public TextureAtlas atlas;

	// Token: 0x04003EE0 RID: 16096
	public TextureAtlas atlasSpec;

	// Token: 0x04003EE1 RID: 16097
	public int sortOrder;

	// Token: 0x04003EE2 RID: 16098
	public BlockTileDecorInfo.Decor[] decor;

	// Token: 0x02001DE0 RID: 7648
	[Serializable]
	public struct ImageInfo
	{
		// Token: 0x04008C7D RID: 35965
		public string name;

		// Token: 0x04008C7E RID: 35966
		public Vector3 offset;

		// Token: 0x04008C7F RID: 35967
		[NonSerialized]
		public TextureAtlas.Item atlasItem;
	}

	// Token: 0x02001DE1 RID: 7649
	[Serializable]
	public struct Decor
	{
		// Token: 0x04008C80 RID: 35968
		public string name;

		// Token: 0x04008C81 RID: 35969
		[EnumFlags]
		public BlockTileRenderer.Bits requiredConnections;

		// Token: 0x04008C82 RID: 35970
		[EnumFlags]
		public BlockTileRenderer.Bits forbiddenConnections;

		// Token: 0x04008C83 RID: 35971
		public float probabilityCutoff;

		// Token: 0x04008C84 RID: 35972
		public BlockTileDecorInfo.ImageInfo[] variants;

		// Token: 0x04008C85 RID: 35973
		public int sortOrder;
	}
}
