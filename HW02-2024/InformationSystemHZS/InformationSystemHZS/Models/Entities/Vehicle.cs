using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSystemHZS.Models.Entities
{
    public class Vehicle
    {
        public required string Name { get; set; }
        public required string Type { get; set; }
        public double FuelConsumption { get; set; }
        public int Speed { get; set; }
        public int Capacity { get; set; }
    }
}
