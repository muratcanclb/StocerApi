using Meb.Core.Domain.Search;
using Intra.Api.Data;
using Intra.Api.Domain.Dto;
using Intra.Api.Domain.Entities;
using Intra.Api.Infrastructure.Cache;
using Intra.Api.Infrastructure.Constants;
using Intra.Api.Infrastructure.Helpers;
using Intra.Api.Models;
using Intra.Api.Services.PasswordHash;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Intra.Api.Controllers.Api
{
    [Route("api/[controller]")]
    public class ActivitiesController : BaseAuthController
    {
        #region Constants

        #endregion

        #region Fields
        private readonly PostgreRepository<Activities> _ActivitiesRepository;
        private readonly PostgreRepository<UsersRoles> _UsersRolesRepository;
        private readonly PostgreRepository<Users> _UsersRepository;
        private readonly IHttpContextAccessor _httpContext;

        #endregion

        #region Ctor
        public ActivitiesController(
            PostgreRepository<Activities> activitiesRepository,
            PostgreRepository<UsersRoles> usersrolesRepository,
            PostgreRepository<Users>usersRepository,
            IHttpContextAccessor httpContext)
        {
            _ActivitiesRepository = activitiesRepository;
            _UsersRolesRepository = usersrolesRepository;
            _UsersRepository = usersRepository;
            _httpContext = httpContext;
            GetUserId(httpContext, usersRepository);

        }


        #endregion

        #region Metods

        [HttpGet]
        public async Task<object>GetById(Guid id)
        {
            var entity = _ActivitiesRepository.Table.FirstOrDefault(t => t.Id == id);
            if (entity.IsArchive == true)
            {
                return new ResponseModel
                {
                    Status = false,
                    ErrorCode = ConstantError.NO_RECORD_FOUND,
                };
            }
            return entity;
        }

        [Route("")]
        [HttpPost]
        public async Task<ResponseModel>Insert([FromBody] ActivitiesPostDto data)
        {
            if (_ActivitiesRepository.Table.FirstOrDefault(t => t.Title == data.Title && t.IsArchive==false)!=null)
            {
                return new ResponseModel
                {
                    Status = false,
                    ErrorCode = ConstantError.THIS_NAME_ALREADY_USED,
                };
            }
            Activities entity = new Activities()
            {
                Title = data.Title,
                StartedDate = data.StartedDate,
                EndedDate = data.EndedDate,
                Repeat = data.Repeat,
                Color = data.Color,
                RolesId = data.RolesId,
                Location = data.Location,
                Url = data.Url,
                Statement = data.Statement,
                //Generic
                IsStatus = data.IsStatus,
                IsArchive = false,
                CreatedBy = LOGIN_USER_ID,
                CreatedDate = DateTime.Now,
                UpdateBy = LOGIN_USER_ID,
                UpdateDate = DateTime.Now,
            };
            _ActivitiesRepository.Insert(entity);
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.INSERT_SUCCESS,
            };  
        }

        [Route("")]
        [HttpPut]
        public async Task<ResponseModel>Update([FromBody] ActivitiesDto data)
        {
            if (_ActivitiesRepository.Table.FirstOrDefault(t => t.Title == data.Title && t.Id != data.Id && !t.IsArchive)!= null)
            {
                    return new ResponseModel
                    {
                        Status = false,
                        ErrorCode = ConstantError.THIS_NAME_ALREADY_USED,
                    };
             }
            var entity = _ActivitiesRepository.Table.FirstOrDefault(t => t.Id == data.Id);
            if (entity.IsArchive==true)
            {
                return new ResponseModel
                    {
                        Status = false,
                        ErrorCode = ConstantError.NO_RECORD_FOUND,
                    };
            }
            entity.Title = data.Title;
            entity.StartedDate = data.StartedDate;
            entity.EndedDate = data.EndedDate;
            entity.Repeat = data.Repeat;
            entity.Color = data.Color;
            entity.RolesId = data.RolesId;
            entity.Location = data.Location;
            entity.Url = data.Url;
            entity.Statement = data.Statement;
            //Generic
            entity.IsStatus = data.IsStatus;
            entity.IsArchive = false;
            entity.CreatedBy = LOGIN_USER_ID;
            entity.CreatedDate = DateTime.Now;
            entity.UpdateBy = LOGIN_USER_ID;
            entity.UpdateDate = DateTime.Now;
            _ActivitiesRepository.Update(entity);
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.UPDATE_SUCCESS,
            };
        }

        [Route("")]
        [HttpDelete]
        public async Task<ResponseModel>Delete([FromBody] ActivitiesDto data)
        {
            var entity = _ActivitiesRepository.Table.FirstOrDefault(t => t.Id == data.Id);
            if (entity.IsArchive == true)
            {
                return new ResponseModel
                {
                    Status = false,
                    ErrorCode = ConstantError.NO_RECORD_FOUND,
                };
            }
            entity.UpdateBy = LOGIN_USER_ID;
            entity.UpdateDate = DateTime.Now;
            entity.IsArchive = true;
            _ActivitiesRepository.Update(entity);
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.DELETE_SUCCESS,
            };
        }

        [Route("Search")]
        [HttpPost]
        public object GetByParameters([FromBody] IEnumerable<SearchModel> searchModels, int pagesize, int pagenumber, string sort)
        {
            return new SearchHelper<ActivitiesDto>().Search(HttpContext, GetQuery(), searchModels, pagesize, pagenumber, sort);
        }
        private IQueryable<ActivitiesDto>GetQuery()
        {

            return from a in _ActivitiesRepository.Table
                   join r in _UsersRolesRepository.Table on a.RolesId equals r.Id into arJoin
                   from r in arJoin.DefaultIfEmpty()
                   where
                       a.IsArchive == false && r.IsArchive == false
                   select new ActivitiesDto
                   {
                       Id = a.Id,
                       Title = a.Title,
                       StartedDate = a.StartedDate,
                       EndedDate = a.EndedDate,
                       Repeat = a.Repeat,
                       Color = a.Color,
                       Location = a.Location,
                       Statement = a.Statement,
                       Url = a.Url,
                       IsStatus = a.IsStatus,
                       //LeftJoin
                       RolesId = a.RolesId
                       
                   };

        }
       
        #endregion
    }
}
