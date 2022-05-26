using MediatR;
using MyBucketList.Core.Domain.Entities;
using MyBucketList.Core.Domain.Features.BucketlistItems.Models;
using MyBucketList.Mobile.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyBucketList.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        public MainPageViewModel(INavigationService navigationService, IMediator mediator)
            : base(navigationService)
        {
            Title = "Main Page";
            _mediator = mediator;

            AddBucketlistItemCommand = new DelegateCommand(AddBucketlistItem);
        }

        private void AddBucketlistItem()
        {
            NavigationService.NavigateAsync(nameof(AddEditBucketlistItemPage));
        }

        private ObservableCollection<BucketlistItem> _bucketlistItems;
        public ObservableCollection<BucketlistItem> BucketlistItems
        {
            get => _bucketlistItems;
            set => SetProperty(ref _bucketlistItems, value);
        }

        public ICommand AddBucketlistItemCommand { get; }

        public async Task LoadItems()
        {
            var result = await _mediator.Send(new GetAllBucketlistItemRequest());

            if (result?.Any() == true)
                BucketlistItems = new ObservableCollection<BucketlistItem>(result);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            await LoadItems();
        }
    }
}
