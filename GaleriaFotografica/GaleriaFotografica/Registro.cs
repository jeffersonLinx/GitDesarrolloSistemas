using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GaleriaFotografica
{
    public partial class Registro : GaleriaFotografica.header
    {
        //private string cadenaDeConexion;
        //cadenaDeConexion = ConfigurationManager.ConnectionStrings["GaleriaFotograficaWeb"].ConnectionString;
        SqlConnection conexion = new SqlConnection("server=DESKTOP-1UHVG6J; database=GaleriaFotografica2;Integrated Security=True");

        public Registro()
        {
            InitializeComponent();

        }
        #region
        public void limpiar_Campos()
        {
            txtCedula.Clear();
            txtClave.Clear();
            txtEstado.Clear();
            txtSalt.Clear();
            txtTelefono.Clear();
            txtUsuario.Clear();

        }
        #endregion

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            conexion.Open();
            //crear bariable consulta para insertar datos
            string consulta = "insert into trnUsuario values('"+txtUsuario.Text +"','" + txtClave.Text + "','"+txtSalt.Text+ "', '"+txtTelefono.Text+ "','"+txtCedula.Text+ "','" + dateTimePicker1.Text + "','" + txtEstado.Text+"')";
            //crear objeto de clase sql comand se utiliza ejecutar consultas en sql
            SqlCommand comando = new SqlCommand(consulta, conexion);
            //eejcutar todo el codigo
            comando.ExecuteNonQuery();
            MessageBox.Show("Rguistro agregado");

            conexion.Close();
            limpiar_Campos();

        }

        private void Registro_Load(object sender, EventArgs e)
        {

        }
    }
}
