using System;
using System.Collections.Generic;

// Token: 0x0200083E RID: 2110
public class Chatty : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x0600398E RID: 14734 RVA: 0x00141882 File Offset: 0x0013FA82
	protected override void OnPrefabInit()
	{
		base.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		base.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		this.identity = base.GetComponent<MinionIdentity>();
	}

	// Token: 0x0600398F RID: 14735 RVA: 0x001418BC File Offset: 0x0013FABC
	private void OnStartedTalking(object data)
	{
		MinionIdentity minionIdentity = data as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		this.conversationPartners.Add(minionIdentity);
	}

	// Token: 0x06003990 RID: 14736 RVA: 0x001418E8 File Offset: 0x0013FAE8
	public void SimEveryTick(float dt)
	{
		if (this.conversationPartners.Count == 0)
		{
			return;
		}
		for (int i = this.conversationPartners.Count - 1; i >= 0; i--)
		{
			MinionIdentity minionIdentity = this.conversationPartners[i];
			this.conversationPartners.RemoveAt(i);
			if (!(minionIdentity == this.identity))
			{
				minionIdentity.AddTag(GameTags.PleasantConversation);
			}
		}
		base.gameObject.AddTag(GameTags.PleasantConversation);
	}

	// Token: 0x04002340 RID: 9024
	private MinionIdentity identity;

	// Token: 0x04002341 RID: 9025
	private List<MinionIdentity> conversationPartners = new List<MinionIdentity>();
}
