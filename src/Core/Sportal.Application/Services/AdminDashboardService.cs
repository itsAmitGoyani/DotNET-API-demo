using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;
using Sportal.Domain.Entities.DepartmentAggregate;
using Sportal.Models;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Sportal.Application.Services;

[ScopedService]
public class AdminDashboardService
{
    private readonly IRepository _repository;

    public AdminDashboardService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<ActivityLogResponseModel> GetActivityLogAsync(Guid departmentId)
    {
        var visitList = await _repository.GetListAsync<Visit>(e =>
            e.DepartmentId == departmentId);
        
        var maleInn = 0;
        var femaleInn = 0;
        var childrenInn = 0;
        var vehicleInn = 0;
        var maleOut = 0;
        var femaleOut = 0;
        var childrenOut = 0;
        var vehicleOut = 0;
        
        if (visitList is not {Count: > 0})
            return new ActivityLogResponseModel
            {
                MaleInn = maleInn,
                FemaleInn = femaleInn,
                ChildrenInn = childrenInn,
                VehicleInn = vehicleInn,
                MaleOut = maleOut,
                FemaleOut = femaleOut,
                ChildrenOut = childrenOut,
                VehicleOut = vehicleOut
            };
        {
            foreach (var visit in visitList.Where(visit => visit.EntryAtUtc.Date == DateTime.UtcNow.Date))
            {
                var visitor = await _repository.GetAsync<Visitor>(e => e.Id == visit.VisitorId);
                if (visitor == null) continue;
                if (visit.EntryAtUtc.Date == DateTime.UtcNow.Date)
                {
                    switch (visitor.Gender)
                    {
                        case "Male":
                            maleInn += 1;
                            break;
                        case "Female":
                            femaleInn += 1;
                            break;
                    }
        
                    if (visitor.Childern != null)
                    {
                        childrenInn += visitor.Childern.Value;
                    }
        
                    if (visitor.Vahicle != null)
                    {
                        vehicleInn += 1;
                    }
                }
        
                if (visit.ExitAtUtc != null)
                {
                    if (visit.ExitAtUtc.Value.Date == DateTime.UtcNow.Date)
                    {
                        switch (visitor.Gender)
                        {
                            case "Male":
                                maleOut += 1;
                                break;
                            case "Female":
                                femaleOut += 1;
                                break;
                        }
        
                        if (visitor.Childern != null)
                        {
                            childrenOut += visitor.Childern.Value;
                        }
        
                        if (visitor.Vahicle != null)
                        {
                            vehicleOut += 1;
                        }
                    }
                }
            }
        }
        
        
        return new ActivityLogResponseModel
        {
            MaleInn = maleInn,
            FemaleInn = femaleInn,
            ChildrenInn = childrenInn,
            VehicleInn = vehicleInn,
            MaleOut = maleOut,
            FemaleOut = femaleOut,
            ChildrenOut = childrenOut,
            VehicleOut = vehicleOut
        };
    }
}