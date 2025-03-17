using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Services;

namespace RecruitmentAgency.Desktop.ViewModels;

public partial class MiniProfileViewModel : ViewModelBase
{
    private readonly UserContainer _userContainer;
    
    public MiniProfileViewModel(IServiceProvider serviceProvider)
    {
        _userContainer = serviceProvider.GetRequiredService<UserContainer>();

        Refresh();
    }

    public void Refresh()
    {
        PhoneNumber = _userContainer.PhoneNumber;
    }

    [ObservableProperty]
    private string? _phoneNumber;
}