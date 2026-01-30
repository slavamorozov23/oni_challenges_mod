using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000608 RID: 1544
[SerializationConfig(MemberSerialization.OptIn)]
public class MinionStorageDataHolder : KMonoBehaviour, StoredMinionIdentity.IStoredMinionExtension
{
	// Token: 0x06002405 RID: 9221 RVA: 0x000D07D0 File Offset: 0x000CE9D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06002406 RID: 9222 RVA: 0x000D07D8 File Offset: 0x000CE9D8
	public MinionStorageDataHolder.DataPack Internal_GetDataPack(string ID)
	{
		if (this.storedDataPacks != null)
		{
			MinionStorageDataHolder.DataPack dataPack = this.storedDataPacks.Find((MinionStorageDataHolder.DataPack d) => d.ID == ID);
			if (dataPack != null)
			{
				return dataPack;
			}
		}
		return null;
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000D0818 File Offset: 0x000CEA18
	public void Internal_UpdateData(string ID, MinionStorageDataHolder.DataPackData data)
	{
		this.SetData(ID, data, false);
	}

	// Token: 0x06002408 RID: 9224 RVA: 0x000D0824 File Offset: 0x000CEA24
	private void SetData(string ID, MinionStorageDataHolder.DataPackData data, bool markAsNewDataToRead)
	{
		if (this.storedDataPacks == null)
		{
			this.storedDataPacks = new List<MinionStorageDataHolder.DataPack>();
		}
		MinionStorageDataHolder.DataPack dataPack = this.storedDataPacks.Find((MinionStorageDataHolder.DataPack d) => d.ID == ID);
		if (dataPack == null)
		{
			dataPack = new MinionStorageDataHolder.DataPack(ID);
			this.storedDataPacks.Add(dataPack);
		}
		dataPack.SetData(data, markAsNewDataToRead);
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x000D088C File Offset: 0x000CEA8C
	public void PullFrom(StoredMinionIdentity source)
	{
		MinionStorageDataHolder component = source.GetComponent<MinionStorageDataHolder>();
		if (component != null && component.storedDataPacks != null)
		{
			for (int i = 0; i < component.storedDataPacks.Count; i++)
			{
				MinionStorageDataHolder.DataPack dataPack = component.storedDataPacks[i];
				if (dataPack != null)
				{
					this.SetData(dataPack.ID, dataPack.ReadData(), true);
				}
			}
		}
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x000D08EC File Offset: 0x000CEAEC
	public void PushTo(StoredMinionIdentity destination)
	{
		Action<StoredMinionIdentity> onCopyBegins = this.OnCopyBegins;
		if (onCopyBegins != null)
		{
			onCopyBegins(destination);
		}
		this.AddStoredMinionGameObjectRequirements(destination.gameObject);
		MinionStorageDataHolder component = destination.gameObject.GetComponent<MinionStorageDataHolder>();
		if (this.storedDataPacks != null)
		{
			for (int i = 0; i < this.storedDataPacks.Count; i++)
			{
				MinionStorageDataHolder.DataPack dataPack = this.storedDataPacks[i];
				if (dataPack != null)
				{
					component.SetData(dataPack.ID, dataPack.ReadData(), true);
				}
			}
		}
	}

	// Token: 0x0600240B RID: 9227 RVA: 0x000D0964 File Offset: 0x000CEB64
	public void AddStoredMinionGameObjectRequirements(GameObject storedMinionGameObject)
	{
		storedMinionGameObject.AddOrGet<MinionStorageDataHolder>();
	}

	// Token: 0x04001501 RID: 5377
	public Action<StoredMinionIdentity> OnCopyBegins;

	// Token: 0x04001502 RID: 5378
	[Serialize]
	private List<MinionStorageDataHolder.DataPack> storedDataPacks;

	// Token: 0x020014DC RID: 5340
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DataPackData
	{
		// Token: 0x04006FD2 RID: 28626
		[Serialize]
		public bool[] Bools;

		// Token: 0x04006FD3 RID: 28627
		[Serialize]
		public Tag[] Tags;
	}

	// Token: 0x020014DD RID: 5341
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DataPack
	{
		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06009153 RID: 37203 RVA: 0x00370E5A File Offset: 0x0036F05A
		public bool IsStoringNewData
		{
			get
			{
				return this.isStoringNewData;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06009154 RID: 37204 RVA: 0x00370E62 File Offset: 0x0036F062
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x06009155 RID: 37205 RVA: 0x00370E6A File Offset: 0x0036F06A
		public DataPack(string id)
		{
			this.id = id;
		}

		// Token: 0x06009156 RID: 37206 RVA: 0x00370E79 File Offset: 0x0036F079
		public void SetData(MinionStorageDataHolder.DataPackData data, bool markAsNewDataToRead)
		{
			this.data = data;
			if (markAsNewDataToRead)
			{
				this.isStoringNewData = markAsNewDataToRead;
			}
		}

		// Token: 0x06009157 RID: 37207 RVA: 0x00370E8C File Offset: 0x0036F08C
		public MinionStorageDataHolder.DataPackData ReadData()
		{
			this.isStoringNewData = false;
			return this.data;
		}

		// Token: 0x06009158 RID: 37208 RVA: 0x00370E9B File Offset: 0x0036F09B
		public MinionStorageDataHolder.DataPackData PeekData()
		{
			return this.data;
		}

		// Token: 0x04006FD4 RID: 28628
		[Serialize]
		private string id;

		// Token: 0x04006FD5 RID: 28629
		[Serialize]
		private bool isStoringNewData;

		// Token: 0x04006FD6 RID: 28630
		[Serialize]
		private MinionStorageDataHolder.DataPackData data;
	}
}
