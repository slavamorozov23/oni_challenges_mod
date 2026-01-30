using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

// Token: 0x02000DC9 RID: 3529
[AddComponentMenu("KMonoBehaviour/scripts/OpenURLButtons")]
public class OpenURLButtons : KMonoBehaviour
{
	// Token: 0x06006E41 RID: 28225 RVA: 0x0029BCC8 File Offset: 0x00299EC8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		for (int i = 0; i < this.buttonData.Count; i++)
		{
			OpenURLButtons.URLButtonData data = this.buttonData[i];
			GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, base.gameObject, true);
			string text = Strings.Get(data.stringKey);
			gameObject.GetComponentInChildren<LocText>().SetText(text);
			switch (data.urlType)
			{
			case OpenURLButtons.URLButtonType.url:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenURL(data.url);
				};
				break;
			case OpenURLButtons.URLButtonType.platformUrl:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenPlatformURL(data.url);
				};
				break;
			case OpenURLButtons.URLButtonType.patchNotes:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenPatchNotes();
				};
				break;
			case OpenURLButtons.URLButtonType.feedbackScreen:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenFeedbackScreen();
				};
				break;
			}
		}
	}

	// Token: 0x06006E42 RID: 28226 RVA: 0x0029BDD3 File Offset: 0x00299FD3
	public void OpenPatchNotes()
	{
		Util.KInstantiateUI(this.patchNotesScreenPrefab, FrontEndManager.Instance.gameObject, true);
	}

	// Token: 0x06006E43 RID: 28227 RVA: 0x0029BDEC File Offset: 0x00299FEC
	public void OpenFeedbackScreen()
	{
		Util.KInstantiateUI(this.feedbackScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, true);
	}

	// Token: 0x06006E44 RID: 28228 RVA: 0x0029BE0A File Offset: 0x0029A00A
	public void OpenURL(string URL)
	{
		App.OpenWebURL(URL);
	}

	// Token: 0x06006E45 RID: 28229 RVA: 0x0029BE14 File Offset: 0x0029A014
	public void OpenPlatformURL(string URL)
	{
		if (DistributionPlatform.Inst.Platform == "Steam" && DistributionPlatform.Inst.Initialized)
		{
			DistributionPlatform.Inst.GetAuthTicket(delegate(byte[] ticket)
			{
				string newValue = string.Concat(Array.ConvertAll<byte, string>(ticket, (byte x) => x.ToString("X2")));
				App.OpenWebURL(URL.Replace("{SteamID}", DistributionPlatform.Inst.LocalUser.Id.ToInt64().ToString()).Replace("{SteamTicket}", newValue));
			});
			return;
		}
		string value = URL.Replace("{SteamID}", "").Replace("{SteamTicket}", "");
		App.OpenWebURL("https://accounts.klei.com/login?goto={gotoUrl}".Replace("{gotoUrl}", WebUtility.HtmlEncode(value)));
	}

	// Token: 0x04004B54 RID: 19284
	public GameObject buttonPrefab;

	// Token: 0x04004B55 RID: 19285
	public List<OpenURLButtons.URLButtonData> buttonData;

	// Token: 0x04004B56 RID: 19286
	[SerializeField]
	private GameObject patchNotesScreenPrefab;

	// Token: 0x04004B57 RID: 19287
	[SerializeField]
	private FeedbackScreen feedbackScreenPrefab;

	// Token: 0x02002015 RID: 8213
	public enum URLButtonType
	{
		// Token: 0x040094C9 RID: 38089
		url,
		// Token: 0x040094CA RID: 38090
		platformUrl,
		// Token: 0x040094CB RID: 38091
		patchNotes,
		// Token: 0x040094CC RID: 38092
		feedbackScreen
	}

	// Token: 0x02002016 RID: 8214
	[Serializable]
	public class URLButtonData
	{
		// Token: 0x040094CD RID: 38093
		public string stringKey;

		// Token: 0x040094CE RID: 38094
		public OpenURLButtons.URLButtonType urlType;

		// Token: 0x040094CF RID: 38095
		public string url;
	}
}
