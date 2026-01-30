using System;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02000B09 RID: 2825
[SerializationConfig(MemberSerialization.OptIn)]
public class ResourceRef<ResourceType> : ISaveLoadable where ResourceType : Resource
{
	// Token: 0x06005244 RID: 21060 RVA: 0x001DD878 File Offset: 0x001DBA78
	public ResourceRef(ResourceType resource)
	{
		this.Set(resource);
	}

	// Token: 0x06005245 RID: 21061 RVA: 0x001DD887 File Offset: 0x001DBA87
	public ResourceRef()
	{
	}

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x06005246 RID: 21062 RVA: 0x001DD88F File Offset: 0x001DBA8F
	public ResourceGuid Guid
	{
		get
		{
			return this.guid;
		}
	}

	// Token: 0x06005247 RID: 21063 RVA: 0x001DD897 File Offset: 0x001DBA97
	public ResourceType Get()
	{
		return this.resource;
	}

	// Token: 0x06005248 RID: 21064 RVA: 0x001DD89F File Offset: 0x001DBA9F
	public void Set(ResourceType resource)
	{
		this.guid = null;
		this.resource = resource;
	}

	// Token: 0x06005249 RID: 21065 RVA: 0x001DD8AF File Offset: 0x001DBAAF
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.resource == null)
		{
			this.guid = null;
			return;
		}
		this.guid = this.resource.Guid;
	}

	// Token: 0x0600524A RID: 21066 RVA: 0x001DD8DC File Offset: 0x001DBADC
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.guid != null)
		{
			this.resource = Db.Get().GetResource<ResourceType>(this.guid);
			if (this.resource != null)
			{
				this.guid = null;
			}
		}
	}

	// Token: 0x040037A0 RID: 14240
	[Serialize]
	private ResourceGuid guid;

	// Token: 0x040037A1 RID: 14241
	private ResourceType resource;
}
