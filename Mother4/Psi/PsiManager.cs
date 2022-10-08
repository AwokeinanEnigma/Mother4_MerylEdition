using System;
using System.Collections.Generic;
using System.Linq;
using fNbt;
using Mother4.Data;
using Rufini.Strings;

namespace Mother4.Psi
{
	public sealed class PsiManager
	{
		public static PsiManager Instance
		{
			get
			{
				if (PsiManager.instance == null)
				{
					PsiManager.instance = new PsiManager();
				}
				return PsiManager.instance;
			}
		}

		private PsiManager()
		{
			NbtFile nbtFile = new NbtFile(PsiManager.PSI_FILE);
			NbtCompound rootTag = nbtFile.RootTag;
			this.offensive = this.InitializePsiList<OffensePsi>();
			this.defensive = this.InitializePsiList<DefensivePsi>();
			this.assistive = this.InitializePsiList<AssistivePsi>();
			this.other = this.InitializePsiList<OtherPsi>();
			this.LoadOffensePsi(rootTag.Get<NbtCompound>("offense"));
				//Console.WriteLine("it loaded??");
			this.LoadDefensePsi(rootTag.Get<NbtCompound>("defense"));
			//Console.WriteLine("it loaded??");
			this.LoadAssistPsi(rootTag.Get<NbtCompound>("assist"));
		//	Console.WriteLine("it loaded??");
			this.LoadOtherPsi(rootTag.Get<NbtCompound>("other"));
	//		Console.WriteLine("it loaded??");
		}

		private Dictionary<CharacterType, List<T>> InitializePsiList<T>() where T : IPsi
		{
			return new Dictionary<CharacterType, List<T>>
			{
				{
					CharacterType.Floyd,
					new List<T>()
				},
				{
					CharacterType.Leo,
					new List<T>()
				},
				{
					CharacterType.Meryl,
					new List<T>()
				},
				{
					CharacterType.Travis,
					new List<T>()
				},
				{
					CharacterType.Zack,
					new List<T>()
				},
				{
					CharacterType.Renee,
					new List<T>()
				}
			};
		}

		internal bool CharacterCanUsePsiType(CharacterType playerCharacter, PsiType psiType)
		{
			switch (psiType)
			{
			case PsiType.Offense:
				return this.offensive.ContainsKey(playerCharacter);
			case PsiType.Defense:
				return this.defensive.ContainsKey(playerCharacter);
			case PsiType.Assist:
				return this.assistive.ContainsKey(playerCharacter);
			case PsiType.Other:
				return this.other.ContainsKey(playerCharacter);
			default:
				Console.WriteLine("Psi Type {0} is not supported", psiType);
				throw new NotSupportedException();
			}
		}

		private void LoadOffensePsi(NbtCompound offenseTag)
		{
			if (offenseTag != null)
			{

				Console.WriteLine("huh1");
				OffensePsi psi = new OffensePsi(new SOMETHING.Debug());
				OffensePsi psi1 = new OffensePsi(new SOMETHING.Beam());
				OffensePsi psi2 = new OffensePsi(new SOMETHING.Fire());
				OffensePsi psi3 = new OffensePsi(new SOMETHING.Freeze());
				Console.WriteLine($"Offensive8 PSI: {psi.aux.QualifiedName} " + Environment.NewLine);
				this.AddPsiToCharacters<OffensePsi>(this.offensive, psi, new List<string>() { "meryl", "travis", "leo" });
				this.AddPsiToCharacters<OffensePsi>(this.offensive, psi1, new List<string>() { "meryl", "travis", "leo" });
				this.AddPsiToCharacters<OffensePsi>(this.offensive, psi2, new List<string>() { "meryl", "travis", "leo" });
				this.AddPsiToCharacters<OffensePsi>(this.offensive, psi3, new List<string>() { "meryl", "travis", "leo" });
				/*
				using (IEnumerator<NbtTag> enumerator = offenseTag.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						/*
						NbtTag nbtTag = enumerator.Current;
						NbtCompound nbtCompound = (NbtCompound)nbtTag;
						string stringValue = nbtCompound.Get<NbtString>("key").StringValue;
						NbtIntArray[] source = nbtCompound.Get<NbtList>("effect").ToArray<NbtIntArray>();
						int[][] effect = (from s in source
						select s.Value).ToArray<int[]>();

					}
					return;
				}*/
			}
		//	Console.WriteLine("Could not load OffensePsi. Is the offense tag present?");
		}

		private void LoadDefensePsi(NbtCompound defenseTag)
		{
			if (defenseTag != null)
			{
				Console.WriteLine("huh1");
				OffensePsi psi = new OffensePsi(new SOMETHING.Debug());
				Console.WriteLine($"Offensive8 PSI: {psi.aux.QualifiedName} " + Environment.NewLine);
			}
		//	Console.WriteLine("Could not load DefensePsi. Is the defense tag present?");
		}

		private void LoadAssistPsi(NbtCompound assistTag)
		{
			if (assistTag != null)
			{
				AssistivePsi psi3 = new AssistivePsi(new SOMETHING.LifeUp());
				Console.WriteLine($"Assistive PSI: {psi3.aux.QualifiedName} " + Environment.NewLine);
				this.AddPsiToCharacters<AssistivePsi>(this.assistive, psi3, new List<string>() { "travis" });

				AssistivePsi psi2 = new AssistivePsi(new SOMETHING.Telepathy());
				Console.WriteLine($"Assistive PSI: {psi2.aux.QualifiedName} " + Environment.NewLine);
				this.AddPsiToCharacters<AssistivePsi>(this.assistive, psi2, new List<string>() { "travis", "meryl", "leo" });
			}
		//	Console.WriteLine("Could not load AssistPsi. Is the assist tag present?");
		}

		private void LoadOtherPsi(NbtCompound otherTag)
		{
			if (otherTag != null)
			{
				Console.WriteLine("huh1");
				OffensePsi psi = new OffensePsi(new SOMETHING.Debug());
				Console.WriteLine($"Offensive8 PSI: {psi.aux.QualifiedName} " + Environment.NewLine);
			}
		//	Console.WriteLine("Could not load OtherPsi. Is the other tag present?");
		}

		private IEnumerable<string> LoadUsersAttribute(NbtCompound ability)
		{
			return from s in ability.Get<NbtList>("users").ToArray<NbtString>()
			select s.Value;
		}

		private int[] LoadPPAttribute(NbtCompound ability)
		{
			return ability.Get<NbtIntArray>("pp").Value;
		}

		private int[] LoadLevelsAttribute(NbtCompound ability)
		{
			return ability.Get<NbtIntArray>("levels").Value;
		}

		private float LoadSpecialAttribute(NbtCompound ability)
		{
			return ability.Get<NbtFloat>("special").Value;
		}

		private string LoadTargetAttribute(NbtCompound ability)
		{
			return ability.Get<NbtString>("target").Value;
		}

		private int LoadAnimationAttribute(NbtCompound ability)
		{
			return ability.Get<NbtInt>("anim").Value;
		}

		private void AddPsiToCharacters<T>(Dictionary<CharacterType, List<T>> dictionary, T psi, IEnumerable<string> characters) where T : IPsi
		{
			foreach (string text in characters)
			{
				string a;
				if ((a = text) != null)
				{
					if (a == "floyd")
					{
						dictionary[CharacterType.Floyd].Add(psi);
						continue;
					}
					if (a == "leo")
					{
						dictionary[CharacterType.Leo].Add(psi);
						continue;
					}
					if (a == "meryl")
					{
						dictionary[CharacterType.Meryl].Add(psi);
						continue;
					}
					if (a == "travis")
					{
						dictionary[CharacterType.Travis].Add(psi);
						continue;
					}
					if (a == "zack")
					{
						dictionary[CharacterType.Zack].Add(psi);
						continue;
					}
				}
				Console.WriteLine("Tried to add psi {0} to invalid character {1}", psi.aux.QualifiedName, text);
			}
		}

		internal IEnumerable<OffensePsi> GetCharacterOffensePsi(CharacterType playerCharacter)
		{
			return this.offensive[playerCharacter];
		}

		internal IEnumerable<DefensivePsi> GetCharacterDefensePsi(CharacterType playerCharacter)
		{
			return this.defensive[playerCharacter];
		}

		internal IEnumerable<AssistivePsi> GetCharacterAssistPsi(CharacterType playerCharacter)
		{
			return this.assistive[playerCharacter];
		}

		internal IEnumerable<OtherPsi> GetCharacterOtherPsi(CharacterType playerCharacter)
		{
			return this.other[playerCharacter];
		}

		internal bool CharacterHasPsi(CharacterType playerCharacter)
		{
            switch (playerCharacter)
            {
                case CharacterType.Travis:
                    return true;
                case CharacterType.Dog:
                    return false;
                case CharacterType.Leo:
                    return true;
                case CharacterType.Floyd:
                    return false;
                case CharacterType.Renee:
                    return true;
                case CharacterType.Zack:
                    return false;
                case CharacterType.Meryl:
                    return true;
					default:
                        return false;
            }

            /*bool flag = this.offensive[playerCharacter].Count > 0;
			bool flag2 = this.defensive[playerCharacter].Count > 0;
			bool flag3 = this.assistive[playerCharacter].Count > 0;
			bool flag4 = this.other[playerCharacter].Count > 0;*/
			//return flag || flag2 || flag3 || flag4;
		}

		private static PsiManager instance;

		private static readonly string PSI_FILE = Paths.PSI + "psi.dat";

		private readonly Dictionary<CharacterType, List<OffensePsi>> offensive;

		private readonly Dictionary<CharacterType, List<DefensivePsi>> defensive;

		private readonly Dictionary<CharacterType, List<AssistivePsi>> assistive;

		private readonly Dictionary<CharacterType, List<OtherPsi>> other;
	}
}
