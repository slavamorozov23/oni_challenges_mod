using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C79 RID: 3193
public class TagFilterScreen : SideScreenContent
{
	// Token: 0x0600617A RID: 24954 RVA: 0x0023F073 File Offset: 0x0023D273
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<TreeFilterable>() != null;
	}

	// Token: 0x0600617B RID: 24955 RVA: 0x0023F084 File Offset: 0x0023D284
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetFilterable = target.GetComponent<TreeFilterable>();
		if (this.targetFilterable == null)
		{
			global::Debug.LogError("The target provided does not have a Tree Filterable component");
			return;
		}
		if (!this.targetFilterable.showUserMenu)
		{
			return;
		}
		this.Filter(this.targetFilterable.AcceptedTags);
		base.Activate();
	}

	// Token: 0x0600617C RID: 24956 RVA: 0x0023F0F0 File Offset: 0x0023D2F0
	protected override void OnActivate()
	{
		this.rootItem = this.BuildDisplay(this.rootTag);
		this.treeControl.SetUserItemRoot(this.rootItem);
		this.treeControl.root.opened = true;
		this.Filter(this.treeControl.root, this.acceptedTags, false);
	}

	// Token: 0x0600617D RID: 24957 RVA: 0x0023F14C File Offset: 0x0023D34C
	public static List<Tag> GetAllTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (TagFilterScreen.TagEntry tagEntry in TagFilterScreen.defaultRootTag.children)
		{
			if (tagEntry.tag.IsValid)
			{
				list.Add(tagEntry.tag);
			}
		}
		return list;
	}

	// Token: 0x0600617E RID: 24958 RVA: 0x0023F198 File Offset: 0x0023D398
	private KTreeControl.UserItem BuildDisplay(TagFilterScreen.TagEntry root)
	{
		KTreeControl.UserItem userItem = null;
		if (root.name != null && root.name != "")
		{
			userItem = new KTreeControl.UserItem
			{
				text = root.name,
				userData = root.tag
			};
			List<KTreeControl.UserItem> list = new List<KTreeControl.UserItem>();
			if (root.children != null)
			{
				foreach (TagFilterScreen.TagEntry root2 in root.children)
				{
					list.Add(this.BuildDisplay(root2));
				}
			}
			userItem.children = list;
		}
		return userItem;
	}

	// Token: 0x0600617F RID: 24959 RVA: 0x0023F224 File Offset: 0x0023D424
	private static KTreeControl.UserItem CreateTree(string tree_name, Tag tree_tag, IList<Element> items)
	{
		KTreeControl.UserItem userItem = new KTreeControl.UserItem
		{
			text = tree_name,
			userData = tree_tag,
			children = new List<KTreeControl.UserItem>()
		};
		foreach (Element element in items)
		{
			KTreeControl.UserItem item = new KTreeControl.UserItem
			{
				text = element.name,
				userData = GameTagExtensions.Create(element.id)
			};
			userItem.children.Add(item);
		}
		return userItem;
	}

	// Token: 0x06006180 RID: 24960 RVA: 0x0023F2C0 File Offset: 0x0023D4C0
	public void SetRootTag(TagFilterScreen.TagEntry root_tag)
	{
		this.rootTag = root_tag;
	}

	// Token: 0x06006181 RID: 24961 RVA: 0x0023F2C9 File Offset: 0x0023D4C9
	public void Filter(HashSet<Tag> acceptedTags)
	{
		this.acceptedTags = acceptedTags;
	}

	// Token: 0x06006182 RID: 24962 RVA: 0x0023F2D4 File Offset: 0x0023D4D4
	private void Filter(KTreeItem root, HashSet<Tag> acceptedTags, bool parentEnabled)
	{
		root.checkboxChecked = (parentEnabled || (root.userData != null && acceptedTags.Contains((Tag)root.userData)));
		foreach (KTreeItem root2 in root.children)
		{
			this.Filter(root2, acceptedTags, root.checkboxChecked);
		}
		if (!root.checkboxChecked && root.children.Count > 0)
		{
			bool checkboxChecked = true;
			using (IEnumerator<KTreeItem> enumerator = root.children.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.checkboxChecked)
					{
						checkboxChecked = false;
						break;
					}
				}
			}
			root.checkboxChecked = checkboxChecked;
		}
	}

	// Token: 0x0400413B RID: 16699
	[SerializeField]
	private KTreeControl treeControl;

	// Token: 0x0400413C RID: 16700
	private KTreeControl.UserItem rootItem;

	// Token: 0x0400413D RID: 16701
	private TagFilterScreen.TagEntry rootTag = TagFilterScreen.defaultRootTag;

	// Token: 0x0400413E RID: 16702
	private HashSet<Tag> acceptedTags = new HashSet<Tag>();

	// Token: 0x0400413F RID: 16703
	private TreeFilterable targetFilterable;

	// Token: 0x04004140 RID: 16704
	public static TagFilterScreen.TagEntry defaultRootTag = new TagFilterScreen.TagEntry
	{
		name = "All",
		tag = default(Tag),
		children = new TagFilterScreen.TagEntry[0]
	};

	// Token: 0x02001E4E RID: 7758
	public class TagEntry
	{
		// Token: 0x04008E50 RID: 36432
		public string name;

		// Token: 0x04008E51 RID: 36433
		public Tag tag;

		// Token: 0x04008E52 RID: 36434
		public TagFilterScreen.TagEntry[] children;
	}
}
