using System;
using System.Collections.Generic;

// Token: 0x02000861 RID: 2145
[Serializable]
public class AttackProperties
{
	// Token: 0x040023CA RID: 9162
	public Weapon attacker;

	// Token: 0x040023CB RID: 9163
	public AttackProperties.DamageType damageType;

	// Token: 0x040023CC RID: 9164
	public AttackProperties.TargetType targetType;

	// Token: 0x040023CD RID: 9165
	public float base_damage_min;

	// Token: 0x040023CE RID: 9166
	public float base_damage_max;

	// Token: 0x040023CF RID: 9167
	public int maxHits;

	// Token: 0x040023D0 RID: 9168
	public float aoe_radius = 2f;

	// Token: 0x040023D1 RID: 9169
	public List<AttackEffect> effects;

	// Token: 0x0200181E RID: 6174
	public enum DamageType
	{
		// Token: 0x040079DA RID: 31194
		Standard
	}

	// Token: 0x0200181F RID: 6175
	public enum TargetType
	{
		// Token: 0x040079DC RID: 31196
		Single,
		// Token: 0x040079DD RID: 31197
		AreaOfEffect
	}
}
