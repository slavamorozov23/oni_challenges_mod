using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E4E RID: 3662
public class LureSideScreen : SideScreenContent
{
	// Token: 0x06007419 RID: 29721 RVA: 0x002C5177 File Offset: 0x002C3377
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<CreatureLure>() != null;
	}

	// Token: 0x0600741A RID: 29722 RVA: 0x002C5188 File Offset: 0x002C3388
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target_lure = target.GetComponent<CreatureLure>();
		using (List<Tag>.Enumerator enumerator = this.target_lure.baitTypes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Tag bait = enumerator.Current;
				Tag bait3 = bait;
				if (!this.toggles_by_tag.ContainsKey(bait))
				{
					GameObject gameObject = Util.KInstantiateUI(this.prefab_toggle, this.toggle_container, true);
					Image reference = gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("FGImage");
					gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").text = ElementLoader.GetElement(bait).name;
					reference.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(bait).substance.anim, "ui", false, "");
					MultiToggle component = gameObject.GetComponent<MultiToggle>();
					this.toggles_by_tag.Add(bait3, component);
				}
				this.toggles_by_tag[bait].onClick = delegate()
				{
					Tag bait2 = bait;
					this.SelectToggle(bait2);
				};
			}
		}
		this.RefreshToggles();
	}

	// Token: 0x0600741B RID: 29723 RVA: 0x002C52D4 File Offset: 0x002C34D4
	public void SelectToggle(Tag tag)
	{
		if (this.target_lure.activeBaitSetting != tag)
		{
			this.target_lure.ChangeBaitSetting(tag);
		}
		else
		{
			this.target_lure.ChangeBaitSetting(Tag.Invalid);
		}
		this.RefreshToggles();
	}

	// Token: 0x0600741C RID: 29724 RVA: 0x002C5310 File Offset: 0x002C3510
	private void RefreshToggles()
	{
		foreach (KeyValuePair<Tag, MultiToggle> keyValuePair in this.toggles_by_tag)
		{
			if (this.target_lure.activeBaitSetting == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(2);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
			keyValuePair.Value.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.UISIDESCREENS.LURE.ATTRACTS, ElementLoader.GetElement(keyValuePair.Key).name, this.baitAttractionStrings[keyValuePair.Key]));
		}
	}

	// Token: 0x0400504C RID: 20556
	protected CreatureLure target_lure;

	// Token: 0x0400504D RID: 20557
	public GameObject prefab_toggle;

	// Token: 0x0400504E RID: 20558
	public GameObject toggle_container;

	// Token: 0x0400504F RID: 20559
	public Dictionary<Tag, MultiToggle> toggles_by_tag = new Dictionary<Tag, MultiToggle>();

	// Token: 0x04005050 RID: 20560
	private Dictionary<Tag, string> baitAttractionStrings = new Dictionary<Tag, string>
	{
		{
			GameTags.SlimeMold,
			CREATURES.SPECIES.PUFT.NAME
		},
		{
			GameTags.Phosphorite,
			CREATURES.SPECIES.LIGHTBUG.NAME
		}
	};
}
