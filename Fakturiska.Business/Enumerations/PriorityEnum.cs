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
        Nizak = 1,
        [Display(Name = "NORMALAN PRIORITET")]
        Normalan,
        [Display(Name = "VAZNO")]
        Vazno,
        [Display(Name = "VRLO VAZNO!")]
        VrloVazno,
    }

    public static class PriorityMethods
    {

        public static String GetString(PriorityEnum? priority)
        {
            switch (priority)
            {
                case PriorityEnum.Nizak:
                    return "NIZAK PRIORITET";
                case PriorityEnum.Normalan:
                    return "NORMALAN PRIORITET";
                case PriorityEnum.Vazno:
                    return "VAZNO";
                case PriorityEnum.VrloVazno:
                    return "VRLO VAZNO!";
                default:
                    return "";
            }
        }
    }
}
