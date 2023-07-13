using System.Linq.Expressions;
using SendGrid.Helpers.Errors.Model;
using Sportal.Application.Utils;
using Sportal.Domain.Entities.DepartmentAggregate;
using Sportal.Models;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Sportal.Application.Services;

[ScopedService]
public class DepartmentService
{
    private readonly IRepository _repository;

    public DepartmentService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DepartmentModel>> GetDepartmentsAsync()
    {
        Expression<Func<Department, DepartmentModel>> expression = ud => new DepartmentModel
        {
            Id = ud.Id,
            Name = ud.Name
        };
        return await _repository.GetListAsync(expression);
    }

    public async Task<EntryResponseModel> EntryAsync(EntryRequestModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var visitorDetails = await _repository.GetAsync<Visitor>(e => e.CNIC == model.CNIC);

        var visit = new Visit
        {
            Id = Guid.NewGuid(),
            DepartmentId = model.DepartmentId,
            PhoneNumber = model.PhoneNumber,
            EntryBy = model.EntryBy,
            EntryAtUtc = DateTime.UtcNow
        };

        if (visitorDetails != null)
        {
            visitorDetails.PhoneNumber = model.PhoneNumber;
            await _repository.UpdateAsync(visitorDetails);
            visit.VisitorId = visitorDetails.Id;
        }
        else
        {
            var visitor = new Visitor
            {
                Id = Guid.NewGuid(),
                PhoneNumber = model.PhoneNumber,
                Name = model.Name,
                Gender = model.Gender,
                Address = model.Address,
                CNIC = model.CNIC,
                DateOfBirth = model.DateOfBirth,
                CardIssue = model.CardIssue,
                CardExpiry = model.CardExpiry,
                Childern = model.Childern,
                Vahicle = model.Vahicle
            };

            object[] primaryKeys = await _repository.InsertAsync(visitor);
            visit.VisitorId = (Guid) primaryKeys[0];
        }

        await _repository.InsertAsync(visit);
        return new EntryResponseModel
        {
            ReceiptId = GuidBase64.Encode(visit.VisitorId)
        };
    }

    public async Task<ExitResponseModel> ExitAsync(ExitRequestModel model)
    {
        try
        {
            var visitId = GuidBase64.Decode(model.ReceiptId);

            Visit? visit = await _repository.GetAsync<Visit>(e => e.Id == visitId);

            if (visit == null)
            {
                throw new NotFoundException("Visitor's entry record not found!");
            }

            visit.ExitBy = model.ExitBy;
            visit.ExitAtUtc = DateTime.UtcNow;

            await _repository.UpdateAsync(visit);

            return new ExitResponseModel {status = "success"};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new BadRequestException("Invalid pass!");
        }
    }
}