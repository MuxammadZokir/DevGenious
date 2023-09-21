using DevGenious.Data.IRepositories;
using DevGenious.Data.Repositories;
using DevGenious.Domain.Entities;
using DevGenious.Service.DTOs;
using DevGenious.Service.Exceptions;
using DevGenious.Service.Interfaces;

namespace DevGenious.Service.Services;

public class SubjectService : ISubjectService
{
    private long _id;
    private readonly IRepository<Subject> subjectRepository = new Repository<Subject>();
    public async Task<SubjectForResultDto> CreateAsync(SubjectForCreationDto dto)
    {
        var subject = (await subjectRepository.SelectAllAsync()).
            FirstOrDefault(s => s.Name.ToLower() == dto.Name.ToLower());
        if (subject != null)
            throw new CustomException(409, "Subject is already exist");

        await GenerateIdAsync();

        var subjectForUpdating = new Subject()
        {
            Id = _id,
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
        };

        await subjectRepository.InsertAsync(subjectForUpdating);

        var result = new SubjectForResultDto()
        {
            Id = subjectForUpdating.Id,
            Name = subjectForUpdating.Name,
            Price = subjectForUpdating.Price,
            Description = subjectForUpdating.Description,
        };

        return result;
    }

    public async Task<List<SubjectForResultDto>> GetAllAsync()
    {
        List<Subject> subjects = await subjectRepository.SelectAllAsync();
        List<SubjectForResultDto> mappedSubjects = new List<SubjectForResultDto>();

        foreach (var subject in subjects)
        {
            var temp = new SubjectForResultDto() 
            {
                Id = subject.Id,
                Name = subject.Name,
                Price = subject.Price,
                Description = subject.Description,
            };
            mappedSubjects.Add(temp);
        }
        return mappedSubjects;
    }

    public async Task<SubjectForResultDto> GetByIdAsync(long id)
    {
        var subject = await subjectRepository.SelectByIdAsync(id);
        if(subject == null)
                throw new CustomException(404, "Subject is not found");

        var result = new SubjectForResultDto()
        {
            Id = subject.Id,
            Name = subject.Name,
            Price = subject.Price,
            Description = subject.Description,
        };

        return result;  
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var subject = await subjectRepository.SelectByIdAsync(id);
        if (subject == null)
            throw new CustomException(404, "Subject is not found");

        await subjectRepository.DeleteAsync(id);

        return true;
    }

    public async Task<SubjectForResultDto> UpdateAsync(SubjectForUpdateDto dto)
    {
        var subject = await subjectRepository.SelectByIdAsync(dto.Id);
        if (subject == null)
            throw new CustomException(404, "Subject is not found");

        var meppedSubject = new Subject()
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description,
            UpdatedAt = DateTime.UtcNow,
        };

        await subjectRepository.UpdateAsync(meppedSubject);

        var result = new SubjectForResultDto()
        {
            Id = meppedSubject.Id,
            Name = meppedSubject.Name,
            Price = meppedSubject.Price,
            Description = meppedSubject.Description,
        };

        return result;
    }

    public async Task GenerateIdAsync()
    {
        var subjects = await subjectRepository.SelectAllAsync();
        if (subjects.Count == 0)
        {
            this._id = 1;
        }
        else
        {
            var subject = subjects[subjects.Count() - 1];
            this._id = ++subject.Id;
        }
    }
}
