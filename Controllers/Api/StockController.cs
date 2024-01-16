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
    public class StockController : BaseAuthController
    {
        #region Constants

        #endregion

        #region Fields
        private readonly PostgreRepository<Stock> _StockRepository;
        private readonly PostgreRepository<Supplier> _SupplierRepository;
        private readonly PostgreRepository<Department> _DepartmentRepository;
        private readonly PostgreRepository<Products> _ProductsRepository;
        private readonly PostgreRepository<Sale> _SaleRepository;
        private readonly PostgreRepository<UsersRoles> _UsersRolesRepository;
        private readonly PostgreRepository<Users> _UsersRepository;
        private readonly IHttpContextAccessor _httpContext;

        #endregion

        #region Ctor
        public StockController(
            PostgreRepository<Stock> stockRepository,
            PostgreRepository<Supplier> supplierkRepository,
            PostgreRepository<Department> departmentRepository,
            PostgreRepository<Products> productsRepository,
            PostgreRepository<Sale> saleRepository,
            PostgreRepository<UsersRoles> usersrolesRepository,
            PostgreRepository<Users>usersRepository,
            IHttpContextAccessor httpContext)
        {
            _StockRepository = stockRepository;
            _SupplierRepository = supplierkRepository;
            _DepartmentRepository = departmentRepository;
            _ProductsRepository = productsRepository;
            _SaleRepository = saleRepository;
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
            var entity = _StockRepository.Table.FirstOrDefault(t => t.ProductId == id);
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

        [Route("")]
        [HttpPost]
        public async Task<ResponseModel>Insert([FromBody] StockPostDto data)
        {
            if (_StockRepository.Table.FirstOrDefault(t => t.SupplierId == data.SupplierId && t.ProductId == data.ProductId && t.IsArchive==false)!=null)
            {
                return new ResponseModel
                {
                    Status = false,
                    ErrorCode = ConstantError.THIS_NAME_ALREADY_USED,
                };
            }
            Stock entity = new Stock()
            {
                SupplierId = data.SupplierId,
                DepartmentId = data.DepartmentId,
                ProductId = data.ProductId,
                Total = data.Total,
                Status = true,
                //Generic
                IsArchive = false,
                CreatedBy = LOGIN_USER_ID,
                CreatedDate = DateTime.Now,
                UpdateBy = LOGIN_USER_ID,
                UpdateDate = DateTime.Now,
            };
            _StockRepository.Insert(entity);
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.INSERT_SUCCESS,
            };  
        }

        [Route("sale")]
        [HttpPut]
        public async Task<ResponseModel>SaleUpdate([FromBody] StockDto data)
        {

            var entity = _StockRepository.Table.FirstOrDefault(t => t.Id == data.Id);
            if (entity.IsArchive==true)
            {
                return new ResponseModel
                    {
                        Status = false,
                        ErrorCode = ConstantError.NO_RECORD_FOUND,
                    };
            }
 
                entity.SupplierId = data.SupplierId;
                entity.DepartmentId = data.DepartmentId;
                entity.ProductId = data.ProductId;
                entity.Total -= data.Total;
                entity.Status = true;
                //Generic
                entity.IsArchive = false;
                entity.CreatedBy = LOGIN_USER_ID;
                entity.CreatedDate = DateTime.Now;
                entity.UpdateBy = LOGIN_USER_ID;
                entity.UpdateDate = DateTime.Now;
                if (entity.Total >= 0)
                {
                    if (entity.Total == 0)
                    {
                        entity.Status = false;

                    }

                _StockRepository.Update(entity);
                    

                    return new ResponseModel
                    {
                        Status = true,
                        Response = entity.Id,
                        Message = ConstantSuccess.UPDATE_SUCCESS,
                    };
                }
            return new ResponseModel
            {
                Status = false,
                Response = entity.Id,
                Message = ConstantError.INSERT_NOT_SAVED,
            };

        }

        [Route("add")]
        [HttpPut]
        public async Task<ResponseModel> AddUpdate([FromBody] StockDto data)
        {

            var entity = _StockRepository.Table.FirstOrDefault(t => t.Id == data.Id);
            if (entity.IsArchive == true)
            {
                return new ResponseModel
                {
                    Status = false,
                    ErrorCode = ConstantError.NO_RECORD_FOUND,
                };
            }

            entity.SupplierId = data.SupplierId;
            entity.DepartmentId = data.DepartmentId;
            entity.ProductId = data.ProductId;
            entity.Total += data.Total;
            entity.Status = true;
            //Generic
            entity.IsArchive = false;
            entity.CreatedBy = LOGIN_USER_ID;
            entity.CreatedDate = DateTime.Now;
            entity.UpdateBy = LOGIN_USER_ID;
            entity.UpdateDate = DateTime.Now;
            if (entity.Total >= 0)
            {
                if (entity.Total == 0)
                {
                    entity.Status = false;

                }

                _StockRepository.Update(entity);


                return new ResponseModel
                {
                    Status = true,
                    Response = entity.Id,
                    Message = ConstantSuccess.UPDATE_SUCCESS,
                };
            }
            return new ResponseModel
            {
                Status = true,
                Response = entity.Id,
                Message = ConstantSuccess.UPDATE_SUCCESS,
            };

        }


        [Route("")]
        [HttpDelete]
        public async Task<ResponseModel>Delete([FromBody] StockDto data)
        {
            var entity = _StockRepository.Table.FirstOrDefault(t => t.Id == data.Id);
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
            _StockRepository.Update(entity);
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
            return new SearchHelper<StockListDto>().Search(HttpContext, GetQuery(), searchModels, pagesize, pagenumber, sort);
        }
        private IQueryable<StockListDto> GetQuery()
        {

            return from a in _StockRepository.Table
                   join r in _SupplierRepository.Table on a.SupplierId equals r.Id into arJoin
                   from r in arJoin.DefaultIfEmpty()
                   join d in _DepartmentRepository.Table on a.DepartmentId equals d.Id into adJoin
                   from d in adJoin.DefaultIfEmpty()
                   join p in _ProductsRepository.Table on a.ProductId equals p.Id into apJoin
                   from p in apJoin.DefaultIfEmpty()
                   where a.IsArchive == false
                   select new StockListDto
                   {
                       Id = a.Id,
                       ProductId = a.ProductId,
                       SupplierId = a.SupplierId,
                       DepartmentId = a.DepartmentId,
                       SupplierName = r.Name,
                       DepartmentName = d.Name,
                       ProductName = p.Name,
                       Productimg = p.Image,
                       Status = a.Status,
                       Price = p.Price,
                       Total = a.Total,
                       Value = 0

                   };

        }


        #endregion
    }
}
