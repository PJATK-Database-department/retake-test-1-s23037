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
        public async Task<bool> DoesMusicianExistAsync(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT IIF(COUNT(1) > 0, 1, 0) FROM Musician where IdMusician = @idMusician", con);
                cmd.Parameters.AddWithValue("idMusician", id);
                await con.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                Console.WriteLine(result);
                return Convert.ToBoolean(result);
            }
        }
        public async Task<bool> DoesTrackHaveAlbum(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT IIF(COUNT(1) > 0, 1, 0) FROM Track t, Musician m, Musician_Track mt  where t.IdTrack = mt.IdTrack AND m.IdMusician = mt.IdMusician and t.IdAlbum IS NOT NULL AND m.IdMusician = @idMusician", con);
                cmd.Parameters.AddWithValue("idMusician", id);
                await con.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                Console.WriteLine(result);
                return Convert.ToBoolean(result);
            }
        }
        public async Task RemoveMusicianAsync(int id)
        {
            //Design an endpoint that will allow you to remove a specific musician.
            //Musician can be deleted only if he is involved in creating songs that have not yet appeared in the target albums.
            //Otherwise, stop processing the request and inform the user using the appropriate error code.
            //The deletion should be done withoutthe use of cascade constraints.
            using (var con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    var cmdTasks = new SqlCommand("DELETE FROM Musician WHERE IdMusician = @idMusician", con, tran);
                    cmdTasks.Parameters.AddWithValue("idMusician", id);
                    await cmdTasks.ExecuteNonQueryAsync();
                    await tran.CommitAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }
            }
        }
        
    }
}
