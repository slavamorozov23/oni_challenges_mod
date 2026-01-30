using System;
using UnityEngine;

// Token: 0x02000ED0 RID: 3792
[Serializable]
public struct ToggleState
{
	// Token: 0x040054E0 RID: 21728
	public string Name;

	// Token: 0x040054E1 RID: 21729
	public string on_click_override_sound_path;

	// Token: 0x040054E2 RID: 21730
	public string on_release_override_sound_path;

	// Token: 0x040054E3 RID: 21731
	public string sound_parameter_name;

	// Token: 0x040054E4 RID: 21732
	public float sound_parameter_value;

	// Token: 0x040054E5 RID: 21733
	public bool has_sound_parameter;

	// Token: 0x040054E6 RID: 21734
	public Sprite sprite;

	// Token: 0x040054E7 RID: 21735
	public Color color;

	// Token: 0x040054E8 RID: 21736
	public Color color_on_hover;

	// Token: 0x040054E9 RID: 21737
	public bool use_color_on_hover;

	// Token: 0x040054EA RID: 21738
	public bool use_rect_margins;

	// Token: 0x040054EB RID: 21739
	public Vector2 rect_margins;

	// Token: 0x040054EC RID: 21740
	public StatePresentationSetting[] additional_display_settings;
}
