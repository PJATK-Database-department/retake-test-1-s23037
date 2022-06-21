using Retake1.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retake1.Services
{
    public interface IDbService
    {
        Task<AlbumDTO> GetAlbumAsync(int id);
        Task<List<TrackDTO>> GetTracksAsync(int id);
        Task<bool> DoesAlbumExistAsync(int id);
        Task RemoveMusicianAsync(int id);
        Task<bool> DoesTrackHaveAlbum(int id);
        Task<bool> DoesMusicianExistAsync(int id);
    }
}
