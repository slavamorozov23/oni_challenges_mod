using System;

// Token: 0x02000B44 RID: 2884
public class SetDefaults
{
	// Token: 0x060054D9 RID: 21721 RVA: 0x001EF488 File Offset: 0x001ED688
	public static void Initialize()
	{
		KSlider.DefaultSounds[0] = GlobalAssets.GetSound("Slider_Start", false);
		KSlider.DefaultSounds[1] = GlobalAssets.GetSound("Slider_Move", false);
		KSlider.DefaultSounds[2] = GlobalAssets.GetSound("Slider_End", false);
		KSlider.DefaultSounds[3] = GlobalAssets.GetSound("Slider_Boundary_Low", false);
		KSlider.DefaultSounds[4] = GlobalAssets.GetSound("Slider_Boundary_High", false);
		KScrollRect.DefaultSounds[KScrollRect.SoundType.OnMouseScroll] = GlobalAssets.GetSound("Mousewheel_Move", false);
		WidgetSoundPlayer.getSoundPath = new Func<string, string>(SetDefaults.GetSoundPath);
	}

	// Token: 0x060054DA RID: 21722 RVA: 0x001EF516 File Offset: 0x001ED716
	private static string GetSoundPath(string sound_name)
	{
		return GlobalAssets.GetSound(sound_name, false);
	}
}
