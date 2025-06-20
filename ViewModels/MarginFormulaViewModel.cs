﻿using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.ViewModels
{
    public class MarginFormulaViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Margin Type")]
        public string MarginType { get; set; }

        [Display(Name = "Margin Formula")]
        public string Formula { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified At")]
        public DateTime? ModifiedAt { get; set; }
    }
}
