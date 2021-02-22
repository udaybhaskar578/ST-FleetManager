using System;

namespace FleetManagementSystem.Models.ViewModel
{
    public class ErrorViewModel
    {
        public string ErrorMessage { get; set; }

        public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    }
}
