using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App_course.Models.ViewModels
{
    public class ProductVM
    {
        public Product product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public IEnumerable<SelectListItem> ApplicationTypeSelectList { get; set; }
    }
}
