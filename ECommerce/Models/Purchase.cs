using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Fornecedor")]
        public int ProviderId { get; set; }


        [Required(ErrorMessage = "O campo Companhia é requerido!!")]
        [Display(Name = "Companhia")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "O campo Armazéns é requerido!!")]
        [Display(Name = "Armazéns")]
        public int WareHouseId { get; set; }


        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Estado")]
        public int StateId { get; set; }

        [Display(Name = "Data")]
        [Required(ErrorMessage = "The field {0} is required")]
        // [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }


        [Display(Name = "Anotação")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public virtual Provider Provider { get; set; }

        public virtual State State { get; set; }

        public virtual Company Company { get; set; }

        public virtual WareHouse WareHouse { get; set; }

        public virtual ICollection<PurchaseDetails> PurchaseDetails { get; set; }
    }
}