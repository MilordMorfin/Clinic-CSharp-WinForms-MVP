﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic
{
    public partial class FormConnectMedDose : Form
    {
        #region Fields
        private readonly DatabaseConnection connection = DatabaseConnection.Instance;
        #endregion

        #region Properties
        private List<string> Medicines
        {
            get
            {
                List<string> medicines = new List<string>();
                foreach(var el in comboBoxMedicine.Items)
                {
                    medicines.Add(el.ToString());
                }
                return medicines;
            }
            set
            {
                if(comboBoxMedicine.Items.Count > 0) { comboBoxMedicine.Items.Clear(); }
                foreach(var el in value)
                {
                    comboBoxMedicine.Items.Add(el);
                }
            }
        }
        private List<string> Doses
        {
            get
            {
                List<string> doses = new List<string>();
                foreach (var el in comboBoxDose.Items)
                {
                    doses.Add(el.ToString());
                }
                return doses;                    
            }
            set
            {
                if (comboBoxDose.Items.Count > 0) { comboBoxDose.Items.Clear(); }
                foreach (var el in value)
                {
                    comboBoxDose.Items.Add(el);
                }
            }
        }

        private int SelectedMedicine
        {
            get
            {
                if (comboBoxMedicine.SelectedIndex > -1)
                {
                    return int.Parse(comboBoxMedicine.SelectedItem.ToString().Split()[0]);
                }
                else return -1;
            }
        }
        private int SelectedDose
        {
            get
            {
                if (comboBoxDose.SelectedIndex > -1)
                {
                    return int.Parse(comboBoxDose.SelectedItem.ToString().Split()[0]);
                }
                else return -1;
            }
        }
        #endregion

        public FormConnectMedDose()
        {
            InitializeComponent();
            connection = DatabaseConnection.Instance;
        }

        #region Methods
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (SelectedMedicine > -1 && SelectedDose > -1)
            {
                if (connection.Open())
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>()
                    {
                        {"@dose", SelectedDose.ToString() },
                        {"@medicine", SelectedMedicine.ToString() }
                    };

                    if (connection.SelectCount($"SELECT COUNT(*) FROM dawki_i_leki WHERE idd=@dose AND idl=@medicine", parameters) == 0)
                    {
                        if (connection.InsertInfo($"INSERT INTO dawki_i_leki(idd, idl) VALUES(@dose, @medicine)", parameters))
                        { 
                            MessageBox.Show("Połączenie leku z dawką zostało pomyślnie dodane. Zmiany będą widoczne po wyłączeniu tego okna.", "Potwierdzenie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            connection.Close();
                        }
                        else { MessageBox.Show("Błąd z przypisywaniem dawki do leku! Zgłoś się do administratora!"); }
                    }
                    else { MessageBox.Show("Taki wpis już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        
                }
                else
                {
                    MessageBox.Show("Błąd z połaczeniem!");
                }
            }
            else
            {
                MessageBox.Show("Wybierz coś!");
            }
        }

        private void FormAddRowToPrescription_Load(object sender, EventArgs e)
        {
            if (connection.Open())
            {
                List<string> medicines = new List<string>();

                foreach (var el in connection.GetPrescription("SELECT idl, nazwa FROM leki ORDER BY 2", new Dictionary<string, string>()))
                {
                    medicines.Add(el);
                }

                List<string> doses = new List<string>();

                foreach (var el in connection.GetPrescription("SELECT idd, ile FROM dawki ORDER BY 2", new Dictionary<string, string>()))
                {
                    doses.Add(el);
                }

                Medicines = medicines;
                Doses = doses;

                connection.Close();
            }
            else
            {
                MessageBox.Show("Błąd z połaczeniem!");
            }
        }
        #endregion
    }
}
