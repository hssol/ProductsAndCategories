using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ProdCat.Models
{
    public class Category
    {
        [Key]
        public int CategoryId {get;set;}
        ////////////////////
        public string Name {get;set;}
        ////////////////////
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        ////////////////////
        public List<Association> ProductsWithCategories {get;set;}
    }
}