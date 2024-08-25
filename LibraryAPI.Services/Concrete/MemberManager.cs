using AutoMapper;
using LibraryAPI.Entities.DTOs.MemberDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services.Concrete
{
    public class MemberManager : IMemberService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberManager(IRepositoryManager repositoryManager, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<MemberResponse>> GetAllMembersAsync(bool trackChanges)
        {
            var members = await _repositoryManager.MemberRepository.GetAllMembersAsync(trackChanges);
            return _mapper.Map<IEnumerable<MemberResponse>>(members);
        }

        public async Task<MemberResponse> GetMemberByIdNumberAsync(string idNumber, bool trackChanges)
        {
            var member = await _repositoryManager.MemberRepository.GetMemberByIdNumberAsync(idNumber, trackChanges);
            if (member == null) throw new KeyNotFoundException("Member not found");

            return _mapper.Map<MemberResponse>(member);
        }

        public async Task<MemberResponse> GetMemberByIdAsync(string id, bool trackChanges)
        {
            var member = await _repositoryManager.MemberRepository.GetMemberByIdAsync(id, trackChanges);
            if (member == null) throw new KeyNotFoundException("Member not found");

            return _mapper.Map<MemberResponse>(member);
        }

        public async Task AddMemberAsync(MemberRequest memberRequest)
        {
            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == memberRequest.Email || u.UserName == memberRequest.UserName);
            if (existingUser != null)
                throw new InvalidOperationException("A user with the same email or username already exists.");

            var existingMember = await _repositoryManager.MemberRepository.GetMemberByIdNumberAsync(memberRequest.IdNumber, false);
            if (existingMember != null)
            {
                if (existingMember.MemberStatus == MemberStatus.BlockedAccount.ToString())
                {
                    throw new InvalidOperationException("The member with this ID number is blocked.");
                }
                else if (existingMember.MemberStatus == MemberStatus.RemovedAccount.ToString())
                {
                    existingMember.MemberStatus = MemberStatus.ActiveAccount.ToString();
                    existingMember.ApplicationUser!.Email = memberRequest.Email;
                    existingMember.ApplicationUser.UserName = memberRequest.UserName;
                    existingMember.ApplicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(existingMember.ApplicationUser, memberRequest.Password);
                    _repositoryManager.MemberRepository.Update(existingMember);
                    await _repositoryManager.SaveAsync();
                    return;
                }
            }

            var user = new ApplicationUser
            {
                IdNumber = memberRequest.IdNumber,
                Name = memberRequest.Name,
                LastName = memberRequest.LastName,
                Gender = memberRequest.Gender,
                BirthDate = memberRequest.BirthDate,
                UserName = memberRequest.UserName,
                PhoneNumber = memberRequest.PhoneNumber,
                Email = memberRequest.Email,
            };

            var result = await _userManager.CreateAsync(user, memberRequest.Password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, "Member");

            var member = new Member
            {
                MemberId = user.Id,
                MemberEducation = memberRequest.EducationalDegree,
                MemberStatus = MemberStatus.ActiveAccount.ToString(),
            };

            await _repositoryManager.MemberRepository.CreateAsync(member);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateMemberAsync(string id, MemberRequest memberRequest)
        {
            var member = await _repositoryManager.MemberRepository.GetMemberByIdAsync(id, true);
            if (member == null) throw new KeyNotFoundException("Member not found");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == memberRequest.Email || u.UserName == memberRequest.UserName);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                throw new InvalidOperationException("A user with the same email or username already exists.");
            }

            user.IdNumber = memberRequest.IdNumber;
            user.Name = memberRequest.Name;
            user.LastName = memberRequest.LastName;
            user.Gender = memberRequest.Gender;
            user.BirthDate = memberRequest.BirthDate;
            user.UserName = memberRequest.UserName;
            user.PhoneNumber = memberRequest.PhoneNumber;
            user.Email = memberRequest.Email;

            var userResult = await _userManager.UpdateAsync(user);
            if (!userResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", userResult.Errors.Select(e => e.Description)));
            }

            member.MemberEducation = memberRequest.EducationalDegree;
            _repositoryManager.MemberRepository.Update(member);
            await _repositoryManager.SaveAsync();
        }

        public async Task SetMemberStatusAsync(string idNumber, string status)
        {
            var member = await _repositoryManager.MemberRepository.GetMemberByIdNumberAsync(idNumber, true);
            if (member == null) throw new KeyNotFoundException("Member not found");

            member.MemberStatus = status;
            _repositoryManager.MemberRepository.Update(member);
            await _repositoryManager.SaveAsync();
        }

        public async Task AddMemberAddressAsync(MemberAddress memberAddress)
        {
            var member = await _repositoryManager.MemberRepository.GetMemberByIdAsync(memberAddress.MemberId, false);
            if (member == null) throw new KeyNotFoundException("Member not found");

            await _repositoryManager.MemberAddressRepository.CreateAsync(memberAddress);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateMemberPasswordAsync(string id, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
