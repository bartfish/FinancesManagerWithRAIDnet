﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FmWebApp.Controllers.Graphs
{
    public class GraphsController : Controller
    {
        // GET: Graphs
        public ActionResult _GenerateGraphsContent()
        {
            return PartialView();
        }
    }
}