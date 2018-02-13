using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDVTEF.Entity
{
    public class Vendas
    {
        public int ID { get; set; }
        public int OPERADORA { get; set; }
        public int VALOR { get; set; }
        public DateTime DATA_HORA { get; set; }
        public int QTD_PARC { get; set; }
        public string STATUS { get; set; }
        public int NUMERO_TRANSACAO { get; set; }
    }
}
