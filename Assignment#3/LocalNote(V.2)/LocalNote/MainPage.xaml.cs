using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LocalNote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        //private async void About_Click(object sender, RoutedEventArgs e)
        //{
        //    // Create the message dialog and set its content
        //    var messageDialog = new MessageDialog("Created by Minsu Lee");
        //    // Show the message dialog
        //    await messageDialog.ShowAsync();
        //}

        //private void Exit_Click(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Exit();
        //}

        //private async void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //https://www.reflectionit.nl/blog/2015/windows-10-xaml-tips-messagedialog-and-contentdialog

        //    //if (mpd.SelectedNote != null)
        //    //{
        //        var deleteDialog = new MessageDialog("Delete Note?");
        //        deleteDialog.Commands.Add(new UICommand("Yes")
        //        {
        //            Id = 0
        //        });
        //        deleteDialog.Commands.Add(new UICommand("No")
        //        {
        //            Id = 1
        //        });
        //        deleteDialog.DefaultCommandIndex = 0;
        //        deleteDialog.CancelCommandIndex = 1;
        //        var result = await deleteDialog.ShowAsync();
        //        if ((int)result.Id == 0)
        //        {
        //            //do your task  
        //        }
        //        else
        //        {
        //            //skip your task  
        //        }
        //    //}
        //}

        //private void EditAppBarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    editAppBarButton.IsEnabled = false;
        //    saveAppBarButton.IsEnabled = true;
        //    NoteContent.IsReadOnly = false;
        //}

        //private async void SaveAppBarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var textBox = new TextBox {Width = 300, Height = 40, FontSize = 20};

        //    var saveDialog = new ContentDialog
        //    {
        //        Title = "Save",
        //        PrimaryButtonText = "Yes",
        //        SecondaryButtonText = "No",
        //    };

        //    saveDialog.Content = textBox;
        //    var savedlg = await saveDialog.ShowAsync();
        //    switch(savedlg)
        //    {
        //        case ContentDialogResult.Primary:
        //            //do your task
        //            editAppBarButton.IsEnabled = true;
        //            saveAppBarButton.IsEnabled = false;
        //            NoteContent.IsReadOnly = true;  
        //            break;
        //        case ContentDialogResult.Secondary:
        //            //skip your task 
        //            break;
        //    }
            
        //}

        //private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    NoteContent.IsReadOnly = false;
        //}
    }
}
