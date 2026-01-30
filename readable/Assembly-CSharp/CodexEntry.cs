using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000CBE RID: 3262
public class CodexEntry : IHasDlcRestrictions
{
	// Token: 0x06006429 RID: 25641 RVA: 0x00255337 File Offset: 0x00253537
	public CodexEntry()
	{
	}

	// Token: 0x0600642A RID: 25642 RVA: 0x00255378 File Offset: 0x00253578
	public CodexEntry(string category, List<ContentContainer> contentContainers, string name)
	{
		this.category = category;
		this.name = name;
		this.contentContainers = contentContainers;
		if (string.IsNullOrEmpty(this.sortString))
		{
			this.sortString = UI.StripLinkFormatting(name);
		}
	}

	// Token: 0x0600642B RID: 25643 RVA: 0x002553F0 File Offset: 0x002535F0
	public CodexEntry(string category, string titleKey, List<ContentContainer> contentContainers)
	{
		this.category = category;
		this.title = titleKey;
		this.contentContainers = contentContainers;
		if (string.IsNullOrEmpty(this.sortString))
		{
			this.sortString = UI.StripLinkFormatting(this.title);
		}
	}

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x0600642C RID: 25644 RVA: 0x0025546D File Offset: 0x0025366D
	// (set) Token: 0x0600642D RID: 25645 RVA: 0x00255475 File Offset: 0x00253675
	public List<ContentContainer> contentContainers
	{
		get
		{
			return this._contentContainers;
		}
		private set
		{
			this._contentContainers = value;
		}
	}

	// Token: 0x0600642E RID: 25646 RVA: 0x00255480 File Offset: 0x00253680
	public static List<string> ContentContainerDebug(List<ContentContainer> _contentContainers)
	{
		List<string> list = new List<string>();
		foreach (ContentContainer contentContainer in _contentContainers)
		{
			if (contentContainer != null)
			{
				string text = string.Concat(new string[]
				{
					"<b>",
					contentContainer.contentLayout.ToString(),
					" container: ",
					((contentContainer.content == null) ? 0 : contentContainer.content.Count).ToString(),
					" items</b>"
				});
				if (contentContainer.content != null)
				{
					text += "\n";
					for (int i = 0; i < contentContainer.content.Count; i++)
					{
						text = string.Concat(new string[]
						{
							text,
							"    • ",
							contentContainer.content[i].ToString(),
							": ",
							CodexEntry.GetContentWidgetDebugString(contentContainer.content[i]),
							"\n"
						});
					}
				}
				list.Add(text);
			}
			else
			{
				list.Add("null container");
			}
		}
		return list;
	}

	// Token: 0x0600642F RID: 25647 RVA: 0x002555D8 File Offset: 0x002537D8
	private static string GetContentWidgetDebugString(ICodexWidget widget)
	{
		CodexText codexText = widget as CodexText;
		if (codexText != null)
		{
			return codexText.text;
		}
		CodexLabelWithIcon codexLabelWithIcon = widget as CodexLabelWithIcon;
		if (codexLabelWithIcon != null)
		{
			return codexLabelWithIcon.label.text + " / " + codexLabelWithIcon.icon.spriteName;
		}
		CodexImage codexImage = widget as CodexImage;
		if (codexImage != null)
		{
			return codexImage.spriteName;
		}
		CodexVideo codexVideo = widget as CodexVideo;
		if (codexVideo != null)
		{
			return codexVideo.name;
		}
		CodexIndentedLabelWithIcon codexIndentedLabelWithIcon = widget as CodexIndentedLabelWithIcon;
		if (codexIndentedLabelWithIcon != null)
		{
			return codexIndentedLabelWithIcon.label.text + " / " + codexIndentedLabelWithIcon.icon.spriteName;
		}
		return "";
	}

	// Token: 0x06006430 RID: 25648 RVA: 0x00255677 File Offset: 0x00253877
	public void CreateContentContainerCollection()
	{
		this.contentContainers = new List<ContentContainer>();
	}

	// Token: 0x06006431 RID: 25649 RVA: 0x00255684 File Offset: 0x00253884
	public void InsertContentContainer(int index, ContentContainer container)
	{
		this.contentContainers.Insert(index, container);
	}

	// Token: 0x06006432 RID: 25650 RVA: 0x00255693 File Offset: 0x00253893
	public void RemoveContentContainerAt(int index)
	{
		this.contentContainers.RemoveAt(index);
	}

	// Token: 0x06006433 RID: 25651 RVA: 0x002556A1 File Offset: 0x002538A1
	public void AddContentContainer(ContentContainer container)
	{
		this.contentContainers.Add(container);
	}

	// Token: 0x06006434 RID: 25652 RVA: 0x002556AF File Offset: 0x002538AF
	public void AddContentContainerRange(IEnumerable<ContentContainer> containers)
	{
		this.contentContainers.AddRange(containers);
	}

	// Token: 0x06006435 RID: 25653 RVA: 0x002556BD File Offset: 0x002538BD
	public void RemoveContentContainer(ContentContainer container)
	{
		this.contentContainers.Remove(container);
	}

	// Token: 0x06006436 RID: 25654 RVA: 0x002556CC File Offset: 0x002538CC
	public ICodexWidget GetFirstWidget()
	{
		for (int i = 0; i < this.contentContainers.Count; i++)
		{
			if (this.contentContainers[i].content != null)
			{
				for (int j = 0; j < this.contentContainers[i].content.Count; j++)
				{
					if (this.contentContainers[i].content[j] != null && Game.IsCorrectDlcActiveForCurrentSave(this.contentContainers[i].content[j] as IHasDlcRestrictions))
					{
						return this.contentContainers[i].content[j];
					}
				}
			}
		}
		return null;
	}

	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x06006437 RID: 25655 RVA: 0x0025577E File Offset: 0x0025397E
	// (set) Token: 0x06006438 RID: 25656 RVA: 0x00255786 File Offset: 0x00253986
	public string[] requiredAtLeastOneDlcIds { get; set; }

	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x06006439 RID: 25657 RVA: 0x0025578F File Offset: 0x0025398F
	// (set) Token: 0x0600643A RID: 25658 RVA: 0x00255797 File Offset: 0x00253997
	public string[] requiredDlcIds { get; set; }

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x0600643B RID: 25659 RVA: 0x002557A0 File Offset: 0x002539A0
	// (set) Token: 0x0600643C RID: 25660 RVA: 0x002557A8 File Offset: 0x002539A8
	public string[] forbiddenDlcIds { get; set; }

	// Token: 0x0600643D RID: 25661 RVA: 0x002557B1 File Offset: 0x002539B1
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x0600643E RID: 25662 RVA: 0x002557B9 File Offset: 0x002539B9
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x0600643F RID: 25663 RVA: 0x002557C1 File Offset: 0x002539C1
	public string[] GetAnyRequiredDlcIds()
	{
		return this.requiredAtLeastOneDlcIds;
	}

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x06006440 RID: 25664 RVA: 0x002557C9 File Offset: 0x002539C9
	// (set) Token: 0x06006441 RID: 25665 RVA: 0x002557D1 File Offset: 0x002539D1
	public string id
	{
		get
		{
			return this._id;
		}
		set
		{
			this._id = value;
		}
	}

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x06006442 RID: 25666 RVA: 0x002557DA File Offset: 0x002539DA
	// (set) Token: 0x06006443 RID: 25667 RVA: 0x002557E2 File Offset: 0x002539E2
	public string parentId
	{
		get
		{
			return this._parentId;
		}
		set
		{
			this._parentId = value;
		}
	}

	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x06006444 RID: 25668 RVA: 0x002557EB File Offset: 0x002539EB
	// (set) Token: 0x06006445 RID: 25669 RVA: 0x002557F3 File Offset: 0x002539F3
	public string category
	{
		get
		{
			return this._category;
		}
		set
		{
			this._category = value;
		}
	}

	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x06006446 RID: 25670 RVA: 0x002557FC File Offset: 0x002539FC
	// (set) Token: 0x06006447 RID: 25671 RVA: 0x00255804 File Offset: 0x00253A04
	public string title
	{
		get
		{
			return this._title;
		}
		set
		{
			this._title = value;
		}
	}

	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x06006448 RID: 25672 RVA: 0x0025580D File Offset: 0x00253A0D
	// (set) Token: 0x06006449 RID: 25673 RVA: 0x00255815 File Offset: 0x00253A15
	public string name
	{
		get
		{
			return this._name;
		}
		set
		{
			this._name = value;
		}
	}

	// Token: 0x17000734 RID: 1844
	// (get) Token: 0x0600644A RID: 25674 RVA: 0x0025581E File Offset: 0x00253A1E
	// (set) Token: 0x0600644B RID: 25675 RVA: 0x00255826 File Offset: 0x00253A26
	public string subtitle
	{
		get
		{
			return this._subtitle;
		}
		set
		{
			this._subtitle = value;
		}
	}

	// Token: 0x17000735 RID: 1845
	// (get) Token: 0x0600644C RID: 25676 RVA: 0x0025582F File Offset: 0x00253A2F
	// (set) Token: 0x0600644D RID: 25677 RVA: 0x00255837 File Offset: 0x00253A37
	public List<SubEntry> subEntries
	{
		get
		{
			return this._subEntries;
		}
		set
		{
			this._subEntries = value;
		}
	}

	// Token: 0x17000736 RID: 1846
	// (get) Token: 0x0600644E RID: 25678 RVA: 0x00255840 File Offset: 0x00253A40
	// (set) Token: 0x0600644F RID: 25679 RVA: 0x00255848 File Offset: 0x00253A48
	public List<CodexEntry_MadeAndUsed> contentMadeAndUsed
	{
		get
		{
			return this._contentMadeAndUsed;
		}
		set
		{
			this._contentMadeAndUsed = value;
		}
	}

	// Token: 0x17000737 RID: 1847
	// (get) Token: 0x06006450 RID: 25680 RVA: 0x00255851 File Offset: 0x00253A51
	// (set) Token: 0x06006451 RID: 25681 RVA: 0x00255859 File Offset: 0x00253A59
	public Sprite icon
	{
		get
		{
			return this._icon;
		}
		set
		{
			this._icon = value;
		}
	}

	// Token: 0x17000738 RID: 1848
	// (get) Token: 0x06006452 RID: 25682 RVA: 0x00255862 File Offset: 0x00253A62
	// (set) Token: 0x06006453 RID: 25683 RVA: 0x0025586A File Offset: 0x00253A6A
	public Color iconColor
	{
		get
		{
			return this._iconColor;
		}
		set
		{
			this._iconColor = value;
		}
	}

	// Token: 0x17000739 RID: 1849
	// (get) Token: 0x06006454 RID: 25684 RVA: 0x00255873 File Offset: 0x00253A73
	// (set) Token: 0x06006455 RID: 25685 RVA: 0x0025587B File Offset: 0x00253A7B
	public string iconPrefabID
	{
		get
		{
			return this._iconPrefabID;
		}
		set
		{
			this._iconPrefabID = value;
		}
	}

	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x06006456 RID: 25686 RVA: 0x00255884 File Offset: 0x00253A84
	// (set) Token: 0x06006457 RID: 25687 RVA: 0x0025588C File Offset: 0x00253A8C
	public string iconLockID
	{
		get
		{
			return this._iconLockID;
		}
		set
		{
			this._iconLockID = value;
		}
	}

	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x06006458 RID: 25688 RVA: 0x00255895 File Offset: 0x00253A95
	// (set) Token: 0x06006459 RID: 25689 RVA: 0x0025589D File Offset: 0x00253A9D
	public string iconAssetName
	{
		get
		{
			return this._iconAssetName;
		}
		set
		{
			this._iconAssetName = value;
		}
	}

	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x0600645A RID: 25690 RVA: 0x002558A6 File Offset: 0x00253AA6
	// (set) Token: 0x0600645B RID: 25691 RVA: 0x002558AE File Offset: 0x00253AAE
	public bool disabled
	{
		get
		{
			return this._disabled;
		}
		set
		{
			this._disabled = value;
		}
	}

	// Token: 0x1700073D RID: 1853
	// (get) Token: 0x0600645C RID: 25692 RVA: 0x002558B7 File Offset: 0x00253AB7
	// (set) Token: 0x0600645D RID: 25693 RVA: 0x002558BF File Offset: 0x00253ABF
	public bool searchOnly
	{
		get
		{
			return this._searchOnly;
		}
		set
		{
			this._searchOnly = value;
		}
	}

	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x0600645E RID: 25694 RVA: 0x002558C8 File Offset: 0x00253AC8
	// (set) Token: 0x0600645F RID: 25695 RVA: 0x002558D0 File Offset: 0x00253AD0
	public int customContentLength
	{
		get
		{
			return this._customContentLength;
		}
		set
		{
			this._customContentLength = value;
		}
	}

	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x06006460 RID: 25696 RVA: 0x002558D9 File Offset: 0x00253AD9
	// (set) Token: 0x06006461 RID: 25697 RVA: 0x002558E1 File Offset: 0x00253AE1
	public string sortString
	{
		get
		{
			return this._sortString;
		}
		set
		{
			this._sortString = value;
		}
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x06006462 RID: 25698 RVA: 0x002558EA File Offset: 0x00253AEA
	// (set) Token: 0x06006463 RID: 25699 RVA: 0x002558F2 File Offset: 0x00253AF2
	public bool showBeforeGeneratedCategoryLinks
	{
		get
		{
			return this._showBeforeGeneratedCategoryLinks;
		}
		set
		{
			this._showBeforeGeneratedCategoryLinks = value;
		}
	}

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x06006464 RID: 25700 RVA: 0x002558FB File Offset: 0x00253AFB
	// (set) Token: 0x06006465 RID: 25701 RVA: 0x00255903 File Offset: 0x00253B03
	public bool insertMergeContentAtBottom
	{
		get
		{
			return this._insertMergeContentAtBottom;
		}
		set
		{
			this._insertMergeContentAtBottom = value;
		}
	}

	// Token: 0x04004417 RID: 17431
	public EntryDevLog log = new EntryDevLog();

	// Token: 0x04004418 RID: 17432
	private List<ContentContainer> _contentContainers = new List<ContentContainer>();

	// Token: 0x0400441C RID: 17436
	private string _id;

	// Token: 0x0400441D RID: 17437
	private string _parentId;

	// Token: 0x0400441E RID: 17438
	private string _category;

	// Token: 0x0400441F RID: 17439
	private string _title;

	// Token: 0x04004420 RID: 17440
	private string _name;

	// Token: 0x04004421 RID: 17441
	private string _subtitle;

	// Token: 0x04004422 RID: 17442
	private List<SubEntry> _subEntries = new List<SubEntry>();

	// Token: 0x04004423 RID: 17443
	private List<CodexEntry_MadeAndUsed> _contentMadeAndUsed = new List<CodexEntry_MadeAndUsed>();

	// Token: 0x04004424 RID: 17444
	private Sprite _icon;

	// Token: 0x04004425 RID: 17445
	private Color _iconColor = Color.white;

	// Token: 0x04004426 RID: 17446
	private string _iconPrefabID;

	// Token: 0x04004427 RID: 17447
	private string _iconLockID;

	// Token: 0x04004428 RID: 17448
	private string _iconAssetName;

	// Token: 0x04004429 RID: 17449
	private bool _disabled;

	// Token: 0x0400442A RID: 17450
	private bool _searchOnly;

	// Token: 0x0400442B RID: 17451
	private int _customContentLength;

	// Token: 0x0400442C RID: 17452
	private string _sortString;

	// Token: 0x0400442D RID: 17453
	private bool _showBeforeGeneratedCategoryLinks;

	// Token: 0x0400442E RID: 17454
	private bool _insertMergeContentAtBottom;
}
