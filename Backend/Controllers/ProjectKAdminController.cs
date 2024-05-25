using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROJECT_K.Models;
using PROJECT_K.Repository;


using static PROJECT_K.Repository.Constants;


namespace PROJECT_K.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectKAdminController : ControllerBase
{
    private readonly IPKRepository repository;
    public ProjectKAdminController(IPKRepository repo)
    {
        repository = repo;
    }
    [HttpPost("/Register")]
    public async Task<IActionResult> RegisterUser(Users users){
        try
        {
            var result = await repository.Register(users);
            if(result!=null){
                return Ok(result);
            }
            else{
                return BadRequest(EXISTING_USER);
            }
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    // [Authorize]
    [HttpGet("/Getallusers")]
    public async Task<IActionResult> GetAllUsers(string token){
        try
        {
            var result = await repository.GetAllUsers(token);
            if(result!=null){
                return Ok(result);
            }
            else{
                return BadRequest(NO_USERS);
            }
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    [HttpPut("/Statusupdate")]
    public async Task<IActionResult> StatusUpdate(string token, Guid guid){
        try
        {
            var result = await repository.StatusUpdate(token, guid);
            if(result!=null){
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    [HttpPost("/Login")]
    public async Task<IActionResult> Login(string username, string password){
        try
        {
            var result = await repository.Login(username, password);
            if(result!=null){
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    [HttpPut]
    [Route("/UpdateRole/{token}/{guid}/{Role}")]
    public async Task<IActionResult> UpdateRole(string token, Guid guid, string Role){
        try
        {
            var result = await repository.UpdateRole(token, guid, Role);
            if(result!=null){
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    [HttpPut]
    [Route("/UpdatePassword/{guid}/{token}/{currentPassword}/{newPassword}")]
    public async Task<IActionResult> UpdatePassword
                (string token, Guid guid, string currentPassword, string newPassword){
        try
        {
            var result = await repository
                            .UpdatePassword(token, guid, currentPassword, newPassword);
            if(result!=null){
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    [HttpPost("/AddProperty")]
    public async Task<IActionResult> AddProperty(string token, [FromBody] Properties property){
        try
        {
            var result = await repository.AddProperties(token, property);
            if(result!=null){
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    [HttpGet("/Getallproperties")]
    public async Task<IActionResult> Getallproperties(string token){
        try
        {
            var result = await repository.GetAllProperties(token);
            if(result!=null){
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    [HttpPut("/Updateproperty")]
    public async Task<IActionResult> Updateproperty(string token, Guid propertyId, [FromBody] Properties property){
        try
        {
            var result = await repository.UpdateProperties(token, propertyId, property);
            if(result!=null){
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    [HttpPost("/Booking")]
    public async Task<IActionResult> Booking(string token, Guid propertyId, string Facing, double paidAmount){
        try
        {
            var result = await repository.Booking(token, propertyId, Facing, paidAmount);
            if(result!=null){
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
}
