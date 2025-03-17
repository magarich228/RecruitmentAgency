using System;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Services;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class MiniProfileViewModel : ViewModelBase
{
    private readonly UserContainer _userContainer;
    
    public MiniProfileViewModel(IServiceProvider serviceProvider)
    {
        _userContainer = serviceProvider.GetRequiredService<UserContainer>();
    }

    public string PhoneNumber => _userContainer.PhoneNumber ?? 
                                 throw new ApplicationException("Phone number is null");
}