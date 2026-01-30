using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000D6A RID: 3434
[Serializable]
public class MainMenu_Motd
{
	// Token: 0x06006A76 RID: 27254 RVA: 0x00284618 File Offset: 0x00282818
	public void Setup()
	{
		this.CleanUp();
		this.boxA.gameObject.SetActive(false);
		this.boxB.gameObject.SetActive(false);
		this.boxC.gameObject.SetActive(false);
		this.motdDataFetchRequest = new MotdDataFetchRequest();
		this.motdDataFetchRequest.Fetch(MotdDataFetchRequest.BuildUrl());
		this.motdDataFetchRequest.OnComplete(delegate(MotdData motdData)
		{
			this.RecieveMotdData(motdData);
		});
	}

	// Token: 0x06006A77 RID: 27255 RVA: 0x00284690 File Offset: 0x00282890
	public void CleanUp()
	{
		if (this.motdDataFetchRequest != null)
		{
			this.motdDataFetchRequest.Dispose();
			this.motdDataFetchRequest = null;
		}
	}

	// Token: 0x06006A78 RID: 27256 RVA: 0x002846AC File Offset: 0x002828AC
	private void RecieveMotdData(MotdData motdData)
	{
		MainMenu_Motd.<>c__DisplayClass6_0 CS$<>8__locals1 = new MainMenu_Motd.<>c__DisplayClass6_0();
		CS$<>8__locals1.<>4__this = this;
		if (motdData == null || motdData.boxesLive == null || motdData.boxesLive.Count == 0)
		{
			global::Debug.LogWarning("MOTD Error: failed to get valid motd data, hiding ui.");
			this.boxA.gameObject.SetActive(false);
			this.boxB.gameObject.SetActive(false);
			this.boxC.gameObject.SetActive(false);
			return;
		}
		CS$<>8__locals1.boxes = motdData.boxesLive.StableSort((MotdData_Box a, MotdData_Box b) => CS$<>8__locals1.<>4__this.CalcScore(a).CompareTo(CS$<>8__locals1.<>4__this.CalcScore(b))).ToList<MotdData_Box>();
		MotdData_Box motdData_Box = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("PatchNotes");
		MotdData_Box motdData_Box2 = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("News");
		MotdData_Box motdData_Box3 = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("Skins");
		if (motdData_Box != null)
		{
			this.boxA.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box)
			});
			this.boxA.gameObject.SetActive(true);
		}
		if (motdData_Box2 != null)
		{
			this.boxB.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box2)
			});
			this.boxB.gameObject.SetActive(true);
		}
		if (motdData_Box3 != null)
		{
			this.boxC.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box3)
			});
			this.boxC.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006A79 RID: 27257 RVA: 0x002847EF File Offset: 0x002829EF
	private int CalcScore(MotdData_Box box)
	{
		return 0;
	}

	// Token: 0x06006A7A RID: 27258 RVA: 0x002847F2 File Offset: 0x002829F2
	private MotdBox.PageData ConvertToPageData(MotdData_Box box)
	{
		return new MotdBox.PageData
		{
			Texture = box.resolvedImage,
			HeaderText = box.title,
			ImageText = box.text,
			URL = box.href
		};
	}

	// Token: 0x0400493C RID: 18748
	[SerializeField]
	private MotdBox boxA;

	// Token: 0x0400493D RID: 18749
	[SerializeField]
	private MotdBox boxB;

	// Token: 0x0400493E RID: 18750
	[SerializeField]
	private MotdBox boxC;

	// Token: 0x0400493F RID: 18751
	private MotdDataFetchRequest motdDataFetchRequest;
}
