using System;
using System.IO;
using fNbt;
using Mother4.Data;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Strings
{
	internal class StringFile
	{
		public static StringFile Instance
		{
			get
			{
				if (StringFile.instance == null)
				{
					StringFile.instance = new StringFile();
				}
				return StringFile.instance;
			}
		}

		private StringFile()
		{
			this.filename = Path.Combine(Paths.TEXT, Settings.Locale, Settings.Locale + ".dat");
			Console.WriteLine(filename);
			this.Reload();
		}

		public void Reload()
		{
			if (File.Exists(this.filename))
			{
				this.file = new NbtFile(this.filename);
				return;
			}
			Console.WriteLine("No strings file was found.");
			NbtCompound rootTag = new NbtCompound("strings");
			this.file = new NbtFile(rootTag);
		}

		public RufiniString Get(string[] nameParts)
		{
			if (nameParts == null)
			{
				throw new ArgumentNullException("Cannot get a string with a null qualified name.");
			}
			string value = null;
			NbtCompound rootTag = this.file.RootTag;
			if (rootTag == null)
			{
				throw new InvalidOperationException("String file root tag is null.");
			}
			for (int i = 0; i < nameParts.Length - 1; i++)
			{
				rootTag.TryGet<NbtCompound>(nameParts[i], out rootTag);
				if (rootTag == null)
				{
					break;
				}
			}
			if (rootTag != null)
			{
				NbtTag nbtTag = null;
				rootTag.TryGet(nameParts[nameParts.Length - 1], out nbtTag);
				if (nbtTag is NbtString)
				{
					value = nbtTag.StringValue;
				}
			}
			return new RufiniString(nameParts, value);
		}

		public RufiniString Get(string qualifiedName)
		{
			if (qualifiedName == null)
			{
				throw new ArgumentNullException("Cannot get a string with a null qualified name.");
			}
			return this.Get(qualifiedName.Split(new char[]
			{
				'.'
			}));
		}

		private static StringFile instance;

		private NbtFile file;

		private string filename;
	}
}
