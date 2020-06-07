using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Controllers.Components;
using SalesAPI.Data;
using SalesAPI.Models.Entities;
using SalesAPI.Services.Exceptions;
using SalesAPI.Services.Sales;

namespace SalesAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        private readonly SellerService _sellerService;
        private readonly ErrorTreatmentComponent _errorThreatment;
        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
            _errorThreatment = new ErrorTreatmentComponent(this);
        }

        // GET: api/Sellers
        [HttpGet]
        [ApiVersion("1.0")]
        [ResponseCache(Duration = 120)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _sellerService.FindAllAsync());
        }

        // GET: api/Sellers/5
        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Get(int id)
        {
            var seller = await _sellerService.FindByIdAsync(id);

            if (seller == null)
            {
                return NotFound();
            }

            return Ok(seller);
        }

        // PUT: api/Sellers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Edit(int id, [FromBody]Seller seller)
        {
            if (id != seller.Id) { return _errorThreatment.BadRequestError("Id not Found"); }
            try
            {
                await _sellerService.UpdateAsync(seller);
            }
            catch (NotFoundException e)
            {
                return NotFound();
            }
            catch (DbConcurrencyException e)
            {
                return Conflict(e.Message) ;
            } 
            catch (DuplicateException<Seller> e)
            {
                return _errorThreatment.BadRequestError(e.Message);
            }

            return NoContent();
        }

        // POST: api/Sellers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Create([FromBody]Seller seller)
        {
            try
            {
                await _sellerService.InsertAsync(seller);

                return Created(nameof(Create), seller);
            } catch(DuplicateException<Seller> e)
            {
                return _errorThreatment.BadRequestError(e.Message);
            }
            //return CreatedAtAction("GetSeller", new { id = seller.Id }, seller);
        }

        // DELETE: api/Sellers/5
        [HttpDelete("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Delete(int id)
        {
            var seller = await _sellerService.FindByIdAsync(id);
            if (seller == null) { return NotFound(); }
            try
            {
                _sellerService.RemoveAsync(id);
            }
            catch (NotFoundException e)
            {
                return NotFound();
            }
            catch (IntegrityException e)
            {
                return Conflict(e.Message);
            }
            return Ok();
        }


    }
}
