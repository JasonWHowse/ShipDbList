using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipDbListApplication.Models {
    public class Ships {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ShipName { get; set; }
        public double Weight { get; set; }

        public int HardPoints { get; set; }

        public char Type { get; set; }

        public char Class { get; set; }

        [NotMapped]
        public string ClassString { get; set; }
        [NotMapped]
        public string TypeString { get; set; }
    }
}
