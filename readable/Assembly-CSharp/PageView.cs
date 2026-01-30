using System;
using UnityEngine;

// Token: 0x02000DD4 RID: 3540
[AddComponentMenu("KMonoBehaviour/scripts/PageView")]
public class PageView : KMonoBehaviour
{
	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x06006ED6 RID: 28374 RVA: 0x0029FE8E File Offset: 0x0029E08E
	public int ChildrenPerPage
	{
		get
		{
			return this.childrenPerPage;
		}
	}

	// Token: 0x06006ED7 RID: 28375 RVA: 0x0029FE96 File Offset: 0x0029E096
	private void Update()
	{
		if (this.oldChildCount != base.transform.childCount)
		{
			this.oldChildCount = base.transform.childCount;
			this.RefreshPage();
		}
	}

	// Token: 0x06006ED8 RID: 28376 RVA: 0x0029FEC4 File Offset: 0x0029E0C4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.nextButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.currentPage = (this.currentPage + 1) % this.pageCount;
			if (this.OnChangePage != null)
			{
				this.OnChangePage(this.currentPage);
			}
			this.RefreshPage();
		}));
		MultiToggle multiToggle2 = this.prevButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.currentPage--;
			if (this.currentPage < 0)
			{
				this.currentPage += this.pageCount;
			}
			if (this.OnChangePage != null)
			{
				this.OnChangePage(this.currentPage);
			}
			this.RefreshPage();
		}));
	}

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x06006ED9 RID: 28377 RVA: 0x0029FF28 File Offset: 0x0029E128
	private int pageCount
	{
		get
		{
			int num = base.transform.childCount / this.childrenPerPage;
			if (base.transform.childCount % this.childrenPerPage != 0)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x06006EDA RID: 28378 RVA: 0x0029FF64 File Offset: 0x0029E164
	private void RefreshPage()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (i < this.currentPage * this.childrenPerPage)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			else if (i >= this.currentPage * this.childrenPerPage + this.childrenPerPage)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			else
			{
				base.transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		this.pageLabel.SetText((this.currentPage % this.pageCount + 1).ToString() + "/" + this.pageCount.ToString());
	}

	// Token: 0x04004BCD RID: 19405
	[SerializeField]
	private MultiToggle nextButton;

	// Token: 0x04004BCE RID: 19406
	[SerializeField]
	private MultiToggle prevButton;

	// Token: 0x04004BCF RID: 19407
	[SerializeField]
	private LocText pageLabel;

	// Token: 0x04004BD0 RID: 19408
	[SerializeField]
	private int childrenPerPage = 8;

	// Token: 0x04004BD1 RID: 19409
	private int currentPage;

	// Token: 0x04004BD2 RID: 19410
	private int oldChildCount;

	// Token: 0x04004BD3 RID: 19411
	public Action<int> OnChangePage;
}
