using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;



namespace TesteMensagemPinPad
{
    public class frmPDVTEF : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnConfirmacaoPinPad;
        private System.Windows.Forms.Button btnConfigura;
        private System.Windows.Forms.Button btnAbre;
        private System.Windows.Forms.Button btnFecha;
        private System.Windows.Forms.Button btnLeCartao;
        private System.Windows.Forms.TextBox textBox1;
        private Button btnVenda;
        private TextBox txtValor;
        private TextBox txtCupomFiscal;
        private TextBox txtOperador;
        private CliSitefAPI clisitef;
        private Timer timer1;
        private System.ComponentModel.IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private GroupBox groupBox3;
        private Label lblQtdParcelas;
        private TrackBar trkQtdParcelas;
        private ComboBox cmbTipoVenda;
        private TextBox txtLog;
        private TextBox txtLogSitef;
        private Button button1;
        private PDVTEF.PagAPI pdvapi;

        public frmPDVTEF()
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
            this.components = new System.ComponentModel.Container();
            this.btnConfigura = new System.Windows.Forms.Button();
            this.btnAbre = new System.Windows.Forms.Button();
            this.btnFecha = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnConfirmacaoPinPad = new System.Windows.Forms.Button();
            this.btnLeCartao = new System.Windows.Forms.Button();
            this.btnVenda = new System.Windows.Forms.Button();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.txtCupomFiscal = new System.Windows.Forms.TextBox();
            this.txtOperador = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblQtdParcelas = new System.Windows.Forms.Label();
            this.trkQtdParcelas = new System.Windows.Forms.TrackBar();
            this.cmbTipoVenda = new System.Windows.Forms.ComboBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtLogSitef = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkQtdParcelas)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConfigura
            // 
            this.btnConfigura.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigura.Location = new System.Drawing.Point(6, 19);
            this.btnConfigura.Name = "btnConfigura";
            this.btnConfigura.Size = new System.Drawing.Size(127, 24);
            this.btnConfigura.TabIndex = 0;
            this.btnConfigura.Text = "1. Configura";
            this.btnConfigura.Click += new System.EventHandler(this.btnConfigura_Click);
            // 
            // btnAbre
            // 
            this.btnAbre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbre.Location = new System.Drawing.Point(6, 49);
            this.btnAbre.Name = "btnAbre";
            this.btnAbre.Size = new System.Drawing.Size(127, 24);
            this.btnAbre.TabIndex = 2;
            this.btnAbre.Text = "2. Abre PinPad";
            this.btnAbre.Click += new System.EventHandler(this.btnAbre_Click);
            // 
            // btnFecha
            // 
            this.btnFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFecha.Location = new System.Drawing.Point(6, 79);
            this.btnFecha.Name = "btnFecha";
            this.btnFecha.Size = new System.Drawing.Size(127, 24);
            this.btnFecha.TabIndex = 3;
            this.btnFecha.Text = "Fecha PinPad";
            this.btnFecha.Click += new System.EventHandler(this.btnFecha_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(6, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(243, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "Mensagem a ser enviada ao PinPad";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnConfirmacaoPinPad
            // 
            this.btnConfirmacaoPinPad.Location = new System.Drawing.Point(6, 48);
            this.btnConfirmacaoPinPad.Name = "btnConfirmacaoPinPad";
            this.btnConfirmacaoPinPad.Size = new System.Drawing.Size(243, 24);
            this.btnConfirmacaoPinPad.TabIndex = 5;
            this.btnConfirmacaoPinPad.Text = "Enviar Mensagem PinPad ";
            this.btnConfirmacaoPinPad.Click += new System.EventHandler(this.btnConfirmacaoPinPad_Click);
            // 
            // btnLeCartao
            // 
            this.btnLeCartao.Location = new System.Drawing.Point(7, 216);
            this.btnLeCartao.Name = "btnLeCartao";
            this.btnLeCartao.Size = new System.Drawing.Size(187, 24);
            this.btnLeCartao.TabIndex = 6;
            this.btnLeCartao.Text = "Le cartao";
            this.btnLeCartao.Click += new System.EventHandler(this.btnLeCartao_Click);
            // 
            // btnVenda
            // 
            this.btnVenda.Location = new System.Drawing.Point(7, 262);
            this.btnVenda.Name = "btnVenda";
            this.btnVenda.Size = new System.Drawing.Size(187, 24);
            this.btnVenda.TabIndex = 1;
            this.btnVenda.Text = "Venda";
            this.btnVenda.Click += new System.EventHandler(this.btnVenda_Click);
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(6, 47);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(188, 20);
            this.txtValor.TabIndex = 7;
            this.txtValor.Text = "Valor";
            // 
            // txtCupomFiscal
            // 
            this.txtCupomFiscal.Location = new System.Drawing.Point(6, 73);
            this.txtCupomFiscal.Name = "txtCupomFiscal";
            this.txtCupomFiscal.Size = new System.Drawing.Size(188, 20);
            this.txtCupomFiscal.TabIndex = 8;
            this.txtCupomFiscal.Text = "CupomFiscal";
            // 
            // txtOperador
            // 
            this.txtOperador.Location = new System.Drawing.Point(5, 99);
            this.txtOperador.Name = "txtOperador";
            this.txtOperador.Size = new System.Drawing.Size(189, 20);
            this.txtOperador.TabIndex = 9;
            this.txtOperador.Text = "Josias";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConfigura);
            this.groupBox1.Controls.Add(this.btnAbre);
            this.groupBox1.Controls.Add(this.btnFecha);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(142, 114);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inicializacao";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.btnConfirmacaoPinPad);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(167, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(255, 113);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Envia Mensagem com Retorno (Sim/Nao)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblQtdParcelas);
            this.groupBox3.Controls.Add(this.trkQtdParcelas);
            this.groupBox3.Controls.Add(this.cmbTipoVenda);
            this.groupBox3.Controls.Add(this.btnVenda);
            this.groupBox3.Controls.Add(this.txtValor);
            this.groupBox3.Controls.Add(this.btnLeCartao);
            this.groupBox3.Controls.Add(this.txtCupomFiscal);
            this.groupBox3.Controls.Add(this.txtOperador);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 132);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(212, 292);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Venda";
            // 
            // lblQtdParcelas
            // 
            this.lblQtdParcelas.AutoSize = true;
            this.lblQtdParcelas.Location = new System.Drawing.Point(4, 140);
            this.lblQtdParcelas.Name = "lblQtdParcelas";
            this.lblQtdParcelas.Size = new System.Drawing.Size(143, 13);
            this.lblQtdParcelas.TabIndex = 12;
            this.lblQtdParcelas.Text = "Quantidade de Parcelas";
            // 
            // trkQtdParcelas
            // 
            this.trkQtdParcelas.Location = new System.Drawing.Point(7, 165);
            this.trkQtdParcelas.Maximum = 48;
            this.trkQtdParcelas.Minimum = 1;
            this.trkQtdParcelas.Name = "trkQtdParcelas";
            this.trkQtdParcelas.Size = new System.Drawing.Size(187, 45);
            this.trkQtdParcelas.TabIndex = 11;
            this.trkQtdParcelas.Value = 1;
            this.trkQtdParcelas.Scroll += new System.EventHandler(this.trkQtdParcelas_Scroll);
            // 
            // cmbTipoVenda
            // 
            this.cmbTipoVenda.FormattingEnabled = true;
            this.cmbTipoVenda.Items.AddRange(new object[] {
            "Debito",
            "Credito"});
            this.cmbTipoVenda.Location = new System.Drawing.Point(7, 20);
            this.cmbTipoVenda.Name = "cmbTipoVenda";
            this.cmbTipoVenda.Size = new System.Drawing.Size(187, 21);
            this.cmbTipoVenda.TabIndex = 0;
            this.cmbTipoVenda.Text = "Debito";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(230, 131);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(192, 124);
            this.txtLog.TabIndex = 13;
            // 
            // txtLogSitef
            // 
            this.txtLogSitef.Location = new System.Drawing.Point(230, 289);
            this.txtLogSitef.Multiline = true;
            this.txtLogSitef.Name = "txtLogSitef";
            this.txtLogSitef.Size = new System.Drawing.Size(192, 143);
            this.txtLogSitef.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(232, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(187, 24);
            this.button1.TabIndex = 15;
            this.button1.Text = "Ler Log Sitef";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmPDVTEF
            // 
            this.ClientSize = new System.Drawing.Size(431, 438);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtLogSitef);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmPDVTEF";
            this.Text = "PDVTEF";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkQtdParcelas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        static void Main() 
        {
            Application.Run(new frmPDVTEF());
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            //int retorno = clisitef.Venda(0, "", "", "", "", "", "");
            //System.Windows.Forms.MessageBox.Show("retorno: [" + retorno.ToString() + "]", "Venda CliSiTef");
        }

        private void btnConfirmacaoPinPad_Click(object sender, System.EventArgs e)
        {

            insereLog("Enviando mensagem para o PinPad e aguardando a resposta");

            Output.WriteLine("%MENSAGEM_CONFIRMACAO% - Texto Enviado: " + textBox1.Text);

            int retorno =  clisitef.LeConfirmacaoPinPad(textBox1.Text);


            //System.Windows.Forms.MessageBox.Show("Retorno Sim/Nao PinPad: [" + retorno.ToString() + "]", "Confirmacao PinPad");
            insereLog("Retorno Sim/Nao PinPad: [" + retorno.ToString() + "]");

            Output.WriteLine("%MENSAGEM_CONFIRMACAO% - Retorno: " + retorno.ToString());

        }

        private void btnConfigura_Click(object sender, System.EventArgs e)
        {
            int retorno = clisitef.Configura("127.0.0.1", "00000000", "SW000001");

            Output.WriteLine("%CONFIGURA_PINPAD% - ('127.0.0.1', '00000000', 'SW000001')");

            if (sender != null)
            {
                //System.Windows.Forms.MessageBox.Show("Retorno Configura: [" + retorno.ToString() + "]", "Configura CliSiTef");
                insereLog("Retorno Configura: [" + retorno.ToString() + "]");
                Output.WriteLine("%CONFIGURA_PINPAD% - " + retorno.ToString());
            }
        }

        private void btnAbre_Click(object sender, System.EventArgs e)
        {
            int retorno = clisitef.AbrirPinPad();

            //System.Windows.Forms.MessageBox.Show("Retorno Abertura PinPad: [" + retorno.ToString() + "]", "Abre PinPad");
            insereLog("Retorno Abertura PinPad: [" + retorno.ToString() + "]");
            Output.WriteLine("%ABRE_PINPAD% - Retorno: " + retorno.ToString());
        }

        private void btnFecha_Click(object sender, System.EventArgs e)
        {
            int retorno = clisitef.FecharPinPad();

            //System.Windows.Forms.MessageBox.Show("Retorno Fechamento PinPad: [" + retorno.ToString() + "]", "Fecha PinPad");
            insereLog("Retorno Fechamento PinPad: [" + retorno.ToString() + "]");

        }

        private void btnLeCartao_Click(object sender, System.EventArgs e)
        {
            string trilha1;
            string trilha2;

            int retorno = clisitef.LeCartao("INSIRA OU PASSE O CARTAO", out trilha1, out trilha2);

            //System.Windows.Forms.MessageBox.Show("Retorno LeCartao: [" + retorno.ToString() + "]", "LeCartao");
            insereLog("Retorno LeCartao: [" + retorno.ToString() + "]");
            Output.WriteLine("%LE_CARTAO% - Retorno: " + retorno.ToString());


            if (retorno != -999)
            {
                //System.Windows.Forms.MessageBox.Show("Trilha1: [" + trilha1.ToString() + "]", "LeCartao");
                //System.Windows.Forms.MessageBox.Show("Trilha2: [" + trilha2.ToString() + "]", "LeCartao");
                insereLog("Trilha1: [" + trilha1.ToString() + "]");
                insereLog("Trilha1: [" + trilha2.ToString() + "]");

                Output.WriteLine("%LE_CARTAO% - Trilha1: [" + trilha1.ToString() + "]");
                Output.WriteLine("%LE_CARTAO% - Trilha2: [" + trilha2.ToString() + "]");

            }
        }

        private void btnVenda_Click(object sender, System.EventArgs e)
        {
            if (!clisitef.Configurado)
            {
                btnConfigura_Click(null, null);
            }



            string valor = txtValor.Text;  //"100,00";
            string cupomFiscal = txtCupomFiscal.Text; //"12345";
            string dataFiscal = "20171203";
            string horario = "165400";
            string operador = "operador";//pdvapi.GetMacAddress(); // txtOperador.Text; //"OPERADOR";
            string restricoes = "";

            //caue = 22/11/2017
            int funcao = 0;
            string qtdeParcelas = "0";
            string valorPrimeiraParcela = "0";
            string valorDemaisParcelas = "0";

            pdvapi = new PDVTEF.PagAPI();
            //System.Windows.Forms.MessageBox.Show("MACADRRES: " + operador);
            insereLog("MACADRRES: " + operador);

            //Caue - 22/11/2017
            // Verifica o tipo de venda a ser realizado
            if (cmbTipoVenda.SelectedItem.ToString() == "Debito")
            {
                funcao = 2; //Debito
            }
            else
            {
                funcao = 3; //Credito
                qtdeParcelas = trkQtdParcelas.Value.ToString();
                valorPrimeiraParcela = "2.00";//txtValorPrimeiraParcela.Text;
                valorDemaisParcelas = "2,00"; //txtValorDemaisParcelas.Text;              
            }

            //int retorno = clisitef.Venda(0, valor, cupomFiscal, dataFiscal, horario, operador, restricoes);
            int retorno = clisitef.Venda(funcao, valor, cupomFiscal, dataFiscal, horario, operador, restricoes, qtdeParcelas, valorPrimeiraParcela, valorDemaisParcelas);

            //System.Windows.Forms.MessageBox.Show("retorno: [" + retorno.ToString() + "]", "Venda CliSiTef");

            insereLog("retorno: [" + retorno.ToString() + "]");

            Output.WriteLine("%VENDA% - Retorno: " + retorno.ToString());

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //GetAllVendas();


        }

        public async void GetAllVendas()
        {
            pdvapi = new PDVTEF.PagAPI();
            string URI = "http://pagapi.azurewebsites.net/API/getStatusVendas";
            GetAtualizaPDV(pdvapi.GetMacAddress());
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(URI))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {


                            var VendasJsonString = await response.Content.ReadAsStringAsync();
                            var lstvendas = JsonConvert.DeserializeObject<PDVTEF.Entity.Vendas[]>(VendasJsonString).ToList();
                            foreach (var itens in lstvendas)
                            {
                                int ID = itens.ID;
                                string valor = Convert.ToString(itens.VALOR);  //"100,00";
                                string cupomFiscal = "1234"; // Convert.ToString(itens.VALOR); //"12345";
                                string dataFiscal = "20171119";
                                string horario = "002400";
                                string operador = pdvapi.GetMacAddress(); // txtOperador.Text; //"OPERADOR";
                                string restricoes = "";
                                string Status = itens.STATUS;

                                //Caue - 22/11/2017
                                string qtdeParcelas = "0";
                                string valorPrimeiraParcela = "0";
                                string valorDemaisParcelas = "0";
                                int funcao = 0;

                                if (Status == "Aguardando Pagto PDV")
                                {
                                    timer1.Enabled = false;
                                    //FuncVenda(valor, cupomFiscal, dataFiscal, horario, restricoes);
                                    FuncVenda(valor, cupomFiscal, dataFiscal, horario, restricoes, qtdeParcelas, valorPrimeiraParcela, valorDemaisParcelas, funcao);
                                    GetAtualizaVendas(ID);
                                }

                            }

                        }
                        catch
                        {
                            insereLog("ERRO - GetAllVendas");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível obter o vendas : " + response.StatusCode);
                    }
                }
            }



        }

        public async void GetAtualizaVendas(int id)
        {
            string URI = "http://pagapi.azurewebsites.net/API/getStatusVendas/" + id;
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(URI))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        //var VendasJsonString = await response.Content.ReadAsStringAsync();
                        //var lstvendas = JsonConvert.DeserializeObject<PDVTEF.Entity.Vendas[]>(VendasJsonString).ToList();
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível obter o vendas : " + response.StatusCode);
                    }
                }
            }
            timer1.Enabled = true;
        }

        public async void GetAtualizaPDV(string macadrres)
        {
            try
            {


                string URI = "http://pagapi.azurewebsites.net/api/getPDV/" + macadrres;
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(URI))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            //var VendasJsonString = await response.Content.ReadAsStringAsync();
                            //var lstvendas = JsonConvert.DeserializeObject<PDVTEF.Entity.Vendas[]>(VendasJsonString).ToList();
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível obter o vendas : " + response.StatusCode);
                        }
                    }
                }
            }
            catch
            {
                insereLog("ERRO - GetAtualizaPDV");
            }
        }

        //private void FuncVenda(string valor, string cupomFiscal, string dataFiscal, string horario, string restricoes)
        //{
           
        //    if (!clisitef.Configurado)
        //    {
        //        btnConfigura_Click(null, null);
        //    }

        //    pdvapi = new PDVTEF.PagAPI();
        //    string operador = pdvapi.GetMacAddress(); // txtOperador.Text; //"OPERADOR";
        //    //int retorno = clisitef.Venda(0, valor, cupomFiscal, dataFiscal, horario, operador, restricoes);
        //    int retorno = clisitef.Venda(funcao, valor, cupomFiscal, dataFiscal, horario, operador, restricoes, qtdeParcelas, valorPrimeiraParcela, valorDemaisParcelas);
        //}

        private void FuncVenda(string valor, string cupomFiscal, string dataFiscal, string horario, string restricoes, string qtdeParcelas, string valorPrimeiraParcela, string valorDemaisParcelas, int funcao)
        {

            if (!clisitef.Configurado)
            {
                btnConfigura_Click(null, null);
            }

            pdvapi = new PDVTEF.PagAPI();
            string operador = pdvapi.GetMacAddress(); // txtOperador.Text; //"OPERADOR";
            int retorno = clisitef.Venda(funcao, valor, cupomFiscal, dataFiscal, horario, operador, restricoes, qtdeParcelas, valorPrimeiraParcela, valorDemaisParcelas);

        }

        public void insereLog(string mensagem)
        {
            txtLog.AppendText(mensagem + "\n");
        }

        private void trkQtdParcelas_Scroll(object sender, EventArgs e)
        {
            lblQtdParcelas.Text = "Quantidade de Parcelas: " + trkQtdParcelas.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtLogSitef.Text = "";

            foreach (string strLog in clisitef.logCliSitefAPI)
            {
                txtLogSitef.AppendText(strLog + "/n");
            } 

        }



    }
}
