using System;
using System.Collections.Generic;
using System.IO;
using Carbine.Flags;
using fNbt;
using SFML.System;

namespace Mother4.Data
{
	internal class SaveFileManager
	{
		public static SaveFileManager Instance
		{
			get
			{
				if (SaveFileManager.instance != null)
				{
					return SaveFileManager.instance;
				}
				return SaveFileManager.instance = new SaveFileManager();
			}
		}

		public SaveProfile CurrentProfile
		{
			get
			{
				return this.currentProfile;
			}
			set
			{
				this.currentProfile = value;
			}
		}

		private SaveFileManager()
		{
			if (File.Exists("sav.dat"))
			{
				this.file = new NbtFile("sav.dat");
			}
			else
			{
				NbtCompound rootTag = new NbtCompound("saves");
				this.file = new NbtFile(rootTag);
			}
			if (!this.file.RootTag.Contains("v"))
			{
				this.file.RootTag.Add(new NbtInt("v", 1));
			}
		}

		private CharacterType[] IntArrayToParty(int[] intArray)
		{
			CharacterType[] array = new CharacterType[(intArray == null) ? 0 : intArray.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (CharacterType)intArray[i];
			}
			return array;
		}

		private int[] PartyToIntArray(CharacterType[] party)
		{
			int[] array = new int[(party == null) ? 0 : party.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (int)party[i];
			}
			return array;
		}

		private SaveProfile LoadSaveProfile(NbtCompound saveTag)
		{
			bool flag = true;
			NbtInt nbtInt = null;
			flag &= saveTag.TryGet<NbtInt>("idx", out nbtInt);
			NbtIntArray nbtIntArray = null;
			flag &= saveTag.TryGet<NbtIntArray>("prty", out nbtIntArray);
			NbtString nbtString = null;
			flag &= saveTag.TryGet<NbtString>("map", out nbtString);
			NbtInt nbtInt2 = null;
			flag &= saveTag.TryGet<NbtInt>("x", out nbtInt2);
			NbtInt nbtInt3 = null;
			flag &= saveTag.TryGet<NbtInt>("y", out nbtInt3);
			NbtInt nbtInt4 = null;
			flag &= saveTag.TryGet<NbtInt>("tm", out nbtInt4);
			NbtInt nbtInt5 = null;
			flag &= saveTag.TryGet<NbtInt>("flv", out nbtInt5);
			Vector2f position = default(Vector2f);
			if (nbtInt2 != null && nbtInt3 != null)
			{
				position.X = (float)nbtInt2.IntValue;
				position.Y = (float)nbtInt3.IntValue;
			}
			return new SaveProfile
			{
				IsValid = flag,
				Index = ((nbtInt == null) ? 0 : nbtInt.IntValue),
				Party = ((nbtIntArray == null) ? new CharacterType[0] : this.IntArrayToParty(nbtIntArray.IntArrayValue)),
				MapName = ((nbtString == null) ? string.Empty : nbtString.StringValue),
				Position = position,
				Time = ((nbtInt4 == null) ? 0 : nbtInt4.IntValue),
				Flavor = ((nbtInt5 == null) ? 0 : nbtInt5.IntValue)
			};
		}

		private NbtTag SaveProfileToNBT(SaveProfile profile)
		{
			return new NbtCompound("prf")
			{
				new NbtInt("idx", profile.Index),
				new NbtIntArray("prty", this.PartyToIntArray(profile.Party)),
				new NbtString("map", profile.MapName ?? string.Empty),
				new NbtInt("x", (int)profile.Position.X),
				new NbtInt("y", (int)profile.Position.Y),
				new NbtInt("tm", profile.Time),
				new NbtInt("flv", profile.Flavor)
			};
		}

		public IDictionary<int, SaveProfile> LoadProfiles()
		{
			IDictionary<int, SaveProfile> dictionary = new Dictionary<int, SaveProfile>();
			NbtCompound rootTag = this.file.RootTag;
			NbtList nbtList = rootTag.Get<NbtList>("sav");
			if (nbtList != null)
			{
				foreach (NbtTag nbtTag in nbtList)
				{
					if (nbtTag is NbtCompound)
					{
						NbtCompound nbtCompound = (NbtCompound)nbtTag;
						NbtCompound nbtCompound2 = nbtCompound.Get<NbtCompound>("prf");
						if (nbtCompound2 != null)
						{
							SaveProfile value = this.LoadSaveProfile(nbtCompound2);
							if (!dictionary.ContainsKey(value.Index))
							{
								dictionary.Add(value.Index, value);
							}
						}
					}
				}
			}
			return dictionary;
		}

		public void LoadFile(int saveIndex)
		{
			NbtCompound rootTag = this.file.RootTag;
			NbtList nbtList = rootTag.Get<NbtList>("sav");
			if (nbtList != null)
			{
				foreach (NbtTag nbtTag in nbtList)
				{
					NbtCompound nbtCompound = (NbtCompound)nbtTag;
					NbtCompound saveTag = null;
					bool flag = nbtCompound.TryGet<NbtCompound>("prf", out saveTag);
					if (flag)
					{
						SaveProfile saveProfile = this.LoadSaveProfile(saveTag);
						if (saveProfile.Index == saveIndex)
						{
							this.currentProfile = saveProfile;
							PartyManager.Instance.Clear();
							PartyManager.Instance.AddAll(this.currentProfile.Party);
							NbtIntArray flagTag = null;
							flag &= nbtCompound.TryGet<NbtIntArray>("flags", out flagTag);
							FlagManager.Instance.LoadFromNBT(flagTag);
							NbtIntArray valueTag = null;
							flag &= nbtCompound.TryGet<NbtIntArray>("vals", out valueTag);
							ValueManager.Instance.LoadFromNBT(valueTag);
							NbtList nameListTag = null;
							flag &= nbtCompound.TryGet<NbtList>("names", out nameListTag);
							CharacterNames.LoadFromNBT(nameListTag);
							break;
						}
					}
				}
			}
		}

		public void SaveFile()
		{//
		//	StreamWriter streamWriter = new StreamWriter("save.log");
		//	streamWriter.WriteLine("start");
			DateTime now = DateTime.Now;
			NbtCompound rootTag = this.file.RootTag;
			NbtList nbtList = rootTag.Get<NbtList>("sav");
			NbtCompound nbtCompound = null;
			//streamWriter.WriteLine("compound");
			if (nbtList != null)
			{
				using (IEnumerator<NbtTag> enumerator = nbtList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NbtTag nbtTag = enumerator.Current;
						NbtCompound nbtCompound2 = (NbtCompound)nbtTag;
						NbtCompound nbtCompound3 = nbtCompound2.Get<NbtCompound>("prf");
						if (nbtCompound3 != null)
						{
							NbtInt nbtInt = nbtCompound3.Get<NbtInt>("idx");
							if (nbtInt != null && nbtInt.IntValue == this.currentProfile.Index)
							{
								nbtCompound = nbtCompound2;
								nbtCompound.Clear();
								break;
							}
						}
					}
					goto IL_A9;
				}
			}
			nbtList = new NbtList("sav", NbtTagType.Compound);
			rootTag.Add(nbtList);
			IL_A9:
			bool flag = false;
			if (nbtCompound == null)
			{
				nbtCompound = new NbtCompound();
				flag = true;
			}
		//	streamWriter.WriteLine("profile");

			nbtCompound.Add(this.SaveProfileToNBT(this.currentProfile));
			nbtCompound.Add(FlagManager.Instance.ToNBT());
			nbtCompound.Add(ValueManager.Instance.ToNBT());
			nbtCompound.Add(CharacterNames.ToNBT());
			//streamWriter.WriteLine("flag");
			if (flag)
			{
				nbtList.Add(nbtCompound);
			}
			//streamWriter.WriteLine("save");
			this.file.SaveToFile("sav.dat", NbtCompression.GZip);
			//streamWriter.Close();
			//Console.WriteLine("Saved profile in {0} seconds", Math.Round((double)(DateTime.Now.Millisecond - now.Millisecond) / 1000.0, 2));
		}

		public const string SAVE_FILE = "sav.dat";

		public const string PROFILE_TAG_NAME = "prf";

		public const string LIST_TAG_NAME = "sav";

		private const int SAVE_FORMAT_VERSION = 1;

		private const int LOWEST_COMPAT_SAVE_FORMAT_VERSION = 1;

		private static SaveFileManager instance;

		private SaveProfile currentProfile;

		private NbtFile file;
	}
}
