using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Retake1.DTO
{
    public class TrackDTO
    {
        [Required]
        public int IdTrack { get; set; }
        [Required]
        public string TrackName { get; set; }
        [Required]
        public float Duration { get; set; }
        
        public TrackDTO(int id, string name, float dur)
        {
            IdTrack = id;
            TrackName = name;
            Duration = dur;
        }

    }
}
