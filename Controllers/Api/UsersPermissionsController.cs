using Meb.Core.Domain.Search;
using Intra.Api.Data;
using Intra.Api.Domain.Dto;
using Intra.Api.Domain.Entities;
using Intra.Api.Infrastructure.Constants;
using Intra.Api.Infrastructure.Helpers;
using Intra.Api.Models;
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
    public class UsersPermissionsController : BaseAuthController
    {

        #region Constants

        #endregion

        #region Fields

        private readonly IHttpContextAccessor _httpContext;

        //Entity
        private readonly PostgreRepository<Users> _UsersRepository;
        private readonly PostgreRepository<UsersPermissions> _Repository;
        

        #endregion


        #region Ctor

        public UsersPermissionsController(
            PostgreRepository<UsersPermissions> Repository,
            IHttpContextAccessor httpContext,
            PostgreRepository<Users> usersRepository
            
            )
        {
            _Repository = Repository;
            _httpContext = httpContext;
            _UsersRepository = usersRepository;
            

            //For Get Login user Info
            GetUserId(httpContext, usersRepository);
        }

        #endregion

        #region Metods

        [Route("{id:int}")]
        [HttpGet]
        public object GetById(int id)
        {
            var entity = _Repository.Table.FirstOrDefault(t => t.Id == id);
            return entity;
        }

        [Route("Search")]
        [HttpPost]
        public object GetByParameters([FromBody] IEnumerable<SearchModel> searchModels, int pagesize, int pagenumber, string sort)
        {
            var query = from t in _Repository.Table
                        join p in _Repository.Table on t.ParentId equals p.Id into tpJoin
                        from p in tpJoin.DefaultIfEmpty()
                        where t.IsArchive == false && t.IsArchive == false
                        select new UsersPermissionsDto
                        {
                            Id = t.Id,
                            Name = t.Name,
                            IsStatus = t.IsStatus,
                            LangCode = t.LangCode,
                            
                            ParentId = t.ParentId,
                            ParentName = p.LangCode
                        };

            return new SearchHelper<UsersPermissionsDto>().Search(HttpContext, query, searchModels, pagesize, pagenumber, sort);
        }

       
        

        [Route("")]
        [HttpGet]
        public object Get()
        {
            var result = GetQuery().ToList();
            return result;
        }
       
        /// <summary>
        /// KAYIT İŞLEMİ
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<ResponseModel> Post([FromBody] UsersPermissionsPostDto data)
        {

            UsersPermissions entity = new UsersPermissions
            {
                Name = data.Name,
                ParentId = data.ParentId,
                IsStatus = data.IsStatus,
                IsArchive = false,
                LangCode = (data.LangCode != null && data.LangCode != "") ? data.LangCode : data.Name,
                CreatedBy = LOGIN_USER_ID,
                CreatedDate = DateTime.Now,
                UpdateBy = LOGIN_USER_ID,
                UpdateDate = DateTime.Now
            };

            if (_Repository.Table.FirstOrDefault(m => m.Name == data.Name) != null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = ConstantError.THIS_NAME_ALREADY_USED,
                };
            }


            _Repository.Insert(entity);

            return new ResponseModel
            {
                Status = true,
                Response = data,
                Message = ConstantSuccess.INSERT_SUCCESS,
            };

        }

        

      

        #endregion

        #region Utilities

        #endregion

        private IQueryable<UsersPermissionsDto> GetQuery()
        {
            return from t in _Repository.Table
                   join tn in _Repository.Table on t.Id equals tn.ParentId into utJoin
                   from tn in utJoin.DefaultIfEmpty()
                   where t.IsArchive == false && tn.IsArchive == false
                   select new UsersPermissionsDto
                   {
                       Id = t.Id,
                       Name = t.Name,
                       IsStatus = t.IsStatus,
                       LangCode = t.LangCode,
                       ParentId = t.ParentId,
                       ParentName = tn.Name
                   };
        }


    }
}
