using System;
using UnityEngine;

// Token: 0x02000EBE RID: 3774
public class URLOpenFunction : MonoBehaviour
{
	// Token: 0x060078EC RID: 30956 RVA: 0x002E7E12 File Offset: 0x002E6012
	private void Start()
	{
		if (this.triggerButton != null)
		{
			this.triggerButton.ClearOnClick();
			this.triggerButton.onClick += delegate()
			{
				this.OpenUrl(this.fixedURL);
			};
		}
	}

	// Token: 0x060078ED RID: 30957 RVA: 0x002E7E44 File Offset: 0x002E6044
	public void OpenUrl(string url)
	{
		if (url == "blueprints")
		{
			if (LockerMenuScreen.Instance != null)
			{
				LockerMenuScreen.Instance.ShowInventoryScreen();
				return;
			}
		}
		else
		{
			App.OpenWebURL(url);
		}
	}

	// Token: 0x060078EE RID: 30958 RVA: 0x002E7E71 File Offset: 0x002E6071
	public void SetURL(string url)
	{
		this.fixedURL = url;
	}

	// Token: 0x04005445 RID: 21573
	[SerializeField]
	private KButton triggerButton;

	// Token: 0x04005446 RID: 21574
	[SerializeField]
	private string fixedURL;
}
