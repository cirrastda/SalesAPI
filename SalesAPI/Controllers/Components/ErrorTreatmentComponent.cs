using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesAPI.Controllers;

namespace SalesAPI.Controllers.Components
{
    public class ErrorTreatmentComponent
    {

        private readonly ControllerBase _controller;

        public ErrorTreatmentComponent(ControllerBase controller)
        {
            _controller = controller;
        }

        public IActionResult BadRequestError(string message)
        {
            return this._controller.BadRequest(new
            {
                type = "Valdation Error",
                title = "Validation Error",
                status = 400,
                errors = new
                {
                    message
                }
            });
        }
    }

}
