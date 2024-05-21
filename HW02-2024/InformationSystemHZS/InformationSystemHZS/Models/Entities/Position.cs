using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystemHZS.Models.HelperModels;


namespace InformationSystemHZS.Models.Entities
{
    public class Position 
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Position(PositionDto dto)
        {
            X = dto.X;
            Y = dto.Y;
        }
    }
}
