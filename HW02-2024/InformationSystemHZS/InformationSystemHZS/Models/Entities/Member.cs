using InformationSystemHZS.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystemHZS.Models.HelperModels;

namespace InformationSystemHZS.Models.Entities
{
    public class Member : IBaseModel
    {
        public string Callsign { get; set; }
        public string UnitCallsign { get; set; }
        public string Name { get; set; }

        public Member(MemberDto m)
        {
            UnitCallsign = m.UnitCallsign;
            Name = m.Name;
            Callsign = m.Callsign;
        }
        public Member() { }
    }
}
