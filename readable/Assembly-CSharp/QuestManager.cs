using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000ABF RID: 2751
[SerializationConfig(MemberSerialization.OptIn)]
public class QuestManager : KMonoBehaviour
{
	// Token: 0x06004FFF RID: 20479 RVA: 0x001D0F24 File Offset: 0x001CF124
	protected override void OnPrefabInit()
	{
		if (QuestManager.instance != null)
		{
			UnityEngine.Object.Destroy(QuestManager.instance);
			return;
		}
		QuestManager.instance = this;
		base.OnPrefabInit();
	}

	// Token: 0x06005000 RID: 20480 RVA: 0x001D0F4C File Offset: 0x001CF14C
	public static QuestInstance InitializeQuest(Tag ownerId, Quest quest)
	{
		QuestInstance questInstance;
		if (!QuestManager.TryGetQuest(ownerId.GetHash(), quest, out questInstance))
		{
			questInstance = (QuestManager.instance.ownerToQuests[ownerId.GetHash()][quest.IdHash] = new QuestInstance(quest));
		}
		questInstance.Initialize(quest);
		return questInstance;
	}

	// Token: 0x06005001 RID: 20481 RVA: 0x001D0FA0 File Offset: 0x001CF1A0
	public static QuestInstance InitializeQuest(HashedString ownerId, Quest quest)
	{
		QuestInstance questInstance;
		if (!QuestManager.TryGetQuest(ownerId.HashValue, quest, out questInstance))
		{
			questInstance = (QuestManager.instance.ownerToQuests[ownerId.HashValue][quest.IdHash] = new QuestInstance(quest));
		}
		questInstance.Initialize(quest);
		return questInstance;
	}

	// Token: 0x06005002 RID: 20482 RVA: 0x001D0FF4 File Offset: 0x001CF1F4
	public static QuestInstance GetInstance(Tag ownerId, Quest quest)
	{
		QuestInstance result;
		QuestManager.TryGetQuest(ownerId.GetHash(), quest, out result);
		return result;
	}

	// Token: 0x06005003 RID: 20483 RVA: 0x001D1014 File Offset: 0x001CF214
	public static QuestInstance GetInstance(HashedString ownerId, Quest quest)
	{
		QuestInstance result;
		QuestManager.TryGetQuest(ownerId.HashValue, quest, out result);
		return result;
	}

	// Token: 0x06005004 RID: 20484 RVA: 0x001D1034 File Offset: 0x001CF234
	public static bool CheckState(HashedString ownerId, Quest quest, Quest.State state)
	{
		QuestInstance questInstance;
		QuestManager.TryGetQuest(ownerId.HashValue, quest, out questInstance);
		return questInstance != null && questInstance.CurrentState == state;
	}

	// Token: 0x06005005 RID: 20485 RVA: 0x001D1060 File Offset: 0x001CF260
	public static bool CheckState(Tag ownerId, Quest quest, Quest.State state)
	{
		QuestInstance questInstance;
		QuestManager.TryGetQuest(ownerId.GetHash(), quest, out questInstance);
		return questInstance != null && questInstance.CurrentState == state;
	}

	// Token: 0x06005006 RID: 20486 RVA: 0x001D108C File Offset: 0x001CF28C
	private static bool TryGetQuest(int ownerId, Quest quest, out QuestInstance qInst)
	{
		qInst = null;
		Dictionary<HashedString, QuestInstance> dictionary;
		if (!QuestManager.instance.ownerToQuests.TryGetValue(ownerId, out dictionary))
		{
			dictionary = (QuestManager.instance.ownerToQuests[ownerId] = new Dictionary<HashedString, QuestInstance>());
		}
		return dictionary.TryGetValue(quest.IdHash, out qInst);
	}

	// Token: 0x04003568 RID: 13672
	private static QuestManager instance;

	// Token: 0x04003569 RID: 13673
	[Serialize]
	private Dictionary<int, Dictionary<HashedString, QuestInstance>> ownerToQuests = new Dictionary<int, Dictionary<HashedString, QuestInstance>>();
}
