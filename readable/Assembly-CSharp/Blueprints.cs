using System;

// Token: 0x02000589 RID: 1417
public class Blueprints
{
	// Token: 0x06001FB9 RID: 8121 RVA: 0x000AB944 File Offset: 0x000A9B44
	public static Blueprints Get()
	{
		if (Blueprints.instance == null)
		{
			Blueprints.instance = new Blueprints();
			Blueprints.instance.all.AddBlueprintsFrom<Blueprints_Default>(new Blueprints_Default());
			foreach (BlueprintProvider provider in Blueprints.instance.skinsReleaseProviders)
			{
				Blueprints.instance.skinsRelease.AddBlueprintsFrom<BlueprintProvider>(provider);
			}
			Blueprints.instance.all.AddBlueprintsFrom(Blueprints.instance.skinsRelease);
			Blueprints.instance.skinsRelease.PostProcess();
			Blueprints.instance.all.PostProcess();
		}
		return Blueprints.instance;
	}

	// Token: 0x0400128E RID: 4750
	public BlueprintCollection all = new BlueprintCollection();

	// Token: 0x0400128F RID: 4751
	public BlueprintCollection skinsRelease = new BlueprintCollection();

	// Token: 0x04001290 RID: 4752
	public BlueprintProvider[] skinsReleaseProviders = new BlueprintProvider[]
	{
		new Blueprints_U51AndBefore(),
		new Blueprints_DlcPack2(),
		new Blueprints_U53(),
		new Blueprints_DlcPack3(),
		new Blueprints_DlcPack4(),
		new Blueprints_U57(),
		new Blueprints_CosmeticPack1()
	};

	// Token: 0x04001291 RID: 4753
	private static Blueprints instance;
}
