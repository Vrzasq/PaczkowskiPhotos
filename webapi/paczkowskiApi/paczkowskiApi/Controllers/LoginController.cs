﻿using Commons;
using DbContract.Entities;
using DbContract.RepositoryContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using paczkowskiApi.Models;
using paczkowskiApi.security;

namespace paczkowskiApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IRepository _repository;

        public LoginController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public ActionResult<LoginResult> SignIn(LoginModel loginModel)
        {
            var loginResult = new LoginResult();
            string userHash = CryptoPassword.GetPasswordHash(loginModel.Password);
            string dbHash = _repository.GetPasswordHash(loginModel.Email);

            if (userHash == dbHash)
            {
                Authenticate(loginModel.Email);
                loginResult.Success = true;
            }

            return loginResult;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = AuthScheme.Cookies)]
        public ActionResult IsLoggedIn()
        {
            return new JsonResult(
                new
                {
                    IsLoggedIn = true
                });
        }

        private void Authenticate(string email)
        {
            var cookies = Response.Cookies;
            var authTokenBlob = new AuthTokenBlob(email, TokenProvider.NewAuthToken);
            PutAuthUserToDb(authTokenBlob);
            string encryptedBlob = TokenEncryption.Encrypt(authTokenBlob);
            cookies.Append(CookieName.AuthToken, encryptedBlob, new CookieOptions { HttpOnly = true, Path = "/" });
        }

        [HttpPost]
        public ActionResult<bool> Logout()
        {
            var cookies = Request.Cookies;
            AuthTokenBlob authTokenBlob = TokenEncryption.Decrypt(cookies[CookieName.AuthToken]);
            BurnOldToken(authTokenBlob.Email);

            return true;
        }

        private void BurnOldToken(string email) =>
            _repository.RemoveLoggedUser(email);


        private void PutAuthUserToDb(AuthTokenBlob authTokenBlob)
        {
            LoggedUser loggedUser = new LoggedUser
            {
                Email = authTokenBlob.Email,
                Token = authTokenBlob.Token
            };

            _repository.AddLoggedUser(loggedUser);
        }
    }
}
