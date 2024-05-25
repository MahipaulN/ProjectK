using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PROJECT_K.Models;

namespace PROJECT_K.Repository
{
    public interface IPKRepository
    {
        public Task<object> Register(Users users);
        public Task<object> Login(string username,string password);
        public Task<object> GetAllUsers(string token);
        public Task<object> StatusUpdate(string token, Guid guid);
        public Task<object> UpdateRole(string token, Guid guid, string Role);
        public Task<object> UpdatePassword(string token, Guid guid, string currentpassword, string newpassword);
        public Task<object> AddProperties(string token, Properties properties);
        public Task<object> GetAllProperties(string token);
        public Task<object> UpdateProperties(string token, Guid guid, Properties properties);
        public Task<object> Booking(string token,Guid propertyId, string Facing, double paidAmount);
    }
}