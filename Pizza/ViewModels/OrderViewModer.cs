using Pizza.Models;
using Pizza.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pizza.ViewModels
{
    class OrderViewModer : BindableBase
    {
       
        private IOrderRepository _repository;
        public OrderViewModer(IOrderRepository repository)
        {

            _repository = repository;
            Orders = new ObservableCollection<Order>();
            LoadOrders();
            CloseCommand = new RelayCommand(Close);
        }
         private Customer _selectedCustomer;

    public Customer SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            SetProperty(ref _selectedCustomer, value);
        }
    }
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public RelayCommand CloseCommand { get; }

        public event Action? Done;

        public void Close()
        {
            Done?.Invoke();
        }

        public async Task LoadOrders()
        {
            var orders = await _repository.GetOrdersByCustomerAsync(SelectedCustomer.Id);
            Orders.Clear();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
        }

        public async Task LoadAllOrders()
        {
            
            if (SelectedCustomer == null) 
            { 
                var orders = await _repository.GetAllOrdersAsync();
                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }
            }
        }

        public event Action<Order> PlaceRequested = delegate { };

        private void OnPlace(Order order)
        {
            PlaceRequested(order);
        }
    }
}
