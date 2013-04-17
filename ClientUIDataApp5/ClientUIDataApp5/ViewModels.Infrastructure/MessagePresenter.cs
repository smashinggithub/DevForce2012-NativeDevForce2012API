using System;
using Intersoft.Client.Framework;
using Intersoft.Client.UI.Aqua.UXDesktop;

namespace ClientUIDataApp5.ViewModels
{
    public interface IMessagePresenter
    {
        void ShowInformationMessage(string message);
        void ShowErrorMessage(string message);
        void AskYesOrNo(string question, Action yesAction = null, Action noAction = null);
    }

    public class MessagePresenter : IMessagePresenter
    {
        public void ShowInformationMessage(string message)
        {
            MessageBoxServiceProvider.Show(
              message,
              "Information", MessageBoxButton.OK, MessageBoxImage.Information, null);
        }

        public void ShowErrorMessage(string message)
        {
            MessageBoxServiceProvider.Show(
              message,
              "Error", MessageBoxButton.OK, MessageBoxImage.Error, null);
        }

        public void AskYesOrNo(string question, Action yesAction, Action noAction)
        {
            MessageBoxServiceProvider.Show(
              "Are you sure you want to delete the selected contact?",
              "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question,

              dialogResult =>
              {
                  if (dialogResult == DialogResult.Yes)
                  {
                      if (yesAction != null)
                          yesAction();
                  }
                  else
                  {
                      if (noAction != null)
                          noAction();
                  }
              });
        }
    }
}