using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using GUFOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GUFOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("Application/json")]
    public class LoginController : ControllerBase
    {
        GufosContext _context = new GufosContext();
        // Define uma variável parar recorrer nossos métodos as configurações appSettings.js
        private IConfiguration _config;
        //Define um método construtor para validar as informações do appSettings.js
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Usuario login)
        {
            // Cria uma variável do tipo IActionResult que por padrão não é autorizado
            IActionResult resposta = Unauthorized();
            //Valida se o usuario passado no post existe no nosso banco de dados
            var usuario = autenticarUsuario(login);
            //Verifica se o usuário é diferente de nulo
            if (usuario != null)
            {
                //Cria uma variável que armazena o valor do nosso token
                var tokenString = gerarJsonWebToken(usuario);
                //Define se a resposta será 200 Ok e retorna um objeto chamado token com o token
                resposta = Ok(new { token = tokenString });
            }
            //Retorna a resposta
            return resposta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private Usuario autenticarUsuario(Usuario login)
        {
            //Declara uma variável que busca no nosso banco de dados um usuário que tenha e-mail e senha presentes no banco
            var usuario = _context.Usuario.FirstOrDefault(user => user.Email == login.Email && user.Senha == login.Senha);
            //Verifica se a resposta do banco de dados é diferente de null
            if (usuario != null)
            {
                //Retorna um usuário válido no banco de dados
                return login;
            }
            //Retorna um usuário
            return usuario;
        }

        private string gerarJsonWebToken(Usuario infoUsuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Definimos nossas Claims (dados da sessão) para poderem ser capturadas
            //A qualquer momento enquanto o Token for Ativo

            var claims = new[]{
            new Claim(JwtRegisteredClaimNames.NameId, infoUsuario.Nome),
            new Claim(JwtRegisteredClaimNames.Email, infoUsuario.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }}}