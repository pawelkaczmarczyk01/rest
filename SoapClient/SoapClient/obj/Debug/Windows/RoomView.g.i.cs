﻿#pragma checksum "..\..\..\Windows\RoomView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "22A44A5C3DF4F7479128E23DAEFA37BBD3671948"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SoapClient.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SoapClient.Windows {
    
    
    /// <summary>
    /// RoomView
    /// </summary>
    public partial class RoomView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\Windows\RoomView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Menu MenuData;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Windows\RoomView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem MenuFirstTab;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Windows\RoomView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonData;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\Windows\RoomView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border BorderBox;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\Windows\RoomView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RoomDetails;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\Windows\RoomView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ReservationButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SoapClient;component/windows/roomview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\RoomView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MenuData = ((System.Windows.Controls.Menu)(target));
            return;
            case 2:
            this.MenuFirstTab = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 3:
            
            #line 13 "..\..\..\Windows\RoomView.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Logout);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 19 "..\..\..\Windows\RoomView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.GoBack);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ButtonData = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\Windows\RoomView.xaml"
            this.ButtonData.Click += new System.Windows.RoutedEventHandler(this.Logout);
            
            #line default
            #line hidden
            return;
            case 6:
            this.BorderBox = ((System.Windows.Controls.Border)(target));
            return;
            case 7:
            this.RoomDetails = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.ReservationButton = ((System.Windows.Controls.Button)(target));
            
            #line 93 "..\..\..\Windows\RoomView.xaml"
            this.ReservationButton.Click += new System.Windows.RoutedEventHandler(this.ReserveRoom);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

