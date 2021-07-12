using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenU_BL.Models;
using MenU_BL.ModelsBL;
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

        
        // This Method logs in a user using an authentication token
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
        
        // This Method logs in a user using credentials (username and password)
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
        
        // This method generates an authentication token, saves it in the database and returns it to the client
        [Route("CreateToken")]
        [HttpGet]
        public string CreateToken()
        {
            bool isUnique = false;
            string token = "";
            while (!isUnique)
            {
                token = GeneralProcessing.GenerateAlphanumerical(16);
                if (!context.TokenExists(token))
                    isUnique = true;
            }    
            
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            if(userDTO != null)
            {
                try
                {
                    context.SaveToken(userDTO.AccountId, token);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return token;
                }
                catch (Exception)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                }
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
            return "";
        }

        // This method logs the user out and removes it's auth token if one exists
        [Route("LogOut")]
        [HttpGet]
        public void LogOut([FromQuery] string token)
        {
            AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
            try
            {
                if (userDTO != null)
                {
                    HttpContext.Session.Clear();
                    context.RemoveToken(token);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
            }
            
        }
        
        // This method returns true if a user exists with the specified username and email address (for sign up purposes)
        [Route("Exists")]
        [HttpGet]
        public bool DoesExist([FromQuery] string username, [FromQuery] string email)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            return context.Exists(username, email);
        }
        
        // This method adds the specified user to the database
        [Route("SignUp")]
        [HttpPost]
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

        //This method returns the salt and number of iterations of the user with the specified username
        [Route("GetSalt")]
        [HttpGet]
        public Dictionary<string, string> GetSaltAndIterations([FromQuery] string username)
        {
            Dictionary<string,string> returnDic = new Dictionary<string, string>();
            try
            {
                returnDic.Add("Salt", context.GetSalt(username));
                returnDic.Add("Iterations", context.GetIterations(username).ToString());
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            }
            catch (Exception)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                returnDic = null;
            }
            return returnDic;
        }

        //This method generates a random unique salt and returns it
        [Route("GenerateSalt")]
        [HttpGet]
        public string GenerateSalt()
        {
            try
            {
                string salt = "";
                bool isUnique = false;
                while (!isUnique)
                {
                    salt = GeneralProcessing.GenerateAlphanumerical(8);
                    if (!context.SaltExists(salt))
                        isUnique = false;
                }

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return salt;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                return "";
            }
            
        }

    }
}
