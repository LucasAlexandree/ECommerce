using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Category
    {
        [Key]
        [Display(Name = "Categoria Id")]
        public int CategoryId { get; set; }
        [MaxLength(100, ErrorMessage = "O campo Nome recebe no máximo 50 caracteres")]
        [Required(ErrorMessage = "O campo Categoria é requerido!!")]
        [Display(Name = "Categoria")]
        [Index("Category_Description_CompanyId_Index", 2, IsUnique = true)]
        public string Description{ get; set; }


        [Required(ErrorMessage = "O campo Distruibuidora é requerido!!")]
        [Index("Category_Description_CompanyId_Index", 1, IsUnique = true)]
        [Display(Name = "Distribuidoras")]
        [Range(1, double.MaxValue, ErrorMessage = "Selecione uma Distruibuidora")]
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Product> Product { get; set; }

    }
}