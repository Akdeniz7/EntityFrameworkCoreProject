using Microsoft.EntityFrameworkCore;
using Proje1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Proje1.FormModel
{
    public class SalerFormModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }  
    }

    public class MultipleSaler
    {
        public List<Saler> multSaler { get; set; } = new List<Saler>();
    }
}
