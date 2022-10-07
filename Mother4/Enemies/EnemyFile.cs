using System;
using System.Collections.Generic;
using System.IO;
using Carbine.Maps;
using Carbine.Utility;
using fNbt;

namespace Mother4.Data.Enemies
{
	internal class EnemyFile
	{

		public static EnemyFile Instance
		{
			get
			{
				EnemyFile.Load();
				return EnemyFile.INSTANCE;
			}
		}

		public static void Load()
		{
			if (EnemyFile.INSTANCE == null)
			{
				EnemyFile.INSTANCE = new EnemyFile();
			}
		}

		private EnemyFile()
		{
			this.enemyDataDict = new Dictionary<int, EnemyData>();
            foreach (string fileInfo in Directory.GetFiles(Paths.DATAENEMIES))
            {
                if (fileInfo.Contains(".edat"))
				{
					
                    Console.WriteLine($"Loading .edat file {fileInfo}");
                    this.Load(fileInfo);
				    
				}
				else
                {
                    throw new Exception($"File {fileInfo} is not of the format .edat, remove it from the enemies folder!");
				}
            }

            /*string text = Paths + "enemy.dat";
			if (File.Exists(text))
			{
				return;
			}*/
			//
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005BEC File Offset: 0x00003DEC
		private void Load(string path)
		{
			NbtFile nbtFile = new NbtFile(path);
            EnemyData enemyData = new EnemyData((NbtCompound)nbtFile.RootTag);
            int key = Hash.Get(enemyData.QualifiedName);
			Console.WriteLine($"Path '{path}', qualified name is {enemyData.QualifiedName}");
            this.enemyDataDict.Add(key, enemyData); 
            
            /*foreach (NbtTag nbtTag in nbtFile.RootTag)
			{
				if (nbtTag is NbtCompound)
				{
					EnemyData enemyData = new EnemyData((NbtCompound)nbtTag);
					int key = Hash.Get(enemyData.QualifiedName);
					this.enemyDataDict.Add(key, enemyData);
				}
			}*/
		}

        public EnemyData GetEnemyData(string name)
        {
            int hash =  Hash.Get(name);
            EnemyData attemptData = null;
            if (enemyDataDict.TryGetValue(hash, out attemptData))
            {
                //Console.WriteLine($"Properly returned enemy: {name}");
                return attemptData;
            }
            Console.WriteLine($"Was unable to return enemy: {name}");
            return attemptData;
        }

        public List<EnemyData> GetAllEnemyData()
		{
			return new List<EnemyData>(this.enemyDataDict.Values);
		}

		// Token: 0x04000193 RID: 403
		private static EnemyFile INSTANCE;

		// Token: 0x04000194 RID: 404
		private Dictionary<int, EnemyData> enemyDataDict;
	}
}
