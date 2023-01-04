using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class WareHouse
    {
        [Key]
        [Display(Name = "Armazém ID")]
        public int WareHouseId { get; set; }


        [Required(ErrorMessage = "O campo Companhia é requerido!!")]
        [Display(Name = "Companhia")]
        [Index("WareHouse_CompanyId_Name_Index", 1, IsUnique = true)]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "O campo Armazém é requerido!!")]
        [MaxLength(250, ErrorMessage = "O campo Armazém recebe no máximo 250 caracteres")]
        [Display(Name = "Armazém")]
        
        [Index("WareHouse_CompanyId_Name_Index", 2, IsUnique = true)]
        public string Name { get; set; }

        
        [Required(ErrorMessage = "O campo Telefone é requerido!!")]
        [MaxLength(50, ErrorMessage = "O campo Nome recebe no máximo 50 caracteres")]
        [Display(Name = "Telefone")]
        //[Index("Departament_Name_Index", IsUnique = true)]
        [DataType(DataType.PhoneNumber)]

        public string Phone { get; set; }



        [Required(ErrorMessage = "O campo Endereço é requerido!!")]
        [MaxLength(100, ErrorMessage = "O campo Nome recebe no máximo 50 caracteres")]
        [Display(Name = "Endereço")]

        public string Address { get; set; }

        
        [Required(ErrorMessage = "O campo Departamento é requerido!!")]
        [Display(Name = "Departamento")]
        public int DepartamentsId { get; set; }


        [Required(ErrorMessage = "O campo Cidade é requerido!!")]
        [Display(Name = "Cidade")]
        public int CityId { get; set; }

        


        public virtual Departaments Departments { get; set; }
        public virtual City Cities { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }

        public virtual ICollection<Purchase> Purchase { get; set; }
    }

}