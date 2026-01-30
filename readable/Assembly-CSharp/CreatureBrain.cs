using System;

// Token: 0x02000489 RID: 1161
public class CreatureBrain : Brain
{
	// Token: 0x060018A3 RID: 6307 RVA: 0x00088E80 File Offset: 0x00087080
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Navigator component = base.GetComponent<Navigator>();
		if (component != null)
		{
			if (base.GetComponent<KPrefabID>().HasTag(GameTags.Robots.Behaviours.HasDoorPermissions))
			{
				component.SetAbilities(new RobotPathFinderAbilities(component));
				return;
			}
			component.SetAbilities(new CreaturePathFinderAbilities(component));
		}
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x00088ECE File Offset: 0x000870CE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.onPreUpdate += delegate()
		{
			Navigator component = base.GetComponent<Navigator>();
			if (component != null)
			{
				component.UpdateProbe(false);
			}
		};
	}

	// Token: 0x04000E46 RID: 3654
	public string symbolPrefix;

	// Token: 0x04000E47 RID: 3655
	public Tag species;
}
