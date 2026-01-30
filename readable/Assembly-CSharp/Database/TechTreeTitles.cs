using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F63 RID: 3939
	public class TechTreeTitles : ResourceSet<TechTreeTitle>
	{
		// Token: 0x06007CFA RID: 31994 RVA: 0x00319F00 File Offset: 0x00318100
		public TechTreeTitles(ResourceSet parent) : base("TreeTitles", parent)
		{
		}

		// Token: 0x06007CFB RID: 31995 RVA: 0x00319F10 File Offset: 0x00318110
		public void Load(TextAsset tree_file)
		{
			foreach (ResourceTreeNode resourceTreeNode in new ResourceTreeLoader<ResourceTreeNode>(tree_file))
			{
				if (string.Equals(resourceTreeNode.Id.Substring(0, 1), "_"))
				{
					new TechTreeTitle(resourceTreeNode.Id, this, Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + resourceTreeNode.Id.ToUpper()), resourceTreeNode);
				}
			}
		}
	}
}
