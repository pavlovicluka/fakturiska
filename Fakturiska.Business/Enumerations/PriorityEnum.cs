using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakturiska.Business.Enumerations
{
    public enum PriorityEnum
    {
        [Display(Name = "NIZAK PRIORITET")]
        Nizak = 0,
        [Display(Name = "NORMALAN PRIORITET")]
        Normalan,
        [Display(Name = "VAZNO")]
        Vazno,
        [Display(Name = "VRLO VAZNO!")]
        VrloVazno,
    }
}
