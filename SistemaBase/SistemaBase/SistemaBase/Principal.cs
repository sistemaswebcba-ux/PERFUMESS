using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaBase
{
    public partial class Principal : Form
    {
        private int childFormNumber = 0;
        //nombre del campo descripcion
        public static string CampoNombreSecundario;
        //nombre de la tabla donde se realiza el grabado
        public static string NombreTablaSecundario;
        public static string CampoIdSecundario;
        public static string OpcionesColumnasGrilla;
        public static string OpcionesdeBusqueda;
        public static string TablaPrincipal;
        public static string ColumnasVisibles;
        public static string ColumnasAncho;
        public static string CodigoPrincipalAbm;
        public static Int32 CodUsuarioLogueado;
        public static string NombreUsuarioLogueado;
        public static Int32 CodProoducto;
        public static Int32 CodCliente;
        public static Int32 CodVenta;
        public Principal()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            FrmAmbProducto childForm = new FrmAmbProducto();
            childForm.MdiParent = this;
           // childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            FrmListadoVentascs frm = new FrmListadoVentascs();
            frm.Show();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmResumen frm = new FrmResumen();
            frm.Show();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.MenuBar;
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAbmUsuario frm = new FrmAbmUsuario();
            frm.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Principal.CodVenta = 0;
            FrmVenta frm = new SistemaBase.FrmVenta();
            frm.Show();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListadoVentascs frm = new FrmListadoVentascs();
            frm.Show();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            FrmActualizarStock childForm = new FrmActualizarStock();
            childForm.MdiParent = this;
            childForm.Text = "Formulario para actualizar stock y precio  ";
            childForm.Show();
        }

        private void actualizarStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmActualizarStock frm = new FrmActualizarStock();
            frm.Show();
        }

        private void marcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAbmMarca frm = new FrmAbmMarca();
            frm.Show();
        }

        private void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAbmCliente frm = new SistemaBase.FrmAbmCliente();
            frm.Show();
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListadoDeudores frm = new FrmListadoDeudores();
            frm.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCopia frm = new FrmCopia();
            frm.Show();
        }
    }
}
