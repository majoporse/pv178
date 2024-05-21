using InformationSystemHZS.Collections;
using InformationSystemHZS.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystemHZS.Models.Interfaces;

namespace InformationSystemHZS.Models.Entities
{
    public class Station : IBaseModel
    {
        public string Callsign { get; set; }
        public Position PositionDto { get; set; }
        public string Name { get; set; }

        public CallsignEntityMap<Unit> unitsMap;

        public Station(StationDto s)
        {
            Callsign = s.Callsign;
            PositionDto = new Position( s.PositionDto.X, s.PositionDto.Y);
            Name = s.Name;
            List<Unit> newUnits = new List<Unit>();
            foreach (UnitDto u in s.Units)
            {
                newUnits.Add(new Unit(u));
            }
            unitsMap = new CallsignEntityMap<Unit>(newUnits);
        }
    }
}
