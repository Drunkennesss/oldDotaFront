﻿#pragma checksum "..\..\Heroes.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BE098E1AE7990D28256A8BE87C6E0FA797652B4C"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

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
using datadatabase;


namespace datadatabase {
    
    
    /// <summary>
    /// Heroes
    /// </summary>
    public partial class Heroes : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SubmitButton;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataGrid;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox OperationCombo;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox AttrCombo;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem _combo_id;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem _combo_attr;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem _combo_attack;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label DependentLabel;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextQuery;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RollBack;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Commit;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\Heroes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider Level;
        
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
            System.Uri resourceLocater = new System.Uri("/datadatabase;component/heroes.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Heroes.xaml"
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
            this.SubmitButton = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\Heroes.xaml"
            this.SubmitButton.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.OperationCombo = ((System.Windows.Controls.ComboBox)(target));
            
            #line 21 "..\..\Heroes.xaml"
            this.OperationCombo.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.OperationCombo_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AttrCombo = ((System.Windows.Controls.ComboBox)(target));
            
            #line 28 "..\..\Heroes.xaml"
            this.AttrCombo.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.AttrCombo_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this._combo_id = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 6:
            this._combo_attr = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 7:
            this._combo_attack = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 8:
            this.DependentLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.TextQuery = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.RollBack = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\Heroes.xaml"
            this.RollBack.Click += new System.Windows.RoutedEventHandler(this.RollBack_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.Commit = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\Heroes.xaml"
            this.Commit.Click += new System.Windows.RoutedEventHandler(this.Commit_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 38 "..\..\Heroes.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 13:
            this.Level = ((System.Windows.Controls.Slider)(target));
            return;
            case 14:
            
            #line 41 "..\..\Heroes.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_2);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
