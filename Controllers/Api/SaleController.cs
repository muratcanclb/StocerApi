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
    public class SaleController : BaseAuthController
    {
        #region Constants

        #endregion

        #region Fields
        private readonly PostgreRepository<Sale> _SaleRepository;
        private readonly PostgreRepository<Supplier> _SupplierRepository;
        private readonly PostgreRepository<Department> _DepartmentRepository;
        private readonly PostgreRepository<Products> _ProductsRepository;
        private readonly PostgreRepository<UsersRoles> _UsersRolesRepository;
        private readonly PostgreRepository<Users> _UsersRepository;
        private readonly IHttpContextAccessor _httpContext;

        #endregion

        #region Ctor
        public SaleController(
            PostgreRepository<Sale> saleRepository,
            PostgreRepository<Supplier> supplierkRepository,
            PostgreRepository<Department> departmentRepository,
            PostgreRepository<Products> productsRepository,
            PostgreRepository<UsersRoles> usersrolesRepository,
            PostgreRepository<Users>usersRepository,
            IHttpContextAccessor httpContext)
        {
            _SaleRepository = saleRepository;
            _SupplierRepository = supplierkRepository;
            _DepartmentRepository = departmentRepository;
            _ProductsRepository = productsRepository;
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
            var entity = _SaleRepository.Table.FirstOrDefault(t => t.Id == id);
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




        [Route("Search")]
        [HttpPost]
        public object GetByParameters([FromBody] IEnumerable<SearchModel> searchModels, int pagesize, int pagenumber, string sort)
        {
            return new SearchHelper<SaleListDto>().Search(HttpContext, GetQuery(), searchModels, pagesize, pagenumber, sort);
        }
        private IQueryable<SaleListDto> GetQuery()
        {

            return from a in _SaleRepository.Table
                   join d in _DepartmentRepository.Table on a.DepartmentId equals d.Id into adJoin
                   from d in adJoin.DefaultIfEmpty()
                   join p in _ProductsRepository.Table on a.ProductId equals p.Id into apJoin
                   from p in apJoin.DefaultIfEmpty()
                   where a.IsArchive == false
                   select new SaleListDto
                   {
                       Id = a.Id,
                       OrderNo = a.OrderNo,
                       ProductId = a.ProductId,
                       DepartmentName = d.Name,
                       ProductName = p.Name,
                       Total = a.Total,
                       Price = p.Price,
                       CreatedDate = a.CreatedDate

                   };

        }
       
        #endregion
    }
}
