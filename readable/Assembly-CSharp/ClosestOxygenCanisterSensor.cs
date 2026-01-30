using System;
using System.Collections.Generic;

// Token: 0x02000521 RID: 1313
public class ClosestOxygenCanisterSensor : ClosestPickupableSensor<Pickupable>
{
	// Token: 0x06001C5D RID: 7261 RVA: 0x0009C1EC File Offset: 0x0009A3EC
	public ClosestOxygenCanisterSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.Gas, shouldStartActive)
	{
		this.requiredTags = new Tag[]
		{
			GameTags.Breathable
		};
		this.BreathableGasses = ElementLoader.FindElements((Element element) => element.HasTag(GameTags.Breathable) && element.HasTag(GameTags.Gas));
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x0009C248 File Offset: 0x0009A448
	public override HashSet<Tag> GetForbbidenTags()
	{
		if (this.consumableConsumer == null)
		{
			return new HashSet<Tag>(0);
		}
		HashSet<Tag> forbbidenTags = base.GetForbbidenTags();
		if (forbbidenTags == null || forbbidenTags.Count <= 0)
		{
			return forbbidenTags;
		}
		Tag[] array = new Tag[forbbidenTags.Count];
		base.GetForbbidenTags().CopyTo(array);
		HashSet<Tag> hashSet = new HashSet<Tag>();
		int i = 0;
		while (i < array.Length)
		{
			Tag tag = array[i];
			if (tag == ClosestOxygenCanisterSensor.GenericBreathableGassesTankTag)
			{
				using (List<Element>.Enumerator enumerator = this.BreathableGasses.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Element element = enumerator.Current;
						hashSet.Add(element.id.ToString());
					}
					goto IL_BB;
				}
				goto IL_B2;
			}
			goto IL_B2;
			IL_BB:
			i++;
			continue;
			IL_B2:
			hashSet.Add(tag);
			goto IL_BB;
		}
		return hashSet;
	}

	// Token: 0x040010B4 RID: 4276
	public static readonly Tag GenericBreathableGassesTankTag = new Tag("BreathableGasTank");

	// Token: 0x040010B5 RID: 4277
	private List<Element> BreathableGasses;
}
