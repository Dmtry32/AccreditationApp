using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AccreditationApp.Pages
{
    public class ClientLogin : PageModel
    {
        [BindProperty] public string Identifier { get; set; }
        [BindProperty] public string PhoneNumber { get; set; }
        [BindProperty] public string Code { get; set; }
        public bool Sent { get; set; }
        public string ErrorMessage { get; set; }

        private const string SessionCodeKey = "MfaCode";

        public void OnPost()
        {
            // Generate code
            var mfaCode = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString(SessionCodeKey, mfaCode);

            // Send SMS via Twilio
            var accountSid = "US733dc47c5e82cb1f91bf73c4cd2edad5";
            var authToken = "M71Z3JPPUZK54GML51K1WKJB";
            var fromPhone = "+33746401202"; // Your Twilio number

            TwilioClient.Init(accountSid, authToken);
            MessageResource.Create(
                to: new PhoneNumber(PhoneNumber),
                from: new PhoneNumber(fromPhone),
                body: $"Votre code de vérification est : {mfaCode}"
            );

            Sent = true;
        }

        public void OnPostVerify()
        {
            var expectedCode = HttpContext.Session.GetString(SessionCodeKey);
            if (Code == expectedCode)
            {
                // Authenticated: set cookie/session as needed
                Sent = false;
                ErrorMessage = "";
                // Redirect or show success
            }
            else
            {
                ErrorMessage = "Code incorrect.";
                Sent = true;
            }
        }
    }
}