﻿#pragma checksum "..\..\..\..\Windows\Authorization\LogIn.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1F9BA69ACADB1E52F5A1A74830696942416DCF14"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SoapClient.Windows.Authorization;
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


namespace SoapClient.Windows.Authorization {
    
    
    /// <summary>
    /// LogIn
    /// </summary>
    public partial class LogIn : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Windows\Authorization\LogIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LoginTextBox;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\Windows\Authorization\LogIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PasswordTextBox;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\Windows\Authorization\LogIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordTextBoxP;
        
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
            System.Uri resourceLocater = new System.Uri("/SoapClient;component/windows/authorization/login.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\Authorization\LogIn.xaml"
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
            this.LoginTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 10 "..\..\..\..\Windows\Authorization\LogIn.xaml"
            this.LoginTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.LoginEnter);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\..\Windows\Authorization\LogIn.xaml"
            this.LoginTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.LoginLeave);
            
            #line default
            #line hidden
            return;
            case 2:
            this.PasswordTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.PasswordTextBoxP = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 14 "..\..\..\..\Windows\Authorization\LogIn.xaml"
            this.PasswordTextBoxP.GotFocus += new System.Windows.RoutedEventHandler(this.PasswordEnter);
            
            #line default
            #line hidden
            
            #line 14 "..\..\..\..\Windows\Authorization\LogIn.xaml"
            this.PasswordTextBoxP.LostFocus += new System.Windows.RoutedEventHandler(this.PasswordLeave);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 16 "..\..\..\..\Windows\Authorization\LogIn.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Login);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 18 "..\..\..\..\Windows\Authorization\LogIn.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SignUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

