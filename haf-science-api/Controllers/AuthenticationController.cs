using AutoMapper;
using EmailService.Interfaces;
using EmailService.Models;
using FluentValidation;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using haf_science_api.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace haf_science_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService<UsuariosModel> _usersService;
        private readonly IPasswordService _passwordService;
        private readonly IEmailSender _emailSenderService;
        private readonly IUserHashesService<UserHash> _userHashesService;
        private readonly FrontEndHafAppInfo _frontEndInfo;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public AuthenticationController(IUserService<UsuariosModel> usersService,
            IPasswordService passwordService, IEmailSender emailSenderService,
            IUserHashesService<UserHash> userHashesService, FrontEndHafAppInfo frontEndHafAppInfo, 
            IMapper mapper, ITokenService tokenService)
        {
            _usersService = usersService;
            _passwordService = passwordService;
            _emailSenderService = emailSenderService;
            _userHashesService = userHashesService;
            _frontEndInfo = frontEndHafAppInfo;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Route("register")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Register([FromBody] RegistrationModel model)
        {
            try
            {
                string generatedUsername = await _usersService
                    .GenerateUsername(model.Nombres, model.Apellidos, model.FechaNacimiento);

                var emailExists = await _usersService.GetUsuarioByEmail(model.CorreoElectronico);

                if (emailExists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "Este correo electronico ya se encuentra registrado" });
                }

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                model.CreadoPor = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                await _usersService.Register(_mapper.Map<UsuariosModel>(model));

                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "Se ha registrado el usuario exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response() {
                        Status = "Error",
                        Message = ex.ToString()
                    });

                throw;
            }
        }
        [HttpPost]
        [Route("token")]
        public async Task<ActionResult> GetToken([FromBody] LoginData requestUser)
        {
            try
            {
                if (requestUser != null && requestUser.NombreUsuario != null && requestUser.Contrasena != null)
                {
                    var user = await _usersService.GetUsuarioLoginInfo(requestUser.NombreUsuario, requestUser.Contrasena);

                    if (user != null)
                    {
                        var token = _tokenService.WriteToken(user);
                        return Ok(
                            new TokenResponse
                            {
                                Status = "Success",
                                Token = token,
                                UserInfo = _mapper.Map<UserInfo>(user)
                            });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized,
                            new Response { Status = "Unauthorized", Message = "No se pudo iniciar sesión, revise el usuario y la contraseña ingresados." });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                            new Response { Status = "Error", Message = "Credenciales inválidas" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = ex.ToString() });
                throw;
            }
        }
        [HttpPost]
        [Route("check")]
        public ActionResult CheckUserIsLoggedIn()
        {
            var isUserLoggedIn = this.User.Identity.IsAuthenticated;

            return Ok(isUserLoggedIn);
        }
        [HttpPost]
        [Route("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            return Ok();
        }
        [Route("change-password")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            try
            {
                var isPasswordValid = await _passwordService.ValidatePassword(changePasswordModel);

                if (isPasswordValid.Errors.Count > 0)
                {
                    throw new ValidationException(isPasswordValid.Errors);
                }

                if (!changePasswordModel.Password.Equals(changePasswordModel.ConfirmPassword))
                {
                    throw new Exception("La contraseña y la confirmación de contraseña deben tener el mismo valor");
                }

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                var userInfo = await _usersService.GetUsuarioById(userId);
                var salt = await _passwordService.ConvertStringSaltToByteArray(userInfo.Salt);
                userInfo.Contrasena = await _passwordService.HashPassword(changePasswordModel.Password, salt);
                await _usersService.Update(userInfo);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.Message
                    });
                throw;
            }
        }
        [Route("reset-password-save")]
        [HttpPost]
        public async Task<ActionResult> ResetPasswordWithHash([FromBody] ResetPasswordModel resetPasswordModel)
        {
            try
            {
                var hash = await _userHashesService.GetUserHashByHash(resetPasswordModel.UserHash);

                if (hash == null)
                {
                    return NotFound();
                }

                var changePassword = new ChangePasswordModel
                {
                    Password = resetPasswordModel.Password,
                    ConfirmPassword = resetPasswordModel.ConfirmPassword
                };

                var isPasswordValid = await _passwordService.ValidatePassword(changePassword);

                if (isPasswordValid.Errors.Count > 0)
                {
                    throw new ValidationException(isPasswordValid.Errors);
                }

                var userInfo = await _usersService.GetUsuarioById(hash.UserId);
                var userSaltBytes = await _passwordService.ConvertStringSaltToByteArray(userInfo.Salt);

                userInfo.Contrasena = await _passwordService.HashPassword(resetPasswordModel.Password, userSaltBytes);
                
                if (userInfo == null)
                {
                    throw new Exception("El usuario no existe");
                }

                await _usersService.Update(userInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.Message
                    });
                throw;
            }
        }
        [Route("check-hash-valid")]
        [HttpGet]
        public async Task<IActionResult> CheckIfHashValid(string hash)
        {
            try
            {
                var userHash = await _userHashesService.GetUserHashByHash(hash);

                if (userHash == null)
                {
                    return Ok(false);
                }

                return Ok(true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [Route("reset-password")]
        [HttpPost]
        public async Task<ActionResult> ResetPassword(Email email)
        {
            try
            {
                var user = await _usersService.GetUsuarioByEmail(email.Mail);

                if(user == null)
                {
                    return NotFound();
                }

                //trabajo con el hash y envío de correo
                string userHash = await _usersService.GenerateUserHashes(user.Id);

                await _usersService.SaveUserHash(user.Id, userHash);

                var confirmationEmail = new Message(
                    new string[] { user.CorreoElectronico },
                    "Recuperación de contraseña", 
                    string.Format("Saludos {0}, dirijase al siguiente link para recuperar su contraseña: {1}reset-password/{2}", user.NombreUsuario, _frontEndInfo.Url, userHash), null);
                await _emailSenderService.SendEmailAsync(confirmationEmail);

                return Ok(new Response()
                {
                    Status = "Success",
                    Message = "Se le ha enviado un correo, ábralo y siga los pasos"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.Message
                    });

                throw;
            }
        }
        [Route("confirm-email")]
        [HttpGet]
        public async Task<ActionResult> VerifyEmailExists(string email)
        {
            try
            {
                var emailExists = await _usersService.EmailExistsAsync(email);

                if (emailExists)
                {
                    return Ok(emailExists);
                }

                return NotFound(emailExists);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = "Ha ocurrido un error al momento de procesar la solicitud: " + ex.Message
                    });
                throw;
            }
        }
        [Route("info")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> UserInfo()
        {
            int loggedUserId = Convert.ToInt32((this.User.Identity as ClaimsIdentity).FindFirst("id").Value);
            var user = _mapper.Map<UserInfo>(await _usersService.GetUsuarioById(loggedUserId));

            return Ok(user);
        }
    }
}
