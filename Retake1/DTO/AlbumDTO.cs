using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Retake1.DTO
{
    public class AlbumDTO
    {
        [Required]
        public int IdAlbum { get; set; }
        [Required]
        public string AlbumName { get; set; }
        public DateTime PublishDate { get; set; }
        [Required]
        public string MusicLabel { get; set; }
        [Required]
        public List<TrackDTO> songs { get; set; }

        public AlbumDTO(int id, string name, DateTime publish, string label, List<TrackDTO> list)
        {
            IdAlbum = id;
            AlbumName = name;
            PublishDate = publish;
            MusicLabel = label;
            songs = list;
        }
    }
}
