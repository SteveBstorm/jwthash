using Demo_WepAPI_Token.DAL.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Demo_WepAPI_Token.TokenJWT
{
    public class TokenManager
    {
        // TODO Add in config
        public static string secret = "OfOuYD2nzPUfL/WLz8JYDJIlhmVA8IbuO2o1vWzY8UOTG/gaVOaNNBar7hdX59USWfK7AzElt2cU+3JSNCrGRWOe/Vj169O1yRbMskpf1xAoDDSneLhmfYMQQRD+1WT66REh55hpdKsJuoFivlsIQtwN9Aq39H7ATI791QNI7RY=";
        public static string issuer = "mysite.com";
        public static string audience = "mysite.com";

        public string GenerateJWT(UserAppEntity user)
        {
            if (user.Email is null)
                throw new ArgumentNullException();

            // Création des crédentials 
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            // Création de l'objet de securité avec les infos a stocker => Claims
            Claim[] claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, (user.IsAdmin ? "admin" : "user" ))
            };

            // Generation du token => Nuget : System.IdentityModel.Tokens.Jwt
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
