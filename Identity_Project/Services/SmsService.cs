using Kavenegar;
using Kavenegar.Models.Enums;
using System.Net;

namespace Identity_Project.Services
{
    public class SmsService
    {
        private readonly KavenegarApi _api;
        public SmsService()
        {
            _api = new KavenegarApi("554D634857335A72422B47564D2F4B68796A5A33426B514E4E5558334335537468413564616D55766E416F3D");
        }
        public Task SendSms(string receptor, string code)
        {
            _api.VerifyLookup(receptor, code, "verifyPhonenumberForIdentityProject");
            return Task.CompletedTask;
        }
    }
}
