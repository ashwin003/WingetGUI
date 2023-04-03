// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using WingetGUI.App.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WingetGUI.App
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            SetCurrentNavigationViewItem(args.SelectedItemContainer as NavigationViewItem);
        }

        private void SetCurrentNavigationViewItem(NavigationViewItem item)
        {
            if (item == null)
            {
                return;
            }

            if (item.Tag == null)
            {
                return;
            }

            var tag = item.Tag.ToString();
            if("Settings".Equals(tag))
            {
                tag = "WingetGUI.App.Views.Settings";
            }

            ContentFrame.Navigate(Type.GetType(tag), item.Content);
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            SetCurrentNavigationViewItem(GetNavigationViewItems(typeof(DiscoverPackages)).First());
        }

        private List<NavigationViewItem> GetNavigationViewItems()
        {
            var result = new List<NavigationViewItem>();
            var items = NavigationView.MenuItems.Select(i => (NavigationViewItem)i).ToList();
            items.AddRange(NavigationView.FooterMenuItems.Select(i => (NavigationViewItem)i));
            result.AddRange(items);

            foreach (NavigationViewItem mainItem in items)
            {
                result.AddRange(mainItem.MenuItems.Select(i => (NavigationViewItem)i));
            }

            return result;
        }

        private List<NavigationViewItem> GetNavigationViewItems(Type type)
        {
            return GetNavigationViewItems().Where(i => i.Tag.ToString() == type.FullName).ToList();
        }
    }
}
