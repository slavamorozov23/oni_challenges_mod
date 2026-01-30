using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CDF RID: 3295
public class ComicViewer : KScreen
{
	// Token: 0x060065CD RID: 26061 RVA: 0x00265754 File Offset: 0x00263954
	public void ShowComic(ComicData comic, bool isVictoryComic)
	{
		for (int i = 0; i < Mathf.Max(comic.images.Length, comic.stringKeys.Length); i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.panelPrefab, this.contentContainer, true);
			this.activePanels.Add(gameObject);
			gameObject.GetComponentInChildren<Image>().sprite = comic.images[i];
			gameObject.GetComponentInChildren<LocText>().SetText(comic.stringKeys[i]);
		}
		this.closeButton.ClearOnClick();
		if (isVictoryComic)
		{
			this.closeButton.onClick += delegate()
			{
				this.Stop();
				this.Show(false);
			};
			return;
		}
		this.closeButton.onClick += delegate()
		{
			this.Stop();
		};
	}

	// Token: 0x060065CE RID: 26062 RVA: 0x00265803 File Offset: 0x00263A03
	public void Stop()
	{
		this.OnStop();
		this.Show(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x04004557 RID: 17751
	public GameObject panelPrefab;

	// Token: 0x04004558 RID: 17752
	public GameObject contentContainer;

	// Token: 0x04004559 RID: 17753
	public List<GameObject> activePanels = new List<GameObject>();

	// Token: 0x0400455A RID: 17754
	public KButton closeButton;

	// Token: 0x0400455B RID: 17755
	public System.Action OnStop;
}
