using AutoMapper;
using LibraryAPI.Entities.DTOs.LoanDTO;
using LibraryAPI.Entities.DTOs.PenaltyDTO;
using LibraryAPI.Entities.Enums;
using LibraryAPI.Entities.Models;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Abstracts;


namespace LibraryAPI.Services.Concrete
{
    public class PenaltyManager : IPenaltyService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public PenaltyManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all penalties associated with a specific member.
        /// </summary>
        /// <param name="memberId">The ID of the member.</param>
        /// <param name="trackChanges">Indicates whether to track changes.</param>
        /// <returns>A list of penalty responses for the specified member.</returns>
        public async Task<IEnumerable<PenaltyResponse>> GetPenaltiesByMemberIdAsync(string memberId, bool trackChanges)
        {
            var penalties = await _repositoryManager.PenaltyRepository
                .GetPenaltiesByMemberIdAsync(memberId, trackChanges);

            return _mapper.Map<IEnumerable<PenaltyResponse>>(penalties);
        }


        /// <summary>
        /// Retrieves a specific penalty by its ID.
        /// </summary>
        /// <param name="penaltyId">The ID of the penalty.</param>
        /// <param name="trackChanges">Indicates whether to track changes.</param>
        /// <returns>The penalty details.</returns>
        public async Task<PenaltyResponse> GetPenaltyByIdAsync(long penaltyId, bool trackChanges)
        {
            var penalty = await _repositoryManager.PenaltyRepository.GetPenaltyByIdAsync(penaltyId, trackChanges);

            if (penalty == null)
                throw new KeyNotFoundException("Penalty not found");

            return _mapper.Map<PenaltyResponse>(penalty);
        }

        /// <summary>
        /// Calculates penalties for overdue book returns.
        /// </summary>
        /// <param name="loanReturnRequest">The loan return request containing details of the loan return.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CalculatePenaltiesAsync(LoanReturnRequest loanReturnRequest)
        {
            if (loanReturnRequest.ReturnDate > loanReturnRequest.DueDate)
            {
                int overdueDays = (loanReturnRequest.ReturnDate.Value.Date - loanReturnRequest.DueDate.Date).Days;
                decimal dailyFee = 0.5m;

                var penalty = new Penalty
                {
                    DailyFee = dailyFee,
                    StartDate = loanReturnRequest.DueDate.Date,
                    EndDate = loanReturnRequest.ReturnDate.Value.Date,
                    OverdueDays = overdueDays,
                    MemberId = loanReturnRequest.MemberId,
                    TotalFee = dailyFee * overdueDays
                };

                penalty.CalculatePenaltyDetails();

                // Only add penalties with a valid type
                if (penalty.Type != PenaltyType.None.ToString())
                {
                    await _repositoryManager.PenaltyRepository.CreateAsync(penalty);
                    await _repositoryManager.SaveAsync();
                }
            }
            // If no overdue, no action needed
        }
    }
}
