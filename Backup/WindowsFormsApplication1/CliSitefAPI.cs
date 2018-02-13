using System;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;

namespace TesteMensagemPinPad
{
    public class CliSitefAPI
    {
        private static int nVezes = 0;
        private static int _tamanhoRecebido = 0;
        private static bool _configurado = false;
        private static byte[] _recebido = new byte[20000];

        public bool Configurado
        {
            get {return _configurado;}
        }

        public int TamanhoRecebido
        {
            get {return _tamanhoRecebido;}
        }

        public byte[] Recebido
        {
            get {return _recebido;}
        }

        public int Configura(string endereco, string loja, string terminal)
        {	
            byte[] _endereco	= Encoding.ASCII.GetBytes(endereco + "\0");
            byte[] _loja		= Encoding.ASCII.GetBytes(loja + "\0");
            byte[] _terminal	= Encoding.ASCII.GetBytes(terminal + "\0");

            try
            {
                int result = CliSitefAPI.ConfiguraIntSiTefInterativo(_endereco, _loja, _terminal, 0);				

                _configurado = (result == 0);

                return result;
            }
            catch(System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Erro");
            }

            return -999;			
        }

        private int RotinaResultado(int tipoCampo, byte[] buffer)
        {
            string mensagem = Encoding.UTF8.GetString(buffer);

            mensagem = mensagem.Substring(0, mensagem.IndexOf('\x0'));

            switch (tipoCampo)
            {
                case 1:
                    System.Windows.Forms.MessageBox.Show("Finalizacao: [" + mensagem.ToString() + "]", "RotinaResultado");
                    break;

                case 121:
                    System.Windows.Forms.MessageBox.Show("Comprovante Cliente: \n" + mensagem.ToString(), "RotinaResultado");
                    break;

                case 122:
                    System.Windows.Forms.MessageBox.Show("Comprovante Estabelecimento: \n" + mensagem.ToString(), "RotinaResultado");
                    break;

                case 131:
                    System.Windows.Forms.MessageBox.Show("Rede Destino: [" + mensagem.ToString() + "]", "RotinaResultado");
                    break;

                case 132:
                    System.Windows.Forms.MessageBox.Show("Tipo Cartao: [" + mensagem.ToString() + "]", "RotinaResultado");
                    break;

                default:
                    System.Windows.Forms.MessageBox.Show("nTipoCampo: [" + tipoCampo.ToString() + "]\nConteudo: [" + mensagem.ToString() + "]", "RotinaResultado");
                    break;
            }

            return 0;
        }

        private int TrataMenu(byte[] pOpcoes, byte[] pEscolha)
        {
            return 0;
        }

        private int LeCampo(short tamanhoMinimo, short tamanhoMaximo, byte[] pMensagem, byte[] pCampo)
        {
            return 0;
        }

        private int RotinaColeta(int comando, int tipoCampo, ref short pTamanhoMinimo, ref short pTamanhoMaximo, byte[] pDadosComando, byte[] pCampo)
        {
            char c;
            string mensagem = Encoding.UTF8.GetString(pDadosComando);
            
            mensagem = mensagem.Substring(0, mensagem.IndexOf('\x0'));
           
            if (comando != 23)
            {
                nVezes = 0;
            }

            switch (comando)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    System.Windows.Forms.MessageBox.Show("Mensagem Visor: [" + mensagem.ToString() + "]", "RotinaColeta");
                    return 0;

                case 11:
                case 12:
                case 13:
                case 14:
                    System.Windows.Forms.MessageBox.Show("Apaga Visor: [" + comando.ToString() + "]", "RotinaColeta");                    
                    return 0;

                case 37:
                    System.Windows.Forms.MessageBox.Show("Coleta confirmacao no PinPad: [" + mensagem.ToString() + "]", "RotinaColeta", System.Windows.Forms.MessageBoxButtons.YesNo);
                    return 0;
                
                case 20:
                    System.Windows.Forms.MessageBox.Show("Coleta Sim/Nao: [" + mensagem.ToString() + "]", "RotinaColeta", System.Windows.Forms.MessageBoxButtons.YesNo);
                    return 0;

                case 21:
                    System.Windows.Forms.MessageBox.Show("Menu: [" + mensagem.ToString() + "]", "RotinaColeta");
                    return this.TrataMenu(pDadosComando, pCampo);

                case 22:
                    System.Windows.Forms.MessageBox.Show("Obtem qualquer tecla: [" + mensagem.ToString() + "]", "RotinaColeta");                    
                    return 0;

                case 23:
                    System.Threading.Thread.Sleep(1000);

                    if (nVezes++ > 30)
                    {
                        return -1;
                    }

                    return 0;

                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 38:
                    System.Windows.Forms.MessageBox.Show("nComando: [" + comando.ToString() + "]\nTipoCampo: [" + tipoCampo.ToString() + "]", "RotinaColeta");
                    return LeCampo(pTamanhoMinimo, pTamanhoMaximo, pDadosComando, pCampo);
            }

            return -1;
        }


        public int Venda(int funcao, string valor, string cupomFiscal, string dataFiscal, string horario, string operador, string restricoes)
        {
            int comando = 0;
            int continua = 0;
            int tipoCampo = 0;
            short tamanhoMinimo = 0;
            short tamanhoMaximo = 0;

            byte[] _valor = Encoding.ASCII.GetBytes(valor + "\0");
            byte[] _cupomFiscal = Encoding.ASCII.GetBytes(cupomFiscal + "\0");
            byte[] _dataFiscal = Encoding.ASCII.GetBytes(dataFiscal + "\0");
            byte[] _horario = Encoding.ASCII.GetBytes(horario + "\0");
            byte[] _operador = Encoding.ASCII.GetBytes(operador + "\0");
            byte[] _restricoes = Encoding.ASCII.GetBytes(restricoes + "\0");

            byte[] buffer = new byte[20000];

            int retorno = CliSitefAPI.IniciaFuncaoSiTefInterativo(funcao, _valor, _cupomFiscal, _dataFiscal, _horario, _operador, _restricoes);
            
            while (retorno == 10000)
            {
                retorno = CliSitefAPI.ContinuaFuncaoSiTefInterativo(ref comando, ref tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer.Length, 0);

                if (comando == 0)
                {
                    continua = this.RotinaResultado(tipoCampo, buffer);
                }
                else
                {
                    continua = this.RotinaColeta(comando, tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer);
                }
            }

            return retorno;
        }

        public int LeConfirmacaoPinPad(string mensagem)
        {	
            try
            {
                byte[] _pcampo = new byte[2000];
                byte[] _mensagem	= Encoding.ASCII.GetBytes(mensagem + "\0");

                int retorno = CliSitefAPI.LeSimNaoPinPad(_mensagem);

                return retorno;
            }
            catch(System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Erro");
            }

            return -999;
        }

        public int AbrirPinPad()
        {
            try
            {
                int retorno = CliSitefAPI.AbrePinPad();

                return retorno;
            }
            catch(System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Erro");
            }

            return -999;
        }

        public int FecharPinPad()
        {
            try
            {
                int retorno = CliSitefAPI.FechaPinPad();

                return retorno;
            }
            catch(System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Erro");
            }

            return -999;
        }

        public int LeCartao(string mensagem, out string trilha1, out string trilha2)
        {		
            try
            {				
                byte[] _mensagem = Encoding.ASCII.GetBytes(mensagem + "\0");
                byte[] _trilha1 = new byte[2000];
                byte[] _trilha2 = new byte[2000];

                CliSitefAPI.LeCartaoDireto(_mensagem, _trilha1, _trilha2);

                trilha1 = System.Text.Encoding.UTF8.GetString(_trilha1);
                trilha1 = trilha1.Substring(0, trilha1.IndexOf('\x0'));

                trilha2 = System.Text.Encoding.UTF8.GetString(_trilha2);
                trilha2 = trilha2.Substring(0, trilha2.IndexOf('\x0'));

                return 0;
            }
            catch(System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Erro");
            }

            trilha1 = null;
            trilha2 = null;

            return -999;
        }

        #region CLISITEF32I imports		

        [DllImport("CliSiTef32I.dll", EntryPoint="ConfiguraIntSiTefInterativo", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int ConfiguraIntSiTefInterativo(byte[] pEnderecoIP, byte[] pCodigoLoja, byte[] pNumeroTerminal, short ConfiguraResultado);

        [DllImport("CliSitef32I.dll", EntryPoint = "IniciaFuncaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int IniciaFuncaoSiTefInterativo(int Funcao, byte[] pValor, byte[] pCupomFiscal, byte[] pDataFiscal, byte[] pHorario, byte[] pOperador, byte[] pRestricoes);

        [DllImport("CliSitef32I.dll", EntryPoint = "ContinuaFuncaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int ContinuaFuncaoSiTefInterativo(ref int pComando, ref int pTipoCampo, ref short pTamMinimo, ref short pTamMaximo, byte[] pBuffer, int TamBuffer, int Continua);

        [DllImport("CliSiTef32I.dll", EntryPoint="EnviaRecebeSiTefDireto", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int EnviaRecebeSiTefDireto(short RedeDestino, short FuncaoSiTef, short OffsetCartao, byte[] pDadosTx, short TamDadosTx, byte[] pDadosRx, short TamMaxDadosRx, short[] pCodigoResposta, short TempoEsperaRx, byte []pNumeroCupon, byte[] pDataFiscal, byte[] pHorario, byte[] pOperador, short TipoTransacao);

        [DllImport("CliSiTef32I.dll", EntryPoint="LeSimNaoPinPad", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int LeSimNaoPinPad(byte[] pMsgDisplay);

        [DllImport("CliSiTef32I.dll", EntryPoint="AbrePinPad", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int AbrePinPad();

        [DllImport("CliSiTef32I.dll", EntryPoint="FechaPinPad", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int FechaPinPad();

        [DllImport("CliSiTef32I.dll", EntryPoint="LeCartaoDireto", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int LeCartaoDireto(byte[] pMsgDisplay, byte[] trilha1, byte[] trilha2);

        #endregion
    }
}
