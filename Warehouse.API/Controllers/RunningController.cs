﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunningController : ControllerBase
    {
        [HttpGet]        
        public ActionResult<string> Get()
        {
            return "Warehouse API is Up and Running!";
        }
    }
}