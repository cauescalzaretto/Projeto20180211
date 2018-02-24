using System;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;

namespace TesteMensagemPinPad
{
    public class CliSitefAPI
    {
        private static int nVezes = 0;
        private static int _tamanhoRecebido = 0;
        private static bool _configurado = false;
        private static byte[] _recebido = new byte[20000];
        public ArrayList logCliSitefAPI = new ArrayList();
        public string sCupomFiscal = "";

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

            // Caue 05/12/2017
            //mensagem = mensagem.Substring(0, mensagem.IndexOf('\x0'));

            switch (tipoCampo)
            {
                case 1:
                    //System.Windows.Forms.MessageBox.Show("Finalizacao: [" + mensagem.ToString() + "]", "RotinaResultado");
                    break;

                case 121:
                    //System.Windows.Forms.MessageBox.Show("Comprovante Cliente: \n" + mensagem.ToString(), "RotinaResultado");
                    break;

                case 122:
                    //System.Windows.Forms.MessageBox.Show("Comprovante Estabelecimento: \n" + mensagem.ToString(), "RotinaResultado");
                    break;

                case 131:
                    //System.Windows.Forms.MessageBox.Show("Rede Destino: [" + mensagem.ToString() + "]", "RotinaResultado");
                    break;

                case 132:
                    //System.Windows.Forms.MessageBox.Show("Tipo Cartao: [" + mensagem.ToString() + "]", "RotinaResultado");
                    break;

                default:
                    //System.Windows.Forms.MessageBox.Show("nTipoCampo: [" + tipoCampo.ToString() + "]\nConteudo: [" + mensagem.ToString() + "]", "RotinaResultado");
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

            string msgLog = "   # Comando: " + comando ;

            int retorno = -1;
            
            //mensagem = mensagem.Substring(0, mensagem.IndexOf('\x0'));
           
            if (comando != 23)
            {
                nVezes = 0;
            }

            switch (comando)
            {

                case 0:
                    msgLog = msgLog + " - Est� devolvendo um valor para, se desejado, ser armazenado pela automacao";
                    break;

                case 1:
                    msgLog = msgLog + " - Mensagem para o visor do operador";
                    break;

                case 2:
                    msgLog = msgLog + " - Mensagem para o visor do cliente";
                    break;

                case 3:
                    msgLog = msgLog + " - Mensagem para os dois visores";
                    break;

                case 4:
                    msgLog = msgLog + " - Texto que dever� ser utilizado como t�tulo na apresentacao do menu ( vide comando 21)";
                    //System.Windows.Forms.MessageBox.Show("Mensagem Visor: [" + mensagem.ToString() + "]", "RotinaColeta");
                    retorno =  0;
                    break;

                case 11:
                    msgLog = msgLog + " - Deve remover a mensagem apresentada no visor do operador (comando 1)";
                    break;

                case 12:
                    msgLog = msgLog + " - Deve remover a mensagem apresentada no visor do cliente (comando 2)";
                    break;

                case 13:
                    msgLog = msgLog + " - Deve remover mensagem apresentada no visor do operador e do cliente (comando 3)";
                    break;

                case 14:
                    msgLog = msgLog + " - Deve limpar o texto utilizado como t�tulo na apresentacao do menu (comando 4)";
                    //System.Windows.Forms.MessageBox.Show("Apaga Visor: [" + comando.ToString() + "]", "RotinaColeta");                    
                    retorno = 0;
                    break;

                case 15:
                    msgLog = msgLog + " - Cabe�alho a ser apresentado pela aplicacao. Refere-se a exibicao de informa��es adicionais que algumas transa��es necessitam mostrar na tela.";
                    break;

                case 16:
                    msgLog = msgLog + " - Deve remover o cabe�alho apresentado pelo comando 15.";
                    break;

                case 37:
                    //System.Windows.Forms.MessageBox.Show("Coleta confirmacao no PinPad: [" + mensagem.ToString() + "]", "RotinaColeta", System.Windows.Forms.MessageBoxButtons.YesNo);
                    retorno =  0;
                    break;

                case 20:
                    msgLog = msgLog + " - Deve apresentar o texto em Buffer, e obter uma resposta do tipo SIM/Nao.";
                    //System.Windows.Forms.MessageBox.Show("Coleta Sim/Nao: [" + mensagem.ToString() + "]", "RotinaColeta", System.Windows.Forms.MessageBoxButtons.YesNo);
                    retorno = 0;
                    break;

                case 21:
                    msgLog = msgLog + " - Deve apresentar um menu de op��es e permitir que o usu�rio selecione uma delas.";
                   // System.Windows.Forms.MessageBox.Show("Menu: [" + mensagem.ToString() + "]", "RotinaColeta");
                    retorno =  this.TrataMenu(pDadosComando, pCampo);
                    break;

                case 22:
                    msgLog = msgLog + " - Deve apresentar a mensagem em Buffer, e aguardar uma tecla do operador. � utilizada quando se deseja que o operador seja avisado de alguma mensagem apresentada na tela.";
                    //System.Windows.Forms.MessageBox.Show("Obtem qualquer tecla: [" + mensagem.ToString() + "]", "RotinaColeta");                    
                    retorno =  0;
                    break;

                case 23:
                    msgLog = msgLog + " - Este comando indica que a rotina est� perguntando para a aplicacao se ele deseja interromper o processo de coleta de dados ou nao.";
                    System.Threading.Thread.Sleep(1000);

                    if (nVezes++ > 30)
                    {
                        retorno =  -1;
                    }
                    else
                    {
                        retorno =  0;
                    }
                    break;
                    
                case 29:
                    msgLog = msgLog + " - An�logo ao comando 30, por�m deve ser coletado um campo que nao requer intervencao do operador de caixa, ou seja, nao precisa que seja digitado/mostrado na tela, e sim passado diretamente para a biblioteca pela automacao.";
                    break;

                case 30:
                    msgLog = msgLog + " - Deve ser lido um campo cujo tamanho est� entre TamMinimo e TamMaximo. O campo lido deve ser devolvido em Buffer.";
                    break;

                case 31:
                    msgLog = msgLog + " - Deve ser lido o n�mero de um cheque. A coleta pode ser feita via leitura de CMC-7, digitacao do CMC-7 ou pela digitacao da primeira linha do cheque.";
                    break;

                case 32:
                case 33:
                case 34:
                    msgLog = msgLog + " - Deve ser lido um campo monet�rio ou seja, aceita o delimitador de centavos e devolvido no par�metro Buffer.";
                    break;

                case 35:
                    msgLog = msgLog + " - Deve ser lido um c�digo em barras ou o mesmo deve ser coletado manualmente";
                    break;

                case 38:
                    //System.Windows.Forms.MessageBox.Show("nComando: [" + comando.ToString() + "]\nTipoCampo: [" + tipoCampo.ToString() + "]", "RotinaColeta");
                    retorno =  LeCampo(pTamanhoMinimo, pTamanhoMaximo, pDadosComando, pCampo);
                    break;

                case 41:
                    msgLog = msgLog + " - An�logo ao Comando 30, por�m o campo deve ser coletado de forma mascarada";
                    break;

                case 42:
                    msgLog = msgLog + " - Menu identificado. Deve apresentar um menu de op��es e permitir que o usu�rio selecione uma delas.";
                    break;
            }

            //LOG
            Output.WriteLine(msgLog);
            
            return retorno;
        }

        //public int Venda(int funcao, string valor, string cupomFiscal, string dataFiscal, string horario, string operador, string restricoes)
        //{
        //    int comando = 0;
        //    int continua = 0;
        //    int tipoCampo = 0;
        //    short tamanhoMinimo = 0;
        //    short tamanhoMaximo = 0;

        //    byte[] _valor = Encoding.ASCII.GetBytes(valor + "\0");
        //    byte[] _cupomFiscal = Encoding.ASCII.GetBytes(cupomFiscal + "\0");
        //    byte[] _dataFiscal = Encoding.ASCII.GetBytes(dataFiscal + "\0");
        //    byte[] _horario = Encoding.ASCII.GetBytes(horario + "\0");
        //    byte[] _operador = Encoding.ASCII.GetBytes(operador + "\0");
        //    byte[] _restricoes = Encoding.ASCII.GetBytes(restricoes + "\0");

        //    byte[] buffer = new byte[20000];

        //    int retorno = CliSitefAPI.IniciaFuncaoSiTefInterativo(funcao, _valor, _cupomFiscal, _dataFiscal, _horario, _operador, _restricoes);
            
        //    while (retorno == 10000)
        //    {
        //        retorno = CliSitefAPI.ContinuaFuncaoSiTefInterativo(ref comando, ref tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer.Length, 0);

        //        if (comando == 0)
        //        {
        //            continua = this.RotinaResultado(tipoCampo, buffer);
        //        }
        //        else
        //        {
        //            continua = this.RotinaColeta(comando, tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer);
        //        }
        //    }

        //    return retorno;
        //}

        public int Venda(int funcao, string valor, string cupomFiscal, string dataFiscal, string horario, string operador, string restricoes, string qtdeParcelas, string valorPrimeiraParcela, string valorDemaisParcelas, string codigoSeguranca)
        {
            int comando = 0;
            int continua = 0;
            int tipoCampo = 0;
            short tamanhoMinimo = 0;
            short tamanhoMaximo = 32767;
            bool eDebito = true;

            //Caue - 04/12/2017
            int myComando = 0;

            if (funcao == 3)
            {
                //Teste credito parcelado
                restricoes = "24;26;";
            }

            byte[] _valor = Encoding.ASCII.GetBytes(valor + "\0");
            byte[] _cupomFiscal = Encoding.ASCII.GetBytes(cupomFiscal + "\0");
            byte[] _dataFiscal = Encoding.ASCII.GetBytes(dataFiscal + "\0");
            byte[] _horario = Encoding.ASCII.GetBytes(horario + "\0");
            byte[] _operador = Encoding.ASCII.GetBytes(operador + "\0");
            byte[] _restricoes = Encoding.ASCII.GetBytes(restricoes + "\0");

            //Verifica se e debito ou credito
            if (qtdeParcelas != "0" || funcao == 3)
            {
                eDebito = false;
            }


            byte[] buffer = new byte[20000];

            // Limpa a variavel cupomFiscal
            //sCupomFiscal = "";


            //LOG
            Output.WriteLine("%IniciaFuncaoSiTefInterativo% - Funcao: " + funcao.ToString() + " valor:" + valor + " cupomFiscal:" + cupomFiscal + " dataFiscal: " + dataFiscal + " horario: " + horario + " operador: " + operador + " restricoes: " + restricoes);
                 
            int retorno = CliSitefAPI.IniciaFuncaoSiTefInterativo(funcao, _valor, _cupomFiscal, _dataFiscal, _horario, _operador, _restricoes);

            while (retorno == 10000)
            {
                retorno = CliSitefAPI.ContinuaFuncaoSiTefInterativo(ref comando, ref tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer.Length, 0);
                //LOG
                Output.WriteLine("%ContinuaFuncaoSiTefInterativo% - comando:" + comando + " tipoCampo: " + tipoCampo + " tamanhoMinimo: " + tamanhoMinimo + " tamanhoMaximo: " + tamanhoMaximo + " buffer: " + buffer + " buffer(string): " + Encoding.UTF8.GetString(buffer));


                if (buffer.Length > 2 && tipoCampo != -1)
                {
                    Debug.Print("oi");
                }


                //// Credito
                //if (comando == 21 & eDebito == false)
                //{
                //    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                //}

                //if (comando == 14 & eDebito == false)
                //{
                //    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                //}
                

                if (tipoCampo == 514)
                {
                    buffer = retornoSolicitacaotipoCampo(tipoCampo, buffer, codigoSeguranca);
                }
                else
                { 
                    //Caue 06/12/2017
                    buffer = retornoSolicitacaotipoCampo(tipoCampo, buffer,"");
                 }
                if (comando == 0)
                {
                    continua = this.RotinaResultado(tipoCampo, buffer);
                }
                else
                {
                    continua = this.RotinaColeta(comando, tipoCampo, ref tamanhoMinimo, ref tamanhoMaximo, buffer, buffer);
                }

            }

            Debug.Print(sCupomFiscal);

            if (retorno==0)
            {
                int retornoFinal = FinalizaFuncaoSiTefInterativo(1, _cupomFiscal, _dataFiscal, _horario, _restricoes);
                Debug.Print(retornoFinal.ToString());
            }

            //FinalizaFuncaoSiTefInterativo
            //- N�mero Identificador do Cupom do Pagamento -> 0\0 - Cupom Fiscal -> Ca - Cupom Fiscal -> T. - Comprovante de Pagamento (via do cliente) -> .. - Comprovante de Pagamento (via do caixa) -> ..

            return retorno;
        }

        private byte[] retornoSolicitacaotipoCampo(int tipoCampo, byte[] buffer, string Adicional)
        {
            string msgLog = "   # TipoCampo:" + tipoCampo;
            

            switch (tipoCampo)
            {   
                case -1:
                    //Nao existem informa��es que podem/devem ser tratadas pela automacao
                    msgLog = msgLog + " - Nao existem informa��es que podem/devem ser tratadas pela automacao";
                    break;

                case 0:
                    //A rotina est� sendo chamada para indicar que acabou de coletar os dados da transacao e ir� iniciar a interacao com o SiTef para obter a autorizacao
                    msgLog = msgLog + " - A rotina est� sendo chamada para indicar que acabou de coletar os dados da transacao e ir� iniciar a interacao com o SiTef para obter a autorizacao";
                    break;

                case 1:
                    //Dados de confirmacao da transacao.
                    msgLog = msgLog + " Dados de confirmacao da transacao.";
                    break;

                case 2:
                    // Informa o c�digo da funcao SiTef utilizado na mensagem enviada para o servidor
                    msgLog = msgLog + " - Informa o c�digo da funcao SiTef utilizado na mensagem enviada para o servidor";
                    break;

                case 15:
                    msgLog = msgLog + "!!!!!!!!! Desconhecido [15]: " + Encoding.UTF8.GetString(buffer);
                    break;

                case 43:
                    msgLog = msgLog + "!!!!!!!!! Desconhecido [43]: " + Encoding.UTF8.GetString(buffer);
                    break;

                case 100:
                    //Modalidade de Pagamento (xxnn)
                    //xx - (00-cheque,01-debito,02-credito,03-voucher,05-fidelidade,98-dinheiro,99-outro)
                    //nn - (00-avista,01-predatado,02-parcelado_estabelecimento,03-parcelado_administradora,99-outro)
                    msgLog = msgLog + " - Modalidade de Pagamento - " + Encoding.UTF8.GetString(buffer);
                    //buffer = Encoding.ASCII.GetBytes("0203" + "\0");
                    break;

                case 101:
                    //Cont�m o texto real da modalidade de pagamento que pode ser memorizado pela aplicacao caso exista essa necessidade. Descreve por extenso o par xxnn fornecido em 100
                    msgLog = msgLog + " - Cont�m o texto real da modalidade de pagamento que pode ser memorizado pela aplicacao caso exista essa necessidade";
                    sCupomFiscal = sCupomFiscal + " - Cupom Fiscal -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 102:
                    //Cont�m o texto descritivo da modalidade de pagamento que deve ser impresso no cupom fiscal (p/ex: T.E.F., Cheque, etc...)
                    msgLog = msgLog + " - Cont�m o texto descritivo da modalidade de pagamento que deve ser impresso no cupom fiscal (p/ex: T.E.F., Cheque, etc...)" + Encoding.UTF8.GetString(buffer);
                    sCupomFiscal = sCupomFiscal + " - Cupom Fiscal -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 105:
                    //Cont�m a data e hora da transacao no formato AAAAMMDDHHMMSS
                    msgLog = msgLog + " - Cont�m a data e hora da transacao no formato AAAAMMDDHHMMSS";
                    break;

                case 110:
                    // Retorna quando uma transacao for cancelada. Cont�m a modalidade de cancelamento no formato xxnn, seguindo o mesmo formato xxnn do TipoCampo 100. O sub-grupo nn todavia, cont�m o valor default 00 por nao ser coletado.
                    msgLog = msgLog + " - Retorna quando uma transacao for cancelada. Cont�m a modalidade de cancelamento no formato xxnn, seguindo o mesmo formato xxnn do TipoCampo 100. O sub-grupo nn todavia, cont�m o valor default 00 por nao ser coletado.";
                    break;

                case 111:
                    // Cont�m o texto real da modalidade de cancelamento que pode ser memorizado pela aplicacao caso exista essa necessidade. Descreve por extenso o par xxnn fornecido em 110.
                    msgLog = msgLog + " - Cont�m o texto real da modalidade de cancelamento que pode ser memorizado pela aplicacao caso exista essa necessidade. Descreve por extenso o par xxnn fornecido em 110.";
                    break;

                case 112:
                    //Cont�m o texto descritivo da modalidade de cancelamento que deve ser impresso no cupom fiscal (p/ex: T.E.F., Cheque, etc...).
                    msgLog = msgLog + " - Cont�m o texto descritivo da modalidade de cancelamento que deve ser impresso no cupom fiscal (p/ex: T.E.F., Cheque, etc...).";
                    sCupomFiscal = sCupomFiscal + " - Cupom Fiscal Cancelado -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 115:
                    //Modalidade Ajuste
                    msgLog = msgLog + " - Modalidade Ajuste";
                    break;

                case 120:
                    //Buffer cont�m a linha de autenticacao do cheque para ser impresso no verso do mesmo
                    msgLog = msgLog + " - Buffer cont�m a linha de autenticacao do cheque para ser impresso no verso do mesmo";
                    break;

                case 121:
                    //Buffer cont�m a primeira via do comprovante de pagamento (via do cliente) a ser impressa na impressora fiscal. Essa via, quando poss�vel, � reduzida de forma a ocupar poucas linhas na impressora. Pode ser um comprovante de venda ou administrativo
                    msgLog = msgLog + " - Buffer cont�m a primeira via do comprovante de pagamento (via do cliente) a ser impressa na impressora fiscal. Essa via, quando poss�vel, � reduzida de forma a ocupar poucas linhas na impressora. Pode ser um comprovante de venda ou administrativo";
                    sCupomFiscal = sCupomFiscal + " - Comprovante de Pagamento (via do cliente) -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 122:
                    //Buffer cont�m a segunda via do comprovante de pagamento (via do caixa) a ser impresso na impressora fiscal. Pode ser um comprovante de venda ou administrativo
                    msgLog = msgLog + " - Buffer cont�m a segunda via do comprovante de pagamento (via do caixa) a ser impresso na impressora fiscal. Pode ser um comprovante de venda ou administrativo";
                    sCupomFiscal = sCupomFiscal + " - Comprovante de Pagamento (via do caixa) -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 123:
                    //Indica que os comprovantes que serao entregues na seq��ncia sao de determinado tipo:
                    msgLog = msgLog + " - Indica que os comprovantes que serao entregues na seq��ncia sao de determinado tipo:";
                    break;

                case 125:
                    //C�digo do Voucher
                    msgLog = msgLog + " - C�digo do Voucher";
                    break;

                case 130:
                    //Indica, na coleta, que o campo em questao � o valor do troco em dinheiro a ser devolvido para o cliente. Na devolucao de resultado (Comando = 0) cont�m o valor efetivamente aprovado para o troco
                    msgLog = msgLog + " - Indica, na coleta, que o campo em questao � o valor do troco em dinheiro a ser devolvido para o cliente. Na devolucao de resultado (Comando = 0) cont�m o valor efetivamente aprovado para o troco";
                    break;

                case 131:
                    //Cont�m um �ndice que indica qual a instituicao que ir� processar a transacao
                    msgLog = msgLog + " - Cont�m um �ndice que indica qual a instituicao que ir� processar a transacao";
                    //buffer = Encoding.ASCII.GetBytes("00002" + "\0");//Itau
                    break;

                case 132:
                    //Cont�m um �ndice que indica qual o tipo do cartao quando esse tipo for identific�vel
                    msgLog = msgLog + " - Cont�m um �ndice que indica qual o tipo do cartao quando esse tipo for identific�vel";
                    //buffer = Encoding.ASCII.GetBytes("00002" + "\0");//MasterCard
                    break;

                case 133:
                    //Cont�m o NSU do SiTef (6 posi��es)
                    msgLog = msgLog + " - Cont�m o NSU do SiTef (6 posix)";
                    break;

                case 134:
                    //Cont�m o NSU do Host autorizador (20 posi��es no m�ximo)
                    msgLog = msgLog + " - Cont�m o NSU do Host autorizador (20 posi��es no m�ximo)";
                    break;

                case 135:
                    //Cont�m o C�digo de Autorizacao para as transa��es de cr�dito (15 posi��es no m�ximo)
                    msgLog = msgLog + " - Cont�m o C�digo de Autorizacao para as transa��es de cr�dito (15 posi��es no m�ximo)";
                    break;

                case 136:
                    //Cont�m as 6 primeiras posi��es do cartao (bin)
                    msgLog = msgLog + " - Cont�m as 6 primeiras posi��es do cartao (bin)";
                    //buffer = Encoding.ASCII.GetBytes("529205" + "\0");
                    break;

                case 137:
                    //Saldo a pagar
                    msgLog = msgLog + " - Saldo a pagar";
                    break;
                
                case 138:
                    //Valor Total Recebido
                    msgLog = msgLog + " - Valor Total Recebido";
                    break;

                case 139:
                    //Valor da Entrada
                    msgLog = msgLog + " - Valor da Entrada";
                    break;

                case 140:
                    //Data da primeira parcela no formato ddmmaaaa
                    msgLog = msgLog + " - Data da primeira parcela no formato ddmmaaaa" + Encoding.UTF8.GetString(buffer);
                    break;

                case 143:
                    //Valor gorjeta
                    msgLog = msgLog + " - Valor gorjeta";
                    break;

                case 144:
                    //Valor devolucao
                    msgLog = msgLog + " - Valor devolucao";
                    break;

                case 145:
                    //Valor de pagamento
                    msgLog = msgLog + " - Valor de pagamento";
                    break;

                case 146:
                    //A rotina est� sendo chamada para ler o Valor a ser cancelado.
                    msgLog = msgLog + " - A rotina est� sendo chamada para ler o Valor a ser cancelado.";
                    break;

                case 147:
                    //Valor a ser cancelado
                    msgLog = msgLog + " - Valor a ser cancelado";
                    break;

                case 150:
                    //Cont�m a Trilha 1, quando dispon�vel, obtida na funcao LeCartaoInterativo
                    msgLog = msgLog + " - Cont�m a Trilha 1, quando dispon�vel, obtida na funcao LeCartaoInterativo";
                    break;

                case 151:
                    //Cont�m a Trilha 2, quando dispon�vel, obtida na funcao LeCartaoInterativo
                    msgLog = msgLog + " - Cont�m a Trilha 2, quando dispon�vel, obtida na funcao LeCartaoInterativo";
                    break;

                case 153:
                    //Contem a senha do cliente capturada atrav�s da rotina LeSenhaInterativo e que deve ser passada a lib de seguran�a da Software Express personalizada para o estabelecimento comercial de forma a obter a senha aberta
                    msgLog = msgLog + " - Contem a senha do cliente capturada atrav�s da rotina LeSenhaInterativo e que deve ser passada a lib de seguran�a da Software Express personalizada para o estabelecimento comercial de forma a obter a senha aberta";
                    break;

                case 154:
                    //Cont�m o novo valor de pagamento
                    msgLog = msgLog + " - Cont�m o novo valor de pagamento";
                    break;

                case 155:
                    //Tipo cartao B�nus
                    msgLog = msgLog + " - Tipo cartao B�nus";
                    break;

                case 156:
                    //Nome da instituicao
                    msgLog = msgLog + " - Nome da instituicao";
                    break;

                case 157:
                    //C�digo de Estabelecimento
                    msgLog = msgLog + " - C�digo de Estabelecimento";
                    break;

                case 158:
                    //C�digo da Rede Autorizadora
                    msgLog = msgLog + " - C�digo da Rede Autorizadora";
                    break;

                case 160:
                    //N�mero do cupom original
                    msgLog = msgLog + " - N�mero do cupom original";
                    sCupomFiscal = sCupomFiscal + " - N�mero do cupom original -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 161:
                    //N�mero Identificador do Cupom do Pagamento
                    msgLog = msgLog + " - N�mero Identificador do Cupom do Pagamento - " + Encoding.UTF8.GetString(buffer);
                    sCupomFiscal = sCupomFiscal + " - N�mero Identificador do Cupom do Pagamento -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 170:
                    //Venda Parcelada Estabelecimento Habilitada               
                    msgLog = msgLog + " **** Venda Parcelada Estabelecimento Habilitada - Enviando 1 - " + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                    break;

                case 171:
                    //N�mero M�nimo de Parcelas � Parcelada Estabelecimento
                    msgLog = msgLog + " **** N�mero M�nimo de Parcelas � Parcelada Estabelecimento - Enviando 1 - " + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                    break;

                case 172:
                    //N�mero M�ximo de Parcelas � Parcelada Estabelecimento
                    msgLog = msgLog + " **** N�mero M�ximo de Parcelas � Parcelada Estabelecimento - Enviando 32 - "  + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("32" + "\0");
                    break;

                case 173:
                    //Valor M�nimo Por Parcela � Parcelada Estabelecimento
                    msgLog = msgLog + " **** Valor M�nimo Por Parcela � Parcelada Estabelecimento - Enviando 1 -" + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                    break;

                case 174:
                    //Venda Parcelada Administradora Habilitada
                    msgLog = msgLog + " **** Venda Parcelada Administradora Habilitada - Enviando 0 - " + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("0" + "\0");
                    break;

                case 175:
                    //N�mero M�nimo de Parcelas � Parcelada Administradora
                    msgLog = msgLog + " **** N�mero M�nimo de Parcelas � Parcelada Administradora - Enviando 1 - " + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                    break;

                case 176:
                    //N�mero M�ximo de Parcelas � Parcelada Administradora
                    msgLog = msgLog + " **** N�mero M�ximo de Parcelas � Parcelada Administradora - Enviando 32 - " + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("32" + "\0");
                    break;

                case 177:
                    //Indica que o campo � num�rico (PBM)
                    msgLog = msgLog + " - Indica que o campo � num�rico (PBM)";
                    break;

                case 178:
                    //Indica que o campo � alfanum�rico (PBM)
                    msgLog = msgLog + " - Indica que o campo � alfanum�rico (PBM)";
                    break;

                case 200:
                    //Saldo dispon�vel*, saldo do produto espec�fico (escolar, vale transporte)
                    msgLog = msgLog + " - Saldo dispon�vel*, saldo do produto espec�fico (escolar, vale transporte)";
                    break;

                case 201:
                    //Saldo Bloqueado
                    msgLog = msgLog + " - Saldo Bloqueado";
                    break;

                case 500:
                    //Indica que o campo em questao � o c�digo do supervisor
                    msgLog = msgLog + " - Indica que o campo em questao � o c�digo do supervisor";
                    break;

                case 501:
                    //Tipo do Documento a ser consultado (0 � CPF, 1 � CGC)
                    msgLog = msgLog + " - Tipo do Documento a ser consultado (0 � CPF, 1 � CGC)";
                    break;

                case 502:
                    //Numero do documento (CPF ou CGC)
                    msgLog = msgLog + " - Numero do documento (CPF ou CGC)";
                    break;

                case 504:
                    //Taxa de Servi�o
                    msgLog = msgLog + " - Taxa de Servi�o";
                    break;

                case 505:
                    //N�mero de Parcelas
                    System.Windows.Forms.MessageBox.Show("VIXI");
                    break;

                case 506:
                    //Data do Pr�-datado no formato ddmmaaaa
                    msgLog = msgLog + " - Data do Pr�-datado no formato ddmmaaaa";
                    break;

                case 507:
                    //Captura se a primeira parcela � a vista ou nao (0 � Primeira a vista, 1 � caso contr�rio)
                    msgLog = msgLog + " - Captura se a primeira parcela � a vista ou nao (0 � Primeira a vista, 1 � caso contr�rio) - " + Encoding.UTF8.GetString(buffer);
                    break;

                case 508:
                    //Intervalo em dias entre parcelas
                    msgLog = msgLog + " - Intervalo em dias entre parcelas - " + Encoding.UTF8.GetString(buffer);
                    break;

                case 509:
                    //Captura se � m�s fechado (0) ou nao (1)
                    msgLog = msgLog + " - Captura se � m�s fechado (0) ou nao (1)";
                    break;

                case 510:
                    //Captura se � com (0) ou sem (1) garantia no pr�-datado com cartao de d�bito
                    msgLog = msgLog + " - Captura se � com (0) ou sem (1) garantia no pr�-datado com cartao de d�bito";
                    break;

                case 511:
                    //N�mero de Parcelas CDC
                    msgLog = msgLog + " - N�mero de Parcelas CDC - " + Encoding.UTF8.GetString(buffer);
                    break;

                case 512:
                    //Numero do cartao
                    msgLog = msgLog + " **** Numero do cartao";
                    //buffer = Encoding.ASCII.GetBytes("5292050000641770" + "\0");
                    break;

                case 513:
                    //Data de Vencimento do Cartao
                    msgLog = msgLog + " **** Data de Vencimento do Cartao";
                    //buffer = Encoding.ASCII.GetBytes("01102021" + "\0");
                    break;

                case 514:
                    //Codigo de Seguranca do cartao
                    msgLog = msgLog + " #### Envi Codigo de Seguranca do cartao - " + Adicional;
                    buffer = Encoding.ASCII.GetBytes(Adicional + "\0");
                    break;

                case 515:
                    //Data da transacao a ser cancelada (DDMMAAAA) ou a ser re-impressa
                    msgLog = msgLog + " - Data da transacao a ser cancelada (DDMMAAAA) ou a ser re-impressa";
                    break;

                case 516:
                    //N�mero do documento a ser cancelado ou a ser re-impresso
                    msgLog = msgLog + " - N�mero do documento a ser cancelado ou a ser re-impresso";
                    break;

                case 517:
                    //A rotina est� sendo chamada para ler o N�mero do cheque segundo o descrito no tipo de comando correspondente ao valor 31
                    msgLog = msgLog + " - A rotina est� sendo chamada para ler o N�mero do cheque segundo o descrito no tipo de comando correspondente ao valor 31";
                    break;

                case 518:
                    //C�digo do Item
                    msgLog = msgLog + " - C�digo do Item";
                    break;

                case 519:
                    //C�digo do Plano de Pagamento
                    msgLog = msgLog + " - C�digo do Plano de Pagamento";
                    break;

                case 520:
                    // NSU do SiTef Original (Cisa)
                    msgLog = msgLog + " - NSU do SiTef Original (Cisa)";
                    break;

                case 521:
                    //N�mero do documento de identidade (RG)
                    msgLog = msgLog + " - N�mero do documento de identidade (RG)";
                    break;

                case 522:
                    //A rotina est� sendo chamada para ler o N�mero do Telefone
                    msgLog = msgLog + " - A rotina est� sendo chamada para ler o N�mero do Telefone";
                    break;

                case 523:
                    //A rotina est� sendo chamada para ler o DDD de um telefone com at� 4 d�gitos
                    msgLog = msgLog + " - A rotina est� sendo chamada para ler o DDD de um telefone com at� 4 d�gitos";
                    break;

                case 524:
                    // Valor da primeira parcela
                    msgLog = msgLog + " - Valor da primeira parcela";
                    break;

                case 525:
                    //Valor das demais parcelas
                    msgLog = msgLog + " - Valor das demais parcelas";
                    break;

                case 526:
                    //Quantidade de cheques
                    msgLog = msgLog + " - Quantidade de cheques";
                    break;

                case 527:
                    //Data de vencimento do cheque
                    msgLog = msgLog + " - Data de vencimento do cheque";
                    break;

                case 529:
                    //A rotina est� sendo chamada para ler a Data de Abertura de Conta no formato (MMAAAA)
                    msgLog = msgLog + " - A rotina est� sendo chamada para ler a Data de Abertura de Conta no formato (MMAAAA)";
                    break;

                case 530:
                    //Autorizacao do supervisor digitada
                    msgLog = msgLog + " - Autorizacao do supervisor digitada";
                    break;

                case 531:
                    //Autorizacao do supervisor especial
                    msgLog = msgLog + " - Autorizacao do supervisor especial";
                    break;

                case 532:
                    //A rotina est� sendo chamada para ler a quantidade de parcelas ou cheques
                    msgLog = msgLog + " - A rotina est� sendo chamada para ler a quantidade de parcelas ou cheques";
                    break;

                case 533:
                    //Dados adicionais da venda
                    msgLog = msgLog + " - Dados adicionais da venda";
                    break;

                case 534:
                    //Emitente do cheque
                    msgLog = msgLog + " - Emitente do cheque";
                    break;

                case 535:
                    //O documento pago pela transacao
                    msgLog = msgLog + " - O documento pago pela transacao";
                    break;

                case 536:
                    //Registros de retorno da consulta cheque CDL-Poa
                    msgLog = msgLog + " - Registros de retorno da consulta cheque CDL-Poa";
                    break;

                case 537:
                    //cheque CDL-Poa 537
                    msgLog = msgLog + " - cheque CDL-Poa 537";
                    break;

                case 550:
                    //Endere�o
                    msgLog = msgLog + " - Endere�o";
                    break;

                case 551:
                    //N�mero do endere�o
                    msgLog = msgLog + " - N�mero do endere�o";
                    break;

                case 552:
                    //Andar do endere�o
                    msgLog = msgLog + " - Andar do endere�o";
                    break;

                case 553:
                    //Conjunto do endere�o
                    msgLog = msgLog + " - Conjunto do endere�o";
                    break;

                case 554:
                    //Bloco do endere�o
                    msgLog = msgLog + " - Bloco do endere�o";
                    break;

                case 555:
                    //CEP do endere�o
                    msgLog = msgLog + " - CEP do endere�o";
                    break;

                case 556:
                    //Bairro do endere�o
                    msgLog = msgLog + " - Bairro do endere�o";
                    break;

                case 557:
                    //CPF para consulta AVS
                    msgLog = msgLog + " - CPF para consulta AVS";
                    break;

                case 558:
                    //Resultado da consulta AVS
                    msgLog = msgLog + " - Resultado da consulta AVS";
                    break;

                case 559:
                    //N�mero de dias do pr�-datado
                    msgLog = msgLog + " - N�mero de dias do pr�-datado";
                    break;

                case 560:
                    //N�mero de Ciclos
                    msgLog = msgLog + " - N�mero de Ciclos";
                    break;

                case 561:
                    //C�digo da Ocorr�ncia
                    msgLog = msgLog + " - C�digo da Ocorr�ncia";
                    break;

                case 562:
                    //C�digo de Loja (EMS)
                    msgLog = msgLog + " - C�digo de Loja (EMS)";
                    break;

                case 563:
                    //C�digo do PDV (EMS)
                    msgLog = msgLog + " - C�digo do PDV (EMS)";
                    break;

                case 564:
                    //Dados Retornados (EMS)
                    msgLog = msgLog + " - Dados Retornados (EMS)";
                    break;

                case 565:
                    //Ramal do Telefone
                    msgLog = msgLog + " - Ramal do Telefone";
                    break;

                case 566:
                    //�rgao Expedidor do RG
                    msgLog = msgLog + " - �rgao Expedidor do RG";
                    break;

                case 567:
                    //Estado onde foi emitido o RG
                    msgLog = msgLog + " - Estado onde foi emitido o RG";
                    break;

                case 568:
                    //Data de expedicao do RG
                    msgLog = msgLog + " - Data de expedicao do RG";
                    break;

                case 569:
                    //Matr�cula do Operador
                    msgLog = msgLog + " - Matr�cula do Operador";
                    break;

                case 570:
                    //Nome do Operador
                    msgLog = msgLog + " - Nome do Operador";
                    break;

                case 571:
                    //Matr�cula do Conferente
                    msgLog = msgLog + " - Matr�cula do Conferente";
                    break;

                case 572:
                    //Nome do Conferente
                    msgLog = msgLog + " - Nome do Conferente";
                    break;

                case 573: 
                    // Percentual de Juros Aplicado
                    msgLog = msgLog + " - Percentual de Juros Aplicado";
                    break;

                case 574:
                    //Matr�cula do Autorizador
                    msgLog = msgLog + " - Matr�cula do Autorizador";
                    break;

                case 575:
                    //Data do Cupom Fiscal da Transacao Original
                    msgLog = msgLog + " - Data do Cupom Fiscal da Transacao Original";
                    sCupomFiscal = sCupomFiscal + " - Data do Cupom Fiscal da Transacao Original -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 576:
                    //Hora do Cupom Fiscal da Transacao Original
                    msgLog = msgLog + " - Hora do Cupom Fiscal da Transacao Original";
                    sCupomFiscal = sCupomFiscal + " - Hora do Cupom Fiscal da Transacao Original -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 577:
                    //Dados do Carn� ou c�digo resumido EMS
                    msgLog = msgLog + " - Dados do Carn� ou c�digo resumido EMS";
                    break;

                case 578:
                    //C�digo de milhas diferenciadas 1
                    msgLog = msgLog + " - C�digo de milhas diferenciadas 1";
                    break;

                case 579: 
                    //Valor das milhas diferenciadas 1 
                    msgLog = msgLog + " - Valor das milhas diferenciadas 1";
                    break;

                case 580:
                    //C�digo de milhas diferenciadas 2 
                    msgLog = msgLog + " - C�digo de milhas diferenciadas 2 ";
                    break;

                case 581:
                    //Valor das milhas diferenciadas 2 
                    msgLog = msgLog + " - Valor das milhas diferenciadas 2 ";
                    break;

                case 582:
                    //Tipo de c�digo externo EMS
                    msgLog = msgLog + " - Tipo de c�digo externo EMS";
                    break;

                case 583:
                    //C�digo externo EMS
                    msgLog = msgLog + " - C�digo externo EMS";
                    break;

                case 587:
                    //C�digo nome da instituicao autorizadora de celular
                    msgLog = msgLog + " - C�digo nome da instituicao autorizadora de celular";
                    break;

                case 588:
                    //C�digo estabelecimento autorizador de celular 
                    msgLog = msgLog + " - C�digo estabelecimento autorizador de celular ";
                    break;

                case 593:
                    //Digito(s) verificadores
                    msgLog = msgLog + " - Digito(s) verificadores";
                    break;

                case 594:
                    //Cep da localidade onde est� o terminal no qual a operacao est� sendo feita
                    msgLog = msgLog + " - Cep da localidade onde est� o terminal no qual a operacao est� sendo feita";
                    break;

                case 597:
                    // C�digo da Filial que atendeu a solicitacao de recarga do celular
                    msgLog = msgLog + " - C�digo da Filial que atendeu a solicitacao de recarga do celular";
                    break;

                case 599:
                    // C�digo da rede autorizadora da recarga de celular
                    msgLog = msgLog + " - C�digo da rede autorizadora da recarga de celular";
                    break;

                case 600:
                    // Data de vencimento do t�tulo/conv�nio no formato DDMMAAAA 
                    msgLog = msgLog + " - Data de vencimento do t�tulo/conv�nio no formato DDMMAAAA ";
                    break;

                case 601:
                    // Valor Pago* 
                    msgLog = msgLog + " - Valor Pago* ";
                    break;

                case 602:
                    // Valor Original 
                    msgLog = msgLog + " - Valor Original ";
                    break;

                case 603:
                    // Valor Acr�scimo
                    msgLog = msgLog + " - Valor Acr�scimo";
                    break;

                case 604:
                    // Valor do Abatimento
                    msgLog = msgLog + " - Valor do Abatimento";
                    break;

                case 605:
                    // Data Cont�bil do Pagamento
                    msgLog = msgLog + " - Data Cont�bil do Pagamento";
                    break;

                case 606:
                    // Nome do Cedente do Titulo. Deve ser impresso no cheque quando o pagamento for feito via essa modalidade
                    msgLog = msgLog + " - Nome do Cedente do Titulo. Deve ser impresso no cheque quando o pagamento for feito via essa modalidade";
                    break;

                case 607:
                    // �ndice do documento, no caso do pagamento em lote, dos campos 600 a 604 que virao em seguida
                    msgLog = msgLog + " - �ndice do documento, no caso do pagamento em lote, dos campos 600 a 604 que virao em seguida";
                    break;

                case 608:
                    // Modalidade de pagamento utilizada na funcao de correspondente banc�rio. Segue a mesma regra de formatacao que o campo de n�mero 100
                    msgLog = msgLog + " - Modalidade de pagamento utilizada na funcao de correspondente banc�rio. Segue a mesma regra de formatacao que o campo de n�mero 100";
                    break;

                case 609:
                    // Valor total dos t�tulos efetivamente pagos no caso de pagamento em lote
                    msgLog = msgLog + " - Valor total dos t�tulos efetivamente pagos no caso de pagamento em lote";
                    break;

                case 610:
                    // Valor total dos t�tulos nao pagos no caso de pagamento em lote
                    msgLog = msgLog + " - Valor total dos t�tulos nao pagos no caso de pagamento em lote";
                    break;
                    
                case 611:
                    // NSU Correspondente Banc�rio 
                    msgLog = msgLog + " - NSU Correspondente Banc�rio ";
                    break;

                case 612:
                    // Tipo do documento: 0 - Arrecadacao, 1 - Titulo (Ficha de compensacao), 2 - Tributo
                    msgLog = msgLog + " - Tipo do documento: 0 - Arrecadacao, 1 - Titulo (Ficha de compensacao), 2 - Tributo";
                    break;

                case 613:
                    // Cont�m os dados do cheque utilizado para efetuar o pagamento das contas no seguinte formato: Compensacao (3), Banco (3), Agencia (4), Conta Corrente (10), e Numero do Cheque (6), nesta ordem. Notar que a ordem � a mesma presente na linha superior do cheque sem os d�gitos verificadores 
                    msgLog = msgLog + " - Cont�m os dados do cheque utilizado para efetuar o pagamento das contas no seguinte formato: Compensacao (3), Banco (3), Agencia (4), Conta Corrente (10), e Numero do Cheque (6), nesta ordem. Notar que a ordem � a mesma presente na linha superior do cheque sem os d�gitos verificadores ";
                    break;

                case 614:
                    // NSU SiTef transacao de pagamento
                    msgLog = msgLog + " - NSU SiTef transacao de pagamento";
                    break;

                case 620:
                    // NSU SiTef da transacao original (transacao de cancelamento) 
                    msgLog = msgLog + " - NSU SiTef da transacao original (transacao de cancelamento) ";
                    break;

                case 621:
                    // NSU Correspondente Banc�rio da transacao original (transacao de cancelamento) 
                    msgLog = msgLog + " - NSU Correspondente Banc�rio da transacao original (transacao de cancelamento) ";
                    break;

                case 622:
                    // Valor do Benef�cio 
                    msgLog = msgLog + " - NSU Correspondente Banc�rio da transacao original (transacao de cancelamento) ";
                    break;

                case 623:
                    // C�digo impresso no rodap� do comprovante do CB e utilizado para re-impressao/cancelamento
                    msgLog = msgLog + " - C�digo impresso no rodap� do comprovante do CB e utilizado para re-impressao/cancelamento";
                    break;

                case 624:
                    // C�digo em barras pago. Aparece uma vez para cada �ndice de documento (campo 607). O formato � o mesmo utilizado para entrada do campo ou seja, 0:numero ou 1:numero
                    msgLog = msgLog + " - C�digo em barras pago. Aparece uma vez para cada �ndice de documento (campo 607). O formato � o mesmo utilizado para entrada do campo ou seja, 0:numero ou 1:numero";
                    break;

                case 625:
                    // Recibo de retirada 
                    msgLog = msgLog + " - Recibo de retirada ";
                    break;

                case 626:
                    // N�mero do banco
                    msgLog = msgLog + " - N�mero do banco";
                    break;

                case 627:
                    // Ag�ncia 
                    msgLog = msgLog + " - Ag�ncia";
                    break;

                case 628:
                    // D�gito da ag�ncia
                    msgLog = msgLog + " - D�gito da ag�ncia";
                    break;

                case 629:
                    // Conta
                    msgLog = msgLog + " - Conta";
                    break;

                case 630:
                    // D�gito da conta
                    msgLog = msgLog + " - D�gito da conta";
                    break;

                case 631:
                    // Valor em dinheiro
                    msgLog = msgLog + " - Valor em dinheiro";
                    break;

                case 632:
                    // Valor em cheque
                    msgLog = msgLog + " - Valor em cheque";
                    break;

                case 633:
                    // Nome do depositante
                    msgLog = msgLog + " - Nome do depositante";
                    break;

                case 634:
                    // Documento original de Correspondente Banc�rio
                    msgLog = msgLog + " - Documento original de Correspondente Banc�rio";
                    break;

                case 635:
                    // Chave do usu�rio utilizada para comunicacao com o Banco 
                    msgLog = msgLog + " - Chave do usu�rio utilizada para comunicacao com o Banco ";
                    break;

                case 636:
                    // Seq�encial �nico da chave do usu�rio no Banco
                    msgLog = msgLog + " - Seq�encial �nico da chave do usu�rio no Banco";
                    break;

                case 637:
                    // C�digo da Ag�ncia de relacionamento da loja do correspondente
                    msgLog = msgLog + " - C�digo da Ag�ncia de relacionamento da loja do correspondente";
                    break;

                case 638:
                    // N�mero do Cheque CB 
                    msgLog = msgLog + " - N�mero do Cheque CB ";
                    break;

                case 639:
                    // N�mero da Fatura
                    msgLog = msgLog + " - N�mero da Fatura";
                    break;

                case 640:
                    // N�mero do Conv�nio
                    msgLog = msgLog + " - N�mero do Conv�nio";
                    break;

                case 641:
                    // Data Inicial do Extrato (DDMMAAAA)
                    msgLog = msgLog + " - Data Inicial do Extrato (DDMMAAAA)";
                    break;

                case 642:
                    // Data Final do Extrato (DDMMAAAA) 
                    msgLog = msgLog + " - Data Final do Extrato (DDMMAAAA) ";
                    break;

                case 643:
                    // Per�odo de Apuracao 
                    msgLog = msgLog + " - Per�odo de Apuracao ";
                    break;

                case 644:
                    // C�digo da Receita Federal 
                    msgLog = msgLog + " - C�digo da Receita Federal ";
                    break;

                case 645:
                    //Valor da Receita Bruta 
                    msgLog = msgLog + " - Valor da Receita Bruta ";
                    break;

                case 646:
                    // Percentual Aplicado
                    msgLog = msgLog + " - Percentual Aplicado";
                    break;

                case 647:
                    // Valor Principal
                    msgLog = msgLog + " - Valor Principal";
                    break;

                case 648:
                    // Valor Multa
                    msgLog = msgLog + " - Valor Multa";
                    break;

                case 649:
                    // Valor Juros
                    msgLog = msgLog + " - Valor Juros";
                    break;

                case 670:
                    // Dado do PinPad
                    msgLog = msgLog + " - Dado do PinPad";
                    break;

                case 700:
                    // Operadora de ValeG�s
                    msgLog = msgLog + " -  Operadora de ValeG�s";
                    break;

                case 701:
                    //Produto ValeG�s 
                    msgLog = msgLog + " - Produto ValeG�s ";
                    break;

                case 702:
                    // N�mero do ValeG�s
                    msgLog = msgLog + " - N�mero do ValeG�s";
                    break;

                case 703:
                    // N�mero de Refer�ncia
                    msgLog = msgLog + " - N�mero de Refer�ncia";
                    break;

                case 704:
                    // C�digo GPS 
                    msgLog = msgLog + " - C�digo GPS ";
                    break;

                case 705:
                    // Compet�ncia GPS
                    msgLog = msgLog + " - Compet�ncia GPS";
                    break;

                case 706:
                    // Identificador Contribuinte
                    msgLog = msgLog + " - Identificador Contribuinte";
                    break;

                case 707:
                    // Valor INSS
                    msgLog = msgLog + " - Valor INSS";
                    break;

                case 708:
                    // Valor Outras Entidades
                    msgLog = msgLog + " - Valor Outras Entidades";
                    break;

                case 709:
                    // Permite Pagamento de Contas Com Dinheiro (0 � Nao Permite; 1 � Permite)
                    msgLog = msgLog + " - Permite Pagamento de Contas Com Dinheiro (0 � Nao Permite; 1 � Permite)";
                    break;

                case 710:
                    // Permite Pagamento de Contas Com Cheque (0 � Nao Permite; 1 � Permite)
                    msgLog = msgLog + " -  Permite Pagamento de Contas Com Cheque (0 � Nao Permite; 1 � Permite)";
                    break;

                case 711:
                    // Permite Pagamento de Contas Com TEF D�bito (0 � Nao Permite; 1 � Permite) 
                    msgLog = msgLog + " - Permite Pagamento de Contas Com TEF D�bito (0 � Nao Permite; 1 � Permite) ";
                    break;

                case 712:
                    // Permite Pagamento de Contas Com TEF Cr�dito (0 � Nao Permite; 1 � Permite) 
                    msgLog = msgLog + " - Permite Pagamento de Contas Com TEF Cr�dito (0 � Nao Permite; 1 � Permite) ";
                    break;

                case 713:
                    // Formas de Pagamento utilizadas na transacao de Pagamento gen�rico
                    msgLog = msgLog + " - Formas de Pagamento utilizadas na transacao de Pagamento gen�rico";
                    break;

                case 714:
                    // Valor do Saque
                    msgLog = msgLog + " - Valor do Saque";
                    break;

                case 715:
                    // Numero do Pedido
                    msgLog = msgLog + " - Numero do Pedido";
                    break;

                case 716:
                    // Valor Limite do Dep�sito CB
                    msgLog = msgLog + " - Valor Limite do Dep�sito CB";
                    break;

                case 717:
                    // Valor Limite do Saque CB
                    msgLog = msgLog + " - Valor Limite do Saque CB";
                    break;

                case 718:
                    // Valor Limite do Saque para Pagamento CB
                    msgLog = msgLog + " - Valor Limite do Saque para Pagamento CB";
                    break;

                case 719:
                    // Valor do produto ValeG�s
                    msgLog = msgLog + " - Valor do produto ValeG�s";
                    break;

                case 722:
                    // Valor m�nimo de pagamento
                    msgLog = msgLog + " - Valor m�nimo de pagamento";
                    break;

                case 723:
                    // Identificacao do Cliente, apenas para recebimento Carrefour
                    msgLog = msgLog + " - Identificacao do Cliente, apenas para recebimento Carrefour";
                    break;

                case 724:
                    // Venda Cr�dito Parcelada com Plano Habilitada
                    msgLog = msgLog + " - Venda Cr�dito Parcelada com Plano Habilitada - " + Encoding.UTF8.GetString(buffer);
                    break;

                case 725:
                    // Venda Cr�dito com Autorizacao a Vista Habilitada
                    msgLog = msgLog + " - Venda Cr�dito com Autorizacao a Vista Habilitada";
                    break;

                case 726:
                    // Venda Cr�dito com Autorizacao Parcela com Plano Habilitada 
                    msgLog = msgLog + " - Venda Cr�dito com Autorizacao Parcela com Plano Habilitada - " + Encoding.UTF8.GetString(buffer);
                    break;

                case 727:
                    // Venda Boleto Habilitada
                    msgLog = msgLog + " - Venda Boleto Habilitada";
                    break;

                case 729:
                    // Valor m�ximo de pagamento 
                    msgLog = msgLog + " - Valor m�ximo de pagamento ";
                    break;

                case 730:
                    // N�mero M�ximo de Formas de Pagamento, 0 para sem limite
                    msgLog = msgLog + " - N�mero M�ximo de Formas de Pagamento, 0 para sem limite";
                    break;

                case 731:
                    // Tipo de Pagamento Habilitado
                    msgLog = msgLog + " - Tipo de Pagamento Habilitado";
                    break;

                case 732:
                    // Dados a serem enviados para o Tipo de Pagamento (Campo 730) retornado anteriormente
                    msgLog = msgLog + " -  Dados a serem enviados para o Tipo de Pagamento (Campo 730) retornado anteriormente";
                    break;

                case 734:
                    // Limite m�nimo de venda para promo��es flex�veis, com 12 d�gitos sendo os 2 �ltimos d�gitos referentes as casas decimais 
                    msgLog = msgLog + " - Limite m�nimo de venda para promo��es flex�veis, com 12 d�gitos sendo os 2 �ltimos d�gitos referentes as casas decimais ";
                    break;

                case 738:
                    // Valor sugerido para o produto selecionado.
                    msgLog = msgLog + " - Valor sugerido para o produto selecionado.";
                    break;

                case 739:
                    // Cliente Preferencial
                    msgLog = msgLog + " - Cliente Preferencial";
                    break;

                case 740:
                    // Consulta Parcela de Credito
                    msgLog = msgLog + " - Consulta Parcela de Credito";
                    break;

                case 750:
                    // Valor Pague F�cil CB
                    msgLog = msgLog + " - Valor Pague F�cil CB";
                    break;

                case 751:
                    // Valor Tarifa Pague F�cil CB 
                    msgLog = msgLog + " - Valor Tarifa Pague F�cil CB ";
                    break;

                case 800:
                    msgLog = msgLog + "!!!!!!!!! Desconhecido [800]: " + Encoding.UTF8.GetString(buffer);
                    break;

                case 952:
                    // N�mero de autorizacao NFCE
                    msgLog = msgLog + " - N�mero de autorizacao NFCE";
                    break;

                case 1002:
                    msgLog = msgLog + "!!!!!!!!! Desconhecido [1002]: " + Encoding.UTF8.GetString(buffer);
                    break;

                case 1003:
                    msgLog = msgLog + "!!!!!!!!! Desconhecido [1003]: " + Encoding.UTF8.GetString(buffer);
                    break;

                case 1010:
                    // Quantidade de medicamentos - PBM
                    msgLog = msgLog + " - Quantidade de medicamentos - PBM";
                    break;

                case 1011:
                    // �ndice do medicamento � PBM
                    msgLog = msgLog + " - �ndice do medicamento � PBM";
                    break;

                case 1012:
                    // C�digo do medicamento � PBM
                    msgLog = msgLog + " - C�digo do medicamento � PBM";
                    break;

                case 1013:
                    // Quantidade autorizada � PBM
                    msgLog = msgLog + " - Quantidade autorizada � PBM";
                    break;

                case 1014:
                    // Pre�o m�ximo ao consumidor � PBM
                    msgLog = msgLog + " - Pre�o m�ximo ao consumidor � PBM";
                    break;

                case 1015:
                    // Pre�o recomendado ao consumidor � PBM
                    msgLog = msgLog + " - Pre�o recomendado ao consumidor � PBM";
                    break;

                case 1016:
                    // Pre�o de venda na farm�cia � PBM
                    msgLog = msgLog + " - Pre�o de venda na farm�cia � PBM";
                    break;

                case 1017:
                    // Valor de reembolso na farm�cia � PBM
                    msgLog = msgLog + " - Valor de reembolso na farm�cia � PBM";
                    break;

                case 1018:
                    // Valor reposicao na farm�cia � PBM
                    msgLog = msgLog + " - Valor reposicao na farm�cia � PBM";
                    break;

                case 1019:
                    // Valor subs�dio do conv�nio � PBM
                    msgLog = msgLog + " - Valor subs�dio do conv�nio � PBM";
                    break;

                case 1020:
                    // CNPJ conv�nio � PBM
                    msgLog = msgLog + " - CNPJ conv�nio � PBM";
                    break;

                case 1021:
                    // C�digo do plano do desconto � PBM 
                    msgLog = msgLog + " - C�digo do plano do desconto � PBM ";
                    break;

                case 1022:
                    // Possui receita m�dica � PBM
                    msgLog = msgLog + " - Possui receita m�dica � PBM";
                    break;

                case 1023:
                    // CRM � PBM 1024 UF � PBM
                    msgLog = msgLog + " - CRM � PBM 1024 UF � PBM";
                    break;

                case 1025:
                    // Descricao do produto* - PBM
                    msgLog = msgLog + " - Descricao do produto* - PBM";
                    break;

                case 1026:
                    // C�digo do produto � PBM 
                    msgLog = msgLog + " - C�digo do produto � PBM ";
                    break;

                case 1027:
                    // Quantidade do produto � PBM 
                    msgLog = msgLog + " - Quantidade do produto � PBM ";
                    break;

                case 1028:
                    // Valor do produto � PBM
                    msgLog = msgLog + " - Valor do produto � PBM";
                    break;

                case 1029:
                    // Data da receita m�dica - PBM
                    msgLog = msgLog + " - Data da receita m�dica - PBM";
                    break;

                case 1030:
                    // C�digo de autorizacao PBM 
                    msgLog = msgLog + " - C�digo de autorizacao PBM ";
                    break;

                case 1031:
                    // Quantidade estornada � PBM
                    msgLog = msgLog + " - Quantidade estornada � PBM";
                    break;

                case 1032:
                    // C�digo de estorno PBM
                    msgLog = msgLog + " - C�digo de estorno PBM";
                    break;

                case 1033:
                    // Pre�o recomendado consumidor a vista � PBM 
                    msgLog = msgLog + " - Pre�o recomendado consumidor a vista � PBM ";
                    break;

                case 1034:
                    // Pre�o recomendado consumido para desconto em folha � PBM
                    msgLog = msgLog + " - Pre�o recomendado consumido para desconto em folha � PBM";
                    break;

                case 1035:
                    // Percentual de reposicao da farm�cia � PBM
                    msgLog = msgLog + " - Percentual de reposicao da farm�cia � PBM";
                    break;

                case 1036:
                    // Comissao de reposicao � PBM 
                    msgLog = msgLog + " - Comissao de reposicao � PBM ";
                    break;

                case 1037:
                    // Tipo de Autorizacao � PBM
                    msgLog = msgLog + " - Tipo de Autorizacao � PBM";
                    break;

                case 1038:
                    // C�digo do conveniado � PBM
                    msgLog = msgLog + " - C�digo do conveniado � PBM";
                    break;

                case 1039:
                    // Nome do conveniado � PBM
                    msgLog = msgLog + " - Nome do conveniado � PBM";
                    break;

                case 1040:
                    // Tipo de Medicamento PBM (01�Medicamento, 02-Manipulacao, 03-Manipulacao Especial, 04-Perfumaria)
                    msgLog = msgLog + " - Tipo de Medicamento PBM (01�Medicamento, 02-Manipulacao, 03-Manipulacao Especial, 04-Perfumaria)";
                    break;

                case 1041:
                    // Descricao do Medicamento � PBM
                    msgLog = msgLog + " -  Descricao do Medicamento � PBM";
                    break;

                case 1042:
                    // Condicao p/venda: Se 0 obrigat�rio utilizar pre�o Funcional Card (PF) Se 1 pode vender por pre�o inferior ao pre�o PF 
                    msgLog = msgLog + " - Condicao p/venda: Se 0 obrigat�rio utilizar pre�o Funcional Card (PF) Se 1 pode vender por pre�o inferior ao pre�o PF ";
                    break;

                case 1043:
                    // Pre�o funcional card
                    msgLog = msgLog + " - Pre�o funcional card";
                    break;

                case 1044:
                    // Pre�o praticado � PBM
                    msgLog = msgLog + " - Pre�o praticado � PBM";
                    break;

                case 1045:
                    // Status do medicamento � PBM
                    msgLog = msgLog + " - Status do medicamento � PBM";
                    break;

                case 1046:
                    // Quantidade receitada � PBM
                    msgLog = msgLog + " - Quantidade receitada � PBM";
                    break;

                case 1047:
                    // Refer�ncia � PBM
                    msgLog = msgLog + " - Refer�ncia � PBM";
                    break;

                case 1048:
                    // Indicador da venda PBM (0-Produto venda cartao 1-Produto venda a vista)
                    msgLog = msgLog + " - Indicador da venda PBM (0-Produto venda cartao 1-Produto venda a vista)";
                    break;

                case 1051:
                    // Data de nascimento
                    msgLog = msgLog + " - Data de nascimento";
                    break;

                case 1052:
                    // Nome da m�e
                    msgLog = msgLog + " - Nome da m�e";
                    break;

                case 1058:
                    // Dados adicionais � ACSP
                    msgLog = msgLog + " - Dados adicionais � ACSP";
                    break;

                case 1100:
                    // Registro anal�tico CHECKCHECK
                    msgLog = msgLog + " - Registro anal�tico CHECKCHECK";
                    break;

                case 1101:
                    // Registro anal�tico ACSP
                    msgLog = msgLog + " - Registro anal�tico ACSP";
                    break;

                case 1102:
                    // Registro anal�tico SERASA
                    msgLog = msgLog  + " - Registro anal�tico SERASA";
                    break;

                case 1103:
                    // Imagem tela anal�tica ACSP
                    msgLog = msgLog + " - Imagem tela anal�tica ACSP";
                    break;

                case 1104:
                    // Imagem tela anal�tica SERASA
                    msgLog = msgLog + " - Imagem tela anal�tica SERASA";
                    break;

                case 1105:
                    // Motivo do cancelamento � ACSP
                    msgLog = msgLog+ " - Motivo do cancelamento � ACSP";
                    break;

                case 1106:
                    // Tipo de consulta � ACSP
                    msgLog = msgLog + " - Tipo de consulta � ACSP";
                    break;

                case 1107:
                    // CNPJ Empresa Conveniada
                    msgLog = msgLog + " - CNPJ Empresa Conveniada";
                    break;

                case 1108:
                    // C�digo da administradora
                    msgLog = msgLog + " - C�digo da administradora";
                    break;

                case 1109:
                    // Dados tabela Telecheque - ACSP 
                    msgLog = msgLog + " -  Dados tabela Telecheque - ACSP";
                    break;

                case 1110:
                    // Matr�cula do motorista � Cartao Combust�vel
                    msgLog = msgLog + " - Matr�cula do motorista � Cartao Combust�vel";
                    break;

                case 1111:
                    // Placa do ve�culo � Cartao Combust�vel 
                    msgLog = msgLog + " - Placa do ve�culo � Cartao Combust�vel ";
                    break;

                case 1112:
                    // Quilometragem � Cartao Combust�vel
                    msgLog = msgLog + " - Quilometragem � Cartao Combust�vel";
                    break;

                case 1113:
                    // Quantidade de litros � Cartao Combust�vel
                    msgLog = msgLog + " - Quantidade de litros � Cartao Combust�vel";
                    break;

                case 1114:
                    // Combust�vel principal � Cartao Combust�vel
                    msgLog = msgLog + " - Combust�vel principal � Cartao Combust�vel";
                    break;

                case 1115:
                    // Produtos de combust�vel � Cartao Combust�vel
                    msgLog = msgLog + " - Produtos de combust�vel � Cartao Combust�vel";
                    break;

                case 1116:
                    // C�digo Produto Host � Cartao Combust�vel
                    msgLog = msgLog + " - C�digo Produto Host � Cartao Combust�vel";
                    break;

                case 1117:
                    // Hor�metro � Cartao Combust�vel
                    msgLog = msgLog + " - Hor�metro � Cartao Combust�vel";
                    break;

                case 1118:
                    // Linha de Cr�dito � Cartao Combust�vel
                    msgLog = msgLog + " - Linha de Cr�dito � Cartao Combust�vel";
                    break;

                case 1119:
                    // Tipo de Mercadoria � Cartao Combust�vel
                    msgLog = msgLog + " - Tipo de Mercadoria � Cartao Combust�vel";
                    break;

                case 1120:
                    // Ramo � Cartao Combust�vel
                    msgLog = msgLog + " - Ramo � Cartao Combust�vel";
                    break;

                case 1121:
                    // Casas decimais de pre�os unit�rios � Cartao Combust�vel
                    msgLog = msgLog + " - Casas decimais de pre�os unit�rios � Cartao Combust�vel";
                    break;

                case 1122:
                    // Quantidade m�xima de produtos � venda
                    msgLog = msgLog + " - Quantidade m�xima de produtos � venda";
                    break;

                case 1123:
                    // Tamanho do c�digo do Produto � Cartao Combust�vel
                    msgLog = msgLog + " - Tamanho do c�digo do Produto � Cartao Combust�vel";
                    break;

                case 1124:
                    // C�digo do ve�culo � Cartao Combust�vel
                    msgLog = msgLog + " - C�digo do ve�culo � Cartao Combust�vel";
                    break;

                case 1125:
                    // Nome da Empresa � Cartao Combust�vel
                    msgLog = msgLog + " - Nome da Empresa � Cartao Combust�vel";
                    break;

                case 1126:
                    // Casas decimais da quantidade � Cartao Combust�vel
                    msgLog = msgLog + " - Casas decimais da quantidade � Cartao Combust�vel";
                    break;

                case 1128:
                    // Lista de Perguntas � Cartao Combust�vel
                    msgLog = msgLog + " - Lista de Perguntas � Cartao Combust�vel";
                    break;

                case 1129:
                    // Permite Coleta de Produto � Cartao Combust�vel
                    msgLog = msgLog + " - Permite Coleta de Produto � Cartao Combust�vel";
                    break;

                case 1131:
                    // C�digo do Limite
                    msgLog = msgLog + " - C�digo do Limite";
                    break;

                case 1132:
                    // Quantidade de Titulares
                    msgLog = msgLog + " - Quantidade de Titulares";
                    break;

                case 1133:
                    // Data de Abertura da Empresa (DDMMAAAA)
                    msgLog = msgLog + " - Data de Abertura da Empresa (DDMMAAAA)";
                    break;

                case 1134:
                    // Nome do Titular 1135 Complemento do Endere�o
                    msgLog = msgLog + " - Nome do Titular 1135 Complemento do Endere�o";
                    break;

                case 1136:
                    // Cidade
                    msgLog = msgLog + " - Cidade";
                    break;

                case 1137:
                    // Estado
                    msgLog = msgLog + " - Estado";
                    break;

                case 1152:
                    // Menu de Valores - SPTrans
                    msgLog = msgLog + " - Menu de Valores - SPTrans";
                    break;

                case 1160:
                    // Produto com Valor de Face � Gift
                    msgLog = msgLog + " - Produto com Valor de Face � Gift";
                    break;

                case 1190:
                    //Quatro ultimos digitos do cartao
                    msgLog = msgLog + " - Quatro ultimos digitos do cartao - " + Encoding.UTF8.GetString(buffer);
                    //buffer = Encoding.ASCII.GetBytes("1770" + "\0");
                    break;

                case 1200:
                    // Total de consultas anteriores
                    msgLog = msgLog + " - Total de consultas anteriores";
                    break;

                case 1201:
                    // Valor acumulado das consultas anteriores, contendo 2 d�gitos decimais por�m sem o caractere decimal. 
                    msgLog = msgLog + " - Valor acumulado das consultas anteriores, contendo 2 d�gitos decimais por�m sem o caractere decimal.";
                    break;

                case 1202:
                    // Total de consultas efetuadas no dia.  
                    msgLog = msgLog + " - Total de consultas efetuadas no dia.";
                    break;

                case 1203:
                    // Valor acumulado das consultas no dia, contendo 2 d�gitos decimais por�m sem o caractere decimal.  
                    msgLog = msgLog + " - Valor acumulado das consultas no dia, contendo 2 d�gitos decimais por�m sem o caractere decimal.";
                    break;

                case 1204:
                    // Total de consultas de cheques pr�-datados realizados no per�odo.  
                    msgLog = msgLog + " - Total de consultas de cheques pr�-datados realizados no per�odo";
                    break;

                case 1205:
                    // Valor acumulado de cheques pr�-datados, contendo 2 d�gitos decimais por�m sem o caractere decimal.  
                    msgLog = msgLog + " - Valor acumulado de cheques pr�-datados, contendo 2 d�gitos decimais por�m sem o caractere decimal.";
                    break;

                case 1206:
                    // Vendedor (Usu�rio) - PBM  
                    msgLog = msgLog + " - Vendedor (Usu�rio) - PBM ";
                    break;

                case 1207:
                    // Senha � PBM  
                    msgLog = msgLog + " - Senha � PBM ";
                    break;

                case 1208:
                    // C�digo de Retorno � PBM  
                    msgLog = msgLog + " - C�digo de Retorno � PBM ";
                    break;

                case 1209:
                    // Origem � PBM  
                    msgLog = msgLog + " - Origem � PBM";
                    break;

                case 1321:
                    // NSU do Host Autorizador da Transacao Cancelada  
                    msgLog = msgLog + " - NSU do Host Autorizador da Transacao Cancelada";
                    break;

                case 2006:
                    // Tipo de criptografia  
                    msgLog = msgLog + " - Tipo de criptografia";
                    break;

                case 2007:
                    // �ndice MasterKey  
                    msgLog = msgLog + " - �ndice MasterKey";
                    break;

                case 2008:
                    // Chave de criptografia  
                    msgLog = msgLog + " - Chave de criptografia";
                    break;

                case 2009:
                    // Senha do cartao  
                    msgLog = msgLog + " - Senha do cartao";
                    break;

                case 2010:
                    // C�digo de resposta do autorizador  
                    msgLog = msgLog + " - C�digo de resposta do autorizador";
                    break;

                case 2011:
                    // Bin da rede  
                    msgLog = msgLog + " - Bin da rede";
                    break;

                case 2012:
                    // N�mero serial do CHIP  
                    msgLog = msgLog + " - N�mero serial do CHIP";
                    break;

                case 2013:
                    // Registro de controle do CHIP  
                    msgLog = msgLog + " - Registro de controle do CHIP";
                    break;

                case 2014:
                    // Saldo comum, saldo do passe comum  
                    msgLog = msgLog + " - Saldo comum, saldo do passe comum";
                    break;

                case 2015:
                    // PAN do cartao presente  
                    msgLog = msgLog + " - PAN do cartao presente ";
                    break;

                case 2017:
                    // Data primeiro vencimento  
                    msgLog = msgLog + " - Data primeiro vencimento";
                    break;

                case 2018:
                    // Valor total  
                    msgLog = msgLog + " - Valor total";
                    break;

                case 2019:
                    // Valor financiado  
                    msgLog = msgLog + " - Valor financiado";
                    break;

                case 2020:
                    // Percentual multa  
                    msgLog = msgLog + " - Percentual multa";
                    break;

                case 2047:
                    // Juros de mora  
                    msgLog = msgLog + " - Juros de mora";
                    break;

                case 2048:
                    // TAC (Taxa de administracao)  
                    msgLog = msgLog + " - TAC (Taxa de administracao)";
                    break;

                case 2053:
                    // Menu (produto) selecionado Visanet  
                    msgLog = msgLog + " - Menu (produto) selecionado Visanet";
                    break;

                case 2054:
                    // Tipo Cr�dito CDC (1 � CDC Produto; 2 � CDC Servi�o)  
                    msgLog = msgLog + " - Tipo Cr�dito CDC (1 � CDC Produto; 2 � CDC Servi�o)";
                    break;

                case 2055:
                    // Data/Hora Sitef (Local)  
                    msgLog = msgLog + " - Data/Hora Sitef (Local)";
                    break;

                case 2056:
                    // Dia da semana Sitef (Local)  
                    msgLog = msgLog + " - Dia da semana Sitef (Local)";
                    break;

                case 2057:
                    // Data/Hora Sitef (GMT)  
                    msgLog = msgLog + " - Data/Hora Sitef (GMT)";
                    break;

                case 2058:
                    // Dia da Semana Sitef (GMT)  
                    msgLog = msgLog + " - Data/Hora Sitef (GMT)";
                    break;

                case 2059:
                    // Dados da Forma de Pagamento - SPTrans  
                    msgLog = msgLog + " - Dados da Forma de Pagamento - SPTrans";
                    break;

                case 2064:
                    // Valor pagamento em dinheiro 
                    msgLog = msgLog + " - Valor pagamento em dinheiro";
                    break;

                case 2065:
                    // C�digo consulta cheque (Gen�rica EMS)  
                    msgLog = msgLog + " - C�digo consulta cheque (Gen�rica EMS)";
                    break;

                case 2067:
                    // Mensagem do autorizador a ser exibida junto com o menu de valores (Se o terminal permitir)  
                    msgLog = msgLog + " - Mensagem do autorizador a ser exibida junto com o menu de valores (Se o terminal permitir)";
                    break;

                case 2078:
                    // C�digo do servi�o  
                    msgLog = msgLog + " - C�digo do servi�o";
                    break;

                case 2079:
                    // Valor do servi�o  
                    msgLog = msgLog + " - Valor do servi�o";
                    break;

                case 2081:
                    // Menu de Produtos  
                    msgLog = msgLog + " - Menu de Produtos";
                    break;

                case 2082:
                    // Nosso n�mero  
                    msgLog = msgLog + " - Nosso n�mero";
                    break;

                case 2083:
                    // Valor total do produto contendo o separador decimal (�,�) e duas casas decimais ap�s a v�rgula.  
                    msgLog = msgLog + " - Valor total do produto contendo o separador decimal (�,�) e duas casas decimais ap�s a v�rgula.";
                    break;

                case 2086:
                    // C�digo do Produto - ValeGas  
                    msgLog = msgLog + " - C�digo do Produto - ValeGas";
                    break;

                case 2087:
                    // Demonstrativo de prazos : 0: Nao; 1: Sim  
                    msgLog = msgLog + " - Demonstrativo de prazos : 0: Nao; 1: Sim";
                    break;

                case 2088:
                    // Cancelamento Total/Parcial : 0: Parcial; 1: Total  
                    msgLog = msgLog + " - Cancelamento Total/Parcial : 0: Parcial; 1: Total";
                    break;

                case 2089:
                    // N�mero de identificacao da fatura.  
                    msgLog = msgLog + " - N�mero de identificacao da fatura. ";
                    break;

                case 2090:
                    // Tipo do cartao Lido 
                    msgLog = msgLog + " - Tipo do cartao Lido";
                    break;

                case 2091:
                    // Status da �ltima leitura do cartao 
                    msgLog = msgLog + " - Status da �ltima leitura do cartao";
                    break;

                case 2093:
                    // C�digo do atendente  
                    msgLog = msgLog + " - C�digo do atendente";
                    break;

                case 2103:
                    // Indica se foi transacao offline : 1 : Sim  
                    msgLog = msgLog + " - Indica se foi transacao offline : 1 : Sim";
                    break;

                case 2109:
                    // Senha tempor�ria  
                    msgLog = msgLog + " - Senha tempor�ria";
                    break;

                case 2124:
                    // Valor da tarifa da Recarga de Celular  
                    msgLog = msgLog + " - Valor da tarifa da Recarga de Celular";
                    break;

                case 2125:
                    // N�mero da parcela (2 caracteres) (Hotcard)  
                    msgLog = msgLog + " - N�mero da parcela (2 caracteres) (Hotcard)";
                    break;

                case 2126:
                    // Seq�encial da transacao (6 caracteres) (Hotcard)  
                    msgLog = msgLog + " - Seq�encial da transacao (6 caracteres) (Hotcard)";
                    break;

                case 2301:
                    // Rodap� do comprovante da via estabelecimento  
                    msgLog = msgLog + "  -Rodap� do comprovante da via estabelecimento";
                    break;

                case 2320:
                    // C�digo do Depositante � CB  
                    msgLog = msgLog + " - C�digo do Depositante � CB";
                    break;

                case 2321:
                    // C�digo do Cliente - CB  
                    msgLog = msgLog + " - C�digo do Cliente - CB";
                    break;

                case 2322:
                    // Sequencia Cartao � CB  
                    msgLog = msgLog +  " - Sequencia Cartao � CB";
                    break;

                case 2323:
                    // Via Cartao - CB  
                    msgLog = msgLog + " - Via Cartao - CB";
                    break;

                case 2324:
                    // Tipo do Extrato � CB  
                    msgLog = msgLog + " - Tipo do Extrato � CB";
                    break;

                case 2325:
                    // Valor limite de Transfer�ncia - CB  
                    msgLog = msgLog + " - Valor limite de Transfer�ncia - CB";
                    break;

                case 2326:
                    // Valor limite para coleta de CPF/CNPJ � CB  
                    msgLog = msgLog + " - Valor limite para coleta de CPF/CNPJ � CB";
                    break;

                case 2327:
                    // CPF/CNPJ do Propriet�rio � CB  
                    msgLog = msgLog + " - CPF/CNPJ do Propriet�rio � CB";
                    break;

                case 2328:
                    // CPF/CNPJ do Portador � CB 
                    msgLog = msgLog + " - CPF/CNPJ do Portador � CB";
                    break;

                case 2329:
                    // Tipo do documento do Propriet�rio - CB  
                    msgLog = msgLog + " - Tipo do documento do Propriet�rio - CB";
                    break;

                case 2330:
                    // Tipo do documento do Portador - CB  
                    msgLog = msgLog + " - Tipo do documento do Portador - CB";
                    break;

                case 2331:
                    // Indica se permite pagamento com cartao CB  
                    msgLog = msgLog + " - Indica se permite pagamento com cartao CB";
                    break;

                case 2332:
                    // Valor da Transfer�ncia  
                    msgLog = msgLog + " - Valor da Transfer�ncia";
                    break;

                case 2333:
                    // Identificacao da transacao  
                    msgLog = msgLog + " - Identificacao da transacao";
                    break;

                case 2334:
                    // Pin Code  
                    msgLog = msgLog + " - Pin Cod";
                    break;

                case 2355:
                    // Quando retornado, atua como uma �dica� para o formato do pr�ximo campo que ser� coletado. 
                    msgLog = msgLog + " - Quando retornado, atua como uma �dica� para o formato do pr�ximo campo que ser� coletado.";
                    break;

                case 2361:
                    // Indica que foi efetuada uma transacao de d�bito para pagamento de carn�  
                    msgLog = msgLog + " - Indica que foi efetuada uma transacao de d�bito para pagamento de carn�";
                    break;

                case 2362:
                    // Retornado logo ap�s a transacao de consulta de bins. O valor 1 indica que o autorizador � capaz de tratar de forma diferenciada transacao de d�bito convencional de d�bito para pagamento de contas.  
                    msgLog = msgLog + " - Retornado logo ap�s a transacao de consulta de bins. O valor 1 indica que o autorizador � capaz de tratar de forma diferenciada transacao de d�bito convencional de d�bito para pagamento de contas.";
                    break;

                case 2364:
                    msgLog = msgLog + "!!!!!!!!! Desconhecido [2364]: " + Encoding.UTF8.GetString(buffer);
                    break;

                case 2369:
                    // Pontos a resgatar (num�rico sem casa decimal).
                    msgLog = msgLog + " - Pontos a resgatar (num�rico sem casa decimal).";
                    break;

                case 2421:
                    //Informa se est� habilitada a funcao de coleta de dados adicionais do cliente (0 ou 1)
                    msgLog = msgLog + " **** Informa se est� habilitada a funcao de coleta de dados adicionais do cliente (0 ou 1) - Enviando valor 1" + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                    break;

                case 2467:
                    //Data no Formato DDMMAA Confirmacao Positiva 
                    msgLog = msgLog + " - Data no Formato DDMMAA Confirmacao Positiva";
                    break;

                case 2468:
                    // Data no Formato DDMM Confirmacao Positiva  
                    msgLog = msgLog + " - Data no Formato DDMM Confirmacao Positiva";
                    break;

                case 2469:
                    // Data no Formato MMAA Confirmacao Positiva  
                    msgLog = msgLog + " - Data no Formato MMAA Confirmacao Positiva";
                    break;

                case 2470:
                    // Campo com Ponto Flutuante  
                    msgLog = msgLog + " -Campo com Ponto Flutuante ";
                    break;

                case 2601:
                    // Mensagem para pinpad  
                    msgLog = msgLog + " - Mensagem para pinpad";
                    break;

                case 2602:
                    // Semente Hash  
                    msgLog = msgLog + " - Semente Hash";
                    break;

                case 2603:
                    // Modalidade para leitura de cartao atrav�s da funcao 431. 
                    msgLog = msgLog + " - Modalidade para leitura de cartao atrav�s da funcao 431.";
                    break;
 
                case 4000:
                    // Status da Pr�-Autorizacao � PBM  
                    msgLog = msgLog + " - Status da Pr�-Autorizacao � PBM";
                    break;

                case 4001:
                    // CRF � PBM  
                    msgLog = msgLog + " - CRF � PBM";
                    break;

                case 4002:
                    // UF do CRF � PBM  
                    msgLog = msgLog + " - UF do CRF � PBM";
                    break;

                case 4003:
                    // Tipo de venda � PBM  
                    msgLog = msgLog + " - Tipo de venda � PBM";
                    break;

                case 4004:
                    // Valor total PBM  
                    msgLog = msgLog + " - Valor total PBM";
                    break;

                case 4005:
                    // Valor a vista PBM 
                    msgLog = msgLog + " - Valor a vista PBM";
                    break;
 
                case 4006:
                    // Valor cartao PBM  
                    msgLog = msgLog + " - Valor cartao PBM";
                    break;

                case 4007:
                    // Nosso n�mero PBM 
                    msgLog = msgLog + " - Nosso n�mero PBM";
                    break;
 
                case 4008:
                    // Percentual de desconto concedido pela administradora (2 casas decimais) 
                    msgLog = msgLog + " - Percentual de desconto concedido pela administradora (2 casas decimais)";
                    break;
 
                case 4016:
                    // Pre�o bruto � PBM 
                    msgLog = msgLog + " - Pre�o bruto � PBM";
                    break;

                case 4017:
                    // Pre�o l�quido � PBM  
                    msgLog = msgLog + " - Pre�o l�quido � PBM";
                    break;

                case 4018:
                    // Valor a receber da Loja, em centavos � PBM  
                    msgLog = msgLog + " - Valor a receber da Loja, em centavos � PBM";
                    break;

                case 4019:
                    // N�mero do lote gerado pela Central � PBM  
                    msgLog = msgLog + " - N�mero do lote gerado pela Central � PBM";
                    break;

                case 4020:
                    // Valor total a receber da loja � PBM  
                    msgLog = msgLog + " - Valor total a receber da loja � PBM";
                    break;

                case 4021:
                    // Valor total a receber da loja � PBM  
                    msgLog = msgLog + " - Valor total a receber da loja � PBM";
                    break;

                case 4022:
                    // Soma dos valores da Operacao � PBM  
                    msgLog = msgLog + " - Soma dos valores da Operacao � PBM ";
                    break;

                case 4023:
                    // Nome da operadora � PBM  
                    msgLog = msgLog + " - Nome da operadora � PBM";
                    break;

                case 4024:
                    // Nome da empresa conveniada � PBM  
                    msgLog = msgLog + " - Nome da empresa conveniada � PBM";
                    break;

                case 4025:
                    // Quantidade de dependentes � PBM  
                    msgLog = msgLog + " - Quantidade de dependentes � PBM";
                    break;

                case 4026:
                    // C�digo do dependente � PBM 
                    msgLog = msgLog + " - C�digo do dependente � PBM";
                    break;
 
                case 4027:
                    // Nome do dependente � PBM  
                    msgLog = msgLog + " - Nome do dependente � PBM";
                    break;

                case 4028:
                    // Valor a receber do conveniado � PBM  
                    msgLog = msgLog + " - Valor a receber do conveniado � PBM";
                    break;

                case 4029:
                    // Valor do desconto total, em centavos  
                    msgLog = msgLog + " - Valor do desconto total, em centavos";
                    break;

                case 4030:
                    // Valor liquido total, em centavos - PBM  
                    msgLog = msgLog + " - Valor liquido total, em centavos - PBM";
                    break;

                case 4031:
                    // C�digo da Operadora Selecionada PBM (dever� ser gravado para posterior envio nas demais transa��es) 
                    msgLog = msgLog + " - C�digo da Operadora Selecionada PBM (dever� ser gravado para posterior envio nas demais transa��es)";
                    break;
 
                case 4032:
                    // Campo de retorno de dados livres referentes �s transa��es PBM. 
                    msgLog = msgLog + " - Campo de retorno de dados livres referentes �s transa��es PBM.";
                    break;
 
                case 4033:
                    // Tipo de documento PBM (0 = CRM, 1 = CRO)  
                    msgLog = msgLog + " - Tipo de documento PBM (0 = CRM, 1 = CRO)";
                    break;

                case 4034:
                    // Dados do Resgate - B�nus  
                    msgLog = msgLog + " - Dados do Resgate - B�nus";
                    break;

                case 4039:
                    // C�digo Resposta PBM (0 = Ok, <>0 = erro)  
                    msgLog = msgLog + " - C�digo Resposta PBM (0 = Ok, <>0 = erro)";
                    break;

                case 4040:
                    // Produto Fracionado PBM (0 = nao, 1 = sim)  
                    msgLog = msgLog + " - Produto Fracionado PBM (0 = nao, 1 = sim)";
                    break;

                case 4041:
                    // Paciente ID PBM (-1 = outros, 00 = titular, 01 = dependente)  
                    msgLog = msgLog + " - Paciente ID PBM (-1 = outros, 00 = titular, 01 = dependente)";
                    break;

                case 4043:
                    // Receita ID PBM (receita cadastrada pela empresa)  
                    msgLog = msgLog + " - Receita ID PBM (receita cadastrada pela empresa)";
                    break;

                case 4044:
                    // Receita item ID PBM (item da receita cadastrada pela empresa)  
                    msgLog = msgLog + " - Receita item ID PBM (item da receita cadastrada pela empresa)";
                    break;

                case 4045:
                    // Receita uso cont�nuo (0 = nao, 1 = sim)  
                    msgLog = msgLog + " - Receita uso cont�nuo (0 = nao, 1 = sim)";
                    break;

                case 4046:
                    // Produto Manipulado PBM (princ�pios ativos)  
                    msgLog = msgLog + " - Produto Manipulado PBM (princ�pios ativos)";
                    break;

                case 4047:
                    // Produto Manipulado PBM Valor Original  
                    msgLog = msgLog + " - Produto Manipulado PBM Valor Original";
                    break;

                case 4058:
                    // Valor do Produto Aprovado com Desconto  
                    msgLog = msgLog + " - Valor do Produto Aprovado com Desconto";
                    break;

                case 4076:
                    // Identificacao da Loja  
                    msgLog = msgLog + " - Identificacao da Loja";
                    break;

                case 4095:
                    // CPF/CNPJ do Beneficiario  
                    msgLog = msgLog + " - CPF/CNPJ do Beneficiario";
                    break;

                case 4096:
                    // CPF/CNPJ do Sacador  
                    msgLog = msgLog + " - CPF/CNPJ do Sacador";
                    break;

                case 4097:
                    // CPF/CNPJ do Pagador 
                    msgLog = msgLog + " - CPF/CNPJ do Pagador";
                    break;

                case 5000:
                    //EVENTO - Indica que a biblioteca est� aguardando a leitura de um cartao 
                    msgLog = msgLog + " [EVENTO] - Indica que a biblioteca est� aguardando a leitura de um cartao";
                    break;

                case 5001:
                    //EVENTO - Indica que a biblioteca est� esperando a digitacao da senha pelo usu�rio  
                    msgLog = msgLog + " [EVENTO] - EVENTO - Indica que a biblioteca est� esperando a digitacao da senha pelo usu�rio";
                    break;

                case 5002:
                    //EVENTO - Indica que a biblioteca est� esperando a digitacao dos dados de confirmacao positiva pelo usu�rio  
                    msgLog = msgLog + " [EVENTO] - Indica que a biblioteca est� esperando a digitacao dos dados de confirmacao positiva pelo usu�rio";
                    break;

                case 5003:
                    //EVENTO - Indica que a biblioteca est� aguardando a leitura do bilhete �nico  
                    msgLog = msgLog + " [EVENTO] - Indica que a biblioteca est� aguardando a leitura do bilhete �nico";
                    break;

                case 5004:
                    //EVENTO - Indica que a biblioteca est� aguardando a remocao do bilhete �nico  
                    msgLog = msgLog + " [EVENTO] - Indica que a biblioteca est� aguardando a remocao do bilhete �nico";
                    break;

                case 5005:
                    //EVENTO - Indica que a transacao foi finalizada  
                    msgLog = msgLog + " [EVENTO] - Indica que a transacao foi finalizada";
                    break;

                case 5006:
                    //EVENTO - Confirma Dados Favorecido  
                    msgLog = msgLog + " [EVENTO] - Confirma Dados Favorecido";
                    break;

                case 5007:
                    //EVENTO - SiTef Conectado  
                    msgLog = msgLog + " [EVENTO] - SiTef Conectado";
                    break;

                case 5008:
                    //EVENTO - SiTef Conectando  
                    msgLog = msgLog + " [EVENTO] - SiTef Conectando";
                    break;

                case 5009:
                    //EVENTO - Consulta OK  
                    msgLog = msgLog + " [EVENTO] -  Consulta OK";
                    break;

                case 5010:
                    //EVENTO - Colher Assinatura  
                    msgLog = msgLog + " [EVENTO] - Colher Assinatura";
                    break;

                case 5011:
                    //EVENTO - Coleta Novo Produto  
                    msgLog = msgLog + " [EVENTO] - Coleta Novo Produto";
                    break;

                case 5012:
                    //EVENTO - Confirma Operacao  
                    msgLog = msgLog + " [EVENTO] - Confirma Operacao";
                    break;

                case 5013:
                    //EVENTO - Confirma Cancelamento  
                    msgLog = msgLog + " [EVENTO] - Confirma Cancelamento";
                    break;

                case 5014:
                    //EVENTO - Confirma Valor Total  
                    msgLog = msgLog + " [EVENTO] - Confirma Valor Total";
                    break;

                case 5015:
                    //EVENTO - Conclusao de Recarga de Bilhete �nico 5016 Reservado  
                    msgLog = msgLog + " [EVENTO] - Conclusao de Recarga de Bilhete �nico 5016 Reservado";
                    break;

                case 5017:
                    //EVENTO - Aguardando leitura de cartao  
                    msgLog = msgLog + " [EVENTO] - Aguardando leitura de cartao";
                    break;

                case 5018:
                    //EVENTO - Aguardando digitacao da senha no PinPad  
                    msgLog = msgLog + " [EVENTO] - Aguardando digitacao da senha no PinPad";
                    break;

                case 5019:
                    //EVENTO - Aguardando processamento do chip  
                    msgLog = msgLog + " [EVENTO] - Aguardando processamento do chip";
                    break;

                case 5020:
                    //EVENTO - Aguardando remocao do cartao  
                    msgLog = msgLog + " [EVENTO] - Aguardando remocao do cartao";
                    break;

                case 5021:
                    //EVENTO - Aguardando confirmacao da operacao  
                    msgLog = msgLog + " [EVENTO] - Aguardando confirmacao da operacao";
                    break;

                case 5027:
                    //EVENTO - Cancelamento da leitura do cartao  
                    msgLog = msgLog + " [EVENTO] - Cancelamento da leitura do cartao";
                    break;

                case 5028:
                    //EVENTO - Cancelamento da digitacao da senha no PinPad  
                    msgLog = msgLog + " [EVENTO] - Cancelamento da digitacao da senha no PinPa";
                    break;

                case 5029:
                    //EVENTO - Cancelamento do processamento do cartao com CHIP  
                    msgLog = msgLog + " [EVENTO] - Cancelamento do processamento do cartao com CHIP";
                    break;

                case 5030:
                    //EVENTO - Cancelamento da remocao do cartao  
                    msgLog = msgLog + " [EVENTO] - Cancelamento da remocao do cartao";
                    break;

                case 5031:
                    //EVENTO - Cancelamento da confirmacao da operacao  
                    msgLog = msgLog + " [EVENTO] - Cancelamento da confirmacao da opera��";
                    break;

                case 5036:
                    //EVENTO - Antes da leitura do cartao magn�tico  
                    msgLog = msgLog + " [EVENTO] - Antes da leitura do cartao magn�tico";
                    break;

                case 5037:
                    //EVENTO - Antes da leitura do cartao com CHIP  
                    msgLog = msgLog + " [EVENTO] - Antes da leitura do cartao com CHIP";
                    break;

                case 5038:
                    //EVENTO - Antes da remocao do cartao com CHIP  
                    msgLog = msgLog + " [EVENTO] - Antes da remocao do cartao com CHIP";
                    break;

                case 5039:
                    //EVENTO - Antes da coleta da senha no pinpad  
                    msgLog = msgLog + " [EVENTO] - Antes da coleta da senha no pinpad";
                    break;

                case 5040:
                    //EVENTO - Antes de abrir a comunicacao com o PinPad 
                    msgLog = msgLog + " [EVENTO] - Antes de abrir a comunicacao com o PinPad";
                    break;

                case 50410:
                    //EVENTO - Antes de fechar a comunicacao com o PinPad 
                    msgLog = msgLog + " [EVENTO] - Antes de fechar a comunicacao com o PinPad ";
                    break;

                case 50420:
                    //EVENTO - Deve bloquear recursos para o PinPad 
                    msgLog = msgLog + " [EVENTO] - Deve bloquear recursos para o PinPad";
                    break;

                case 50430:
                    //EVENTO - Deve liberar recursos para o PinPad 
                    msgLog = msgLog + " [EVENTO] - Deve liberar recursos para o PinPad ";
                    break;

                case 50440:
                    //EVENTO - Depois de abrir a comunicacao com o PinPad 
                    msgLog = msgLog + " [EVENTO] - Depois de abrir a comunicacao com o PinPad";
                    break;

                case 50500:
                    //EVENTO - Atualizacao de tabelas. O conte�do deste campo varia de acordo com a transacao sendo realizada. 
                    msgLog = msgLog + " [EVENTO] - Atualizacao de tabelas. O conte�do deste campo varia de acordo com a transacao sendo realizada.";
                    break;

                case 55010:
                    //EVENTO - In�cio de uma transacao do tipo Correspondente Banc�rio.
                    msgLog = msgLog + " [EVENTO] - In�cio de uma transacao do tipo Correspondente Banc�rio.";
                    break;


                default:
                    break;
            }

            //LOG
            Output.WriteLine(msgLog);

            return buffer;

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

        [DllImport("CliSiTef64I.dll", EntryPoint="ConfiguraIntSiTefInterativo", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int ConfiguraIntSiTefInterativo(byte[] pEnderecoIP, byte[] pCodigoLoja, byte[] pNumeroTerminal, short ConfiguraResultado);
        //static extern int ConfiguraIntSiTefInterativo(byte[] pEnderecoIP, byte[] pCodigoLoja, byte[] pNumeroTerminal, short ConfiguraResultado);

        [DllImport("CliSiTef64I.dll", EntryPoint = "IniciaFuncaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int IniciaFuncaoSiTefInterativo(int Funcao, byte[] pValor, byte[] pCupomFiscal, byte[] pDataFiscal, byte[] pHorario, byte[] pOperador, byte[] pRestricoes);

        [DllImport("CliSiTef64I.dll", EntryPoint = "ContinuaFuncaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int ContinuaFuncaoSiTefInterativo(ref int pComando, ref int pTipoCampo, ref short pTamMinimo, ref short pTamMaximo, byte[] pBuffer, int TamBuffer, int Continua);

        [DllImport("CliSiTef64I.dll", EntryPoint="EnviaRecebeSiTefDireto", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int EnviaRecebeSiTefDireto(short RedeDestino, short FuncaoSiTef, short OffsetCartao, byte[] pDadosTx, short TamDadosTx, byte[] pDadosRx, short TamMaxDadosRx, short[] pCodigoResposta, short TempoEsperaRx, byte []pNumeroCupon, byte[] pDataFiscal, byte[] pHorario, byte[] pOperador, short TipoTransacao);

        [DllImport("CliSiTef64I.dll", EntryPoint="LeSimNaoPinPad", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int LeSimNaoPinPad(byte[] pMsgDisplay);

        [DllImport("CliSiTef64I.dll", EntryPoint="AbrePinPad", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int AbrePinPad();

        [DllImport("CliSiTef64I.dll", EntryPoint="FechaPinPad", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int FechaPinPad();

        [DllImport("CliSiTef64I.dll", EntryPoint="LeCartaoDireto", CharSet=CharSet.Auto, SetLastError = true)]
        static extern int LeCartaoDireto(byte[] pMsgDisplay, byte[] trilha1, byte[] trilha2);

        [DllImport("CliSiTef64I.dll", EntryPoint = "FinalizaFuncaoSiTefInterativo", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int FinalizaFuncaoSiTefInterativo(short Confirma, byte[] CupomFiscal, byte[] DataFiscal, byte[] HoraFiscal, byte[] ParamAdic);

        #endregion
    }
}
