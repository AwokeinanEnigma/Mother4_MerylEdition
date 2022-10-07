using System;
using System.Collections.Generic;
using Mother4.Data;

namespace Mother4.Items
{
	internal class InventoryManager
	{
		public static InventoryManager Instance
		{
			get
			{
				if (InventoryManager.instance == null)
				{
					InventoryManager.instance = new InventoryManager();
				}
				return InventoryManager.instance;
			}
		}

		public Inventory KeyInventory
		{
			get
			{
				return this.keyInventory;
			}
		}

		private InventoryManager()
		{
			this.keyInventory = new Inventory(14);
			this.inventories = new Dictionary<CharacterType, Inventory>();
		}

		public Inventory Add(CharacterType key)
		{
			Inventory inventory;
			if (!this.inventories.ContainsKey(key))
			{
				inventory = new Inventory(14);
				this.inventories.Add(key, inventory);
			}
			else
			{
				inventory = this.inventories[key];
			}
			return inventory;
		}

		public void Remove(CharacterType key)
		{
			this.inventories.Remove(key);
		}

		public Inventory Get(CharacterType key)
		{
			if (!this.inventories.ContainsKey(key))
			{
				this.Add(key);
			}
			return this.inventories[key];
		}

		public const int INVENTORY_SIZE = 14;

		public const int KEY_INVENTORY_SIZE = 14;

		private static InventoryManager instance;

		private Inventory keyInventory;

		private Dictionary<CharacterType, Inventory> inventories;
	}
}
