using MediatR;
using MyBucketList.Core.Domain.Features.BucketlistItems.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyBucketList.Mobile.ViewModels
{
    public class AddEditBucketlistItemPageViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        public AddEditBucketlistItemPageViewModel(INavigationService navigationService, IMediator mediator) : base(navigationService)
        {
            _mediator = mediator;
            SaveCommand = new DelegateCommand(async () => await SaveCommandExecute());
        }

        private async Task SaveCommandExecute()
        {
            await _mediator.Send(new AddEditBucketlistItemRequest { Title = Title });
            await NavigationService.GoBackAsync();
        }

        public ICommand SaveCommand { get; }

        public string Title { get; set; }
    }
}
