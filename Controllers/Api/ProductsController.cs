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
    public class ProductsController : BaseAuthController
    {
        #region Constants

        #endregion

        #region Fields
        private readonly PostgreRepository<Products> _ProductsRepository;
        private readonly PostgreRepository<Groups> _GroupsRepository;
        private readonly PostgreRepository<Supplier> _SupplierRepository;
        private readonly PostgreRepository<UsersRoles> _UsersRolesRepository;
        private readonly PostgreRepository<Users> _UsersRepository;
        private readonly IHttpContextAccessor _httpContext;

        #endregion

        #region Ctor
        public ProductsController(
            PostgreRepository<Products> productsRepository,
            PostgreRepository<Groups> groupsRepository,
            PostgreRepository<Supplier> supplierRepository,
            PostgreRepository<UsersRoles> usersrolesRepository,
            PostgreRepository<Users> usersRepository,
            IHttpContextAccessor httpContext)
        {
            _ProductsRepository = productsRepository;
            _GroupsRepository = groupsRepository;
            _SupplierRepository = supplierRepository;
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
            var entity = _ProductsRepository.Table.FirstOrDefault(t => t.Id == id);
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
        public async Task<ResponseModel> Insert([FromBody] ProductsPostDto data)
        {
            if (_ProductsRepository.Table.FirstOrDefault(t => t.Name == data.Name && t.SupplierId == data.SupplierId && t.IsArchive == false) != null)
            {
                return new ResponseModel
                {
                    Status = false,
                    ErrorCode = ConstantError.THIS_NAME_ALREADY_USED,
                };
            }
            Products entity = new Products()
            {
                Name = data.Name,
                GroupId = data.GroupId,
                SupplierId = data.SupplierId,
                Price = data.Price,
                Property = data.Property,
                Image = data.Image,
                //Generic
                IsArchive = false,
                CreatedBy = LOGIN_USER_ID,
                CreatedDate = DateTime.Now,
                UpdateBy = LOGIN_USER_ID,
                UpdateDate = DateTime.Now,
            };
            _ProductsRepository.Insert(entity);
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.INSERT_SUCCESS,
            };
        }

        [Route("")]
        [HttpPut]
        public async Task<ResponseModel> Update([FromBody] ProductsDto data)
        {
            if (_ProductsRepository.Table.FirstOrDefault(t => t.Name == data.Name && t.Id != data.Id && !t.IsArchive) != null)
            {
                return new ResponseModel
                {
                    Status = false,
                    ErrorCode = ConstantError.THIS_NAME_ALREADY_USED,
                };
            }
            var entity = _ProductsRepository.Table.FirstOrDefault(t => t.Id == data.Id);
            if (entity.IsArchive == true)
            {
                return new ResponseModel
                {
                    Status = false,
                    ErrorCode = ConstantError.NO_RECORD_FOUND,
                };
            }
            entity.Name = data.Name;
            entity.GroupId = data.GroupId;
            entity.SupplierId = data.SupplierId;
            entity.Price = data.Price;
            entity.Property = data.Property;
            entity.Image = data.Image;
            //Generic
            entity.IsArchive = false;
            entity.CreatedBy = LOGIN_USER_ID;
            entity.CreatedDate = DateTime.Now;
            entity.UpdateBy = LOGIN_USER_ID;
            entity.UpdateDate = DateTime.Now;
            _ProductsRepository.Update(entity);
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.UPDATE_SUCCESS,
            };
        }

        [Route("")]
        [HttpDelete]
        public async Task<ResponseModel> Delete([FromBody] ProductsDto data)
        {
            var entity = _ProductsRepository.Table.FirstOrDefault(t => t.Id == data.Id);
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
            _ProductsRepository.Update(entity);
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
            return new SearchHelper<ProductsSearchDto>().Search(HttpContext, GetQuery(), searchModels, pagesize, pagenumber, sort);
        }
        private IQueryable<ProductsSearchDto>GetQuery()
        {

            return 
                   from p in _ProductsRepository.Table
                   join g in _GroupsRepository.Table on p.GroupId equals g.Id into pgJoin
                   from g in pgJoin.DefaultIfEmpty()
                   join s in _SupplierRepository.Table on p.SupplierId equals s.Id into gsJoin
                   from s in gsJoin.DefaultIfEmpty()
                   where
                       p.IsArchive == false
                   select new ProductsSearchDto
                   {
                       Id = p.Id,
                       GroupId = p.GroupId,
                       SupplierId = s.Id,
                       Name = p.Name,
                       Price = p.Price,
                       Property = p.Property,
                       Image = p.Image,
                       GroupName = g.Name,
                       SupplierName = s.Name
                       
                   };

        }
        private IQueryable<ProductSelectedDto> GetQuerySelected()
        {

            return from a in _ProductsRepository.Table
                   where
                       a.IsArchive == false
                   select new ProductSelectedDto
                   {
                       value = a.Id,
                       text = a.Name,
                       SupplierId = a.SupplierId
                   };

        }

        #endregion
    }
}
