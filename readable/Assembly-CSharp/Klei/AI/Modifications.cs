using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200104D RID: 4173
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Modifications<ModifierType, InstanceType> : ISaveLoadableDetails where ModifierType : Resource where InstanceType : ModifierInstance<ModifierType>
	{
		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06008135 RID: 33077 RVA: 0x0033E4D7 File Offset: 0x0033C6D7
		public int Count
		{
			get
			{
				return this.ModifierList.Count;
			}
		}

		// Token: 0x06008136 RID: 33078 RVA: 0x0033E4E4 File Offset: 0x0033C6E4
		public IEnumerator<InstanceType> GetEnumerator()
		{
			return this.ModifierList.GetEnumerator();
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06008137 RID: 33079 RVA: 0x0033E4F6 File Offset: 0x0033C6F6
		// (set) Token: 0x06008138 RID: 33080 RVA: 0x0033E4FE File Offset: 0x0033C6FE
		public GameObject gameObject { get; private set; }

		// Token: 0x17000921 RID: 2337
		public InstanceType this[int idx]
		{
			get
			{
				return this.ModifierList[idx];
			}
		}

		// Token: 0x0600813A RID: 33082 RVA: 0x0033E515 File Offset: 0x0033C715
		public ComponentType GetComponent<ComponentType>()
		{
			return this.gameObject.GetComponent<ComponentType>();
		}

		// Token: 0x0600813B RID: 33083 RVA: 0x0033E522 File Offset: 0x0033C722
		public void Trigger(GameHashes hash, object data = null)
		{
			this.gameObject.GetComponent<KPrefabID>().Trigger((int)hash, data);
		}

		// Token: 0x0600813C RID: 33084 RVA: 0x0033E538 File Offset: 0x0033C738
		public virtual InstanceType CreateInstance(ModifierType modifier)
		{
			return default(InstanceType);
		}

		// Token: 0x0600813D RID: 33085 RVA: 0x0033E54E File Offset: 0x0033C74E
		public Modifications(GameObject go, ResourceSet<ModifierType> resources = null)
		{
			this.resources = resources;
			this.gameObject = go;
		}

		// Token: 0x0600813E RID: 33086 RVA: 0x0033E56F File Offset: 0x0033C76F
		public virtual InstanceType Add(InstanceType instance)
		{
			this.ModifierList.Add(instance);
			return instance;
		}

		// Token: 0x0600813F RID: 33087 RVA: 0x0033E580 File Offset: 0x0033C780
		public virtual void Remove(InstanceType instance)
		{
			for (int i = 0; i < this.ModifierList.Count; i++)
			{
				if (this.ModifierList[i] == instance)
				{
					this.ModifierList.RemoveAt(i);
					instance.OnCleanUp();
					return;
				}
			}
		}

		// Token: 0x06008140 RID: 33088 RVA: 0x0033E5D4 File Offset: 0x0033C7D4
		public bool Has(ModifierType modifier)
		{
			return this.Get(modifier) != null;
		}

		// Token: 0x06008141 RID: 33089 RVA: 0x0033E5E8 File Offset: 0x0033C7E8
		public InstanceType Get(ModifierType modifier)
		{
			foreach (InstanceType instanceType in this.ModifierList)
			{
				if (instanceType.modifier == modifier)
				{
					return instanceType;
				}
			}
			return default(InstanceType);
		}

		// Token: 0x06008142 RID: 33090 RVA: 0x0033E65C File Offset: 0x0033C85C
		public InstanceType Get(string id)
		{
			foreach (InstanceType instanceType in this.ModifierList)
			{
				if (instanceType.modifier.Id == id)
				{
					return instanceType;
				}
			}
			return default(InstanceType);
		}

		// Token: 0x06008143 RID: 33091 RVA: 0x0033E6D4 File Offset: 0x0033C8D4
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(this.ModifierList.Count);
			foreach (InstanceType instanceType in this.ModifierList)
			{
				writer.WriteKleiString(instanceType.modifier.Id);
				long position = writer.BaseStream.Position;
				writer.Write(0);
				long position2 = writer.BaseStream.Position;
				Serializer.SerializeTypeless(instanceType, writer);
				long position3 = writer.BaseStream.Position;
				long num = position3 - position2;
				writer.BaseStream.Position = position;
				writer.Write((int)num);
				writer.BaseStream.Position = position3;
			}
		}

		// Token: 0x06008144 RID: 33092 RVA: 0x0033E7B4 File Offset: 0x0033C9B4
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string text = reader.ReadKleiString();
				int num2 = reader.ReadInt32();
				int position = reader.Position;
				InstanceType instanceType = this.Get(text);
				if (instanceType == null && this.resources != null)
				{
					ModifierType modifierType = this.resources.TryGet(text);
					if (modifierType != null)
					{
						instanceType = this.CreateInstance(modifierType);
					}
				}
				if (instanceType == null)
				{
					if (text != "Condition")
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							this.gameObject.name,
							"Missing modifier: " + text
						});
					}
					reader.SkipBytes(num2);
				}
				else if (!(instanceType is ISaveLoadable))
				{
					reader.SkipBytes(num2);
				}
				else
				{
					Deserializer.DeserializeTypeless(instanceType, reader);
					if (reader.Position != position + num2)
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							"Expected to be at offset",
							position + num2,
							"but was only at offset",
							reader.Position,
							". Skipping to catch up."
						});
						reader.SkipBytes(position + num2 - reader.Position);
					}
				}
			}
		}

		// Token: 0x040061EC RID: 25068
		public List<InstanceType> ModifierList = new List<InstanceType>();

		// Token: 0x040061EE RID: 25070
		private ResourceSet<ModifierType> resources;
	}
}
