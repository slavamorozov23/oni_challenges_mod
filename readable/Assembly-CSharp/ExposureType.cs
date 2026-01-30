using System;
using System.Collections.Generic;

// Token: 0x02000A29 RID: 2601
public class ExposureType
{
	// Token: 0x04003275 RID: 12917
	public string germ_id;

	// Token: 0x04003276 RID: 12918
	public string sickness_id;

	// Token: 0x04003277 RID: 12919
	public string infection_effect;

	// Token: 0x04003278 RID: 12920
	public int exposure_threshold;

	// Token: 0x04003279 RID: 12921
	public bool infect_immediately;

	// Token: 0x0400327A RID: 12922
	public List<string> required_traits;

	// Token: 0x0400327B RID: 12923
	public List<string> excluded_traits;

	// Token: 0x0400327C RID: 12924
	public List<string> excluded_effects;

	// Token: 0x0400327D RID: 12925
	public int base_resistance;
}
