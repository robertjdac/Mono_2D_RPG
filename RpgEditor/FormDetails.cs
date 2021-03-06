﻿using System.IO;
using System.Windows.Forms;
using RpgLibrary.Characters;
using RpgLibrary.Items;
using RpgLibrary.Skills;

namespace RpgEditor
{
    public partial class FormDetails : Form
    {
        public static ItemDataManager ItemDataManager { get; protected set; }
        public static EntityDataManager EntityDataManager { get; protected set; }
        public static SkillDataManager SkillDataManager { get; protected set; }

        public FormDetails()
        {
            InitializeComponent();

            if (ItemDataManager == null)
                ItemDataManager = new ItemDataManager();

            if (EntityDataManager == null)
                EntityDataManager = new EntityDataManager();

            FormClosing += FormDetails_FormClosing;
        }

        private void FormDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }

            if (e.CloseReason == CloseReason.MdiFormClosing)
            {
                e.Cancel = false;
                Close();
            }
        }

        public static void WriteEntityData()
        {
            foreach (var s in EntityDataManager.EntityData.Keys)
            {
                XmlSerializer.Serialize(
                    FormMain.ClassPath + @"\" + s + ".xml",
                    EntityDataManager.EntityData[s]);
            }
        }

        public static void WriteItemData()
        {
            foreach (var s in ItemDataManager.ArmorData.Keys)
            {
                XmlSerializer.Serialize(
                    FormMain.ItemPath + @"\Armor\" + s + ".xml",
                    ItemDataManager.ArmorData[s]);
            }

            foreach (var s in ItemDataManager.ShieldData.Keys)
            {
                XmlSerializer.Serialize(
                    FormMain.ItemPath + @"\Shield\" + s + ".xml",
                    ItemDataManager.ShieldData[s]);
            }

            foreach (var s in ItemDataManager.WeaponData.Keys)
            {
                XmlSerializer.Serialize(
                    FormMain.ItemPath + @"\Weapon\" + s + ".xml",
                    ItemDataManager.WeaponData[s]);
            }
        }

        public static void WriteKeyData()
        {
            foreach (var s in ItemDataManager.KeyData.Keys)
            {
                XmlSerializer.Serialize(
                    FormMain.KeyPath + @"\" + s + ".xml",
                    ItemDataManager.KeyData[s]);
            }
        }

        public static void WriteChestData()
        {
            foreach (var s in ItemDataManager.ChestData.Keys)
            {
                XmlSerializer.Serialize(
                    FormMain.ChestPath + @"\" + s + ".xml",
                    ItemDataManager.ChestData[s]);
            }
        }

        public static void WriteSkillData()
        {
            foreach (var s in SkillDataManager.SkillData.Keys)
            {
                XmlSerializer.Serialize(
                    FormMain.SkillPath + @"\" + s + ".xml",
                    SkillDataManager.SkillData[s]);
            }
        }

        public static void ReadEntityData()
        {
            EntityDataManager = new EntityDataManager();

            var fileNames = Directory.GetFiles(FormMain.ClassPath, "*.xml");

            foreach (var s in fileNames)
            {
                var entityData = XmlSerializer.Deserialize<EntityData>(s);
                EntityDataManager.EntityData.Add(entityData.Name, entityData);
            }
        }

        public static void ReadItemData()
        {
            ItemDataManager = new ItemDataManager();

            var fileNames = Directory.GetFiles(
                Path.Combine(FormMain.ItemPath, "Armor"),
                "*.xml");

            foreach (var s in fileNames)
            {
                var armorData = XmlSerializer.Deserialize<ArmorData>(s);
                ItemDataManager.ArmorData.Add(armorData.Name, armorData);
            }

            fileNames = Directory.GetFiles(
                Path.Combine(FormMain.ItemPath, "Shield"),
                "*.xml");

            foreach (var s in fileNames)
            {
                var shieldData = XmlSerializer.Deserialize<ShieldData>(s);
                ItemDataManager.ShieldData.Add(shieldData.Name, shieldData);
            }

            fileNames = Directory.GetFiles(
                Path.Combine(FormMain.ItemPath, "Weapon"),
                "*.xml");

            foreach (var s in fileNames)
            {
                var weaponData = XmlSerializer.Deserialize<WeaponData>(s);
                ItemDataManager.WeaponData.Add(weaponData.Name, weaponData);
            }
        }

        public static void ReadKeyData()
        {
            var fileNames = Directory.GetFiles(FormMain.KeyPath, "*.xml");

            foreach (var s in fileNames)
            {
                var keyData = XmlSerializer.Deserialize<KeyData>(s);

                ItemDataManager.KeyData.Add(keyData.Name, keyData);
            }
        }

        public static void ReadChestData()
        {
            var fileNames = Directory.GetFiles(FormMain.ChestPath, "*.xml");

            foreach (var s in fileNames)
            {
                var chestData = XmlSerializer.Deserialize<ChestData>(s);

                ItemDataManager.ChestData.Add(chestData.Name, chestData);
            }
        }

        public static void ReadSkillData()
        {
            SkillDataManager = new SkillDataManager();

            var fileNames = Directory.GetFiles(FormMain.SkillPath, "*.xml");

            foreach (var s in fileNames)
            {
                var skillData = XmlSerializer.Deserialize<SkillData>(s);
                SkillDataManager.SkillData.Add(skillData.Name, skillData);
            }
        }
    }
}
