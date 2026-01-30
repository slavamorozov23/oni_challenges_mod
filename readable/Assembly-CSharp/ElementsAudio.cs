using System;

// Token: 0x02000579 RID: 1401
public class ElementsAudio
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06001F25 RID: 7973 RVA: 0x000A9BBE File Offset: 0x000A7DBE
	public static ElementsAudio Instance
	{
		get
		{
			if (ElementsAudio._instance == null)
			{
				ElementsAudio._instance = new ElementsAudio();
			}
			return ElementsAudio._instance;
		}
	}

	// Token: 0x06001F26 RID: 7974 RVA: 0x000A9BD6 File Offset: 0x000A7DD6
	public void LoadData(ElementsAudio.ElementAudioConfig[] elements_audio_configs)
	{
		this.elementAudioConfigs = elements_audio_configs;
	}

	// Token: 0x06001F27 RID: 7975 RVA: 0x000A9BE0 File Offset: 0x000A7DE0
	public ElementsAudio.ElementAudioConfig GetConfigForElement(SimHashes id)
	{
		if (this.elementAudioConfigs != null)
		{
			for (int i = 0; i < this.elementAudioConfigs.Length; i++)
			{
				if (this.elementAudioConfigs[i].elementID == id)
				{
					return this.elementAudioConfigs[i];
				}
			}
		}
		return null;
	}

	// Token: 0x0400122C RID: 4652
	private static ElementsAudio _instance;

	// Token: 0x0400122D RID: 4653
	private ElementsAudio.ElementAudioConfig[] elementAudioConfigs;

	// Token: 0x02001406 RID: 5126
	public class ElementAudioConfig : Resource
	{
		// Token: 0x04006D3B RID: 27963
		public SimHashes elementID;

		// Token: 0x04006D3C RID: 27964
		public AmbienceType ambienceType = AmbienceType.None;

		// Token: 0x04006D3D RID: 27965
		public SolidAmbienceType solidAmbienceType = SolidAmbienceType.None;

		// Token: 0x04006D3E RID: 27966
		public string miningSound = "";

		// Token: 0x04006D3F RID: 27967
		public string miningBreakSound = "";

		// Token: 0x04006D40 RID: 27968
		public string oreBumpSound = "";

		// Token: 0x04006D41 RID: 27969
		public string floorEventAudioCategory = "";

		// Token: 0x04006D42 RID: 27970
		public string creatureChewSound = "";
	}
}
