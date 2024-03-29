﻿using EnTier;
using EnTier.Controllers;
using Ludwig.DataAccess.Models;
using Ludwig.Presentation.Authentication.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Ludwig.Presentation.Controllers
{
    [ApiController]
    [Route("story-users")]
    [AuthorizeIssueManagers]
    public class StoryUsersController : CrudControllerBase<StoryUser,long>
    {
        public StoryUsersController(EnTierEssence essence) : base(essence)
        {
        }

        protected override StoryUser OnGetById(long id)
        {
            return base.OnGetById(id);
        }


        public override IActionResult GetAll()
        {
            return base.GetAll();
        }
    }
}
