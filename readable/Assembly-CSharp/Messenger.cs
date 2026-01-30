using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000D98 RID: 3480
[AddComponentMenu("KMonoBehaviour/scripts/Messenger")]
public class Messenger : KMonoBehaviour
{
	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x06006C5D RID: 27741 RVA: 0x00291339 File Offset: 0x0028F539
	public int Count
	{
		get
		{
			return this.messages.Count;
		}
	}

	// Token: 0x06006C5E RID: 27742 RVA: 0x00291346 File Offset: 0x0028F546
	public IEnumerator<Message> GetEnumerator()
	{
		return this.messages.GetEnumerator();
	}

	// Token: 0x06006C5F RID: 27743 RVA: 0x00291353 File Offset: 0x0028F553
	public static void DestroyInstance()
	{
		Messenger.Instance = null;
	}

	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x06006C60 RID: 27744 RVA: 0x0029135B File Offset: 0x0028F55B
	public SerializedList<Message> Messages
	{
		get
		{
			return this.messages;
		}
	}

	// Token: 0x06006C61 RID: 27745 RVA: 0x00291363 File Offset: 0x0028F563
	protected override void OnPrefabInit()
	{
		Messenger.Instance = this;
	}

	// Token: 0x06006C62 RID: 27746 RVA: 0x0029136C File Offset: 0x0028F56C
	protected override void OnSpawn()
	{
		int i = 0;
		while (i < this.messages.Count)
		{
			if (this.messages[i].IsValid())
			{
				i++;
			}
			else
			{
				this.messages.RemoveAt(i);
			}
		}
		base.Trigger(-599791736, null);
	}

	// Token: 0x06006C63 RID: 27747 RVA: 0x002913BC File Offset: 0x0028F5BC
	public void QueueMessage(Message message)
	{
		this.messages.Add(message);
		base.Trigger(1558809273, message);
	}

	// Token: 0x06006C64 RID: 27748 RVA: 0x002913D8 File Offset: 0x0028F5D8
	public Message DequeueMessage()
	{
		Message result = null;
		if (this.messages.Count > 0)
		{
			result = this.messages[0];
			this.messages.RemoveAt(0);
		}
		return result;
	}

	// Token: 0x06006C65 RID: 27749 RVA: 0x00291410 File Offset: 0x0028F610
	public void ClearAllMessages()
	{
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			this.messages.RemoveAt(i);
		}
	}

	// Token: 0x06006C66 RID: 27750 RVA: 0x00291441 File Offset: 0x0028F641
	public void RemoveMessage(Message m)
	{
		this.messages.Remove(m);
		base.Trigger(-599791736, null);
	}

	// Token: 0x04004A37 RID: 18999
	[Serialize]
	private SerializedList<Message> messages = new SerializedList<Message>();

	// Token: 0x04004A38 RID: 19000
	public static Messenger Instance;
}
