using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newspaper.Core.Common;
using Newspaper.DTO.User;
using Newspaper.Services.Newspaper.Security.User;
using System;
using System.Threading.Tasks;

namespace NewsPaperApp.API.Area.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ReaderRegister")]
        public async Task<AjaxResult> ReaderRegister([FromBody] RegisterReaderDto registerReaderDto)
        {
            try
            {
                var res = await _userService.RegisterReader(registerReaderDto);
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [AllowAnonymous]
        [HttpPost, DisableRequestSizeLimit]
        [Route("WriterRegister")]
        public async Task<AjaxResult> WriterRegister([FromBody] RegisterWriterDto registerWriterDto)
        {
            try
            {
                var res = await _userService.RegisterWriter(registerWriterDto);
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
