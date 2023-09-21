using DevGenious.Service.DTOs;

namespace DevGenious.Service.Interfaces;

public interface ISubjectService
{
    public Task<bool> RemoveAsync(long id);
    public Task<List<SubjectForResultDto>> GetAllAsync();
    public Task<SubjectForResultDto> GetByIdAsync(long id);
    public Task<SubjectForResultDto> UpdateAsync(SubjectForUpdateDto dto);
    public Task<SubjectForResultDto> CreateAsync(SubjectForCreationDto dto);
}
