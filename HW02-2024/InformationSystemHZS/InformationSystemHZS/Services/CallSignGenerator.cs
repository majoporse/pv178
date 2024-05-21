using InformationSystemHZS.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using InformationSystemHZS.Models.HelperModels;
using InformationSystemHZS.Models.Entities;

namespace InformationSystemHZS.Services
{
    public static class CallSignGenerator
    {
        static char GetFirstLetter<T>(List<T> obj) where T: IBaseModel
        {
            if (obj is List<StationDto> || obj is List<Station>)
                return 'S';
            if (obj is List<UnitDto> || obj is List<Unit>)
                return 'J';
            if (obj is List<MemberDto> || obj is List<Member>)
                return 'H';

            throw new ArgumentException();
        }

        public static string? GenerateCallSign<T>(List<T> siblings) where T: IBaseModel//IEnumerable because of covariance
        {
            HashSet<int> signs = new HashSet<int>();
            foreach(var s in siblings)
            {
                signs.Add(int.Parse(s.Callsign.Substring(1)));
            }
            var max = signs.Count > 0 ?  signs.Max() : 0;
            if (max < 100)
                return $"{GetFirstLetter(siblings)}{max + 1:00}";

            return default;

        }   
    }
}
