using System;
using System.Collections.Generic;
namespace App_course.Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Product = new Product();
        }

        public Product Product { get; set; }
        public bool ExistsInCart { get; set; }

    }
}
