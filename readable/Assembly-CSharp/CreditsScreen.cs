using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02000C51 RID: 3153
public class CreditsScreen : KModalScreen
{
	// Token: 0x06005FD6 RID: 24534 RVA: 0x00232948 File Offset: 0x00230B48
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (TextAsset csv in this.creditsFiles)
		{
			this.AddCredits(csv);
		}
		this.CloseButton.onClick += this.Close;
	}

	// Token: 0x06005FD7 RID: 24535 RVA: 0x00232992 File Offset: 0x00230B92
	public void Close()
	{
		this.Deactivate();
	}

	// Token: 0x06005FD8 RID: 24536 RVA: 0x0023299C File Offset: 0x00230B9C
	private void AddCredits(TextAsset csv)
	{
		string[,] array = CSVReader.SplitCsvGrid(csv.text, csv.name);
		List<string> list = new List<string>();
		for (int i = 1; i < array.GetLength(1); i++)
		{
			string text = string.Format("{0} {1}", array[0, i], array[1, i]);
			if (!(text == " "))
			{
				list.Add(text);
			}
		}
		list.Shuffle<string>();
		string text2 = array[0, 0];
		GameObject gameObject = Util.KInstantiateUI(this.teamHeaderPrefab, this.entryContainer.gameObject, true);
		gameObject.GetComponent<LocText>().text = text2;
		this.teamContainers.Add(text2, gameObject);
		foreach (string text3 in list)
		{
			Util.KInstantiateUI(this.entryPrefab, this.teamContainers[text2], true).GetComponent<LocText>().text = text3;
		}
	}

	// Token: 0x04004007 RID: 16391
	public GameObject entryPrefab;

	// Token: 0x04004008 RID: 16392
	public GameObject teamHeaderPrefab;

	// Token: 0x04004009 RID: 16393
	private Dictionary<string, GameObject> teamContainers = new Dictionary<string, GameObject>();

	// Token: 0x0400400A RID: 16394
	public Transform entryContainer;

	// Token: 0x0400400B RID: 16395
	public KButton CloseButton;

	// Token: 0x0400400C RID: 16396
	public TextAsset[] creditsFiles;
}
