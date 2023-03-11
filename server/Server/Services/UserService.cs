using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.API.Security.Domain.Services;
using Server.API.Security.Domain.Services.Communication;
using Server.API.Security.Domain.Services.Communication.Models;
using Server.API.Server.Domain.Models;
using Server.API.Server.Domain.Repositories;
using Server.API.Server.Domain.Services;
using Server.API.Server.Domain.Services.Communication;
using Server.API.Shared.Domain.Repositories;
using System.Net.Mail;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Server.API.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;


        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _userRepository.ListAsync();
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _userRepository.FindByIdAsync(id);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userRepository.FindByEmailAsync(email);
        }


        public async Task<UserResponse> SaveAsync(User user)
        {
 
            // Verify if this email is registered
            var email = await _userRepository.FindByEmailAsync(user.Email);

            if (email != null)
                return new UserResponse($"Email is registered.");

            try
            {
                await _userRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                return new UserResponse(user);
            }
            catch (Exception e)
            {
                return new UserResponse($"An error ocurred while adding this user: {e.Message}");
            }
        }

        public async Task<UserResponse> UpdateAsync(int id, User user)
        {
            // Validate if user exist
            var existing = await _userRepository.FindByIdAsync(id);

            if (existing == null)
            {
                return new UserResponse("User not found");
            }


            // Verify if this email is being used
            var email = await _userRepository.FindByEmailAsync(user.Email);

            if (email != null)
            {
                //Validate if email is the same
                if (user.Email == existing.Email)
                {
                    return new UserResponse("Don't insert the same email.");
                }

                return new UserResponse($"Email is being used.");
            }


            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
            existing.Password = user.Password;
            existing.ImageProfile = user.ImageProfile;

            try
            {
                _userRepository.Update(existing);
                await _unitOfWork.CompleteAsync();

                return new UserResponse(existing);
            }
            catch (Exception e)
            {
                return new UserResponse($"An error ocurred while updating the user: {e.Message}");
            }
        }

        public async Task<UserResponse> DeleteAsync(int id)
        {
            var existing = await _userRepository.FindByIdAsync(id);

            if(existing == null)
            {
                return new UserResponse("User not found");
            }
            
            try { 
                _userRepository.Remove(existing);
                await _unitOfWork.CompleteAsync();

                return new UserResponse(existing);
            }
            catch (Exception e) {
                return new UserResponse($"An error ocurred while deleting the user: {e.Message}");
            }
        }
    }
}
