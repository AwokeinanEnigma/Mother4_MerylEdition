// Decompiled with JetBrains decompiler
// Type: Mother4.Scripts.Text.TextProcessor
// Assembly: Mother4, Version=0.6.6072.35821, Culture=neutral, PublicKeyToken=null
// MVID: 75372462-4B0E-4582-8CEA-2817B7024D27
// Assembly location: D:\OddityPrototypes\Mother4 - Copy (2).exe

using Carbine.GUI;
using Mother4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mother4.Scripts.Text
{
    internal class TextProcessor
    {
        private const char NEWLINE_CHAR = '\n';
        private const char BULLET_CHAR = '@';
        private const char SPACE_CHAR = ' ';
        private const int PAUSE_CHAR_DURATION = 10;
        private const int BULLET_PAUSE_DURATION = 30;
        private const string ENCLOSING_CMD_REGEX = "\\[([a-zA-Z][a-zA-Z0-9]*):?(\\b[^\\]]*)\\](.*?)\\[\\/\\1\\]";
        private const string SINGLE_CMD_REGEX = "\\[([a-zA-Z][a-zA-Z0-9]*):?(\\b[^\\]]*)\\]";
        private const string CMD_PAUSE = "p";
        private const string CMD_CHARNAME = "cn";
        private const string CMD_TRIGGER = "t";
        private const string CMD_TRAVIS = "travis";
        private const string CMD_FLOYD = "floyd";
        private const string CMD_MERYL = "meryl";
        private const string CMD_LEO = "leo";
        private const string CMD_ZACK = "zack";
        private const string CMD_RENEE = "renee";
        private static char[] PAUSE_CHARS = new char[2]
        {
      ',',
      '?'
        };

        public static TextBlock Process(FontData font, string text, int frameWidth)
        {
            List<string> stringList = new List<string>();
            Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
            List<ITextCommand> commands = new List<ITextCommand>();
            StringBuilder stringBuilder = new StringBuilder(text ?? "").Replace("\r", "");
            MatchCollection matchCollection = Regex.Matches(stringBuilder.ToString(), "\\[([a-zA-Z][a-zA-Z0-9]*):?(\\b[^\\]]*)\\](.*?)\\[\\/\\1\\]");
            int num1 = 0;
            foreach (Match match in matchCollection)
            {
                string str1 = match.Groups[1].Value;
                string str2 = match.Groups[2].Value;
                string str3 = match.Groups[3].Value;
                stringBuilder = stringBuilder.Remove(match.Index, match.Length);
                stringBuilder = stringBuilder.Insert(match.Index, str3);
            }
            foreach (Match match in Regex.Matches(stringBuilder.ToString(), "\\[([a-zA-Z][a-zA-Z0-9]*):?(\\b[^\\]]*)\\]"))
            {
                string str = match.Groups[1].Value;
                string s = match.Groups[2].Value;
                string[] sourceArray = s.Split(',');
                for (int index = 0; index < sourceArray.Length; ++index)
                    sourceArray[index] = sourceArray[index].Trim();
                int num2 = match.Index - num1;
                TextProcessor.OffsetCommandPositions(commands, num2, match.Length);
                stringBuilder = stringBuilder.Remove(num2, match.Length);
                num1 += match.Length;
                switch (str)
                {
                    case "p":
                        int result1;
                        int.TryParse(s, out result1);
                        commands.Add((ITextCommand)new TextPause(num2, result1));
                        continue;
                    case "cn":
                        CharacterType result2;
                        Enum.TryParse<CharacterType>(s, out result2);
                        string name1 = CharacterNames.GetName(result2);
                        stringBuilder = stringBuilder.Insert(num2, name1);
                        num1 -= name1.Length;
                        continue;
                    case "t":
                        int result3;
                        int.TryParse(sourceArray[0], out result3);
                        string[] strArray = new string[sourceArray.Length - 1];
                        Array.Copy((Array)sourceArray, 1, (Array)strArray, 0, strArray.Length);
                        commands.Add((ITextCommand)new TextTrigger(num2, result3, strArray));
                        continue;
                    case "travis":
                        string name2 = CharacterNames.GetName(CharacterType.Travis);
                        stringBuilder = stringBuilder.Insert(num2, name2);
                        num1 -= name2.Length;
                        continue;
                    case "floyd":
                        string name3 = CharacterNames.GetName(CharacterType.Floyd);
                        stringBuilder = stringBuilder.Insert(num2, name3);
                        num1 -= name3.Length;
                        continue;
                    case "meryl":
                        string name4 = CharacterNames.GetName(CharacterType.Meryl);
                        stringBuilder = stringBuilder.Insert(num2, name4);
                        num1 -= name4.Length;
                        continue;
                    case "leo":
                        string name5 = CharacterNames.GetName(CharacterType.Leo);
                        stringBuilder = stringBuilder.Insert(num2, name5);
                        num1 -= name5.Length;
                        continue;
                    case "zack":
                        string name6 = CharacterNames.GetName(CharacterType.Zack);
                        stringBuilder = stringBuilder.Insert(num2, name6);
                        num1 -= name6.Length;
                        continue;
                    case "renee":
                        string name7 = CharacterNames.GetName(CharacterType.Renee);
                        stringBuilder = stringBuilder.Insert(num2, name7);
                        num1 -= name7.Length;
                        continue;
                    default:
                        continue;
                }
            }
            SFML.Graphics.Text text1 = new SFML.Graphics.Text(stringBuilder.ToString().Replace('@'.ToString(), string.Empty).Replace('\n'.ToString(), string.Empty), font.Font, font.Size);
            float num3 = 0.0f;
            float num4 = text1.FindCharacterPos(0U).X;
            int num5 = 0;
            int startIndex = 0;
            int num6 = 0;
            bool flag1 = false;
            bool flag2 = false;
            for (int index = 0; index < stringBuilder.Length; ++index)
            {
                char ch = stringBuilder[index];
                if (((IEnumerable<char>)TextProcessor.PAUSE_CHARS).Contains<char>(ch))
                {
                    commands.Add((ITextCommand)new TextPause(index + 1, 10));
                }
                else
                {
                    switch (ch)
                    {
                        case '\n':
                            num6 = index;
                            stringBuilder = stringBuilder.Remove(index, 1);
                            TextProcessor.OffsetCommandPositions(commands, index, -1);
                            --index;
                            flag1 = true;
                            break;
                        case ' ':
                            num5 = index;
                            break;
                        case '@':
                            num6 = index;
                            stringBuilder = stringBuilder.Remove(index, 1);
                            TextProcessor.OffsetCommandPositions(commands, index, -1);
                            --index;
                            flag1 = index > startIndex;
                            if (flag2)
                                commands.Add((ITextCommand)new TextWait(index));
                            dictionary.Add(stringList.Count + (flag1 ? 1 : 0), true);
                            flag2 = true;
                            continue;
                    }
                }
                float x = text1.FindCharacterPos((uint)index).X;
                float num7 = x - num4;
                num3 += num7;
                num4 = x;
                if ((double)num3 > (double)frameWidth)
                {
                    num6 = num5;
                    flag1 = true;
                }
                if (flag1)
                {
                    string str = stringBuilder.ToString(startIndex, num6 - startIndex);
                    stringList.Add(str);
                    num3 = 0.0f;
                    int num8 = 0;
                    int length = stringBuilder.Length;
                    while (num6 + num8 < length && stringBuilder[num6 + num8] == ' ')
                        ++num8;
                    startIndex = num6 + num8;
                    flag1 = false;
                }
            }
            string str4 = stringBuilder.ToString(startIndex, stringBuilder.Length - startIndex);
            if (str4.Length > 0)
                stringList.Add(str4);
            List<TextLine> lines = new List<TextLine>();
            int startPosition = 0;
            for (int index = 0; index < stringList.Count; ++index)
            {
                bool bullet = false;
                if (dictionary.ContainsKey(index))
                    bullet = dictionary[index];
                string text2 = stringList[index];
                lines.Add(new TextLine(bullet, TextProcessor.GetCommandRange(commands, startPosition, startPosition + text2.Length), text2));
                startPosition += text2.Length;
            }
            return new TextBlock(lines);
        }

        private static ITextCommand[] GetCommandRange(
          List<ITextCommand> commands,
          int startPosition,
          int endPosition)
        {
            List<ITextCommand> textCommandList = new List<ITextCommand>();
            foreach (ITextCommand command in commands)
            {
                if (command.Position >= startPosition && command.Position < endPosition)
                    textCommandList.Add(command);
            }
            return textCommandList.ToArray();
        }

        private static void OffsetCommandPositions(
          List<ITextCommand> commands,
          int afterPosition,
          int offset)
        {
            foreach (ITextCommand command in commands)
            {
                if (command.Position > afterPosition)
                    command.Position += offset;
            }
        }
    }
}
