using Microsoft.Extensions.Configuration;
using Retake1.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Retake1.Services
{
    public class DbService : IDbService
    {
        private readonly string _connectionString;
        public DbService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


public async Task<List<TrackDTO>> GetTracksAsync(int id)
            {
            var result = new List<TrackDTO>();
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT IdTrack, TrackName, Duration FROM Track WHERE IdAlbum = @idAlbum ", con);
                cmd.Parameters.AddWithValue("idAlbum", id);
                await con.OpenAsync();
                var dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    var singleTrack = new TrackDTO(Convert.ToInt32(dr[0]), dr[1].ToString(), float.Parse(dr[2].ToString()));
                    result.Add(singleTrack);
                }
                return result;
            }
        }
        public async Task<AlbumDTO> GetAlbumAsync(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT a.IdAlbum, a.AlbumName, a.PublishDate, m.Name FROM MusicLabel m, Album a WHERE a.IdAlbum = @idalbum AND a.IdMusicLabel = m.IdMusicLabel", con);
                cmd.Parameters.AddWithValue("idAlbum", id);
                await con.OpenAsync();
                var dr = await cmd.ExecuteReaderAsync();
                await dr.ReadAsync();
                var songs = await GetTracksAsync(id);
                
                var result = new AlbumDTO(Convert.ToInt32(dr[0]), dr[1].ToString(),Convert.ToDateTime(dr[2]), dr[3].ToString(), songs);
                return result;
            }
        }
public async Task<bool> DoesAlbumExistAsync(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT IIF(COUNT(1) > 0, 1, 0) FROM Album where IdAlbum = @idAlbum", con);
                cmd.Parameters.AddWithValue("idAlbum", id);
                await con.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                Console.WriteLine(result);
                return Convert.ToBoolean(result);
            }
        }
    }
}
