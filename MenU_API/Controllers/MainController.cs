using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenU_BL.Models;
using MenU_API.DataTransferObjects;
using System.Net.Http;

namespace MenU_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class MainController : ControllerBase
    {
        MenUContext context;

        public MainController(MenUContext context) { this.context = context; }

        [Route("TokenLogin")]
        [HttpGet]
        public AccountDTO Login([FromQuery] string token)
        {
            Account acc;
            try
            {
                acc = context.Login(token);
            }
            catch (Exception)
            {
                acc = null;
            }

            if (acc != null)
            {
                AccountDTO userDTO = new AccountDTO(acc);

                HttpContext.Session.SetObject("account", userDTO);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return userDTO;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }

        [Route("LoginCredentials")]
        [HttpGet]
        public AccountDTO Login([FromQuery] string username,[FromQuery] string pass)
        {
            Account acc;
            try
            {
                acc = context.Login(username, pass);
            }
            catch (Exception)
            {
                acc = null;
            }

            if (acc != null)
            {
                AccountDTO userDTO = new AccountDTO(acc);

                HttpContext.Session.SetObject("user", userDTO);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return userDTO;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }

        [Route("LogOut")]
        [HttpGet]
        public void LogOut()
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            if (userDTO != null)
            {
                HttpContext.Session.Clear();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
        }

        [Route("Exists")]
        [HttpGet]
        public bool DoesExist([FromQuery] string username, [FromQuery] string email)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            return context.Exists(username, email);
        }

        [Route("SignUp")]
        [HttpGet]
        public bool SignUp([FromBody] AccountDTO acc)
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");

            if (userDTO != null)
            {
                Account newAcc = new Account()
                {
                    FirstName = acc.FirstName,
                    LastName = acc.LastName,
                    Username = acc.Username,
                    Email = acc.Email,
                    Pass = acc.Pass,
                    Salt = acc.Salt,
                    Iterations = acc.Iterations,
                    DateOfBirth = acc.DateOfBirth,
                    AccountType = acc.AccountType,
                    ProfilePicture = acc.ProfilePicture,
                    AccountStatus = acc.AccountStatus
                };

                try
                {
                    context.AddAccount(newAcc);
                }
                catch (Exception)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                    return false;
                }

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return true;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return false;
            }
        }
    }
}
