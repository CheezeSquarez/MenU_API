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
using System.Reflection;
using System.Text.Json;

namespace MenU_API.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        MenUContext context;

        public AccountsController(MenUContext context) { this.context = context; }

        
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
            catch
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
        
        // This Method logs in a user using credentials (username and password)
        [Route("LoginCredentials")]
        [HttpGet]
        public AccountDTO Login([FromQuery] string username,[FromQuery] string pass)
        {
            Account acc = null;
            try { acc = context.Login(username, pass); }
            catch { Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError; }

            if (acc != null)
            {
                AccountDTO userDTO = new AccountDTO(acc);

                HttpContext.Session.SetObject("user", userDTO);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return userDTO;
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                return null;
            }
        }
        
        // This method generates an authentication token, saves it in the database and returns it to the client
        [Route("CreateToken")]
        [HttpGet]
        public string CreateToken()
        {
            try
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
                
                        context.SaveToken(userDTO.AccountId, token);
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return token;
                
                }
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
            catch { Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError; }
            return "";
        }

        // This method logs the user out and removes it's auth token if one exists
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
                    bool exists = context.Exists(acc.Username, acc.Email);
                    if (!exists)
                    {
                        context.AddAccount(newAcc);
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return true;
                    }
                    else
                        Response.StatusCode = Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                }
                catch { Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError; }     
            }
            else
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
         
            return false;
        }
            

        //This method returns the salt and number of iterations of the user with the specified username
        [Route("GetSaltAndIterations")]
        [HttpGet]
        public Dictionary<string, string> GetSaltAndIterations([FromQuery] string username)
        {
            Dictionary<string,string> returnDic = new Dictionary<string, string>();
            try
            {
                returnDic.Add("Salt", context.GetSalt(username));
                returnDic.Add("Iterations", context.GetIterations(username).ToString());
                if(returnDic != null && returnDic.Count > 0)
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.NoContent;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
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
                        isUnique = true;
                }
                if(salt != "")
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK; 
                else
                    Response.StatusCode = (int)System.Net.HttpStatusCode.NoContent;

                return salt;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return "";
            }
            
        }

        //To be Implemented

        //[Route("UpdateAccountInfo")]
        //[HttpPost]
        //public void UpdateAccountInfo([FromBody] AccountDTO user)
        //{
        //    AccountDTO userDTO = HttpContext.Session.GetObject<AccountDTO>("user");
        //    try
        //    {
        //        if (userDTO != null && userDTO.AccountId == user.AccountId)
        //        {
                    
        //            context.UpdateUser(userDTO.AccountId, );
        //            Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
        //        }
        //        else
        //            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
        //    }
        //    catch { Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError; }
            
        //}


    }
}
