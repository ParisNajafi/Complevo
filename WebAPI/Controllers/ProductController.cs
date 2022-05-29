using API.Helper;
using Core.Dtos.Product;
using Core.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await  _productManager.GetAll();
                return Ok(list);
            }
            catch (ArgumentValidationException ex)
            {
                return BadRequest(ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.InternalServerError));
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddProduct(AddProductModel model)
        {
            try
            {
                await _productManager.AddProduct(model);
                return Ok();
            }
            catch (ArgumentValidationException ex)
            {
                return BadRequest(ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.InternalServerError));
            }

        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateProduct(UpdateProductModel model)
        {
            try
            {
                await _productManager.UpdateProductAsync(model);
                return Ok();
            }
            catch (ArgumentValidationException ex)
            {
                return BadRequest(ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.InternalServerError));
            }

        }
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            try
            {
                await _productManager.DeleteProduct(id);
                return Ok();
            }
            catch (ArgumentValidationException ex)
            {
                return BadRequest(ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.InternalServerError));
            }

        }
    }
}