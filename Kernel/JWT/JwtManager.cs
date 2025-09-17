using Kernel.Contract;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kernel.JWT
{
    /// <summary>
    /// Json web token manager
    /// </summary>
    public class JwtManager : IJwtManager
    {
        static List<Type> directTypes = new List<Type> { typeof(string), typeof(Guid), typeof(Guid?), typeof(DateTime), typeof(DateTime?), typeof(int), typeof(int?), typeof(byte), typeof(byte?), typeof(short), typeof(short?), typeof(long), typeof(long?), typeof(float), typeof(float?), typeof(double), typeof(double?), typeof(bool), typeof(bool?), typeof(decimal), typeof(decimal?) };

        private IIdentitySettings _identitySettings = null;
        public JwtManager(IIdentitySettings identitySettings)
        {
            _identitySettings = identitySettings;
        }

        /// <summary>
        /// Generate access token
        /// </summary>
        /// <returns>Access Token</returns>
        public string GenerateToken(string subject, int tokenExpiryInSeconds = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subject))
                {
                    subject = "unknown";
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_identitySettings.JwtSettings.SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("UserName", subject) }),
                    Expires = DateTime.UtcNow.AddSeconds(tokenExpiryInSeconds > 0 ? tokenExpiryInSeconds : _identitySettings.UserLoginSetting.TokenExpiryInSeconds),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                JwtSecurityToken token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);

                Dictionary<string, object> tokenObj = new Dictionary<string, object>();
                tokenObj.Add("access_token", tokenHandler.WriteToken(token));
                tokenObj.Add("expires_in", token.Payload.Exp.Value);
                tokenObj.Add("token_type", "Bearer");
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Generate entity access token
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="session">Data</param>
        /// <returns>Entity access token</returns>
        public object GenerateEntityToken<T>(T session, int tokenExpiryInSeconds = 0) where T : class
        {
            try
            {
                SecurityTokenDescriptor tokenDescriptor;
                var propsValues = typeof(T).GetProperties();
                var tokenProps = new List<TokenProperty>();

                foreach (var propsValue in propsValues)
                {
                    var currentValue = propsValue.GetValue(session);

                    if (currentValue != null)
                    {
                        if (currentValue is ICollection)
                        {
                            var list = currentValue as ICollection;
                            if (list != null && list.Count > 0)
                            {
                                tokenProps.Add(new TokenProperty { Type = propsValue.PropertyType, Name = propsValue.Name, Value = list });
                            }
                        }
                        else
                        {
                            tokenProps.Add(new TokenProperty { Type = propsValue.PropertyType, Name = propsValue.Name, Value = currentValue });
                        }
                    }
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_identitySettings.JwtSettings.SecretKey);
                var isNeverExpire = tokenProps.FirstOrDefault(x => x.Name.Equals("IsNeverExpire"));

                if (tokenProps == null || !tokenProps.Any())
                {
                    tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[] { new Claim("UserName", "unknown") }),
                        Expires = DateTime.UtcNow.AddSeconds(tokenExpiryInSeconds > 0 ? tokenExpiryInSeconds : _identitySettings.UserLoginSetting.TokenExpiryInSeconds),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                }
                else
                {
                    List<Claim> claims = new List<Claim>();
                    if (!tokenProps.Any(x => x.Name.EqualsIgnoreCase("UserName")))
                    {
                        claims.Add(new Claim("UserName", "unknown"));
                    }
                    claims.AddRange(tokenProps.Select(x => new Claim(x.Name, directTypes.Contains(x.Type) ? x.Value.ToString() : JsonSerializer.Serialize(x.Value, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = false,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    }))));

                    tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = isNeverExpire != null && (bool)isNeverExpire.Value  ? DateTime.UtcNow.AddYears(5) :
                        tokenExpiryInSeconds > 0 ? DateTime.UtcNow.AddSeconds(tokenExpiryInSeconds) : 
                        DateTime.UtcNow.AddSeconds(_identitySettings.UserLoginSetting.TokenExpiryInSeconds),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                }

                JwtSecurityToken token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);
                var tokenStr = tokenHandler.WriteToken(token);

               

                Dictionary<string, object> tokenObj = new Dictionary<string, object>();
                tokenObj.Add("access_token", tokenStr);
                tokenObj.Add("expires_in", token.Payload.Exp.Value);
                tokenObj.Add("token_type", "Bearer");
                return tokenObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Parse token to cliams
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>Token claims</returns>
        public IEnumerable<Claim> ParseToken(string token, bool validateLifeTime = true)
        {
            try
            {
                if (token == null)
                    return null;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_identitySettings.JwtSettings.SecretKey);

               

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = validateLifeTime,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Claim> ParseTokenV2(string token)
        {
            try
            {
                if (token == null)
                    return null;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_identitySettings.JwtSettings.SecretKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        class TokenProperty
        {
            public string Name { get; set; }
            public object Value { get; set; }
            public Type Type { get; set; }
        }
    }
}
