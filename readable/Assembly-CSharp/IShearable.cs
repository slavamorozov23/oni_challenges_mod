using System;

// Token: 0x02000410 RID: 1040
public interface IShearable
{
	// Token: 0x0600156F RID: 5487
	bool IsFullyGrown();

	// Token: 0x06001570 RID: 5488
	void Shear();

	// Token: 0x06001571 RID: 5489
	global::Tuple<Tag, float> GetItemDroppedOnShear();
}
