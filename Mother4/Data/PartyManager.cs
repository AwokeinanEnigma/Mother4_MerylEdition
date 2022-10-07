using System;
using System.Collections.Generic;

namespace Mother4.Data
{
	internal class PartyManager
	{
		public static PartyManager Instance
		{
			get
			{
				if (PartyManager.instance == null)
				{
					PartyManager.instance = new PartyManager();
				}
				return PartyManager.instance;
			}
		}

		public int Count
		{
			get
			{
				return this.party.Count;
			}
		}

		public CharacterType this[int i]
		{
			get
			{
				return this.party[i];
			}
			set
			{
				this.party[i] = value;
			}
		}

		public event PartyManager.OnPartyChangeHandler OnPartyChange;

		private PartyManager()
		{
			this.party = new List<CharacterType>();
		}

		public void Clear()
		{
			this.party.Clear();
		}

		public void Add(CharacterType character)
		{
			this.party.Add(character);
			if (this.OnPartyChange != null)
			{
				PartyManager.PartyChangeArgs args = new PartyManager.PartyChangeArgs
				{
					Added = new CharacterType?(character)
				};
				this.OnPartyChange(args);
			}
		}

		public void AddAll(ICollection<CharacterType> characters)
		{
			foreach (CharacterType character in characters)
			{
				this.Add(character);
			}
		}

		public void Insert(int index, CharacterType character)
		{
			this.party.Insert(index, character);
			if (this.OnPartyChange != null)
			{
				PartyManager.PartyChangeArgs args = new PartyManager.PartyChangeArgs
				{
					Added = new CharacterType?(character)
				};
				this.OnPartyChange(args);
			}
		}

		public void Remove(CharacterType character)
		{
			this.party.Remove(character);
			if (this.OnPartyChange != null)
			{
				PartyManager.PartyChangeArgs args = new PartyManager.PartyChangeArgs
				{
					Removed = new CharacterType?(character)
				};
				this.OnPartyChange(args);
			}
		}

		public int IndexOf(CharacterType character)
		{
			return this.party.IndexOf(character);
		}

		public CharacterType[] ToArray()
		{
			return this.party.ToArray();
		}

		private static PartyManager instance;

		private List<CharacterType> party;

		public struct PartyChangeArgs
		{
			public CharacterType? Added;

			public CharacterType? Removed;
		}

		public delegate void OnPartyChangeHandler(PartyManager.PartyChangeArgs args);
	}
}
