using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TurismoReal.Context.Login;
using TurismoReal.Context.Usuario;
using TurismoReal.Infrastructure;
using TurismoReal.Interface;
using TurismoReal.Models;

namespace TurismoReal.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager _userManager;
        private readonly UsuarioContext _usuarioContext;
        private readonly IJwtAuthManager _jwtAuthManager;
        private ILoggerManager _loggerManager;

        public AccountController(UserManager userManager, IJwtAuthManager jwtAuthManager, ILoggerManager loggerManager, UsuarioContext usuarioContext)
        {
            _usuarioContext = usuarioContext;
            _userManager = userManager;
            _jwtAuthManager = jwtAuthManager;
            _loggerManager = loggerManager;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest Usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var valid = _usuarioContext.ValidarUsuario(Usuario.Correo, Usuario.Password);

             //valid = _userManager.IsPasswordUsuario(Usuario.IdUsuario, Usuario.Password);

            //if( Usuario == new LoginRequest())
            //{
            //    return Unauthorized();
            //}

            if (valid <= 0)
            {
                return BadRequest();
            }

            //_loggerManager.LogInfo(string.Format("Usuario {0} logeado", request.UserName));
            //var role = _userService.GetUserRole(request.UserName);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, Usuario.Nombre),
                new Claim(ClaimTypes.Role, Usuario.Tipo),
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(Usuario.Nombre, claims, DateTime.Now);

            General.Token token = new General.Token();

            token.acces_token = jwtResult.AccessToken;
            token.refresh_token = jwtResult.RefreshToken;
            //_logger.LogInformation($"User [{request.UserName}] logged in the system.");
            return Ok(token);
        }
        //[AllowAnonymous]
        //[HttpPost("loginByKey")]
        //public ActionResult LoginByKey([FromBody] LoginRequest Usuario)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    int valid = _userManager.GetIdTiendaToken(Usuario.IdUsuario, Usuario.Password);

        //    //if( Usuario == new LoginRequest())
        //    //{
        //    //    return Unauthorized();
        //    //}

        //    if (valid <= 0)
        //    {
        //        return BadRequest();
        //    }

        //    //_loggerManager.LogInfo(string.Format("Usuario {0} logeado", request.UserName));
        //    //var role = _userService.GetUserRole(request.UserName);
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, Usuario.Nombre),
        //        new Claim(ClaimTypes.Role, Usuario.Tipo),
        //    };

        //    var jwtResult = _jwtAuthManager.GenerateTokens(Usuario.Nombre, claims, DateTime.Now);

        //    General.Token token = new General.Token();

        //    token.acces_token = jwtResult.AccessToken;
        //    token.refresh_token = jwtResult.RefreshToken;
        //    //_logger.LogInformation($"User [{request.UserName}] logged in the system.");
        //    return Ok(token);
        //}

        [AllowAnonymous]
        [HttpGet("GetUsuario/{usuario}")]
        public IActionResult GetUsuarioByNombre(string usuario)
        {
            var Usuario = _userManager.GetUsuarioByNombre(new UsuarioViewModel() { nombres = usuario });
            return Ok(Usuario);
        }

        //[HttpGet("GetCliente/{id}")]
        //[AllowAnonymous]
        //public ActionResult GetClienteById(int id)
        //{
        //    var IdCliente = 0;
        //        //_userManager.GetIdCentroDistribucionByIdUsuario(id);
        //    return Ok(IdCliente);
        //}

        //[HttpGet("GetDespachador/{id}")]
        //[AllowAnonymous]
        //public ActionResult GetDespachadorById(int id)
        //{
        //    var IdDespachador = 0;
        //    //_userManager.GetIdDespachadorByIdUsuario(id);
        //    return Ok(IdDespachador);
        //}
        //[HttpGet("user")]
        //[Authorize]
        //public ActionResult GetCurrentUser()
        //{
        //    return Ok(new UsuarioViewModel
        //    {
        //        nombres = User.Identity.Name,
        //        tipoUsuario = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
        //        // OriginalUserName = User.FindFirst("OriginalUserName")?.Value
        //    });
        //}

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            // optionally "revoke" JWT token on the server side --> add the current token to a block-list
            // https://github.com/auth0/node-jsonwebtoken/issues/375

            var userName = User.Identity.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
            //_logger.LogInformation($"User [{userName}] logged out the system.");
            return Ok();
        }

        //[HttpPost("refresh-token")]
        //[Authorize]
        //public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        //{
        //    try
        //    {
        //        var userName = User.Identity.Name;
        //        _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

        //        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        //        {
        //            return Unauthorized();
        //        }

        //        var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
        //        var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
        //        _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
        //        return Ok(new LoginResult
        //        {
        //            UserName = userName,
        //            Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
        //            AccessToken = jwtResult.AccessToken,
        //            RefreshToken = jwtResult.RefreshToken.TokenString
        //        });
        //    }
        //    catch (SecurityTokenException e)
        //    {
        //        return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
        //    }
        //}

        //[HttpPost("impersonation")]
        //public ActionResult Impersonate([FromBody] ImpersonationRequest request)
        //{
        //    var userName = User.Identity.Name;
        //    _logger.LogInformation($"User [{userName}] is trying to impersonate [{request.UserName}].");

        //    var impersonatedRole = _userService.GetUserRole(request.UserName);
        //    if (string.IsNullOrWhiteSpace(impersonatedRole))
        //    {
        //        _logger.LogInformation($"User [{userName}] failed to impersonate [{request.UserName}] due to the target user not found.");
        //        return BadRequest($"The target user [{request.UserName}] is not found.");
        //    }
        //    if (impersonatedRole == UserRoles.Admin)
        //    {
        //        _logger.LogInformation($"User [{userName}] is not allowed to impersonate another Admin.");
        //        return BadRequest("This action is not supported.");
        //    }

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name,request.UserName),
        //        new Claim(ClaimTypes.Role, impersonatedRole),
        //        new Claim("OriginalUserName", userName)
        //    };

        //    var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
        //    _logger.LogInformation($"User [{request.UserName}] is impersonating [{request.UserName}] in the system.");
        //    return Ok(new LoginResult
        //    {
        //        UserName = request.UserName,
        //        Role = impersonatedRole,
        //        OriginalUserName = userName,
        //        AccessToken = jwtResult.AccessToken,
        //        RefreshToken = jwtResult.RefreshToken.TokenString
        //    });
        //}

        //[HttpPost("stop-impersonation")]
        //public ActionResult StopImpersonation()
        //{
        //    var userName = User.Identity.Name;
        //    var originalUserName = User.FindFirst("OriginalUserName")?.Value;
        //    if (string.IsNullOrWhiteSpace(originalUserName))
        //    {
        //        return BadRequest("You are not impersonating anyone.");
        //    }
        //    _logger.LogInformation($"User [{originalUserName}] is trying to stop impersonate [{userName}].");

        //    var role = _userService.GetUserRole(originalUserName);
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name,originalUserName),
        //        new Claim(ClaimTypes.Role, role)
        //    };

        //    var jwtResult = _jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
        //    _logger.LogInformation($"User [{originalUserName}] has stopped impersonation.");
        //    return Ok(new LoginResult
        //    {
        //        UserName = originalUserName,
        //        Role = role,
        //        OriginalUserName = null,
        //        AccessToken = jwtResult.AccessToken,
        //        RefreshToken = jwtResult.RefreshToken.TokenString
        //    });
        //}
    }

    public class LoginRequest
    {
        
        [JsonPropertyName("IdUsuario")]
        public int IdUsuario { get; set; }

        
        [JsonPropertyName("Nombre")]
        public string Nombre { get; set; }
        
        [Required]
        [JsonPropertyName("Correo")]
        public string Correo { get; set; }

        [Required]
        [JsonPropertyName("Password")]
        public string Password { get; set; }

        
        [JsonPropertyName("Tipo")]
        public string Tipo { get; set; }

    }



    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

    public class ImpersonationRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}

