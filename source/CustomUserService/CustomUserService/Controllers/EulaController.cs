﻿using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Extensions;

namespace SampleApp.Controllers
{
    public class EulaController : Controller
    {
        [Route("core/eula")]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            // this verifies that we have a prtial signin from idsvr
            var ctx = Request.GetOwinContext();
            var authentication = await ctx.Authentication.AuthenticateAsync(Constants.PartialSignInAuthenticationType);
            if (authentication == null)
            {
                return View("Error");
            }
            
            return View();
        }

        [Route("core/eula")]
        [HttpPost]
        public async Task<ActionResult> Index(string button)
        {
            var ctx = Request.GetOwinContext();
            var authentication = await ctx.Authentication.AuthenticateAsync(Constants.PartialSignInAuthenticationType);
            if (authentication == null)
            {
                return View("Error");
            }

            if (button == "yes")
            {
                // update the "database" for our users with the outcome
                var subject = authentication.Identity.GetSubjectId();
                var user = EulaAtLoginUserService.Users.Single(x => x.Subject == subject);
                user.AccpetedEula = true;

                // find the URL to continue with the process to the issue the token to the RP
                var resumeUrl = authentication.Identity.Claims.Single(x => x.Type == Constants.ClaimTypes.PartialLoginReturnUrl).Value;
                return Redirect(resumeUrl);
            }

            ViewBag.Message = "Well, until you accept you can't continue.";
            return View();
        }
    }
}
