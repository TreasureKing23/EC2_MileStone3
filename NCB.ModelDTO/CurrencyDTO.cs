using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCB.ModelDTO
{


    public class CurrencyDTO : CreateCurrencyDTO
    {
        public int Id { get; set; }
    }
    public class CreateCurrencyDTO
    {

        public string Name { get; set; }
        [StringLength(3)]

        public string ShortName { get; set; }
    }
}
