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
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService _departmentService;
        private readonly ErrorTreatmentComponent _errorThreatment;
        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
            _errorThreatment = new ErrorTreatmentComponent(this);
        }

        // GET: api/Departments
        [HttpGet]
        [ApiVersion("1.0")]
        [ResponseCache(Duration = 120)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _departmentService.FindAllAsync());
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Get(int id)
        {
            var department = await _departmentService.FindByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Edit(int id, [FromBody]Department department)
        {
            if (id != department.Id) { return _errorThreatment.BadRequestError("Id not Found"); }
            try
            {
                await _departmentService.UpdateAsync(department);
            }
            catch (NotFoundException e)
            {
                return NotFound();
            }
            catch (DbConcurrencyException e)
            {
                return Conflict(e.Message) ;
            } 
            catch (DuplicateException<Department> e)
            {
                return _errorThreatment.BadRequestError(e.Message);
            }

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Create([FromBody]Department department)
        {
            try
            {
                await _departmentService.InsertAsync(department);

                return Created(nameof(Create), department);
            } catch(DuplicateException<Department> e)
            {
                return _errorThreatment.BadRequestError(e.Message);
            }
            //return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.FindByIdAsync(id);
            if (department == null) { return NotFound(); }
            try
            {
                _departmentService.RemoveAsync(id);
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
