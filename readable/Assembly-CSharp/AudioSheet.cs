using System;
using UnityEngine;

// Token: 0x02000576 RID: 1398
[Serializable]
public class AudioSheet
{
	// Token: 0x04001226 RID: 4646
	public TextAsset asset;

	// Token: 0x04001227 RID: 4647
	public string defaultType;

	// Token: 0x04001228 RID: 4648
	public AudioSheet.SoundInfo[] soundInfos;

	// Token: 0x02001405 RID: 5125
	public class SoundInfo : Resource
	{
		// Token: 0x04006D1E RID: 27934
		public string File;

		// Token: 0x04006D1F RID: 27935
		public string Anim;

		// Token: 0x04006D20 RID: 27936
		public string Type;

		// Token: 0x04006D21 RID: 27937
		public string RequiredDlcId;

		// Token: 0x04006D22 RID: 27938
		public float MinInterval;

		// Token: 0x04006D23 RID: 27939
		public string Name0;

		// Token: 0x04006D24 RID: 27940
		public int Frame0;

		// Token: 0x04006D25 RID: 27941
		public string Name1;

		// Token: 0x04006D26 RID: 27942
		public int Frame1;

		// Token: 0x04006D27 RID: 27943
		public string Name2;

		// Token: 0x04006D28 RID: 27944
		public int Frame2;

		// Token: 0x04006D29 RID: 27945
		public string Name3;

		// Token: 0x04006D2A RID: 27946
		public int Frame3;

		// Token: 0x04006D2B RID: 27947
		public string Name4;

		// Token: 0x04006D2C RID: 27948
		public int Frame4;

		// Token: 0x04006D2D RID: 27949
		public string Name5;

		// Token: 0x04006D2E RID: 27950
		public int Frame5;

		// Token: 0x04006D2F RID: 27951
		public string Name6;

		// Token: 0x04006D30 RID: 27952
		public int Frame6;

		// Token: 0x04006D31 RID: 27953
		public string Name7;

		// Token: 0x04006D32 RID: 27954
		public int Frame7;

		// Token: 0x04006D33 RID: 27955
		public string Name8;

		// Token: 0x04006D34 RID: 27956
		public int Frame8;

		// Token: 0x04006D35 RID: 27957
		public string Name9;

		// Token: 0x04006D36 RID: 27958
		public int Frame9;

		// Token: 0x04006D37 RID: 27959
		public string Name10;

		// Token: 0x04006D38 RID: 27960
		public int Frame10;

		// Token: 0x04006D39 RID: 27961
		public string Name11;

		// Token: 0x04006D3A RID: 27962
		public int Frame11;
	}
}
