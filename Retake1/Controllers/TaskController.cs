﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Retake1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retake1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly IDbService _databaseService;

        public TaskController(IDbService databaseService)
        {
            _databaseService = databaseService;
        }

        

        [HttpGet("{idAlbum}")]
        public async Task<IActionResult> GetAlbum([FromRoute] int idAlbum)
        {

            if (!await _databaseService.DoesAlbumExistAsync(idAlbum))
            {
                return NotFound("Album does not exist.");
            }
            var result = await _databaseService.GetAlbumAsync(idAlbum);
            return Ok(result);
        }
    }
}
