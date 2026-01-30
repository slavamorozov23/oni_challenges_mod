using System;

// Token: 0x020006DA RID: 1754
internal class ElementAudioFileLoader : AsyncCsvLoader<ElementAudioFileLoader, ElementsAudio.ElementAudioConfig>
{
	// Token: 0x06002B19 RID: 11033 RVA: 0x000FC239 File Offset: 0x000FA439
	public ElementAudioFileLoader() : base(Assets.instance.elementAudio)
	{
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x000FC24B File Offset: 0x000FA44B
	public override void Run()
	{
		base.Run();
	}
}
