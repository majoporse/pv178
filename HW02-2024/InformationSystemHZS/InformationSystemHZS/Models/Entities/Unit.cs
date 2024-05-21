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
    public class Unit : IBaseModel
    {
        public string Callsign { get; set; }
        public string StationCallsign { get; set; }
        public string State { get; set; }
        public VehicleDto VehicleDto { get; set; }

        public CallsignEntityMap<Member> membersMap;
        public Unit(UnitDto u)
        {
            Callsign = u.Callsign;
            StationCallsign = u.StationCallsign;
            State = u.State;
            VehicleDto = u.VehicleDto;
            var newMembers = new List<Member>();
            foreach (MemberDto m in u.Members)
            {
                newMembers.Add(new Member(m));
            }
            membersMap = new CallsignEntityMap<Member>(newMembers);
        }
    }
}
