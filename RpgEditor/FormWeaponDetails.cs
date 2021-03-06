﻿using System;
using System.Linq;
using System.Windows.Forms;
using RpgLibrary;
using RpgLibrary.Effects;
using RpgLibrary.Items;

namespace RpgEditor
{
    public partial class FormWeaponDetails : Form
    {
        public WeaponData Weapon { get; set; }

        public FormWeaponDetails()
        {
            InitializeComponent();

            Load += FormWeaponDetails_Load;
            FormClosing += FormWeaponDetails_FormClosing;

            btnOK.Click += btnOK_Click;
            btnCancel.Click += btnCancel_Click;
            btnMoveAllowed.Click += btnMoveAllowed_Click;
            btnRemoveAllowed.Click += btnRemoveAllowed_Click;
        }

        private void FormWeaponDetails_Load(object sender, EventArgs e)
        {
            foreach (var s in FormDetails.EntityDataManager.EntityData.Keys)
                lbClasses.Items.Add(s);

            foreach (var location in Enum.GetValues(typeof(Hands)))
                cboHands.Items.Add(location);

            var enumerable = Enum.GetName(typeof(DieType), DieType.D4);
            if (enumerable != null)
                foreach (var die in enumerable)
                    cboDieType.Items.Add(die);

            cboHands.SelectedIndex = 0;
            cboDieType.SelectedIndex = 0;
            cboDieType.SelectedValue = Enum.GetName(typeof(DieType), DieType.D4);

            if (Weapon == null) return;

            tbName.Text = Weapon.Name;
            tbType.Text = Weapon.Type;
            mtbPrice.Text = Weapon.Price.ToString();
            nudWeight.Value = (decimal) Weapon.Weight;
            cboHands.SelectedIndex = (int) Weapon.NumberHands;
            mtbAttackValue.Text = Weapon.AttackValue.ToString();
            mtbAttackModifier.Text = Weapon.AttackModifier.ToString();

            for (var i = 0; i < cboDieType.Items.Count; ++i)
            {
                if (cboDieType.Items[i].ToString() != Weapon.DamageEffectData.DieType.ToString()) continue;

                cboDieType.SelectedIndex = i;
                cboDieType.SelectedValue = cboDieType.Items[i];
                break;
            }

            nudDice.Value = Weapon.DamageEffectData.NumberOfDice;
            mtbDamageModifier.Text = Weapon.DamageEffectData.Modifier.ToString();

            foreach (var s in Weapon.AllowableClasses)
            {
                if (lbClasses.Items.Contains(s))
                    lbClasses.Items.Remove(s);

                lbAllowedClasses.Items.Add(s);
            }
        }

        private static void FormWeaponDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
        }

        private void btnMoveAllowed_Click(object sender, EventArgs e)
        {
            if (lbClasses.SelectedItem == null) return;

            lbAllowedClasses.Items.Add(lbClasses.SelectedItem);
            lbClasses.Items.RemoveAt(lbClasses.SelectedIndex);
        }

        private void btnRemoveAllowed_Click(object sender, EventArgs e)
        {
            if (lbAllowedClasses.SelectedItem == null) return;

            lbClasses.Items.Add(lbAllowedClasses.SelectedItem);
            lbAllowedClasses.Items.RemoveAt(lbAllowedClasses.SelectedIndex);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("You must enter a name for the item.");
                return;
            }

            if (!int.TryParse(mtbPrice.Text, out int price))
            {
                MessageBox.Show("Price must be an integer value.");
                return;
            }

            var weight = (float) nudWeight.Value;

            if (!int.TryParse(mtbAttackValue.Text, out int attVal))
            {
                MessageBox.Show("Attack value must be an interger value.");
                return;
            }

            if (!int.TryParse(mtbAttackModifier.Text, out int attMod))
            {
                MessageBox.Show("Attack value must be an interger value.");
                return;
            }

            if (!int.TryParse(mtbDamageModifier.Text, out int damMod))
            {
                MessageBox.Show("Damage value must be an interger value.");
                return;
            }

            Weapon = new WeaponData
            {
                Name = tbName.Text,
                Type = tbType.Text,
                Price = price,
                Weight = weight,
                NumberHands = (Hands) cboHands.SelectedIndex,
                AttackValue = attVal,
                AttackModifier = attMod,
                AllowableClasses = (from object o in lbAllowedClasses.Items select o.ToString()).ToArray(),
                DamageEffectData =
                {
                    Name = tbName.Text,
                    AttackType = AttackType.Health,

                    // TODO: Add different damage types
                    DamageType = DamageType.Weapon,
                    DieType = (DieType) Enum.Parse(
                        typeof(DieType),
                        cboDieType.Items[cboDieType.SelectedIndex].ToString()),
                    NumberOfDice = (int) nudDice.Value,
                    Modifier = damMod
                }
            };

            FormClosing -= FormWeaponDetails_FormClosing;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Weapon = null;
            FormClosing -= FormWeaponDetails_FormClosing;
            Close();
        }
    }
}