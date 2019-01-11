using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace CrubSqlConnection
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Click on the link below to continue learning how to build a desktop app using WinForms!
            System.Diagnostics.Process.Start("http://aka.ms/dotnet-get-started-desktop");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks!");
        }

        private void helloWorldLabel_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshUsers();
        }

        public void RefreshUsers() {
            this.dgvUsuarios.DataSource = new UsuarioDAO().GetUsuario();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            UsuarioBean u = new UsuarioBean();
            string id = string.IsNullOrEmpty(txtID.Text) ? "0" : txtID.Text;
            u.Id = Convert.ToInt32(id);
            u.UsuarioName = txtUsuario.Text;
            u.Pwd = txtPwd.Text;

            //guardar usuario
            new UsuarioDAO().SaveUsuario(u);

            txtID.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtPwd.Text = string.Empty;

            RefreshUsers();

        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1) {
                txtID.Text = dgvUsuarios.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtUsuario.Text = dgvUsuarios.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtPwd.Text = dgvUsuarios.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            UsuarioBean u = new UsuarioBean();
            string id = string.IsNullOrEmpty(txtID.Text) ? "0" : txtID.Text;
            u.Id = Convert.ToInt32(id);
            u.UsuarioName = txtUsuario.Text;
            u.Pwd = txtPwd.Text;

            //guardar usuario
            new UsuarioDAO().DeleteUsuario(u);

            txtID.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtPwd.Text = string.Empty;

            RefreshUsers();
        }
    }
}
