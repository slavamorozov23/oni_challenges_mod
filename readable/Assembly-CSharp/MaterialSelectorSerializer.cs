using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000D8A RID: 3466
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MaterialSelectorSerializer")]
public class MaterialSelectorSerializer : KMonoBehaviour
{
	// Token: 0x06006BF5 RID: 27637 RVA: 0x00290970 File Offset: 0x0028EB70
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.previouslySelectedElementsPerWorld == null)
		{
			this.previouslySelectedElementsPerWorld = new List<Dictionary<Tag, Tag>>[255];
			if (this.previouslySelectedElements != null)
			{
				foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
				{
					List<Dictionary<Tag, Tag>> list = this.previouslySelectedElements.ConvertAll<Dictionary<Tag, Tag>>((Dictionary<Tag, Tag> input) => new Dictionary<Tag, Tag>(input));
					this.previouslySelectedElementsPerWorld[worldContainer.id] = list;
				}
				this.previouslySelectedElements = null;
			}
		}
	}

	// Token: 0x06006BF6 RID: 27638 RVA: 0x00290A2C File Offset: 0x0028EC2C
	public void WipeWorldSelectionData(int worldID)
	{
		this.previouslySelectedElementsPerWorld[worldID] = null;
	}

	// Token: 0x06006BF7 RID: 27639 RVA: 0x00290A38 File Offset: 0x0028EC38
	public void SetSelectedElement(int worldID, int selectorIndex, Tag recipe, Tag element)
	{
		if (this.previouslySelectedElementsPerWorld[worldID] == null)
		{
			this.previouslySelectedElementsPerWorld[worldID] = new List<Dictionary<Tag, Tag>>();
		}
		List<Dictionary<Tag, Tag>> list = this.previouslySelectedElementsPerWorld[worldID];
		while (list.Count <= selectorIndex)
		{
			list.Add(new Dictionary<Tag, Tag>());
		}
		list[selectorIndex][recipe] = element;
	}

	// Token: 0x06006BF8 RID: 27640 RVA: 0x00290A8C File Offset: 0x0028EC8C
	public Tag GetPreviousElement(int worldID, int selectorIndex, Tag recipe)
	{
		Tag invalid = Tag.Invalid;
		if (this.previouslySelectedElementsPerWorld[worldID] == null)
		{
			return invalid;
		}
		List<Dictionary<Tag, Tag>> list = this.previouslySelectedElementsPerWorld[worldID];
		if (list.Count <= selectorIndex)
		{
			return invalid;
		}
		list[selectorIndex].TryGetValue(recipe, out invalid);
		return invalid;
	}

	// Token: 0x04004A1C RID: 18972
	[Serialize]
	private List<Dictionary<Tag, Tag>> previouslySelectedElements;

	// Token: 0x04004A1D RID: 18973
	[Serialize]
	private List<Dictionary<Tag, Tag>>[] previouslySelectedElementsPerWorld;
}
