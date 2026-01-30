using System;

// Token: 0x02000927 RID: 2343
public class EntitySizeVisualizer : KMonoBehaviour
{
	// Token: 0x0600418B RID: 16779 RVA: 0x0017206C File Offset: 0x0017026C
	protected override void OnPrefabInit()
	{
		OreSizeVisualizerData cmp = new OreSizeVisualizerData(base.gameObject);
		cmp.tierSetType = this.TierSetType;
		GameComps.OreSizeVisualizers.Add(base.gameObject, cmp);
		base.OnPrefabInit();
	}

	// Token: 0x0600418C RID: 16780 RVA: 0x001720AB File Offset: 0x001702AB
	protected override void OnCleanUp()
	{
		GameComps.OreSizeVisualizers.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x040028ED RID: 10477
	public OreSizeVisualizerComponents.TiersSetType TierSetType;
}
