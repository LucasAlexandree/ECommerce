using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Produto Id")]
        public int ProductId { get; set; }


        [Required(ErrorMessage = "O campo Distruibuidora é requerido!!")]
        [Index("Product_Description_CompanyId_Index", 1, IsUnique = true)]
        [Display(Name = "Distribuidoras")]
        [Range(1, double.MaxValue, ErrorMessage = "Selecione uma Distruibuidora")]
        [Index("Product_BarCode_CompanyId_Index", 1, IsUnique = true)]
        public int CompanyId { get; set; }

        [MaxLength(100, ErrorMessage = "O campo Nome recebe no máximo 50 caracteres")]
        [Required(ErrorMessage = "O campo Categoria é requerido!!")]
        [Display(Name = "Produto")]
        [Index("Product_Description_CompanyId_Index", 2, IsUnique = true)]
        public string Description { get; set; }


        [MaxLength(100, ErrorMessage = "O campo código de barras recebe no máximo 100 caracteres")]
        [Required(ErrorMessage = "O campo Cód Barras é requerido!!")]
        [Display(Name = "Código de Barras")]
        [Index("Product_BarCode_CompanyId_Index", 2, IsUnique = true)]
        public string BarCode { get; set; }

        [Required(ErrorMessage = "O campo Taxa é requerido!!")]
        [Range(1, double.MaxValue, ErrorMessage = "Selecione uma taxa")]
        [Display(Name = "Taxa")]
        public int TaxId { get; set; }

        [Required(ErrorMessage = "O campo Categoria é requerido!!")]
        [Range(1, double.MaxValue, ErrorMessage = "Selecione uma Categoria")]
        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Estoque")]


        public double Stock { get {
                if(Inventory == null) { 
                return 0;
                }
                else
                {
                    return Inventory.Sum(i => i.Stock);
                }
            } }


        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo Valor é requerido!!")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
       
        public decimal Price { get; set; }


        [Display(Name = "Imagem")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [NotMapped]
        [Display(Name = "Imagem")]
        public HttpPostedFileBase ImageFile { get; set; }


        
        [Display(Name = "Anotações")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }



        public virtual Company Company { get; set; }
        public virtual Tax Tax { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual ICollection<OrderDetailTmp> OrderDetailTmp { get; set; }

        public virtual ICollection<PurchaseDetails> PurchaseDetails { get; set; }

        public virtual ICollection<PurchaseDetailTmp> PurchaseDetailTmps { get; set; }

    }
}