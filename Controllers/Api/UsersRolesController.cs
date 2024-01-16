using Meb.Core.Domain.Search;
using Intra.Api.Data;
using Intra.Api.Domain.Dto;
using Intra.Api.Domain.Entities;
using Intra.Api.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Intra.Api.Controllers.Api
{
    [Route("api/[controller]")]
    public class UsersRolesController : BaseAuthController
    {

        #region Constants

        #endregion

        #region Fields
        private readonly IHttpContextAccessor _httpContext;

        //Entity
        private readonly PostgreRepository<UsersRoles> _UsersRolesRepository;

        #endregion


        #region Ctor

        public UsersRolesController(
            IHttpContextAccessor httpContext,
            PostgreRepository<Users> usersRepository,
            PostgreRepository<UsersRoles> usersRolesRepository
            )
        {
            _httpContext = httpContext;
            _UsersRolesRepository = usersRolesRepository;
            //For Get Login user Info
            GetUserId(httpContext, usersRepository);
        }

        #endregion

        #region Metods

        [Route("{id:int}")]
        [HttpGet]
        public object GetById(int id)
        {
            var entity = _UsersRolesRepository.Table.FirstOrDefault(t => t.Id == id);
            return entity;
        }

        [Route("Search")]
        [HttpPost]
        public object GetByParameters([FromBody] IEnumerable<SearchModel> searchModels, int pagesize, int pagenumber, string sort)
        {
            var query = from t in _UsersRolesRepository.Table
                        where t.IsArchive == false
                        select new UsersRolesDto
                        {
                            Id = t.Id,
                            Name = t.Name,
                            IsStatus = t.IsStatus,
                            LangCode = t.LangCode,
                            IsAdmin = t.IsAdmin
                        };

            return new SearchHelper<UsersRolesDto>().Search(HttpContext, query, searchModels, pagesize, pagenumber, sort);
        }

        [Route("")]
        [HttpGet]
        public object Get()
        {
            var result = _UsersRolesRepository.Table.ToList();
            return result;
        }

        [Route("")]
        [HttpPost]
        public object Insert([FromBody] UsersRolesPostDto model)
        {
            var data = new UsersRoles
            {
                IsAdmin = false,
                Name = model.Name,
                IsStatus = model.IsStatus,
                LangCode = "",
                IsArchive = false,
                CreatedBy = LOGIN_USER_ID,
                UpdateBy = LOGIN_USER_ID,
                CreatedDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };

            _UsersRolesRepository.Insert(data);

            return data;
        }


        [Route("")]
        [HttpPut]
        public object Update([FromBody] UsersRolesPostDto model)
        {

            var entity = _UsersRolesRepository.GetById(model.Id);

            entity.Name = model.Name;
            entity.UpdateBy = LOGIN_USER_ID;
            entity.UpdateDate = DateTime.Now;

            _UsersRolesRepository.Update(entity);

            return entity;
        }



        #endregion

        #region Utilities

        #endregion
    }
}
