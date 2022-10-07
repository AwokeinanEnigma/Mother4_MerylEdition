using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Mother4.Utility;
using SFML.System;

namespace Mother4.Data.Config
{
	internal class ConfigReader
	{
		public static ConfigReader Instance
		{
			get
			{
				if (ConfigReader.instance == null)
				{
					ConfigReader.instance = new ConfigReader();
				}
				return ConfigReader.instance;
			}
		}

		public string StartingMapName
		{
			get
			{
				return this.startingMap;
			}
		}

		public Vector2i StartingPosition
		{
			get
			{
				return this.startingPosition;
			}
		}

		public CharacterType[] StartingParty
		{
			get
			{
				return this.partyList.ToArray();
			}
		}

		public ConfigReader()
		{
			this.stateStack = new Stack<ConfigReader.ReadState>();
			this.stateStack.Push(ConfigReader.ReadState.Root);
			this.partyList = new List<CharacterType>();
			this.Load();
		}

		private void ReadStartElement(XmlTextReader reader)
		{
			string name;
			if ((name = reader.Name) != null)
			{
				if (!(name == "map"))
				{
					if (!(name == "position"))
					{
						return;
					}
					while (reader.MoveToNextAttribute())
					{
						string name2;
						if ((name2 = reader.Name) != null)
						{
							if (!(name2 == "x"))
							{
								if (name2 == "y")
								{
									int.TryParse(reader.Value, out this.startingPosition.Y);
								}
							}
							else
							{
								int.TryParse(reader.Value, out this.startingPosition.X);
							}
						}
					}
				}
				else if (reader.MoveToNextAttribute() && reader.Name == "value")
				{
					this.startingMap = reader.Value;
					return;
				}
			}
		}

		private void ReadPartyElement(XmlTextReader reader)
		{
			string name;
			if ((name = reader.Name) != null)
			{
				if (!(name == "character"))
				{
					return;
				}
				string value;
				if (reader.MoveToNextAttribute() && reader.Name == "id" && (value = reader.Value) != null)
				{
					if (value == "travis")
					{
						this.partyList.Add(CharacterType.Travis);
						return;
					}
					if (value == "floyd")
					{
						this.partyList.Add(CharacterType.Floyd);
						return;
					}
					if (value == "meryl")
					{
						this.partyList.Add(CharacterType.Meryl);
						return;
					}
					if (value == "leo")
					{
						this.partyList.Add(CharacterType.Leo);
						return;
					}
					if (value == "zack")
					{
						this.partyList.Add(CharacterType.Zack);
						return;
					}
					if (!(value == "renee"))
					{
						return;
					}
					this.partyList.Add(CharacterType.Renee);
				}
			}
		}

		private void HandleInnerElement(XmlTextReader reader)
		{
			switch (this.stateStack.Peek())
			{
			case ConfigReader.ReadState.Start:
				this.ReadStartElement(reader);
				return;
			case ConfigReader.ReadState.Party:
				this.ReadPartyElement(reader);
				return;
			default:
				return;
			}
		}

		private void HandleElement(XmlTextReader reader)
		{
			string name;
			if ((name = reader.Name) != null)
			{
				if (name == "start")
				{
					this.stateStack.Push(ConfigReader.ReadState.Start);
					return;
				}
				if (name == "party")
				{
					this.stateStack.Push(ConfigReader.ReadState.Party);
					return;
				}
			}
			this.HandleInnerElement(reader);
		}

		private void HandleEndElement(XmlTextReader reader)
		{
			this.stateStack.Pop();
		}

		private void Load()
		{
			using (Stream stream = EmbeddedResources.GetStream("Mother4.Resources.config.xml"))
			{
				XmlTextReader xmlTextReader = new XmlTextReader(stream);
				while (xmlTextReader.Read())
				{
					XmlNodeType nodeType = xmlTextReader.NodeType;
					if (nodeType == XmlNodeType.Element)
					{
						this.HandleElement(xmlTextReader);
					}
				}
			}
		}

		private const string TAG_NAME_START = "start";

		private const string TAG_NAME_START_MAP = "map";

		private const string TAG_NAME_START_POSITION = "position";

		private const string TAG_NAME_PARTY = "party";

		private const string TAG_NAME_PARTY_CHARACTER = "character";

		private const string ATTR_NAME_VALUE = "value";

		private const string ATTR_NAME_ID = "id";

		private const string ATTR_NAME_X = "x";

		private const string ATTR_NAME_Y = "y";

		private static ConfigReader instance;

		private Stack<ConfigReader.ReadState> stateStack;

		private string startingMap;

		private Vector2i startingPosition;

		private List<CharacterType> partyList;

		private enum ReadState
		{
			Root,
			BaseStats,
			Start,
			Party
		}
	}
}
