using System;
using System.Collections.Generic;

// Token: 0x0200051E RID: 1310
public class ClosestEdibleSensor : Sensor
{
	// Token: 0x06001C56 RID: 7254 RVA: 0x0009C060 File Offset: 0x0009A260
	public ClosestEdibleSensor(Sensors sensors) : base(sensors)
	{
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x0009C06C File Offset: 0x0009A26C
	public override void Update()
	{
		HashSet<Tag> forbiddenTagSet = base.GetComponent<ConsumableConsumer>().forbiddenTagSet;
		Pickupable pickupable = Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), forbiddenTagSet, ClosestEdibleSensor.requiredSearchTags);
		bool flag = this.edibleInReachButNotPermitted;
		Edible x = null;
		bool flag2 = false;
		if (pickupable != null)
		{
			x = pickupable.GetComponent<Edible>();
			flag2 = true;
			flag = false;
		}
		else
		{
			flag = (Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), new HashSet<Tag>(), ClosestEdibleSensor.requiredSearchTags) != null);
		}
		if (x != this.edible || this.hasEdible != flag2)
		{
			this.edible = x;
			this.hasEdible = flag2;
			this.edibleInReachButNotPermitted = flag;
			base.Trigger(86328522, this.edible);
		}
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x0009C129 File Offset: 0x0009A329
	public Edible GetEdible()
	{
		return this.edible;
	}

	// Token: 0x040010AF RID: 4271
	private Edible edible;

	// Token: 0x040010B0 RID: 4272
	private bool hasEdible;

	// Token: 0x040010B1 RID: 4273
	public bool edibleInReachButNotPermitted;

	// Token: 0x040010B2 RID: 4274
	public static Tag[] requiredSearchTags = new Tag[]
	{
		GameTags.Edible
	};
}
