using System;

// Token: 0x02000B76 RID: 2934
public class ClustercraftInteriorDoor : KMonoBehaviour
{
	// Token: 0x0600573F RID: 22335 RVA: 0x001FBCBE File Offset: 0x001F9EBE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.ClusterCraftInteriorDoors.Add(this);
	}

	// Token: 0x06005740 RID: 22336 RVA: 0x001FBCD1 File Offset: 0x001F9ED1
	protected override void OnCleanUp()
	{
		Components.ClusterCraftInteriorDoors.Remove(this);
		base.OnCleanUp();
	}
}
