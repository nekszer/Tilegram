using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Light.UWP.Services.Navigation
{
    public class NavigationService
    {
        public static NavigationService _navigationService;
        public static NavigationService Instance
        {
            get
            {
                return _navigationService;
            }
        }

        public static Type Initialize(Frame rootFrame)
        {
            _navigationService = new NavigationService(rootFrame);
            var viewType = RouteService.Current.Routes.Values.FirstOrDefault(v => v.IsMainPage)?.View;
            return viewType;
        }

        public Frame RootFrame { get; set; }
        private NavigationService(Frame rootFrame)
        {
            RootFrame = rootFrame;
        }

        public void PushAsync(string path, object arguments = null)
        {
            if (RootFrame == null)
                throw new NavigationServiceInitializationException("Service not initialized");

            if (!RouteService.Current.Routes.ContainsKey(path))
                throw new KeyNotFoundException("Path not found: " + path);

            var routeInfo = RouteService.Current.Routes[path];
            RootFrame.Navigate(routeInfo.View, arguments);
        }

        public class NavigationServiceInitializationException : Exception
        {
            public NavigationServiceInitializationException(string message) : base(message)
            {
            }
        }
    }

    public class RouteService
    {
        public Dictionary<string, RouteInfo> Routes { get; set; }


        public static RouteService _routeService;
        public static RouteService Current
        {
            get
            {
                if (_routeService == null)
                    _routeService = new RouteService();

                return _routeService;
            }
        }

        public void Add<View>(string path, bool isMainPage = false) where View : Windows.UI.Xaml.Controls.Page
        {
            if (Routes == null)
                Routes = new Dictionary<string, RouteInfo>();

            if (Routes.ContainsKey(path))
                throw new DuplicateRoutePathException("Path: " + path);

            if (isMainPage)
                foreach (var item in Routes.Values)
                    item.IsMainPage = false;

            Routes.Add(path, new RouteInfo
            {
                View = typeof(View),
                Path = path,
                IsMainPage = isMainPage
            });
        }

        public class RouteInfo
        {
            public Type View { get; set; }
            public Type ViewModel { get; set; }
            public string Path { get; set; }
            public bool IsMainPage { get; set; }
        }

        public class DuplicateRoutePathException : Exception
        {
            public DuplicateRoutePathException(string message) : base(message)
            {
            }
        }
    }
}
