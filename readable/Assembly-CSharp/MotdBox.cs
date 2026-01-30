using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DB3 RID: 3507
public class MotdBox : KMonoBehaviour
{
	// Token: 0x06006D77 RID: 28023 RVA: 0x0029787C File Offset: 0x00295A7C
	public void Config(MotdBox.PageData[] data)
	{
		this.pageDatas = data;
		if (this.pageButtons != null)
		{
			for (int i = this.pageButtons.Length - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.pageButtons[i]);
			}
			this.pageButtons = null;
		}
		this.pageButtons = new GameObject[data.Length];
		for (int j = 0; j < this.pageButtons.Length; j++)
		{
			int idx = j;
			GameObject gameObject = Util.KInstantiateUI(this.pageCarouselButtonPrefab, this.pageCarouselContainer, false);
			gameObject.SetActive(true);
			this.pageButtons[j] = gameObject;
			MultiToggle component = gameObject.GetComponent<MultiToggle>();
			component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
			{
				this.SwitchPage(idx);
			}));
		}
		this.SwitchPage(0);
	}

	// Token: 0x06006D78 RID: 28024 RVA: 0x00297948 File Offset: 0x00295B48
	private void SwitchPage(int newPage)
	{
		this.selectedPage = newPage;
		for (int i = 0; i < this.pageButtons.Length; i++)
		{
			this.pageButtons[i].GetComponent<MultiToggle>().ChangeState((i == this.selectedPage) ? 1 : 0);
		}
		this.image.texture = this.pageDatas[newPage].Texture;
		this.headerLabel.SetText(this.pageDatas[newPage].HeaderText);
		this.urlOpener.SetURL(this.pageDatas[newPage].URL);
		if (string.IsNullOrEmpty(this.pageDatas[newPage].ImageText))
		{
			this.imageLabel.gameObject.SetActive(false);
			this.imageLabel.SetText("");
			return;
		}
		this.imageLabel.gameObject.SetActive(true);
		this.imageLabel.SetText(this.pageDatas[newPage].ImageText);
	}

	// Token: 0x04004AC5 RID: 19141
	[SerializeField]
	private GameObject pageCarouselContainer;

	// Token: 0x04004AC6 RID: 19142
	[SerializeField]
	private GameObject pageCarouselButtonPrefab;

	// Token: 0x04004AC7 RID: 19143
	[SerializeField]
	private RawImage image;

	// Token: 0x04004AC8 RID: 19144
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04004AC9 RID: 19145
	[SerializeField]
	private LocText imageLabel;

	// Token: 0x04004ACA RID: 19146
	[SerializeField]
	private URLOpenFunction urlOpener;

	// Token: 0x04004ACB RID: 19147
	private int selectedPage;

	// Token: 0x04004ACC RID: 19148
	private GameObject[] pageButtons;

	// Token: 0x04004ACD RID: 19149
	private MotdBox.PageData[] pageDatas;

	// Token: 0x02002002 RID: 8194
	public class PageData
	{
		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x0600B807 RID: 47111 RVA: 0x003F4323 File Offset: 0x003F2523
		// (set) Token: 0x0600B808 RID: 47112 RVA: 0x003F432B File Offset: 0x003F252B
		public Texture2D Texture { get; set; }

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x0600B809 RID: 47113 RVA: 0x003F4334 File Offset: 0x003F2534
		// (set) Token: 0x0600B80A RID: 47114 RVA: 0x003F433C File Offset: 0x003F253C
		public string HeaderText { get; set; }

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x0600B80B RID: 47115 RVA: 0x003F4345 File Offset: 0x003F2545
		// (set) Token: 0x0600B80C RID: 47116 RVA: 0x003F434D File Offset: 0x003F254D
		public string ImageText { get; set; }

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x0600B80D RID: 47117 RVA: 0x003F4356 File Offset: 0x003F2556
		// (set) Token: 0x0600B80E RID: 47118 RVA: 0x003F435E File Offset: 0x003F255E
		public string URL { get; set; }
	}
}
