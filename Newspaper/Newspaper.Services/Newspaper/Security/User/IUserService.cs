using Newspaper.Core.Common;
using Newspaper.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Newspaper.Security.User
{
    public interface IUserService
    {
        Task<AjaxResult> RegisterWriter(RegisterWriterDto registerWriterDto);

        Task<AjaxResult> RegisterReader(RegisterReaderDto registerReaderDto);
        
    }
}
