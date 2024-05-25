using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PROJECT_K.Models;

using static PROJECT_K.Repository.Constants;
using Microsoft.IdentityModel.JsonWebTokens;


namespace PROJECT_K.Repository
{
    public class PKRepository : IPKRepository
    {
        private readonly PK_DbContext pK_DbContext;
        private readonly JwtService jwtService;
        private readonly string secretKey;

        public PKRepository(PK_DbContext _DbContext, JwtService jwt, IConfiguration configuration)
        {
            pK_DbContext = _DbContext;
            jwtService = jwt;
            secretKey = configuration["JwtSettings:Key"];
        }

        public async Task<object> GetAllUsers(string token)
        {
            var role = JwtDecoder.DecodeJwtToken(token, secretKey,"role");
            if(role=="SuperAdmin" || role=="Admin"){
                return await pK_DbContext.UserInfo.ToListAsync();
            }
            return UNAUTHORIZED_ACCESS;
        }

        public async Task<object> Register(Users users)
        {
            List<Users> Allusers = new List<Users>();
            StoringSalt storingSalt = new StoringSalt();
            try
            {
                pK_DbContext.Database.EnsureCreated();
                Allusers = await (from ui in pK_DbContext.UserInfo
                                  where ui.Email == users.Email || ui.Phone == users.Phone
                                  select ui).ToListAsync();

                if (Allusers.Count == 0)
                {
                    Guid guid = Guid.NewGuid();
                    users.Guid = guid;
                    string password = users.Password;
                    byte[] salt = GenerateSalt();
                    storingSalt.Guid = users.Guid;
                    storingSalt.Salt = salt;
                    await pK_DbContext.Salt.AddAsync(storingSalt);
                    string hashedPassword = HashPassword(password, salt);
                    users.Password = hashedPassword;
                    await pK_DbContext.UserInfo.AddAsync(users);
                    await pK_DbContext.SaveChangesAsync();
                    return users;
                }
                return EXISTING_USER;
            }
            catch (Exception)
            {
                return UNABLE_TO_REGISTER;
            }
        }

        public async Task<object> StatusUpdate(string token, Guid guid)
        {
            Users users = new Users();
            try
            {
                users = await (from ui in pK_DbContext.UserInfo
                    where ui.Guid == guid
                    select ui).FirstAsync();
                var role = JwtDecoder.DecodeJwtToken(token, secretKey, "role");
                var username = JwtDecoder.DecodeJwtToken(token, secretKey, "username");
                if(users!=null){
                    if (role == "SuperAdmin" || role == "Admin"){
                        if(username!=users.Email){
                            await pK_DbContext.Database.EnsureCreatedAsync();
                            users.UserStatusId = users.UserStatusId==1?0:1;
                            await pK_DbContext.SaveChangesAsync();
                            return STATUS_CHANGES;
                        }
                        return CURRENT_USER_STATUS_NOUPDATE;
                    }
                    return UNAUTHORIZED_ACCESS;
                }
                return INVALID_ID;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Update the role to the user using the guid and checking the token for access

        public async Task<object> UpdateRole(string token, Guid guid, string Role){
            Users users = new Users();
            try
            {
                await pK_DbContext.Database.EnsureCreatedAsync();
                users = await (from ui in pK_DbContext.UserInfo
                        where ui.Guid == guid
                        select ui).FirstAsync();
                var role = JwtDecoder.DecodeJwtToken(token, secretKey,"role");
                var username = JwtDecoder.DecodeJwtToken(token, secretKey, "username");
                if(role=="SuperAdmin"){
                    if (users != null && users.Email!=username){
                        var roleid = await (from r in pK_DbContext.Roles
                                            where r.RoleName==Role
                                            select r.Id).FirstAsync();
                        users.RolesId = roleid;
                        await pK_DbContext.SaveChangesAsync();
                        return users;
                    }
                    else{
                        return UPDATE_CURRENT_USER;
                    }
                }
                return UNAUTHORIZED_ACCESS;
            }
            catch (System.Exception)
            {
                return ERR_DB_CONN;
            }
        }
        public async Task<object> UpdatePassword(string token, Guid guid, string currentpassword, string newpassword)
        {
            Users users = new Users();
            StoringSalt ss = new StoringSalt();
            try
            {
                users = await (from ui in pK_DbContext.UserInfo
                                where ui.Guid==guid
                                select ui).FirstAsync();
                ss = await (from ui in pK_DbContext.Salt
                                where ui.Guid==guid
                                select ui).FirstAsync();
                var username = JwtDecoder.DecodeJwtToken(token, secretKey, "username");

                if(users!=null && ss!=null){
                    //string password = users.Password;
                    if(username==users.Email){
                        bool passwordMatch = VerifyPassword(currentpassword, users.Password);
                        if(passwordMatch){
                            byte[] salt = GenerateSalt();
                            ss.Guid = guid;
                            ss.Salt = salt;
                            string hashedPassword = HashPassword(newpassword, salt);
                            users.Password = hashedPassword;
                            await pK_DbContext.SaveChangesAsync();
                            return PASSWORD_UPDATED;
                        }
                        return WRONG_PASSWORD;
                    }
                    return UNABLE_TO_CHANGE_PASSWORD;
                }
                return NEW_USER;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        //Adding the properties to database
        public async Task<object> AddProperties(string token, Properties properties){
            try
            {
                var role = JwtDecoder.DecodeJwtToken(token, secretKey, "role");
                if(role=="SuperAdmin"){
                    await pK_DbContext.Database.EnsureCreatedAsync();
                    await pK_DbContext.Properties.AddAsync(properties);
                    await pK_DbContext.SaveChangesAsync();
                    return PROPERTY_ADDED;
                }
                return UNAUTHORIZED_ACCESS;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        //Updating the existing property
        public async Task<object> GetAllProperties(string token){
            List<Properties> properties = new List<Properties>();
            try
            {
                //Guid guid = Guid.Parse(guidString);
                Guid uid = Guid.Parse(JwtDecoder.DecodeJwtToken(token,secretKey,"userId"));
                var user = await(from ui in pK_DbContext.UserInfo
                                    where ui.Guid==uid
                                    select ui).FirstAsync();
                if(user!=null){
                    properties = await (from property in pK_DbContext.Properties
                                        select property).ToListAsync();
                    return properties;
                }
                return NEW_USER;
            }
            catch (System.Exception)
            {
                return "SORRY FOR THE TROUBLE, CAN YOU PLEASE LOGIN AGAIN !!";
            }
        }
        public async Task<object> UpdateProperties(string token,Guid propertyId, Properties properties){
            try
            {
                var role = JwtDecoder.DecodeJwtToken(token, secretKey, "role");
                if(role=="SuperAdmin"){
                    await pK_DbContext.Database.EnsureCreatedAsync();
                    var property = await (from prop in pK_DbContext.Properties
                    where prop.PropertyId==propertyId
                    select prop).FirstAsync();
                    if(property!=null){
                        property.PropertyName=properties.PropertyName;
                        property.Address=properties.Address;
                        property.City=properties.City;
                        property.Pincode = properties.Pincode;
                        property.State=properties.State;
                        property.TotalPlots=properties.TotalPlots;
                        property.WestFacingPlots=properties.WestFacingPlots;
                        property.EastFacingPlots = properties.EastFacingPlots;
                        property.EFPrice = properties.EFPrice;
                        property.WFPrice = properties.WFPrice;
                        await pK_DbContext.SaveChangesAsync();
                        return UPDATED_SUCCESSFULLY;
                    }
                    return INVALID_ID;
                }
                return UNAUTHORIZED_ACCESS;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        public async Task<object> Booking(string token,Guid propertyId, string Facing, double paidAmount){
            Properties property = new Properties();
            BookingHistory newBooking = new BookingHistory();
            Users user = new Users();
            try
            {
                var userId = Guid.Parse(JwtDecoder.DecodeJwtToken(token, secretKey, "userId"));
                user = await pK_DbContext.UserInfo.Where(x=>x.Guid==userId).FirstAsync();
                if(user!=null){
                    property = await (from prop in pK_DbContext.Properties
                    where prop.PropertyId==propertyId
                    select prop).FirstAsync();
                    if(property!=null){
                        if(Facing=="EastFacing"){
                            if(property.EastFacingPlots>0){
                                await pK_DbContext.Database.EnsureCreatedAsync();
                                newBooking.Users = userId;
                                newBooking.Properties = propertyId;
                                newBooking.Facing = Facing;
                                newBooking.PendingAmount = property.EFPrice;
                                newBooking.PendingAmount -= paidAmount;
                                newBooking.TotalAmount = property.EFPrice;
                                await pK_DbContext.BookingHistory.AddAsync(newBooking);
                                property.EastFacingPlots--;
                                property.TotalPlots--;
                                await pK_DbContext.SaveChangesAsync();
                                return BOOKED_SUCCESSFULLY;
                            }
                            else{
                                return NO_PLOTS_AVAILABLE;
                            }
                        }
                        else if(Facing=="WestFacing"){
                            if(property.WestFacingPlots>0){
                                await pK_DbContext.Database.EnsureCreatedAsync();
                                newBooking.Users = userId;
                                newBooking.Properties = propertyId;
                                newBooking.Facing = Facing;
                                newBooking.PendingAmount = property.WFPrice;
                                newBooking.PendingAmount -= paidAmount;
                                newBooking.TotalAmount = property.WFPrice;
                                await pK_DbContext.BookingHistory.AddAsync(newBooking);
                                property.WestFacingPlots--;
                                property.TotalPlots--;
                                await pK_DbContext.SaveChangesAsync();
                                return BOOKED_SUCCESSFULLY;
                            }
                            else{
                                return NO_PLOTS_AVAILABLE;
                            }
                        }
                    }
                    return INVALID_ID;
                }
                return UNAUTHORIZED_ACCESS;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }


        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public async Task<object> Login(string username, string password)
        {
            Users users = new Users();
            try
            {
                users = await (from ui in pK_DbContext.UserInfo
                               where ui.Email == username
                               select ui).FirstAsync();
                if (users != null)
                {
                    if (users.UserStatusId == 1)
                    {
                        string storedPassword = users.Password;
                        bool passwordMatch = VerifyPassword(password, storedPassword);
                        string role = await (from r in pK_DbContext.Roles
                                             where r.Id == users.RolesId
                                             select r.RoleName).FirstAsync();
                        string uid = users.Guid.ToString();
                        if (passwordMatch)
                        {
                            var token = jwtService.GenerateToken(uid, username, role);
                            return token;
                        }
                    }
                    else
                    {
                        return DEACTIVE_USER;
                    }
                }
                return NEW_USER;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            byte[] storedHashBytes = Convert.FromBase64String(storedHashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(storedHashBytes, 0, salt, 0, 16);

            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000))
            {
                byte[] enteredHash = pbkdf2.GetBytes(20);
                for (int i = 0; i < 20; i++)
                {
                    if (enteredHash[i] != storedHashBytes[i + 16])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
