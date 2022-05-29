using API.Helper;
using Application.Services;
using Core.Dtos.Product;
using Core.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        
        private readonly ILogger<ProductController> _logger;
        private readonly IProductManager _productManager;

        public ProductController(ILogger<ProductController> logger, ProductManager productManager)
        {
            _logger = logger;
            _productManager = productManager;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpPost]
        [Route("Add")]
        public IActionResult AddProduct(AddProductModel model)
        {
            try
            {
                _productManager.AddProduct(model);
                return Ok();
            }
            catch (ArgumentValidationException ex)
            {
                _logger.Log(LogLevel.Error,ex.Message);
                return BadRequest(ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.InternalServerError));
            }

        }
        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateProduct(UpdateProductModel model)
        {
            try
            {
                _productManager.UpdateProduct(model);
                return Ok();
            }
            catch (ArgumentValidationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.InternalServerError));
            }

        }
        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteProduct(long id)
        {
            try
            {
                _productManager.DeleteProduct(id);
                return Ok();
            }
            catch (ArgumentValidationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.InternalServerError));
            }

        }
    }
}