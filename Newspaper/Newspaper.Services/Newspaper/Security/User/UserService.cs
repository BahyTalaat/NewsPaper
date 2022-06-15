using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newspaper.Core.Common;
using Newspaper.Core.Enums;
using Newspaper.Data.DataContext;
using Newspaper.Data.DbModels;
using Newspaper.Data.DbModels.SecuritySchema;
using Newspaper.DTO.User;
using Newspaper.Repositories.NewspaperRepositories.UserAccounts;
using Newspaper.Repositories.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Newspaper.Security.User
{
    public class UserService: IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IReaderAccountRepository _readerAccountRepository;
        private readonly IWriterAccountRepository _writerAccountRepository;

        public UserService(UserManager<ApplicationUser> userManager, 
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            IMapper mapper,
            IReaderAccountRepository readerAccountRepository,
            IWriterAccountRepository writerAccountRepository)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _readerAccountRepository = readerAccountRepository;
            _writerAccountRepository = writerAccountRepository;
        }

        public async Task<AjaxResult> RegisterReader(RegisterReaderDto registerReaderDto)
        {
            ApplicationUser User = null;
            Reader reader=null;
            try
            {
                var strategy = _unitOfWork.CreateExecutionStrategy();
                var taskResult = await strategy.ExecuteAsync(async () =>
                {
                    using (var trans = _unitOfWork.BeginTrainsaction())
                    {
                        //
                        if (await _userManager.FindByEmailAsync(registerReaderDto.Email) != null)
                            return "Email Already Exist";

                        //create identity user
                        User = new ApplicationUser()
                        {
                            Address = registerReaderDto.Address,
                            FullName = registerReaderDto.FullName,
                            PersonalImagePath = registerReaderDto.ProfilePicture,
                            Email = registerReaderDto.Email,
                            PhoneNumber = registerReaderDto.Phone,
                            UserName = registerReaderDto.Email,
                            NormalizedEmail = registerReaderDto.Email.ToUpper(),
                            NormalizedUserName = registerReaderDto.Email.ToUpper()
                        };
                        IdentityResult result = await _userManager.CreateAsync(User, registerReaderDto.Password);
                        if (!result.Succeeded)
                            return String.Join(",", result.Errors.Select(x => x.Description).ToList());

                        // add role

                        List<string> roles = new List<string>();
                        roles.Add(EnAppMainRoles.Reader.ToString());

                        var AddRoleResult = await _userManager.AddToRolesAsync(User, roles.ToArray());

                        if (!AddRoleResult.Succeeded)
                        {
                            throw new Exception("Fail to save role");
                        }


                        reader = _mapper.Map<Reader>(registerReaderDto);
                        reader.Id = User.Id;
                        reader.LMD = DateTime.Now;
                        reader.UID = User.Id;

                        _readerAccountRepository.Add(reader);
                        _unitOfWork.Save();
                        trans.Commit();

                        var res = new AjaxResult();
                        res.AddParameter("Message", "Account for Reader Created");
                        return res;
                    }
                });
                return taskResult;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AjaxResult> RegisterWriter(RegisterWriterDto registerWriterDto)
        {
            ApplicationUser User = null;
            Writer writer = null;
            try
            {
                var strategy = _unitOfWork.CreateExecutionStrategy();
                var taskResult = await strategy.ExecuteAsync(async () =>
                {
                    using (var trans = _unitOfWork.BeginTrainsaction())
                    {
                        //
                        if (await _userManager.FindByEmailAsync(registerWriterDto.Email) != null)
                            return "Email Already Exist";

                        //create identity user
                        User = new ApplicationUser()
                        {
                            Address = registerWriterDto.Address,
                            FullName = registerWriterDto.FullName,
                            PersonalImagePath = registerWriterDto.ProfilePicture,
                            Email = registerWriterDto.Email,
                            PhoneNumber = registerWriterDto.Phone,
                            UserName = registerWriterDto.Email,
                            NormalizedEmail = registerWriterDto.Email.ToUpper(),
                            NormalizedUserName = registerWriterDto.Email.ToUpper()
                        };
                        IdentityResult result = await _userManager.CreateAsync(User, registerWriterDto.Password);
                        if (!result.Succeeded)
                            return String.Join(",", result.Errors.Select(x => x.Description).ToList());

                        // add role

                        List<string> roles = new List<string>();
                        roles.Add(EnAppMainRoles.Writer.ToString());

                        var AddRoleResult = await _userManager.AddToRolesAsync(User, roles.ToArray());

                        if (!AddRoleResult.Succeeded)
                        {
                            throw new Exception("Fail to save role");
                        }


                        writer = _mapper.Map<Writer>(registerWriterDto);
                        writer.Id = User.Id;
                        writer.LMD = DateTime.Now;
                        writer.UID = User.Id;

                        _writerAccountRepository.Add(writer);
                        _unitOfWork.Save();
                        trans.Commit();

                        var res = new AjaxResult();
                        res.AddParameter("Message", "Account for Writer Created");
                        return res;
                    }
                });
                return taskResult;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
