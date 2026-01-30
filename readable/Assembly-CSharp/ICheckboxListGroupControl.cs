using System;

// Token: 0x02000E24 RID: 3620
public interface ICheckboxListGroupControl
{
	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x060072D2 RID: 29394
	string Title { get; }

	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x060072D3 RID: 29395
	string Description { get; }

	// Token: 0x060072D4 RID: 29396
	ICheckboxListGroupControl.ListGroup[] GetData();

	// Token: 0x060072D5 RID: 29397
	bool SidescreenEnabled();

	// Token: 0x060072D6 RID: 29398
	int CheckboxSideScreenSortOrder();

	// Token: 0x020020A4 RID: 8356
	public struct ListGroup
	{
		// Token: 0x0600B9F8 RID: 47608 RVA: 0x003FA21E File Offset: 0x003F841E
		public ListGroup(string title, ICheckboxListGroupControl.CheckboxItem[] checkboxItems, Func<string, string> resolveTitleCallback = null, System.Action onItemClicked = null)
		{
			this.title = title;
			this.checkboxItems = checkboxItems;
			this.resolveTitleCallback = resolveTitleCallback;
			this.onItemClicked = onItemClicked;
		}

		// Token: 0x040096C7 RID: 38599
		public Func<string, string> resolveTitleCallback;

		// Token: 0x040096C8 RID: 38600
		public System.Action onItemClicked;

		// Token: 0x040096C9 RID: 38601
		public string title;

		// Token: 0x040096CA RID: 38602
		public ICheckboxListGroupControl.CheckboxItem[] checkboxItems;
	}

	// Token: 0x020020A5 RID: 8357
	public struct CheckboxItem
	{
		// Token: 0x040096CB RID: 38603
		public string text;

		// Token: 0x040096CC RID: 38604
		public string tooltip;

		// Token: 0x040096CD RID: 38605
		public bool isOn;

		// Token: 0x040096CE RID: 38606
		public Func<string, bool> overrideLinkActions;

		// Token: 0x040096CF RID: 38607
		public Func<string, object, string> resolveTooltipCallback;
	}
}
