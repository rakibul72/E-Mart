//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_Mart.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Admin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Admin()
        {
            this.Orders = new HashSet<Order>();
        }

        public int AdminID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Admin Name: ")]
        [StringLength(40, ErrorMessage = "Name can be atmost 40 characters!")]
        public string AdminName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail: ")]
        public string AdminEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password: ")]
        [MaxLength(12, ErrorMessage = "Password can be atmost 12 characters!")]
        [MinLength(6, ErrorMessage = "Password can be atmost 6 characters!")]
        public string AdminPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Retype Password: ")]
        [Compare("AdminPassword", ErrorMessage = "Password and Confirm Password not matched!")]
        public string ConfirmAdminPassword { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}