using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;

namespace TesteMensagemPinPad
{
    public class Form1 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnConfirmacaoPinPad;
        private System.Windows.Forms.Button btnConfigura;
        private System.Windows.Forms.Button btnAbre;
        private System.Windows.Forms.Button btnFecha;
        private System.Windows.Forms.Button btnLeCartao;
        private System.Windows.Forms.TextBox textBox1;
        private Button btnVenda;

        private CliSitefAPI clisitef;

        public Form1()
        {
            clisitef = new CliSitefAPI();
            InitializeComponent();
        }

        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConfigura = new System.Windows.Forms.Button();
            this.btnAbre = new System.Windows.Forms.Button();
            this.btnFecha = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnConfirmacaoPinPad = new System.Windows.Forms.Button();
            this.btnLeCartao = new System.Windows.Forms.Button();
            this.btnVenda = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConfigura
            // 
            this.btnConfigura.Location = new System.Drawing.Point(16, 16);
            this.btnConfigura.Name = "btnConfigura";
            this.btnConfigura.Size = new System.Drawing.Size(96, 24);
            this.btnConfigura.TabIndex = 0;
            this.btnConfigura.Text = "Configura";
            this.btnConfigura.Click += new System.EventHandler(this.btnConfigura_Click);
            // 
            // btnAbre
            // 
            this.btnAbre.Location = new System.Drawing.Point(16, 48);
            this.btnAbre.Name = "btnAbre";
            this.btnAbre.Size = new System.Drawing.Size(96, 24);
            this.btnAbre.TabIndex = 2;
            this.btnAbre.Text = "Abre PinPad";
            this.btnAbre.Click += new System.EventHandler(this.btnAbre_Click);
            // 
            // btnFecha
            // 
            this.btnFecha.Location = new System.Drawing.Point(128, 48);
            this.btnFecha.Name = "btnFecha";
            this.btnFecha.Size = new System.Drawing.Size(96, 24);
            this.btnFecha.TabIndex = 3;
            this.btnFecha.Text = "Fecha PinPad";
            this.btnFecha.Click += new System.EventHandler(this.btnFecha_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(208, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "Mensagem PinPad";
            // 
            // btnConfirmacaoPinPad
            // 
            this.btnConfirmacaoPinPad.Location = new System.Drawing.Point(16, 112);
            this.btnConfirmacaoPinPad.Name = "btnConfirmacaoPinPad";
            this.btnConfirmacaoPinPad.Size = new System.Drawing.Size(96, 24);
            this.btnConfirmacaoPinPad.TabIndex = 5;
            this.btnConfirmacaoPinPad.Text = "Sim/Nao PinPad";
            this.btnConfirmacaoPinPad.Click += new System.EventHandler(this.btnConfirmacaoPinPad_Click);
            // 
            // btnLeCartao
            // 
            this.btnLeCartao.Location = new System.Drawing.Point(128, 112);
            this.btnLeCartao.Name = "btnLeCartao";
            this.btnLeCartao.Size = new System.Drawing.Size(96, 24);
            this.btnLeCartao.TabIndex = 6;
            this.btnLeCartao.Text = "Le cartao";
            this.btnLeCartao.Click += new System.EventHandler(this.btnLeCartao_Click);
            // 
            // btnVenda
            // 
            this.btnVenda.Location = new System.Drawing.Point(128, 16);
            this.btnVenda.Name = "btnVenda";
            this.btnVenda.Size = new System.Drawing.Size(96, 24);
            this.btnVenda.TabIndex = 1;
            this.btnVenda.Text = "Venda";
            this.btnVenda.Click += new System.EventHandler(this.btnVenda_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(239, 156);
            this.Controls.Add(this.btnConfigura);
            this.Controls.Add(this.btnVenda);
            this.Controls.Add(this.btnAbre);
            this.Controls.Add(this.btnFecha);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnConfirmacaoPinPad);
            this.Controls.Add(this.btnLeCartao);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        static void Main() 
        {
            Application.Run(new Form1());
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {			
        }

        private void btnConfirmacaoPinPad_Click(object sender, System.EventArgs e)
        {
            int retorno =  clisitef.LeConfirmacaoPinPad(textBox1.Text);

            System.Windows.Forms.MessageBox.Show("Retorno Sim/Nao PinPad: [" + retorno.ToString() + "]", "Confirmacao PinPad");
        }

        private void btnConfigura_Click(object sender, System.EventArgs e)
        {
            int retorno = clisitef.Configura("127.0.0.1", "00000000", "SW000001");

            if (sender != null)
            {
                System.Windows.Forms.MessageBox.Show("Retorno Configura: [" + retorno.ToString() + "]", "Configura CliSiTef");
            }
        }

        private void btnAbre_Click(object sender, System.EventArgs e)
        {
            int retorno = clisitef.AbrirPinPad();

            System.Windows.Forms.MessageBox.Show("Retorno Abertura PinPad: [" + retorno.ToString() + "]", "Abre PinPad");
        }

        private void btnFecha_Click(object sender, System.EventArgs e)
        {
            int retorno = clisitef.FecharPinPad();

            System.Windows.Forms.MessageBox.Show("Retorno Fechamento PinPad: [" + retorno.ToString() + "]", "Fecha PinPad");

        }

        private void btnLeCartao_Click(object sender, System.EventArgs e)
        {
            string trilha1;
            string trilha2;

            int retorno = clisitef.LeCartao(textBox1.Text, out trilha1, out trilha2);

            System.Windows.Forms.MessageBox.Show("Retorno LeCartao: [" + retorno.ToString() + "]", "LeCartao");

            if (retorno != -999)
            {
                System.Windows.Forms.MessageBox.Show("Trilha1: [" + trilha1.ToString() + "]", "LeCartao");
                System.Windows.Forms.MessageBox.Show("Trilha2: [" + trilha2.ToString() + "]", "LeCartao");
            }
        }

        private void btnVenda_Click(object sender, System.EventArgs e)
        {
            if (!clisitef.Configurado)
            {
                btnConfigura_Click(null, null);
            }
            
            string valor = "100,00";
            string cupomFiscal = "12345";
            string dataFiscal = "20001231";
            string horario = "120000";
            string operador = "OPERADOR";
            string restricoes = "";

            int retorno = clisitef.Venda(0, valor, cupomFiscal, dataFiscal, horario, operador, restricoes);

            System.Windows.Forms.MessageBox.Show("retorno: [" + retorno.ToString() + "]", "Venda CliSiTef");
        }
    }
}
