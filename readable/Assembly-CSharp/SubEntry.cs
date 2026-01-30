using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000CBB RID: 3259
public class SubEntry : IHasDlcRestrictions
{
	// Token: 0x06006400 RID: 25600 RVA: 0x002550EC File Offset: 0x002532EC
	public SubEntry()
	{
	}

	// Token: 0x06006401 RID: 25601 RVA: 0x00255100 File Offset: 0x00253300
	public SubEntry(string id, string parentEntryID, List<ContentContainer> contentContainers, string name)
	{
		this.id = id;
		this.parentEntryID = parentEntryID;
		this.name = name;
		this.contentContainers = contentContainers;
		if (!string.IsNullOrEmpty(this.lockID))
		{
			foreach (ContentContainer contentContainer in contentContainers)
			{
				contentContainer.lockID = this.lockID;
			}
		}
		if (string.IsNullOrEmpty(this.sortString))
		{
			if (!string.IsNullOrEmpty(this.title))
			{
				this.sortString = UI.StripLinkFormatting(this.title);
				return;
			}
			this.sortString = UI.StripLinkFormatting(name);
		}
	}

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06006402 RID: 25602 RVA: 0x002551C8 File Offset: 0x002533C8
	// (set) Token: 0x06006403 RID: 25603 RVA: 0x002551D0 File Offset: 0x002533D0
	public List<ContentContainer> contentContainers { get; set; }

	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06006404 RID: 25604 RVA: 0x002551D9 File Offset: 0x002533D9
	// (set) Token: 0x06006405 RID: 25605 RVA: 0x002551E1 File Offset: 0x002533E1
	public string parentEntryID { get; set; }

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06006406 RID: 25606 RVA: 0x002551EA File Offset: 0x002533EA
	// (set) Token: 0x06006407 RID: 25607 RVA: 0x002551F2 File Offset: 0x002533F2
	public string id { get; set; }

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06006408 RID: 25608 RVA: 0x002551FB File Offset: 0x002533FB
	// (set) Token: 0x06006409 RID: 25609 RVA: 0x00255203 File Offset: 0x00253403
	public string name { get; set; }

	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x0600640A RID: 25610 RVA: 0x0025520C File Offset: 0x0025340C
	// (set) Token: 0x0600640B RID: 25611 RVA: 0x00255214 File Offset: 0x00253414
	public string title { get; set; }

	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x0600640C RID: 25612 RVA: 0x0025521D File Offset: 0x0025341D
	// (set) Token: 0x0600640D RID: 25613 RVA: 0x00255225 File Offset: 0x00253425
	public string subtitle { get; set; }

	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x0600640E RID: 25614 RVA: 0x0025522E File Offset: 0x0025342E
	// (set) Token: 0x0600640F RID: 25615 RVA: 0x00255236 File Offset: 0x00253436
	public Sprite icon { get; set; }

	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x06006410 RID: 25616 RVA: 0x0025523F File Offset: 0x0025343F
	// (set) Token: 0x06006411 RID: 25617 RVA: 0x00255247 File Offset: 0x00253447
	public int layoutPriority { get; set; }

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x06006412 RID: 25618 RVA: 0x00255250 File Offset: 0x00253450
	// (set) Token: 0x06006413 RID: 25619 RVA: 0x00255258 File Offset: 0x00253458
	public bool disabled { get; set; }

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x06006414 RID: 25620 RVA: 0x00255261 File Offset: 0x00253461
	// (set) Token: 0x06006415 RID: 25621 RVA: 0x00255269 File Offset: 0x00253469
	public string lockID { get; set; }

	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x06006416 RID: 25622 RVA: 0x00255272 File Offset: 0x00253472
	// (set) Token: 0x06006417 RID: 25623 RVA: 0x0025527A File Offset: 0x0025347A
	public string[] requiredAtLeastOneDlcIds { get; set; }

	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x06006418 RID: 25624 RVA: 0x00255283 File Offset: 0x00253483
	// (set) Token: 0x06006419 RID: 25625 RVA: 0x0025528B File Offset: 0x0025348B
	public string[] requiredDlcIds { get; set; }

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x0600641A RID: 25626 RVA: 0x00255294 File Offset: 0x00253494
	// (set) Token: 0x0600641B RID: 25627 RVA: 0x0025529C File Offset: 0x0025349C
	public string[] forbiddenDlcIds { get; set; }

	// Token: 0x0600641C RID: 25628 RVA: 0x002552A5 File Offset: 0x002534A5
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x0600641D RID: 25629 RVA: 0x002552AD File Offset: 0x002534AD
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x0600641E RID: 25630 RVA: 0x002552B5 File Offset: 0x002534B5
	public string[] GetAnyRequiredDlcIds()
	{
		return this.requiredAtLeastOneDlcIds;
	}

	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x0600641F RID: 25631 RVA: 0x002552BD File Offset: 0x002534BD
	// (set) Token: 0x06006420 RID: 25632 RVA: 0x002552C5 File Offset: 0x002534C5
	public string sortString { get; set; }

	// Token: 0x04004404 RID: 17412
	public ContentContainer lockedContentContainer;

	// Token: 0x0400440B RID: 17419
	public Color iconColor = Color.white;
}
