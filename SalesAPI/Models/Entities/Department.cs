using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SalesAPI.Models.Entities
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [StringLength(128, MinimumLength = 3, ErrorMessage = "{0} length must be between {2} and {1}")]
        public String Name { get; set; }

        [JsonIgnore]
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public Department() { }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            this.Sellers.Add(seller);
        }
        public void RemoveSeller(Seller seller)
        {
            this.Sellers.Remove(seller);
        }

        public double TotalSales(DateTime startDate, DateTime endDate)
        {
            return Sellers.Sum(seller => seller.TotalSales(startDate, endDate));
        }

    }
}
