using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Tilegram.Services.Authentication
{
    public class JwtService
    {
        private readonly string _secretKey;

        public JwtService(string secretKey)
        {
            _secretKey = secretKey;
        }


        // Métodos auxiliares (deben coincidir con los de generación)
        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }

        private static byte[] Base64UrlDecode(string input)
        {
            string base64 = input
                .Replace('-', '+')
                .Replace('_', '/');

            // Añadir padding si es necesario
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }

        // Generar token (tu método original)
        public string GenerateToken(Dictionary<string, object> payload, TimeSpan? expiry = null)
        {
            var header = new { alg = "HS256", typ = "JWT" };
            string headerJson = JsonConvert.SerializeObject(header);
            string encodedHeader = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));

            // Agregar expiración si se especifica
            if (expiry.HasValue)
            {
                var exp = DateTimeOffset.UtcNow.Add(expiry.Value).ToUnixTimeSeconds();
                payload["exp"] = exp;
            }

            // Agregar timestamp de emisión
            payload["iat"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            string payloadJson = JsonConvert.SerializeObject(payload);
            string encodedPayload = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));

            string unsignedToken = $"{encodedHeader}.{encodedPayload}";

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey)))
            {
                byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));
                string signature = Base64UrlEncode(signatureBytes);
                return $"{unsignedToken}.{signature}";
            }
        }

        // Leer y validar token
        public TokenValidationResult ValidateToken(string token)
        {
            try
            {
                var parts = token.Split('.');
                if (parts.Length != 3)
                    return TokenValidationResult.Invalid("Formato de token inválido");

                var encodedHeader = parts[0];
                var encodedPayload = parts[1];
                var encodedSignature = parts[2];

                // Verificar firma
                var unsignedToken = $"{encodedHeader}.{encodedPayload}";
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey)))
                {
                    byte[] computedSignatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));
                    string computedSignature = Base64UrlEncode(computedSignatureBytes);

                    if (!string.Equals(computedSignature, encodedSignature, StringComparison.Ordinal))
                        return TokenValidationResult.Invalid("Firma inválida");
                }

                // Decodificar payload
                var payload = DecodePayload(encodedPayload);

                // Verificar expiración
                if (payload.ContainsKey("exp"))
                {
                    var exp = Convert.ToInt64(payload["exp"]);
                    var expiryTime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;

                    if (expiryTime < DateTime.UtcNow)
                        return TokenValidationResult.Expired(payload);
                }

                return TokenValidationResult.Valid(payload);
            }
            catch (Exception ex)
            {
                return TokenValidationResult.Invalid($"Error: {ex.Message}");
            }
        }

        // Solo leer el payload sin validar (útil para debug)
        public Dictionary<string, object> ReadPayload(string token)
        {
            try
            {
                var parts = token.Split('.');
                if (parts.Length != 3)
                    return null;

                var encodedPayload = parts[1];
                return DecodePayload(encodedPayload);
            }
            catch
            {
                return null;
            }
        }

        private Dictionary<string, object> DecodePayload(string encodedPayload)
        {
            byte[] payloadBytes = Base64UrlDecode(encodedPayload);
            string payloadJson = Encoding.UTF8.GetString(payloadBytes);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadJson);
        }

        // Clase para resultados de validación
        public class TokenValidationResult
        {
            public bool IsValid { get; set; }
            public Dictionary<string, object> Payload { get; set; }
            public string Error { get; set; }
            public bool IsExpired { get; set; }

            public static TokenValidationResult Valid(Dictionary<string, object> payload)
                => new TokenValidationResult { IsValid = true, Payload = payload };

            public static TokenValidationResult Expired(Dictionary<string, object> payload)
                => new TokenValidationResult { IsValid = false, IsExpired = true, Payload = payload, Error = "Token expirado" };

            public static TokenValidationResult Invalid(string error)
                => new TokenValidationResult { IsValid = false, Error = error };
        }
    }
}
