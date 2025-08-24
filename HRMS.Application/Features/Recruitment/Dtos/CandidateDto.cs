using Microsoft.AspNetCore.Http;

namespace HRMS.Application.Features.Recruitment.Dtos;

 public record CandidateDto(
     Guid JobVacancyId ,
     string FirstName , 
     string LastName , 
     string Email , 
     string PhoneNumber ,
     DateTime AppliedOn , 
      string Status
);