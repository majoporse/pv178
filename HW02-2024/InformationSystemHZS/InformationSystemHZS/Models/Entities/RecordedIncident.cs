using InformationSystemHZS.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSystemHZS.Models.Entities
{
    public class RecordedIncident
    {
        public string Type { get; set; }
        public Position Location { get; set; }
        public string Description { get; set; }
        public string IncidentStartTIme { get; set; }
        public string AssignedStation { get; set; }
        public string AssignedUnit { get; set; }
        public RecordedIncident(RecordedIncidentDto dto)
        {
            Type = dto.Type;
            Location = new Position(dto.Location);
            Description = dto.Description;
            IncidentStartTIme = dto.IncidentStartTIme;
            AssignedStation = dto.AssignedStation;
            AssignedUnit = dto.AssignedUnit;
        }
        public RecordedIncident() { }
    }
}
