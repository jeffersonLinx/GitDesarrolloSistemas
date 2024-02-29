using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GaleriaFotografica
{
    public partial class Proyectos : GaleriaFotografica.header
    {
        //private string cadenaDeConexion;
        //cadenaDeConexion = ConfigurationManager.ConnectionStrings["GaleriaFotograficaWeb"].ConnectionString;
        SqlConnection conexion = new SqlConnection("server=DESKTOP-1UHVG6J; database=GaleriaFotografica2;Integrated Security=True");

        public Proyectos()
        {
            InitializeComponent();
        }
        //definir variables a utilizar
        #region"Mis variables"
        #endregion
        
        #region "Mis metodos"
        private void Estado_texto(bool lEstado)
        {
            //bloquear o desbloquear los textos
            // ojo con esto parece que no lo ocupare
            txtNombre.ReadOnly = !lEstado;
            txtDescripcion.ReadOnly = !lEstado;
            txtFotografo.ReadOnly = !lEstado;
        }
        private void Limpia_texto()
        {

            txtNombre.Clear();
            txtFotografo.Clear();
            txtDescripcion.Clear();
            txtDescripcion.Clear();           
           
        }
        private void EstadoBotonesProceso(bool lEstado)
        {
            //metodo para mostrar los objetos que esten con visibilidad valsa
            //visibilidad de los botones
            //btnLupaCat.Visible = lEstado;
            groupBox1.Visible = lEstado;
        }
        private void estadoBotonesPrincipare(bool lEstado)
        {
            // metodo para bloquear otros botones cunado se usa una accion en un formulario
            btnNuevo.Enabled = lEstado;
            btnEliminar.Enabled = lEstado;
            btnModificar.Enabled = lEstado;
            dataGridView1.Enabled = lEstado;
        }
        //
        private void EliminarRegistro(int id) 
        {
            try
            {
                conexion.Open();
                string consulta = "DELETE FROM trnProyectos WHERE IdProyectos = @IdProyectos";
                SqlCommand comando = new SqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@IdProyectos", id);
                comando.ExecuteNonQuery();
                MessageBox.Show("Registro eliminado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar eliminar el registro: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }
        private void Formato_ar(bool lEstado)
        {
            ////para ocultar y dar formato al data grid view
            //dataGridView1.Columns[0].Visible = true;


            //USAR this.formato_ar();

        }
        private void CargarCategoriaEnConboBox()
        {
            string consulta = "SELECT IdCategorias AS Value, Categoria AS Display FROM trnCategorias";
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Display"; // Columna que se mostrará en el ComboBox
            comboBox1.ValueMember = "Value"; // Valor asociado a cada elemento del ComboBox
        }
        private string SeleccionarImagen()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;

            }
            else
            {
                return null;
            }
        }
        private string GuardarImagenEnCarpeta(string rutaImagenSeleccionada)
        {
            string nombreArchivo = Path.GetFileName(rutaImagenSeleccionada);
            string carpetaAssets = Path.Combine(Application.StartupPath, "assets");
            string nuevaRuta = Path.Combine(carpetaAssets, nombreArchivo);

            try
            {
                if (!Directory.Exists(carpetaAssets))
                {
                    Directory.CreateDirectory(carpetaAssets);
                }

                File.Copy(rutaImagenSeleccionada, nuevaRuta, true);
                return nuevaRuta;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al copiar la imagen: " + ex.Message);
                return null;
            }
        }
        public void llenar_tabla()
        {
            string consulta = "SELECT p.*, c.IdCategorias AS id_cat, c.Categoria FROM trnProyectos p INNER JOIN trnCategorias c ON c.IdCategorias = p.IdCategorias ORDER BY p.IdProyectos DESC;";
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dataGridView1.DataSource = dt;
            conexion.Close();
        }

        private string rutaImagenSeleccionada;// ruta global para obtener la direccion de la imagen

        #endregion


        private void Proyectos_Load(object sender, EventArgs e)
        {
            //para mostar los datos de la base de datos en un datagridview
            //estableser una cadena de consultas
            string consulta = "SELECT p.*, c.IdCategorias AS id_cat, c.Categoria FROM trnProyectos p INNER JOIN trnCategorias c ON c.IdCategorias = p.IdCategorias ORDER BY p.IdProyectos DESC; ";
            //parametros cosulta, conexion, objeto adatador
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
            //pasar la consulta a la tabla memoria datable
            DataTable dt = new DataTable();
            //lo que se tenga en el adaptador se llenara en datatable
            adaptador.Fill(dt);
            //vista vista tomara los datos de dt
            dataGridView1.DataSource = dt;

            // Agregar columna de botón Eliminar al DataGridView
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Eliminar";
            buttonColumn.Name = "Eliminar"; // Nombre de la columna
            buttonColumn.Text = "Eliminar";
            buttonColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(buttonColumn);

            //EVENTO 3
            CargarCategoriaEnConboBox();
            //CargarDatosEnDataGridView();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
          
            this.EstadoBotonesProceso(true);
            this.Limpia_texto();
            this.estadoBotonesPrincipare(false);
            this.Estado_texto(true);
            txtNombre.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.EstadoBotonesProceso(false);
            this.Limpia_texto();
            this.estadoBotonesPrincipare(true);
            this.Estado_texto(false);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           

            // vsita de las celdas para devolver a los txt para su modificacion
            dataGridView1.SelectedCells[0].Value.ToString();
            dataGridView1.SelectedCells[1].Value.ToString();
            dataGridView1.SelectedCells[2].Value.ToString();
            dataGridView1.SelectedCells[3].Value.ToString();
            dataGridView1.SelectedCells[4].Value.ToString();
            dataGridView1.SelectedCells[5].Value.ToString();
            dataGridView1.SelectedCells[6].Value.ToString();


            // Verificar si se hizo clic en el botón Eliminar
            if (e.ColumnIndex == dataGridView1.Columns["Eliminar"].Index && e.RowIndex >= 0)
            {
                // Obtener el ID del registro a eliminar
                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["IdProyectos"].Value);

                // Eliminar el registro de la base de datos
                EliminarRegistro(id);

                // Eliminar la fila del DataGridView
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }

            //this.Formato_ar(true);
        }


        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (rutaImagenSeleccionada != null)
            {
                // Guardar la imagen en la carpeta "assets"
                string nuevaRuta = GuardarImagenEnCarpeta(rutaImagenSeleccionada);
                if (nuevaRuta != null)
                {
                    try
                    {
                        conexion.Open();
                        string consulta = "INSERT INTO trnProyectos (IdCategorias, descripcion, imagenPath, nombre, fotografo, EstadoRegistro) " +
                                          "VALUES (@IdCategorias, @descripcion, @imagenPath, @nombre, @fotografo, @EstadoRegistro)";
                        SqlCommand comando = new SqlCommand(consulta, conexion);
                        comando.Parameters.AddWithValue("@IdCategorias", comboBox1.SelectedValue);
                        comando.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                        comando.Parameters.AddWithValue("@imagenPath", nuevaRuta); // Utiliza la nueva ruta de la imagen
                        comando.Parameters.AddWithValue("@nombre", txtNombre.Text);
                        comando.Parameters.AddWithValue("@fotografo", txtFotografo.Text);
                        comando.Parameters.AddWithValue("@EstadoRegistro", 1); // valor de EstadoRegistro
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Registro insertado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al intentar insertar el registro: " + ex.Message);
                    }
                    finally
                    {
                        conexion.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una imagen antes de guardar.");
            }

            llenar_tabla();
            Limpia_texto();
        }

    


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            //string rutaImagen = SeleccionarImagen();
            //if(rutaImagen != null)
            //{
            //    pictureBox1.Image = Image.FromFile(rutaImagen);
            //}
            if(openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                rutaImagenSeleccionada = openFileDialog1.FileName;
                txtFotografia.Text = openFileDialog1.FileName;
                //capas y lo vuelbo invisible esto XD
                pictureBox1.Image = Image.FromFile(rutaImagenSeleccionada);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtFotografia_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFotografo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
