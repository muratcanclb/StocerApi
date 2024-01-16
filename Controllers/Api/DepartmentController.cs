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
    public class DepartmentController : BaseAuthController
    {
        #region Constants

        #endregion

        #region Fields
        private readonly PostgreRepository<Department> _DepartmentRepository;
        private readonly PostgreRepository<UsersRoles> _UsersRolesRepository;
        private readonly PostgreRepository<Users> _UsersRepository;
        private readonly IHttpContextAccessor _httpContext;

        #endregion

        #region Ctor
        public DepartmentController(
            PostgreRepository<Department> departmentRepository,
            PostgreRepository<UsersRoles> usersrolesRepository,
            PostgreRepository<Users>usersRepository,
            IHttpContextAccessor httpContext)
        {
            _DepartmentRepository = departmentRepository;
            _UsersRolesRepository = usersrolesRepository;
            _UsersRepository = usersRepository;
            _httpContext = httpContext;
            GetUserId(httpContext, usersRepository);

        }


        #endregion

        #region Metods
        [Route("")]
        [HttpGet]
        public async Task<object>GetById(Guid id)
        {
            var entity = _DepartmentRepository.Table.FirstOrDefault(t => t.Id == id);
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

        [Route("all")]
        [HttpGet]
        public object Get()
        {
            //var entity = _ProjectsRepository.Table.Where(t => t.IsArchive==false).ToList();
            var entity = GetQuery().ToList();
            return entity;
        }

        [Route("selected")]
        [HttpGet]
        public object GetSelect()
        {
            //var entity = _ProjectsRepository.Table.Where(t => t.IsArchive==false).ToList();
            var entity = GetQuerySelected().ToList();
            return entity;
        }

        [Route("")]
        [HttpPost]
        public async Task<ResponseModel>Insert([FromBody] DepartmentPostDto data)
        {
            if (_DepartmentRepository.Table.FirstOrDefault(t => t.Name == data.Name && t.IsArchive==false)!=null)
            {
                return new ResponseModel
                {
                    Status = false,
                    ErrorCode = ConstantError.THIS_NAME_ALREADY_USED,
                };
            }
            Department entity = new Department()
            {
                Name = data.Name,
                Address = data.Address,
                Phone = data.Phone,
                Status = data.Status,
                //Generic
                IsArchive = false,
                CreatedBy = LOGIN_USER_ID,
                CreatedDate = DateTime.Now,
                UpdateBy = LOGIN_USER_ID,
                UpdateDate = DateTime.Now,
            };
            _DepartmentRepository.Insert(entity);
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.INSERT_SUCCESS,
            };  
        }

        [Route("")]
        [HttpPut]
        public async Task<ResponseModel>Update([FromBody] DepartmentDto data)
        {
            if (_DepartmentRepository.Table.FirstOrDefault(t => t.Name == data.Name && t.Id != data.Id && !t.IsArchive)!= null)
            {
                    return new ResponseModel
                    {
                        Status = false,
                        ErrorCode = ConstantError.THIS_NAME_ALREADY_USED,
                    };
             }
            var entity = _DepartmentRepository.Table.FirstOrDefault(t => t.Id == data.Id);
            if (entity.IsArchive==true)
            {
                return new ResponseModel
                    {
                        Status = false,
                        ErrorCode = ConstantError.NO_RECORD_FOUND,
                    };
            }
            entity.Name = data.Name;
            entity.Address = data.Address;
            entity.Phone = data.Phone;
            entity.Status = data.Status;
            //Generic
            entity.IsArchive = false;
            entity.CreatedBy = LOGIN_USER_ID;
            entity.CreatedDate = DateTime.Now;
            entity.UpdateBy = LOGIN_USER_ID;
            entity.UpdateDate = DateTime.Now;
            _DepartmentRepository.Update(entity);
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.UPDATE_SUCCESS,
            };
        }

        [Route("")]
        [HttpDelete]
        public async Task<ResponseModel>Delete([FromBody] DepartmentDto data)
        {
            var entity = _DepartmentRepository.Table.FirstOrDefault(t => t.Id == data.Id);
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
            _DepartmentRepository.Update(entity);
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.DELETE_SUCCESS,
            };
        }

        [Route("Open")]
        [HttpDelete]
        public async Task<ResponseModel> OpenDepartment([FromBody] DepartmentDto data)
        {
            var entity = _DepartmentRepository.Table.FirstOrDefault(t => t.Id == data.Id);
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
            entity.Status = true;
            _DepartmentRepository.Update(entity);
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.DELETE_SUCCESS,
            };
        }

        [Route("Close")]
        [HttpDelete]
        public async Task<ResponseModel> CloseDepartment([FromBody] DepartmentDto data)
        {
            var entity = _DepartmentRepository.Table.FirstOrDefault(t => t.Id == data.Id);
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
            entity.Status = false;
            _DepartmentRepository.Update(entity);
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
            return new SearchHelper<DepartmentDto>().Search(HttpContext, GetQuery(), searchModels, pagesize, pagenumber, sort);
        }
        private IQueryable<DepartmentDto>GetQuery()
        {

            return from a in _DepartmentRepository.Table
                   where a.IsArchive == false
                   select new DepartmentDto
                   {
                       Id = a.Id,
                       Name = a.Name,
                       Address = a.Address,
                       Phone = a.Phone,
                       Status = a.Status
                    
                   };

        }
        private IQueryable<SelectedDto> GetQuerySelected()
        {

            return from a in _DepartmentRepository.Table
                   where
                       a.IsArchive == false
                   select new SelectedDto
                   {
                       value = a.Id,
                       text = a.Name
                   };

        }

        #endregion
    }
}
