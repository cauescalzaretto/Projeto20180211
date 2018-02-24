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
                    msgLog = msgLog + " - Está devolvendo um valor para, se desejado, ser armazenado pela automacao";
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
                    msgLog = msgLog + " - Texto que deverá ser utilizado como título na apresentacao do menu ( vide comando 21)";
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
                    msgLog = msgLog + " - Deve limpar o texto utilizado como título na apresentacao do menu (comando 4)";
                    //System.Windows.Forms.MessageBox.Show("Apaga Visor: [" + comando.ToString() + "]", "RotinaColeta");                    
                    retorno = 0;
                    break;

                case 15:
                    msgLog = msgLog + " - Cabeçalho a ser apresentado pela aplicacao. Refere-se a exibicao de informações adicionais que algumas transações necessitam mostrar na tela.";
                    break;

                case 16:
                    msgLog = msgLog + " - Deve remover o cabeçalho apresentado pelo comando 15.";
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
                    msgLog = msgLog + " - Deve apresentar um menu de opções e permitir que o usuário selecione uma delas.";
                   // System.Windows.Forms.MessageBox.Show("Menu: [" + mensagem.ToString() + "]", "RotinaColeta");
                    retorno =  this.TrataMenu(pDadosComando, pCampo);
                    break;

                case 22:
                    msgLog = msgLog + " - Deve apresentar a mensagem em Buffer, e aguardar uma tecla do operador. É utilizada quando se deseja que o operador seja avisado de alguma mensagem apresentada na tela.";
                    //System.Windows.Forms.MessageBox.Show("Obtem qualquer tecla: [" + mensagem.ToString() + "]", "RotinaColeta");                    
                    retorno =  0;
                    break;

                case 23:
                    msgLog = msgLog + " - Este comando indica que a rotina está perguntando para a aplicacao se ele deseja interromper o processo de coleta de dados ou nao.";
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
                    msgLog = msgLog + " - Análogo ao comando 30, porém deve ser coletado um campo que nao requer intervencao do operador de caixa, ou seja, nao precisa que seja digitado/mostrado na tela, e sim passado diretamente para a biblioteca pela automacao.";
                    break;

                case 30:
                    msgLog = msgLog + " - Deve ser lido um campo cujo tamanho está entre TamMinimo e TamMaximo. O campo lido deve ser devolvido em Buffer.";
                    break;

                case 31:
                    msgLog = msgLog + " - Deve ser lido o número de um cheque. A coleta pode ser feita via leitura de CMC-7, digitacao do CMC-7 ou pela digitacao da primeira linha do cheque.";
                    break;

                case 32:
                case 33:
                case 34:
                    msgLog = msgLog + " - Deve ser lido um campo monetário ou seja, aceita o delimitador de centavos e devolvido no parâmetro Buffer.";
                    break;

                case 35:
                    msgLog = msgLog + " - Deve ser lido um código em barras ou o mesmo deve ser coletado manualmente";
                    break;

                case 38:
                    //System.Windows.Forms.MessageBox.Show("nComando: [" + comando.ToString() + "]\nTipoCampo: [" + tipoCampo.ToString() + "]", "RotinaColeta");
                    retorno =  LeCampo(pTamanhoMinimo, pTamanhoMaximo, pDadosComando, pCampo);
                    break;

                case 41:
                    msgLog = msgLog + " - Análogo ao Comando 30, porém o campo deve ser coletado de forma mascarada";
                    break;

                case 42:
                    msgLog = msgLog + " - Menu identificado. Deve apresentar um menu de opções e permitir que o usuário selecione uma delas.";
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
            //- Número Identificador do Cupom do Pagamento -> 0\0 - Cupom Fiscal -> Ca - Cupom Fiscal -> T. - Comprovante de Pagamento (via do cliente) -> .. - Comprovante de Pagamento (via do caixa) -> ..

            return retorno;
        }

        private byte[] retornoSolicitacaotipoCampo(int tipoCampo, byte[] buffer, string Adicional)
        {
            string msgLog = "   # TipoCampo:" + tipoCampo;
            

            switch (tipoCampo)
            {   
                case -1:
                    //Nao existem informações que podem/devem ser tratadas pela automacao
                    msgLog = msgLog + " - Nao existem informações que podem/devem ser tratadas pela automacao";
                    break;

                case 0:
                    //A rotina está sendo chamada para indicar que acabou de coletar os dados da transacao e irá iniciar a interacao com o SiTef para obter a autorizacao
                    msgLog = msgLog + " - A rotina está sendo chamada para indicar que acabou de coletar os dados da transacao e irá iniciar a interacao com o SiTef para obter a autorizacao";
                    break;

                case 1:
                    //Dados de confirmacao da transacao.
                    msgLog = msgLog + " Dados de confirmacao da transacao.";
                    break;

                case 2:
                    // Informa o código da funcao SiTef utilizado na mensagem enviada para o servidor
                    msgLog = msgLog + " - Informa o código da funcao SiTef utilizado na mensagem enviada para o servidor";
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
                    //Contém o texto real da modalidade de pagamento que pode ser memorizado pela aplicacao caso exista essa necessidade. Descreve por extenso o par xxnn fornecido em 100
                    msgLog = msgLog + " - Contém o texto real da modalidade de pagamento que pode ser memorizado pela aplicacao caso exista essa necessidade";
                    sCupomFiscal = sCupomFiscal + " - Cupom Fiscal -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 102:
                    //Contém o texto descritivo da modalidade de pagamento que deve ser impresso no cupom fiscal (p/ex: T.E.F., Cheque, etc...)
                    msgLog = msgLog + " - Contém o texto descritivo da modalidade de pagamento que deve ser impresso no cupom fiscal (p/ex: T.E.F., Cheque, etc...)" + Encoding.UTF8.GetString(buffer);
                    sCupomFiscal = sCupomFiscal + " - Cupom Fiscal -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 105:
                    //Contém a data e hora da transacao no formato AAAAMMDDHHMMSS
                    msgLog = msgLog + " - Contém a data e hora da transacao no formato AAAAMMDDHHMMSS";
                    break;

                case 110:
                    // Retorna quando uma transacao for cancelada. Contém a modalidade de cancelamento no formato xxnn, seguindo o mesmo formato xxnn do TipoCampo 100. O sub-grupo nn todavia, contém o valor default 00 por nao ser coletado.
                    msgLog = msgLog + " - Retorna quando uma transacao for cancelada. Contém a modalidade de cancelamento no formato xxnn, seguindo o mesmo formato xxnn do TipoCampo 100. O sub-grupo nn todavia, contém o valor default 00 por nao ser coletado.";
                    break;

                case 111:
                    // Contém o texto real da modalidade de cancelamento que pode ser memorizado pela aplicacao caso exista essa necessidade. Descreve por extenso o par xxnn fornecido em 110.
                    msgLog = msgLog + " - Contém o texto real da modalidade de cancelamento que pode ser memorizado pela aplicacao caso exista essa necessidade. Descreve por extenso o par xxnn fornecido em 110.";
                    break;

                case 112:
                    //Contém o texto descritivo da modalidade de cancelamento que deve ser impresso no cupom fiscal (p/ex: T.E.F., Cheque, etc...).
                    msgLog = msgLog + " - Contém o texto descritivo da modalidade de cancelamento que deve ser impresso no cupom fiscal (p/ex: T.E.F., Cheque, etc...).";
                    sCupomFiscal = sCupomFiscal + " - Cupom Fiscal Cancelado -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 115:
                    //Modalidade Ajuste
                    msgLog = msgLog + " - Modalidade Ajuste";
                    break;

                case 120:
                    //Buffer contém a linha de autenticacao do cheque para ser impresso no verso do mesmo
                    msgLog = msgLog + " - Buffer contém a linha de autenticacao do cheque para ser impresso no verso do mesmo";
                    break;

                case 121:
                    //Buffer contém a primeira via do comprovante de pagamento (via do cliente) a ser impressa na impressora fiscal. Essa via, quando possível, é reduzida de forma a ocupar poucas linhas na impressora. Pode ser um comprovante de venda ou administrativo
                    msgLog = msgLog + " - Buffer contém a primeira via do comprovante de pagamento (via do cliente) a ser impressa na impressora fiscal. Essa via, quando possível, é reduzida de forma a ocupar poucas linhas na impressora. Pode ser um comprovante de venda ou administrativo";
                    sCupomFiscal = sCupomFiscal + " - Comprovante de Pagamento (via do cliente) -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 122:
                    //Buffer contém a segunda via do comprovante de pagamento (via do caixa) a ser impresso na impressora fiscal. Pode ser um comprovante de venda ou administrativo
                    msgLog = msgLog + " - Buffer contém a segunda via do comprovante de pagamento (via do caixa) a ser impresso na impressora fiscal. Pode ser um comprovante de venda ou administrativo";
                    sCupomFiscal = sCupomFiscal + " - Comprovante de Pagamento (via do caixa) -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 123:
                    //Indica que os comprovantes que serao entregues na seqüência sao de determinado tipo:
                    msgLog = msgLog + " - Indica que os comprovantes que serao entregues na seqüência sao de determinado tipo:";
                    break;

                case 125:
                    //Código do Voucher
                    msgLog = msgLog + " - Código do Voucher";
                    break;

                case 130:
                    //Indica, na coleta, que o campo em questao é o valor do troco em dinheiro a ser devolvido para o cliente. Na devolucao de resultado (Comando = 0) contém o valor efetivamente aprovado para o troco
                    msgLog = msgLog + " - Indica, na coleta, que o campo em questao é o valor do troco em dinheiro a ser devolvido para o cliente. Na devolucao de resultado (Comando = 0) contém o valor efetivamente aprovado para o troco";
                    break;

                case 131:
                    //Contém um índice que indica qual a instituicao que irá processar a transacao
                    msgLog = msgLog + " - Contém um índice que indica qual a instituicao que irá processar a transacao";
                    //buffer = Encoding.ASCII.GetBytes("00002" + "\0");//Itau
                    break;

                case 132:
                    //Contém um índice que indica qual o tipo do cartao quando esse tipo for identificável
                    msgLog = msgLog + " - Contém um índice que indica qual o tipo do cartao quando esse tipo for identificável";
                    //buffer = Encoding.ASCII.GetBytes("00002" + "\0");//MasterCard
                    break;

                case 133:
                    //Contém o NSU do SiTef (6 posições)
                    msgLog = msgLog + " - Contém o NSU do SiTef (6 posix)";
                    break;

                case 134:
                    //Contém o NSU do Host autorizador (20 posições no máximo)
                    msgLog = msgLog + " - Contém o NSU do Host autorizador (20 posições no máximo)";
                    break;

                case 135:
                    //Contém o Código de Autorizacao para as transações de crédito (15 posições no máximo)
                    msgLog = msgLog + " - Contém o Código de Autorizacao para as transações de crédito (15 posições no máximo)";
                    break;

                case 136:
                    //Contém as 6 primeiras posições do cartao (bin)
                    msgLog = msgLog + " - Contém as 6 primeiras posições do cartao (bin)";
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
                    //A rotina está sendo chamada para ler o Valor a ser cancelado.
                    msgLog = msgLog + " - A rotina está sendo chamada para ler o Valor a ser cancelado.";
                    break;

                case 147:
                    //Valor a ser cancelado
                    msgLog = msgLog + " - Valor a ser cancelado";
                    break;

                case 150:
                    //Contém a Trilha 1, quando disponível, obtida na funcao LeCartaoInterativo
                    msgLog = msgLog + " - Contém a Trilha 1, quando disponível, obtida na funcao LeCartaoInterativo";
                    break;

                case 151:
                    //Contém a Trilha 2, quando disponível, obtida na funcao LeCartaoInterativo
                    msgLog = msgLog + " - Contém a Trilha 2, quando disponível, obtida na funcao LeCartaoInterativo";
                    break;

                case 153:
                    //Contem a senha do cliente capturada através da rotina LeSenhaInterativo e que deve ser passada a lib de segurança da Software Express personalizada para o estabelecimento comercial de forma a obter a senha aberta
                    msgLog = msgLog + " - Contem a senha do cliente capturada através da rotina LeSenhaInterativo e que deve ser passada a lib de segurança da Software Express personalizada para o estabelecimento comercial de forma a obter a senha aberta";
                    break;

                case 154:
                    //Contém o novo valor de pagamento
                    msgLog = msgLog + " - Contém o novo valor de pagamento";
                    break;

                case 155:
                    //Tipo cartao Bônus
                    msgLog = msgLog + " - Tipo cartao Bônus";
                    break;

                case 156:
                    //Nome da instituicao
                    msgLog = msgLog + " - Nome da instituicao";
                    break;

                case 157:
                    //Código de Estabelecimento
                    msgLog = msgLog + " - Código de Estabelecimento";
                    break;

                case 158:
                    //Código da Rede Autorizadora
                    msgLog = msgLog + " - Código da Rede Autorizadora";
                    break;

                case 160:
                    //Número do cupom original
                    msgLog = msgLog + " - Número do cupom original";
                    sCupomFiscal = sCupomFiscal + " - Número do cupom original -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 161:
                    //Número Identificador do Cupom do Pagamento
                    msgLog = msgLog + " - Número Identificador do Cupom do Pagamento - " + Encoding.UTF8.GetString(buffer);
                    sCupomFiscal = sCupomFiscal + " - Número Identificador do Cupom do Pagamento -> " + Encoding.UTF8.GetString(buffer);
                    break;

                case 170:
                    //Venda Parcelada Estabelecimento Habilitada               
                    msgLog = msgLog + " **** Venda Parcelada Estabelecimento Habilitada - Enviando 1 - " + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                    break;

                case 171:
                    //Número Mínimo de Parcelas – Parcelada Estabelecimento
                    msgLog = msgLog + " **** Número Mínimo de Parcelas – Parcelada Estabelecimento - Enviando 1 - " + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                    break;

                case 172:
                    //Número Máximo de Parcelas – Parcelada Estabelecimento
                    msgLog = msgLog + " **** Número Máximo de Parcelas – Parcelada Estabelecimento - Enviando 32 - "  + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("32" + "\0");
                    break;

                case 173:
                    //Valor Mínimo Por Parcela – Parcelada Estabelecimento
                    msgLog = msgLog + " **** Valor Mínimo Por Parcela – Parcelada Estabelecimento - Enviando 1 -" + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                    break;

                case 174:
                    //Venda Parcelada Administradora Habilitada
                    msgLog = msgLog + " **** Venda Parcelada Administradora Habilitada - Enviando 0 - " + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("0" + "\0");
                    break;

                case 175:
                    //Número Mínimo de Parcelas – Parcelada Administradora
                    msgLog = msgLog + " **** Número Mínimo de Parcelas – Parcelada Administradora - Enviando 1 - " + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("1" + "\0");
                    break;

                case 176:
                    //Número Máximo de Parcelas – Parcelada Administradora
                    msgLog = msgLog + " **** Número Máximo de Parcelas – Parcelada Administradora - Enviando 32 - " + Encoding.UTF8.GetString(buffer);
                    buffer = Encoding.ASCII.GetBytes("32" + "\0");
                    break;

                case 177:
                    //Indica que o campo é numérico (PBM)
                    msgLog = msgLog + " - Indica que o campo é numérico (PBM)";
                    break;

                case 178:
                    //Indica que o campo é alfanumérico (PBM)
                    msgLog = msgLog + " - Indica que o campo é alfanumérico (PBM)";
                    break;

                case 200:
                    //Saldo disponível*, saldo do produto específico (escolar, vale transporte)
                    msgLog = msgLog + " - Saldo disponível*, saldo do produto específico (escolar, vale transporte)";
                    break;

                case 201:
                    //Saldo Bloqueado
                    msgLog = msgLog + " - Saldo Bloqueado";
                    break;

                case 500:
                    //Indica que o campo em questao é o código do supervisor
                    msgLog = msgLog + " - Indica que o campo em questao é o código do supervisor";
                    break;

                case 501:
                    //Tipo do Documento a ser consultado (0 – CPF, 1 – CGC)
                    msgLog = msgLog + " - Tipo do Documento a ser consultado (0 – CPF, 1 – CGC)";
                    break;

                case 502:
                    //Numero do documento (CPF ou CGC)
                    msgLog = msgLog + " - Numero do documento (CPF ou CGC)";
                    break;

                case 504:
                    //Taxa de Serviço
                    msgLog = msgLog + " - Taxa de Serviço";
                    break;

                case 505:
                    //Número de Parcelas
                    System.Windows.Forms.MessageBox.Show("VIXI");
                    break;

                case 506:
                    //Data do Pré-datado no formato ddmmaaaa
                    msgLog = msgLog + " - Data do Pré-datado no formato ddmmaaaa";
                    break;

                case 507:
                    //Captura se a primeira parcela é a vista ou nao (0 – Primeira a vista, 1 – caso contrário)
                    msgLog = msgLog + " - Captura se a primeira parcela é a vista ou nao (0 – Primeira a vista, 1 – caso contrário) - " + Encoding.UTF8.GetString(buffer);
                    break;

                case 508:
                    //Intervalo em dias entre parcelas
                    msgLog = msgLog + " - Intervalo em dias entre parcelas - " + Encoding.UTF8.GetString(buffer);
                    break;

                case 509:
                    //Captura se é mês fechado (0) ou nao (1)
                    msgLog = msgLog + " - Captura se é mês fechado (0) ou nao (1)";
                    break;

                case 510:
                    //Captura se é com (0) ou sem (1) garantia no pré-datado com cartao de débito
                    msgLog = msgLog + " - Captura se é com (0) ou sem (1) garantia no pré-datado com cartao de débito";
                    break;

                case 511:
                    //Número de Parcelas CDC
                    msgLog = msgLog + " - Número de Parcelas CDC - " + Encoding.UTF8.GetString(buffer);
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
                    //Número do documento a ser cancelado ou a ser re-impresso
                    msgLog = msgLog + " - Número do documento a ser cancelado ou a ser re-impresso";
                    break;

                case 517:
                    //A rotina está sendo chamada para ler o Número do cheque segundo o descrito no tipo de comando correspondente ao valor 31
                    msgLog = msgLog + " - A rotina está sendo chamada para ler o Número do cheque segundo o descrito no tipo de comando correspondente ao valor 31";
                    break;

                case 518:
                    //Código do Item
                    msgLog = msgLog + " - Código do Item";
                    break;

                case 519:
                    //Código do Plano de Pagamento
                    msgLog = msgLog + " - Código do Plano de Pagamento";
                    break;

                case 520:
                    // NSU do SiTef Original (Cisa)
                    msgLog = msgLog + " - NSU do SiTef Original (Cisa)";
                    break;

                case 521:
                    //Número do documento de identidade (RG)
                    msgLog = msgLog + " - Número do documento de identidade (RG)";
                    break;

                case 522:
                    //A rotina está sendo chamada para ler o Número do Telefone
                    msgLog = msgLog + " - A rotina está sendo chamada para ler o Número do Telefone";
                    break;

                case 523:
                    //A rotina está sendo chamada para ler o DDD de um telefone com até 4 dígitos
                    msgLog = msgLog + " - A rotina está sendo chamada para ler o DDD de um telefone com até 4 dígitos";
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
                    //A rotina está sendo chamada para ler a Data de Abertura de Conta no formato (MMAAAA)
                    msgLog = msgLog + " - A rotina está sendo chamada para ler a Data de Abertura de Conta no formato (MMAAAA)";
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
                    //A rotina está sendo chamada para ler a quantidade de parcelas ou cheques
                    msgLog = msgLog + " - A rotina está sendo chamada para ler a quantidade de parcelas ou cheques";
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
                    //Endereço
                    msgLog = msgLog + " - Endereço";
                    break;

                case 551:
                    //Número do endereço
                    msgLog = msgLog + " - Número do endereço";
                    break;

                case 552:
                    //Andar do endereço
                    msgLog = msgLog + " - Andar do endereço";
                    break;

                case 553:
                    //Conjunto do endereço
                    msgLog = msgLog + " - Conjunto do endereço";
                    break;

                case 554:
                    //Bloco do endereço
                    msgLog = msgLog + " - Bloco do endereço";
                    break;

                case 555:
                    //CEP do endereço
                    msgLog = msgLog + " - CEP do endereço";
                    break;

                case 556:
                    //Bairro do endereço
                    msgLog = msgLog + " - Bairro do endereço";
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
                    //Número de dias do pré-datado
                    msgLog = msgLog + " - Número de dias do pré-datado";
                    break;

                case 560:
                    //Número de Ciclos
                    msgLog = msgLog + " - Número de Ciclos";
                    break;

                case 561:
                    //Código da Ocorrência
                    msgLog = msgLog + " - Código da Ocorrência";
                    break;

                case 562:
                    //Código de Loja (EMS)
                    msgLog = msgLog + " - Código de Loja (EMS)";
                    break;

                case 563:
                    //Código do PDV (EMS)
                    msgLog = msgLog + " - Código do PDV (EMS)";
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
                    //Órgao Expedidor do RG
                    msgLog = msgLog + " - Órgao Expedidor do RG";
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
                    //Matrícula do Operador
                    msgLog = msgLog + " - Matrícula do Operador";
                    break;

                case 570:
                    //Nome do Operador
                    msgLog = msgLog + " - Nome do Operador";
                    break;

                case 571:
                    //Matrícula do Conferente
                    msgLog = msgLog + " - Matrícula do Conferente";
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
                    //Matrícula do Autorizador
                    msgLog = msgLog + " - Matrícula do Autorizador";
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
                    //Dados do Carnê ou código resumido EMS
                    msgLog = msgLog + " - Dados do Carnê ou código resumido EMS";
                    break;

                case 578:
                    //Código de milhas diferenciadas 1
                    msgLog = msgLog + " - Código de milhas diferenciadas 1";
                    break;

                case 579: 
                    //Valor das milhas diferenciadas 1 
                    msgLog = msgLog + " - Valor das milhas diferenciadas 1";
                    break;

                case 580:
                    //Código de milhas diferenciadas 2 
                    msgLog = msgLog + " - Código de milhas diferenciadas 2 ";
                    break;

                case 581:
                    //Valor das milhas diferenciadas 2 
                    msgLog = msgLog + " - Valor das milhas diferenciadas 2 ";
                    break;

                case 582:
                    //Tipo de código externo EMS
                    msgLog = msgLog + " - Tipo de código externo EMS";
                    break;

                case 583:
                    //Código externo EMS
                    msgLog = msgLog + " - Código externo EMS";
                    break;

                case 587:
                    //Código nome da instituicao autorizadora de celular
                    msgLog = msgLog + " - Código nome da instituicao autorizadora de celular";
                    break;

                case 588:
                    //Código estabelecimento autorizador de celular 
                    msgLog = msgLog + " - Código estabelecimento autorizador de celular ";
                    break;

                case 593:
                    //Digito(s) verificadores
                    msgLog = msgLog + " - Digito(s) verificadores";
                    break;

                case 594:
                    //Cep da localidade onde está o terminal no qual a operacao está sendo feita
                    msgLog = msgLog + " - Cep da localidade onde está o terminal no qual a operacao está sendo feita";
                    break;

                case 597:
                    // Código da Filial que atendeu a solicitacao de recarga do celular
                    msgLog = msgLog + " - Código da Filial que atendeu a solicitacao de recarga do celular";
                    break;

                case 599:
                    // Código da rede autorizadora da recarga de celular
                    msgLog = msgLog + " - Código da rede autorizadora da recarga de celular";
                    break;

                case 600:
                    // Data de vencimento do título/convênio no formato DDMMAAAA 
                    msgLog = msgLog + " - Data de vencimento do título/convênio no formato DDMMAAAA ";
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
                    // Valor Acréscimo
                    msgLog = msgLog + " - Valor Acréscimo";
                    break;

                case 604:
                    // Valor do Abatimento
                    msgLog = msgLog + " - Valor do Abatimento";
                    break;

                case 605:
                    // Data Contábil do Pagamento
                    msgLog = msgLog + " - Data Contábil do Pagamento";
                    break;

                case 606:
                    // Nome do Cedente do Titulo. Deve ser impresso no cheque quando o pagamento for feito via essa modalidade
                    msgLog = msgLog + " - Nome do Cedente do Titulo. Deve ser impresso no cheque quando o pagamento for feito via essa modalidade";
                    break;

                case 607:
                    // Índice do documento, no caso do pagamento em lote, dos campos 600 a 604 que virao em seguida
                    msgLog = msgLog + " - Índice do documento, no caso do pagamento em lote, dos campos 600 a 604 que virao em seguida";
                    break;

                case 608:
                    // Modalidade de pagamento utilizada na funcao de correspondente bancário. Segue a mesma regra de formatacao que o campo de número 100
                    msgLog = msgLog + " - Modalidade de pagamento utilizada na funcao de correspondente bancário. Segue a mesma regra de formatacao que o campo de número 100";
                    break;

                case 609:
                    // Valor total dos títulos efetivamente pagos no caso de pagamento em lote
                    msgLog = msgLog + " - Valor total dos títulos efetivamente pagos no caso de pagamento em lote";
                    break;

                case 610:
                    // Valor total dos títulos nao pagos no caso de pagamento em lote
                    msgLog = msgLog + " - Valor total dos títulos nao pagos no caso de pagamento em lote";
                    break;
                    
                case 611:
                    // NSU Correspondente Bancário 
                    msgLog = msgLog + " - NSU Correspondente Bancário ";
                    break;

                case 612:
                    // Tipo do documento: 0 - Arrecadacao, 1 - Titulo (Ficha de compensacao), 2 - Tributo
                    msgLog = msgLog + " - Tipo do documento: 0 - Arrecadacao, 1 - Titulo (Ficha de compensacao), 2 - Tributo";
                    break;

                case 613:
                    // Contém os dados do cheque utilizado para efetuar o pagamento das contas no seguinte formato: Compensacao (3), Banco (3), Agencia (4), Conta Corrente (10), e Numero do Cheque (6), nesta ordem. Notar que a ordem é a mesma presente na linha superior do cheque sem os dígitos verificadores 
                    msgLog = msgLog + " - Contém os dados do cheque utilizado para efetuar o pagamento das contas no seguinte formato: Compensacao (3), Banco (3), Agencia (4), Conta Corrente (10), e Numero do Cheque (6), nesta ordem. Notar que a ordem é a mesma presente na linha superior do cheque sem os dígitos verificadores ";
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
                    // NSU Correspondente Bancário da transacao original (transacao de cancelamento) 
                    msgLog = msgLog + " - NSU Correspondente Bancário da transacao original (transacao de cancelamento) ";
                    break;

                case 622:
                    // Valor do Benefício 
                    msgLog = msgLog + " - NSU Correspondente Bancário da transacao original (transacao de cancelamento) ";
                    break;

                case 623:
                    // Código impresso no rodapé do comprovante do CB e utilizado para re-impressao/cancelamento
                    msgLog = msgLog + " - Código impresso no rodapé do comprovante do CB e utilizado para re-impressao/cancelamento";
                    break;

                case 624:
                    // Código em barras pago. Aparece uma vez para cada índice de documento (campo 607). O formato é o mesmo utilizado para entrada do campo ou seja, 0:numero ou 1:numero
                    msgLog = msgLog + " - Código em barras pago. Aparece uma vez para cada índice de documento (campo 607). O formato é o mesmo utilizado para entrada do campo ou seja, 0:numero ou 1:numero";
                    break;

                case 625:
                    // Recibo de retirada 
                    msgLog = msgLog + " - Recibo de retirada ";
                    break;

                case 626:
                    // Número do banco
                    msgLog = msgLog + " - Número do banco";
                    break;

                case 627:
                    // Agência 
                    msgLog = msgLog + " - Agência";
                    break;

                case 628:
                    // Dígito da agência
                    msgLog = msgLog + " - Dígito da agência";
                    break;

                case 629:
                    // Conta
                    msgLog = msgLog + " - Conta";
                    break;

                case 630:
                    // Dígito da conta
                    msgLog = msgLog + " - Dígito da conta";
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
                    // Documento original de Correspondente Bancário
                    msgLog = msgLog + " - Documento original de Correspondente Bancário";
                    break;

                case 635:
                    // Chave do usuário utilizada para comunicacao com o Banco 
                    msgLog = msgLog + " - Chave do usuário utilizada para comunicacao com o Banco ";
                    break;

                case 636:
                    // Seqüencial único da chave do usuário no Banco
                    msgLog = msgLog + " - Seqüencial único da chave do usuário no Banco";
                    break;

                case 637:
                    // Código da Agência de relacionamento da loja do correspondente
                    msgLog = msgLog + " - Código da Agência de relacionamento da loja do correspondente";
                    break;

                case 638:
                    // Número do Cheque CB 
                    msgLog = msgLog + " - Número do Cheque CB ";
                    break;

                case 639:
                    // Número da Fatura
                    msgLog = msgLog + " - Número da Fatura";
                    break;

                case 640:
                    // Número do Convênio
                    msgLog = msgLog + " - Número do Convênio";
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
                    // Período de Apuracao 
                    msgLog = msgLog + " - Período de Apuracao ";
                    break;

                case 644:
                    // Código da Receita Federal 
                    msgLog = msgLog + " - Código da Receita Federal ";
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
                    // Operadora de ValeGás
                    msgLog = msgLog + " -  Operadora de ValeGás";
                    break;

                case 701:
                    //Produto ValeGás 
                    msgLog = msgLog + " - Produto ValeGás ";
                    break;

                case 702:
                    // Número do ValeGás
                    msgLog = msgLog + " - Número do ValeGás";
                    break;

                case 703:
                    // Número de Referência
                    msgLog = msgLog + " - Número de Referência";
                    break;

                case 704:
                    // Código GPS 
                    msgLog = msgLog + " - Código GPS ";
                    break;

                case 705:
                    // Competência GPS
                    msgLog = msgLog + " - Competência GPS";
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
                    // Permite Pagamento de Contas Com Dinheiro (0 – Nao Permite; 1 – Permite)
                    msgLog = msgLog + " - Permite Pagamento de Contas Com Dinheiro (0 – Nao Permite; 1 – Permite)";
                    break;

                case 710:
                    // Permite Pagamento de Contas Com Cheque (0 – Nao Permite; 1 – Permite)
                    msgLog = msgLog + " -  Permite Pagamento de Contas Com Cheque (0 – Nao Permite; 1 – Permite)";
                    break;

                case 711:
                    // Permite Pagamento de Contas Com TEF Débito (0 – Nao Permite; 1 – Permite) 
                    msgLog = msgLog + " - Permite Pagamento de Contas Com TEF Débito (0 – Nao Permite; 1 – Permite) ";
                    break;

                case 712:
                    // Permite Pagamento de Contas Com TEF Crédito (0 – Nao Permite; 1 – Permite) 
                    msgLog = msgLog + " - Permite Pagamento de Contas Com TEF Crédito (0 – Nao Permite; 1 – Permite) ";
                    break;

                case 713:
                    // Formas de Pagamento utilizadas na transacao de Pagamento genérico
                    msgLog = msgLog + " - Formas de Pagamento utilizadas na transacao de Pagamento genérico";
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
                    // Valor Limite do Depósito CB
                    msgLog = msgLog + " - Valor Limite do Depósito CB";
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
                    // Valor do produto ValeGás
                    msgLog = msgLog + " - Valor do produto ValeGás";
                    break;

                case 722:
                    // Valor mínimo de pagamento
                    msgLog = msgLog + " - Valor mínimo de pagamento";
                    break;

                case 723:
                    // Identificacao do Cliente, apenas para recebimento Carrefour
                    msgLog = msgLog + " - Identificacao do Cliente, apenas para recebimento Carrefour";
                    break;

                case 724:
                    // Venda Crédito Parcelada com Plano Habilitada
                    msgLog = msgLog + " - Venda Crédito Parcelada com Plano Habilitada - " + Encoding.UTF8.GetString(buffer);
                    break;

                case 725:
                    // Venda Crédito com Autorizacao a Vista Habilitada
                    msgLog = msgLog + " - Venda Crédito com Autorizacao a Vista Habilitada";
                    break;

                case 726:
                    // Venda Crédito com Autorizacao Parcela com Plano Habilitada 
                    msgLog = msgLog + " - Venda Crédito com Autorizacao Parcela com Plano Habilitada - " + Encoding.UTF8.GetString(buffer);
                    break;

                case 727:
                    // Venda Boleto Habilitada
                    msgLog = msgLog + " - Venda Boleto Habilitada";
                    break;

                case 729:
                    // Valor máximo de pagamento 
                    msgLog = msgLog + " - Valor máximo de pagamento ";
                    break;

                case 730:
                    // Número Máximo de Formas de Pagamento, 0 para sem limite
                    msgLog = msgLog + " - Número Máximo de Formas de Pagamento, 0 para sem limite";
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
                    // Limite mínimo de venda para promoções flexíveis, com 12 dígitos sendo os 2 últimos dígitos referentes as casas decimais 
                    msgLog = msgLog + " - Limite mínimo de venda para promoções flexíveis, com 12 dígitos sendo os 2 últimos dígitos referentes as casas decimais ";
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
                    // Valor Pague Fácil CB
                    msgLog = msgLog + " - Valor Pague Fácil CB";
                    break;

                case 751:
                    // Valor Tarifa Pague Fácil CB 
                    msgLog = msgLog + " - Valor Tarifa Pague Fácil CB ";
                    break;

                case 800:
                    msgLog = msgLog + "!!!!!!!!! Desconhecido [800]: " + Encoding.UTF8.GetString(buffer);
                    break;

                case 952:
                    // Número de autorizacao NFCE
                    msgLog = msgLog + " - Número de autorizacao NFCE";
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
                    // Índice do medicamento – PBM
                    msgLog = msgLog + " - Índice do medicamento – PBM";
                    break;

                case 1012:
                    // Código do medicamento – PBM
                    msgLog = msgLog + " - Código do medicamento – PBM";
                    break;

                case 1013:
                    // Quantidade autorizada – PBM
                    msgLog = msgLog + " - Quantidade autorizada – PBM";
                    break;

                case 1014:
                    // Preço máximo ao consumidor – PBM
                    msgLog = msgLog + " - Preço máximo ao consumidor – PBM";
                    break;

                case 1015:
                    // Preço recomendado ao consumidor – PBM
                    msgLog = msgLog + " - Preço recomendado ao consumidor – PBM";
                    break;

                case 1016:
                    // Preço de venda na farmácia – PBM
                    msgLog = msgLog + " - Preço de venda na farmácia – PBM";
                    break;

                case 1017:
                    // Valor de reembolso na farmácia – PBM
                    msgLog = msgLog + " - Valor de reembolso na farmácia – PBM";
                    break;

                case 1018:
                    // Valor reposicao na farmácia – PBM
                    msgLog = msgLog + " - Valor reposicao na farmácia – PBM";
                    break;

                case 1019:
                    // Valor subsídio do convênio – PBM
                    msgLog = msgLog + " - Valor subsídio do convênio – PBM";
                    break;

                case 1020:
                    // CNPJ convênio – PBM
                    msgLog = msgLog + " - CNPJ convênio – PBM";
                    break;

                case 1021:
                    // Código do plano do desconto – PBM 
                    msgLog = msgLog + " - Código do plano do desconto – PBM ";
                    break;

                case 1022:
                    // Possui receita médica – PBM
                    msgLog = msgLog + " - Possui receita médica – PBM";
                    break;

                case 1023:
                    // CRM – PBM 1024 UF – PBM
                    msgLog = msgLog + " - CRM – PBM 1024 UF – PBM";
                    break;

                case 1025:
                    // Descricao do produto* - PBM
                    msgLog = msgLog + " - Descricao do produto* - PBM";
                    break;

                case 1026:
                    // Código do produto – PBM 
                    msgLog = msgLog + " - Código do produto – PBM ";
                    break;

                case 1027:
                    // Quantidade do produto – PBM 
                    msgLog = msgLog + " - Quantidade do produto – PBM ";
                    break;

                case 1028:
                    // Valor do produto – PBM
                    msgLog = msgLog + " - Valor do produto – PBM";
                    break;

                case 1029:
                    // Data da receita médica - PBM
                    msgLog = msgLog + " - Data da receita médica - PBM";
                    break;

                case 1030:
                    // Código de autorizacao PBM 
                    msgLog = msgLog + " - Código de autorizacao PBM ";
                    break;

                case 1031:
                    // Quantidade estornada – PBM
                    msgLog = msgLog + " - Quantidade estornada – PBM";
                    break;

                case 1032:
                    // Código de estorno PBM
                    msgLog = msgLog + " - Código de estorno PBM";
                    break;

                case 1033:
                    // Preço recomendado consumidor a vista – PBM 
                    msgLog = msgLog + " - Preço recomendado consumidor a vista – PBM ";
                    break;

                case 1034:
                    // Preço recomendado consumido para desconto em folha – PBM
                    msgLog = msgLog + " - Preço recomendado consumido para desconto em folha – PBM";
                    break;

                case 1035:
                    // Percentual de reposicao da farmácia – PBM
                    msgLog = msgLog + " - Percentual de reposicao da farmácia – PBM";
                    break;

                case 1036:
                    // Comissao de reposicao – PBM 
                    msgLog = msgLog + " - Comissao de reposicao – PBM ";
                    break;

                case 1037:
                    // Tipo de Autorizacao – PBM
                    msgLog = msgLog + " - Tipo de Autorizacao – PBM";
                    break;

                case 1038:
                    // Código do conveniado – PBM
                    msgLog = msgLog + " - Código do conveniado – PBM";
                    break;

                case 1039:
                    // Nome do conveniado – PBM
                    msgLog = msgLog + " - Nome do conveniado – PBM";
                    break;

                case 1040:
                    // Tipo de Medicamento PBM (01–Medicamento, 02-Manipulacao, 03-Manipulacao Especial, 04-Perfumaria)
                    msgLog = msgLog + " - Tipo de Medicamento PBM (01–Medicamento, 02-Manipulacao, 03-Manipulacao Especial, 04-Perfumaria)";
                    break;

                case 1041:
                    // Descricao do Medicamento – PBM
                    msgLog = msgLog + " -  Descricao do Medicamento – PBM";
                    break;

                case 1042:
                    // Condicao p/venda: Se 0 obrigatório utilizar preço Funcional Card (PF) Se 1 pode vender por preço inferior ao preço PF 
                    msgLog = msgLog + " - Condicao p/venda: Se 0 obrigatório utilizar preço Funcional Card (PF) Se 1 pode vender por preço inferior ao preço PF ";
                    break;

                case 1043:
                    // Preço funcional card
                    msgLog = msgLog + " - Preço funcional card";
                    break;

                case 1044:
                    // Preço praticado – PBM
                    msgLog = msgLog + " - Preço praticado – PBM";
                    break;

                case 1045:
                    // Status do medicamento – PBM
                    msgLog = msgLog + " - Status do medicamento – PBM";
                    break;

                case 1046:
                    // Quantidade receitada – PBM
                    msgLog = msgLog + " - Quantidade receitada – PBM";
                    break;

                case 1047:
                    // Referência – PBM
                    msgLog = msgLog + " - Referência – PBM";
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
                    // Nome da mãe
                    msgLog = msgLog + " - Nome da mãe";
                    break;

                case 1058:
                    // Dados adicionais – ACSP
                    msgLog = msgLog + " - Dados adicionais – ACSP";
                    break;

                case 1100:
                    // Registro analítico CHECKCHECK
                    msgLog = msgLog + " - Registro analítico CHECKCHECK";
                    break;

                case 1101:
                    // Registro analítico ACSP
                    msgLog = msgLog + " - Registro analítico ACSP";
                    break;

                case 1102:
                    // Registro analítico SERASA
                    msgLog = msgLog  + " - Registro analítico SERASA";
                    break;

                case 1103:
                    // Imagem tela analítica ACSP
                    msgLog = msgLog + " - Imagem tela analítica ACSP";
                    break;

                case 1104:
                    // Imagem tela analítica SERASA
                    msgLog = msgLog + " - Imagem tela analítica SERASA";
                    break;

                case 1105:
                    // Motivo do cancelamento – ACSP
                    msgLog = msgLog+ " - Motivo do cancelamento – ACSP";
                    break;

                case 1106:
                    // Tipo de consulta – ACSP
                    msgLog = msgLog + " - Tipo de consulta – ACSP";
                    break;

                case 1107:
                    // CNPJ Empresa Conveniada
                    msgLog = msgLog + " - CNPJ Empresa Conveniada";
                    break;

                case 1108:
                    // Código da administradora
                    msgLog = msgLog + " - Código da administradora";
                    break;

                case 1109:
                    // Dados tabela Telecheque - ACSP 
                    msgLog = msgLog + " -  Dados tabela Telecheque - ACSP";
                    break;

                case 1110:
                    // Matrícula do motorista – Cartao Combustível
                    msgLog = msgLog + " - Matrícula do motorista – Cartao Combustível";
                    break;

                case 1111:
                    // Placa do veículo – Cartao Combustível 
                    msgLog = msgLog + " - Placa do veículo – Cartao Combustível ";
                    break;

                case 1112:
                    // Quilometragem – Cartao Combustível
                    msgLog = msgLog + " - Quilometragem – Cartao Combustível";
                    break;

                case 1113:
                    // Quantidade de litros – Cartao Combustível
                    msgLog = msgLog + " - Quantidade de litros – Cartao Combustível";
                    break;

                case 1114:
                    // Combustível principal – Cartao Combustível
                    msgLog = msgLog + " - Combustível principal – Cartao Combustível";
                    break;

                case 1115:
                    // Produtos de combustível – Cartao Combustível
                    msgLog = msgLog + " - Produtos de combustível – Cartao Combustível";
                    break;

                case 1116:
                    // Código Produto Host – Cartao Combustível
                    msgLog = msgLog + " - Código Produto Host – Cartao Combustível";
                    break;

                case 1117:
                    // Horímetro – Cartao Combustível
                    msgLog = msgLog + " - Horímetro – Cartao Combustível";
                    break;

                case 1118:
                    // Linha de Crédito – Cartao Combustível
                    msgLog = msgLog + " - Linha de Crédito – Cartao Combustível";
                    break;

                case 1119:
                    // Tipo de Mercadoria – Cartao Combustível
                    msgLog = msgLog + " - Tipo de Mercadoria – Cartao Combustível";
                    break;

                case 1120:
                    // Ramo – Cartao Combustível
                    msgLog = msgLog + " - Ramo – Cartao Combustível";
                    break;

                case 1121:
                    // Casas decimais de preços unitários – Cartao Combustível
                    msgLog = msgLog + " - Casas decimais de preços unitários – Cartao Combustível";
                    break;

                case 1122:
                    // Quantidade máxima de produtos à venda
                    msgLog = msgLog + " - Quantidade máxima de produtos à venda";
                    break;

                case 1123:
                    // Tamanho do código do Produto – Cartao Combustível
                    msgLog = msgLog + " - Tamanho do código do Produto – Cartao Combustível";
                    break;

                case 1124:
                    // Código do veículo – Cartao Combustível
                    msgLog = msgLog + " - Código do veículo – Cartao Combustível";
                    break;

                case 1125:
                    // Nome da Empresa – Cartao Combustível
                    msgLog = msgLog + " - Nome da Empresa – Cartao Combustível";
                    break;

                case 1126:
                    // Casas decimais da quantidade – Cartao Combustível
                    msgLog = msgLog + " - Casas decimais da quantidade – Cartao Combustível";
                    break;

                case 1128:
                    // Lista de Perguntas – Cartao Combustível
                    msgLog = msgLog + " - Lista de Perguntas – Cartao Combustível";
                    break;

                case 1129:
                    // Permite Coleta de Produto – Cartao Combustível
                    msgLog = msgLog + " - Permite Coleta de Produto – Cartao Combustível";
                    break;

                case 1131:
                    // Código do Limite
                    msgLog = msgLog + " - Código do Limite";
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
                    // Nome do Titular 1135 Complemento do Endereço
                    msgLog = msgLog + " - Nome do Titular 1135 Complemento do Endereço";
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
                    // Produto com Valor de Face – Gift
                    msgLog = msgLog + " - Produto com Valor de Face – Gift";
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
                    // Valor acumulado das consultas anteriores, contendo 2 dígitos decimais porém sem o caractere decimal. 
                    msgLog = msgLog + " - Valor acumulado das consultas anteriores, contendo 2 dígitos decimais porém sem o caractere decimal.";
                    break;

                case 1202:
                    // Total de consultas efetuadas no dia.  
                    msgLog = msgLog + " - Total de consultas efetuadas no dia.";
                    break;

                case 1203:
                    // Valor acumulado das consultas no dia, contendo 2 dígitos decimais porém sem o caractere decimal.  
                    msgLog = msgLog + " - Valor acumulado das consultas no dia, contendo 2 dígitos decimais porém sem o caractere decimal.";
                    break;

                case 1204:
                    // Total de consultas de cheques pré-datados realizados no período.  
                    msgLog = msgLog + " - Total de consultas de cheques pré-datados realizados no período";
                    break;

                case 1205:
                    // Valor acumulado de cheques pré-datados, contendo 2 dígitos decimais porém sem o caractere decimal.  
                    msgLog = msgLog + " - Valor acumulado de cheques pré-datados, contendo 2 dígitos decimais porém sem o caractere decimal.";
                    break;

                case 1206:
                    // Vendedor (Usuário) - PBM  
                    msgLog = msgLog + " - Vendedor (Usuário) - PBM ";
                    break;

                case 1207:
                    // Senha – PBM  
                    msgLog = msgLog + " - Senha – PBM ";
                    break;

                case 1208:
                    // Código de Retorno – PBM  
                    msgLog = msgLog + " - Código de Retorno – PBM ";
                    break;

                case 1209:
                    // Origem – PBM  
                    msgLog = msgLog + " - Origem – PBM";
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
                    // Índice MasterKey  
                    msgLog = msgLog + " - Índice MasterKey";
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
                    // Código de resposta do autorizador  
                    msgLog = msgLog + " - Código de resposta do autorizador";
                    break;

                case 2011:
                    // Bin da rede  
                    msgLog = msgLog + " - Bin da rede";
                    break;

                case 2012:
                    // Número serial do CHIP  
                    msgLog = msgLog + " - Número serial do CHIP";
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
                    // Tipo Crédito CDC (1 – CDC Produto; 2 – CDC Serviço)  
                    msgLog = msgLog + " - Tipo Crédito CDC (1 – CDC Produto; 2 – CDC Serviço)";
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
                    // Código consulta cheque (Genérica EMS)  
                    msgLog = msgLog + " - Código consulta cheque (Genérica EMS)";
                    break;

                case 2067:
                    // Mensagem do autorizador a ser exibida junto com o menu de valores (Se o terminal permitir)  
                    msgLog = msgLog + " - Mensagem do autorizador a ser exibida junto com o menu de valores (Se o terminal permitir)";
                    break;

                case 2078:
                    // Código do serviço  
                    msgLog = msgLog + " - Código do serviço";
                    break;

                case 2079:
                    // Valor do serviço  
                    msgLog = msgLog + " - Valor do serviço";
                    break;

                case 2081:
                    // Menu de Produtos  
                    msgLog = msgLog + " - Menu de Produtos";
                    break;

                case 2082:
                    // Nosso número  
                    msgLog = msgLog + " - Nosso número";
                    break;

                case 2083:
                    // Valor total do produto contendo o separador decimal (“,”) e duas casas decimais após a vírgula.  
                    msgLog = msgLog + " - Valor total do produto contendo o separador decimal (“,”) e duas casas decimais após a vírgula.";
                    break;

                case 2086:
                    // Código do Produto - ValeGas  
                    msgLog = msgLog + " - Código do Produto - ValeGas";
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
                    // Número de identificacao da fatura.  
                    msgLog = msgLog + " - Número de identificacao da fatura. ";
                    break;

                case 2090:
                    // Tipo do cartao Lido 
                    msgLog = msgLog + " - Tipo do cartao Lido";
                    break;

                case 2091:
                    // Status da última leitura do cartao 
                    msgLog = msgLog + " - Status da última leitura do cartao";
                    break;

                case 2093:
                    // Código do atendente  
                    msgLog = msgLog + " - Código do atendente";
                    break;

                case 2103:
                    // Indica se foi transacao offline : 1 : Sim  
                    msgLog = msgLog + " - Indica se foi transacao offline : 1 : Sim";
                    break;

                case 2109:
                    // Senha temporária  
                    msgLog = msgLog + " - Senha temporária";
                    break;

                case 2124:
                    // Valor da tarifa da Recarga de Celular  
                    msgLog = msgLog + " - Valor da tarifa da Recarga de Celular";
                    break;

                case 2125:
                    // Número da parcela (2 caracteres) (Hotcard)  
                    msgLog = msgLog + " - Número da parcela (2 caracteres) (Hotcard)";
                    break;

                case 2126:
                    // Seqüencial da transacao (6 caracteres) (Hotcard)  
                    msgLog = msgLog + " - Seqüencial da transacao (6 caracteres) (Hotcard)";
                    break;

                case 2301:
                    // Rodapé do comprovante da via estabelecimento  
                    msgLog = msgLog + "  -Rodapé do comprovante da via estabelecimento";
                    break;

                case 2320:
                    // Código do Depositante – CB  
                    msgLog = msgLog + " - Código do Depositante – CB";
                    break;

                case 2321:
                    // Código do Cliente - CB  
                    msgLog = msgLog + " - Código do Cliente - CB";
                    break;

                case 2322:
                    // Sequencia Cartao – CB  
                    msgLog = msgLog +  " - Sequencia Cartao – CB";
                    break;

                case 2323:
                    // Via Cartao - CB  
                    msgLog = msgLog + " - Via Cartao - CB";
                    break;

                case 2324:
                    // Tipo do Extrato – CB  
                    msgLog = msgLog + " - Tipo do Extrato – CB";
                    break;

                case 2325:
                    // Valor limite de Transferência - CB  
                    msgLog = msgLog + " - Valor limite de Transferência - CB";
                    break;

                case 2326:
                    // Valor limite para coleta de CPF/CNPJ – CB  
                    msgLog = msgLog + " - Valor limite para coleta de CPF/CNPJ – CB";
                    break;

                case 2327:
                    // CPF/CNPJ do Proprietário – CB  
                    msgLog = msgLog + " - CPF/CNPJ do Proprietário – CB";
                    break;

                case 2328:
                    // CPF/CNPJ do Portador – CB 
                    msgLog = msgLog + " - CPF/CNPJ do Portador – CB";
                    break;

                case 2329:
                    // Tipo do documento do Proprietário - CB  
                    msgLog = msgLog + " - Tipo do documento do Proprietário - CB";
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
                    // Valor da Transferência  
                    msgLog = msgLog + " - Valor da Transferência";
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
                    // Quando retornado, atua como uma “dica” para o formato do próximo campo que será coletado. 
                    msgLog = msgLog + " - Quando retornado, atua como uma “dica” para o formato do próximo campo que será coletado.";
                    break;

                case 2361:
                    // Indica que foi efetuada uma transacao de débito para pagamento de carnê  
                    msgLog = msgLog + " - Indica que foi efetuada uma transacao de débito para pagamento de carnê";
                    break;

                case 2362:
                    // Retornado logo após a transacao de consulta de bins. O valor 1 indica que o autorizador é capaz de tratar de forma diferenciada transacao de débito convencional de débito para pagamento de contas.  
                    msgLog = msgLog + " - Retornado logo após a transacao de consulta de bins. O valor 1 indica que o autorizador é capaz de tratar de forma diferenciada transacao de débito convencional de débito para pagamento de contas.";
                    break;

                case 2364:
                    msgLog = msgLog + "!!!!!!!!! Desconhecido [2364]: " + Encoding.UTF8.GetString(buffer);
                    break;

                case 2369:
                    // Pontos a resgatar (numérico sem casa decimal).
                    msgLog = msgLog + " - Pontos a resgatar (numérico sem casa decimal).";
                    break;

                case 2421:
                    //Informa se está habilitada a funcao de coleta de dados adicionais do cliente (0 ou 1)
                    msgLog = msgLog + " **** Informa se está habilitada a funcao de coleta de dados adicionais do cliente (0 ou 1) - Enviando valor 1" + Encoding.UTF8.GetString(buffer);
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
                    // Modalidade para leitura de cartao através da funcao 431. 
                    msgLog = msgLog + " - Modalidade para leitura de cartao através da funcao 431.";
                    break;
 
                case 4000:
                    // Status da Pré-Autorizacao – PBM  
                    msgLog = msgLog + " - Status da Pré-Autorizacao – PBM";
                    break;

                case 4001:
                    // CRF – PBM  
                    msgLog = msgLog + " - CRF – PBM";
                    break;

                case 4002:
                    // UF do CRF – PBM  
                    msgLog = msgLog + " - UF do CRF – PBM";
                    break;

                case 4003:
                    // Tipo de venda – PBM  
                    msgLog = msgLog + " - Tipo de venda – PBM";
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
                    // Nosso número PBM 
                    msgLog = msgLog + " - Nosso número PBM";
                    break;
 
                case 4008:
                    // Percentual de desconto concedido pela administradora (2 casas decimais) 
                    msgLog = msgLog + " - Percentual de desconto concedido pela administradora (2 casas decimais)";
                    break;
 
                case 4016:
                    // Preço bruto – PBM 
                    msgLog = msgLog + " - Preço bruto – PBM";
                    break;

                case 4017:
                    // Preço líquido – PBM  
                    msgLog = msgLog + " - Preço líquido – PBM";
                    break;

                case 4018:
                    // Valor a receber da Loja, em centavos – PBM  
                    msgLog = msgLog + " - Valor a receber da Loja, em centavos – PBM";
                    break;

                case 4019:
                    // Número do lote gerado pela Central – PBM  
                    msgLog = msgLog + " - Número do lote gerado pela Central – PBM";
                    break;

                case 4020:
                    // Valor total a receber da loja – PBM  
                    msgLog = msgLog + " - Valor total a receber da loja – PBM";
                    break;

                case 4021:
                    // Valor total a receber da loja – PBM  
                    msgLog = msgLog + " - Valor total a receber da loja – PBM";
                    break;

                case 4022:
                    // Soma dos valores da Operacao – PBM  
                    msgLog = msgLog + " - Soma dos valores da Operacao – PBM ";
                    break;

                case 4023:
                    // Nome da operadora – PBM  
                    msgLog = msgLog + " - Nome da operadora – PBM";
                    break;

                case 4024:
                    // Nome da empresa conveniada – PBM  
                    msgLog = msgLog + " - Nome da empresa conveniada – PBM";
                    break;

                case 4025:
                    // Quantidade de dependentes – PBM  
                    msgLog = msgLog + " - Quantidade de dependentes – PBM";
                    break;

                case 4026:
                    // Código do dependente – PBM 
                    msgLog = msgLog + " - Código do dependente – PBM";
                    break;
 
                case 4027:
                    // Nome do dependente – PBM  
                    msgLog = msgLog + " - Nome do dependente – PBM";
                    break;

                case 4028:
                    // Valor a receber do conveniado – PBM  
                    msgLog = msgLog + " - Valor a receber do conveniado – PBM";
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
                    // Código da Operadora Selecionada PBM (deverá ser gravado para posterior envio nas demais transações) 
                    msgLog = msgLog + " - Código da Operadora Selecionada PBM (deverá ser gravado para posterior envio nas demais transações)";
                    break;
 
                case 4032:
                    // Campo de retorno de dados livres referentes às transações PBM. 
                    msgLog = msgLog + " - Campo de retorno de dados livres referentes às transações PBM.";
                    break;
 
                case 4033:
                    // Tipo de documento PBM (0 = CRM, 1 = CRO)  
                    msgLog = msgLog + " - Tipo de documento PBM (0 = CRM, 1 = CRO)";
                    break;

                case 4034:
                    // Dados do Resgate - Bônus  
                    msgLog = msgLog + " - Dados do Resgate - Bônus";
                    break;

                case 4039:
                    // Código Resposta PBM (0 = Ok, <>0 = erro)  
                    msgLog = msgLog + " - Código Resposta PBM (0 = Ok, <>0 = erro)";
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
                    // Receita uso contínuo (0 = nao, 1 = sim)  
                    msgLog = msgLog + " - Receita uso contínuo (0 = nao, 1 = sim)";
                    break;

                case 4046:
                    // Produto Manipulado PBM (princípios ativos)  
                    msgLog = msgLog + " - Produto Manipulado PBM (princípios ativos)";
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
                    //EVENTO - Indica que a biblioteca está aguardando a leitura de um cartao 
                    msgLog = msgLog + " [EVENTO] - Indica que a biblioteca está aguardando a leitura de um cartao";
                    break;

                case 5001:
                    //EVENTO - Indica que a biblioteca está esperando a digitacao da senha pelo usuário  
                    msgLog = msgLog + " [EVENTO] - EVENTO - Indica que a biblioteca está esperando a digitacao da senha pelo usuário";
                    break;

                case 5002:
                    //EVENTO - Indica que a biblioteca está esperando a digitacao dos dados de confirmacao positiva pelo usuário  
                    msgLog = msgLog + " [EVENTO] - Indica que a biblioteca está esperando a digitacao dos dados de confirmacao positiva pelo usuário";
                    break;

                case 5003:
                    //EVENTO - Indica que a biblioteca está aguardando a leitura do bilhete único  
                    msgLog = msgLog + " [EVENTO] - Indica que a biblioteca está aguardando a leitura do bilhete único";
                    break;

                case 5004:
                    //EVENTO - Indica que a biblioteca está aguardando a remocao do bilhete único  
                    msgLog = msgLog + " [EVENTO] - Indica que a biblioteca está aguardando a remocao do bilhete único";
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
                    //EVENTO - Conclusao de Recarga de Bilhete Único 5016 Reservado  
                    msgLog = msgLog + " [EVENTO] - Conclusao de Recarga de Bilhete Único 5016 Reservado";
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
                    msgLog = msgLog + " [EVENTO] - Cancelamento da confirmacao da operaçã";
                    break;

                case 5036:
                    //EVENTO - Antes da leitura do cartao magnético  
                    msgLog = msgLog + " [EVENTO] - Antes da leitura do cartao magnético";
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
                    //EVENTO - Atualizacao de tabelas. O conteúdo deste campo varia de acordo com a transacao sendo realizada. 
                    msgLog = msgLog + " [EVENTO] - Atualizacao de tabelas. O conteúdo deste campo varia de acordo com a transacao sendo realizada.";
                    break;

                case 55010:
                    //EVENTO - Início de uma transacao do tipo Correspondente Bancário.
                    msgLog = msgLog + " [EVENTO] - Início de uma transacao do tipo Correspondente Bancário.";
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
